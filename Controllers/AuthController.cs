using CalBal.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CalBal.Controllers
{
    public class AuthController : Controller
    {
        private readonly CalbalContext _context;

        public AuthController(CalbalContext context)
        {
            _context = context;
        }

        // GET: Auth/Register
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Auth/Register
        [HttpPost]
#if !DEBUG
        [ValidateAntiForgeryToken]
#endif
        public async Task<IActionResult> Register([Bind("Ime,Prezime,Email,Lozinka,DatumRodenja,Visina,Tezina")] Korisnik korisnik)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                // Check if email already exists
                if (_context.Korisniks.Any(k => k.Email == korisnik.Email))
                {
                    ModelState.AddModelError("Email", "Email already registered.");
                    return View(korisnik);
                }

                var hasher = new PasswordHasher<Korisnik>();
                korisnik.Lozinka = hasher.HashPassword(korisnik, korisnik.Lozinka); // Hash the password

                korisnik.RazinaOvlasti = Models.Enums.RazinaOvlasti.niska; // Default role
                _context.Add(korisnik);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            return View(korisnik);
        }

        // GET: Auth/Login
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
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
            {
                return RedirectToAction("Index", "Home");
            }
            var korisnik = _context.Korisniks.FirstOrDefault(k => k.Email == email);
            if (korisnik == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attemp.");
                return View();
            }

            var hasher = new PasswordHasher<Korisnik>();
            var result = hasher.VerifyHashedPassword(korisnik, korisnik.Lozinka, lozinka);

            if (result == PasswordVerificationResult.Failed)
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
            {
                return RedirectToAction("Login", "Auth");
            }
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
