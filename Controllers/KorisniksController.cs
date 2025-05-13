using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CalBal.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace CalBal.Controllers
{
    public class KorisniksController : Controller
    {
        private readonly CalbalContext _context;

        public KorisniksController(CalbalContext context)
        {
            _context = context;
        }

        // GET: Korisniks
        public async Task<IActionResult> Index()
        {
            return View(await _context.Korisniks.ToListAsync());
        }

        // GET: Korisniks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnik = await _context.Korisniks
                .FirstOrDefaultAsync(m => m.KorisnikId == id);
            if (korisnik == null)
            {
                return NotFound();
            }

            return View(korisnik);
        }

        // GET: Korisniks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Korisniks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KorisnikId,Ime,Prezime,Email,Lozinka,DatumRodenja,Visina,Tezina,RazinaOvlasti")] Korisnik korisnik)
        {
            if (ModelState.IsValid)
            {
                _context.Add(korisnik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(korisnik);
        }

        // GET: Korisniks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnik = await _context.Korisniks.FindAsync(id);
            if (korisnik == null)
            {
                return NotFound();
            }
            return View(korisnik);
        }

        // POST: Korisniks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("KorisnikId,Ime,Prezime,Email,Lozinka,DatumRodenja,Visina,Tezina,RazinaOvlasti")] Korisnik korisnik)
        {
            if (id != korisnik.KorisnikId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(korisnik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KorisnikExists(korisnik.KorisnikId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(korisnik);
        }

        // GET: Korisniks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnik = await _context.Korisniks
                .FirstOrDefaultAsync(m => m.KorisnikId == id);
            if (korisnik == null)
            {
                return NotFound();
            }

            return View(korisnik);
        }

        // POST: Korisniks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var korisnik = await _context.Korisniks.FindAsync(id);
            if (korisnik != null)
            {
                _context.Korisniks.Remove(korisnik);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KorisnikExists(int id)
        {
            return _context.Korisniks.Any(e => e.KorisnikId == id);
        }

        // GET: Korisniks/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Korisniks/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Ime,Prezime,Email,Lozinka,DatumRodenja,Visina,Tezina")] Korisnik korisnik)
        {
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

        // GET: Korisniks/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Korisniks/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string lozinka)
        {
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

        // GET: Korisniks/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
