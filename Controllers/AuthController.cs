using CalBal.Models;
using CalBal.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CalBal.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // GET: Auth/Register
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        // POST: Auth/Register
        [HttpPost]
#if !DEBUG
        [ValidateAntiForgeryToken]
#endif
        public async Task<IActionResult> Register([Bind("Ime,Prezime,Email,DatumRodenja,Visina,Tezina")] Korisnik korisnik, string lozinka)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
                return View(korisnik);

            var (IsSuccess, ErrorMessage) = await _authService.RegisterAsync(korisnik, lozinka);

            if (!IsSuccess)
            {
                ModelState.AddModelError("Email", ErrorMessage ?? "Registration failed.");
                return View(korisnik);
            }

            return RedirectToAction("Login");
        }

        // GET: Auth/Login
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        // POST: Auth/Login
        [HttpPost]
#if !DEBUG
        [ValidateAntiForgeryToken]
#endif
        public async Task<IActionResult> Login(string email, string lozinka)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            var (IsSuccess, korisnik) = await _authService.LoginAsync(email, lozinka);

            if (!IsSuccess || korisnik == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, korisnik.Email),
                new Claim("KorisnikId", korisnik.KorisnikId.ToString()),
                new Claim("RazinaOvlasti", korisnik.RazinaOvlasti.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }

        // GET: Auth/Logout
        public async Task<IActionResult> Logout()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Auth");

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
