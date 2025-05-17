using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CalBal.Models;
using CalBal.Models.Enums;
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
            if (!User.Identity.IsAuthenticated || User.FindFirst("RazinaOvlasti")?.Value != Models.Enums.RazinaOvlasti.visoka.ToString())
            {
                return RedirectToAction("Login", "Korisniks");
            }
            return View(await _context.Korisniks.ToListAsync());
        }

        // GET: Korisniks/Create
        public IActionResult Create()
        {
            if (!User.Identity.IsAuthenticated || User.FindFirst("RazinaOvlasti")?.Value != Models.Enums.RazinaOvlasti.visoka.ToString())
            {
                return RedirectToAction("Login", "Korisniks");
            }
            ViewBag.RazinaOvlasti = new SelectList(Enum.GetValues(typeof(RazinaOvlasti)));
            return View();
        }

        // POST: Korisniks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KorisnikId,Ime,Prezime,Email,Lozinka,DatumRodenja,Visina,Tezina,RazinaOvlasti")] Korisnik korisnik)
        {
            if (!User.Identity.IsAuthenticated || User.FindFirst("RazinaOvlasti")?.Value != Models.Enums.RazinaOvlasti.visoka.ToString())
            {
                return RedirectToAction("Login", "Korisniks");
            }
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
            if (!User.Identity.IsAuthenticated || (User.FindFirst("RazinaOvlasti")?.Value != Models.Enums.RazinaOvlasti.visoka.ToString() && User.FindFirst("KorisnikId")?.Value != id.ToString()))
            {
                return RedirectToAction("Login", "Korisniks");
            }
            if (id == null)
            {
                return NotFound();
            }

            var korisnik = await _context.Korisniks.FindAsync(id);
            if (korisnik == null)
            {
                return NotFound();
            }
            ViewBag.RazinaOvlasti = new SelectList(Enum.GetValues(typeof(RazinaOvlasti)));
            return View(korisnik);
        }

        // POST: Korisniks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("KorisnikId,Ime,Prezime,Email,Lozinka,DatumRodenja,Visina,Tezina,RazinaOvlasti")] Korisnik korisnik)
        {
            var isAdmin = User.FindFirst("RazinaOvlasti")?.Value == Models.Enums.RazinaOvlasti.visoka.ToString();

            if (!User.Identity.IsAuthenticated || (!isAdmin && User.FindFirst("KorisnikId")?.Value != id.ToString()))
            {
                return RedirectToAction("Login", "Korisniks");
            }
            if (id != korisnik.KorisnikId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.Korisniks.AsNoTracking().FirstOrDefaultAsync(k => k.KorisnikId == id);
                    if (existing == null)
                        return NotFound();

                    if (!isAdmin)
                    {
                        korisnik.RazinaOvlasti = existing.RazinaOvlasti;
                    }

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
            if (!User.Identity.IsAuthenticated || User.FindFirst("RazinaOvlasti")?.Value != Models.Enums.RazinaOvlasti.visoka.ToString())
            {
                return RedirectToAction("Login", "Korisniks");
            }
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
            if (!User.Identity.IsAuthenticated || User.FindFirst("RazinaOvlasti")?.Value != Models.Enums.RazinaOvlasti.visoka.ToString())
            {
                return RedirectToAction("Login", "Korisniks");
            }
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
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Korisniks/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: Korisniks/Login
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Korisniks/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: Korisniks/Logout
        public async Task<IActionResult> Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Korisniks");
            }
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // GET: Korisniks/MasterDetail
        public async Task<IActionResult> MasterDetail()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Korisniks");

            var korisnikIdClaim = User.FindFirst("KorisnikId")?.Value;
            if (korisnikIdClaim == null)
                return RedirectToAction("Login", "Korisniks");

            if (!int.TryParse(korisnikIdClaim, out int korisnikId))
                return NotFound();

            var korisnik = await _context.Korisniks
                .Include(k => k.Provedbatjakts)
                    .ThenInclude(p => p.Aktivnost)
                .Include(k => k.Unosprehnams)
                    .ThenInclude(u => u.Hrana)
                .FirstOrDefaultAsync(k => k.KorisnikId == korisnikId);

            if (korisnik == null)
                return NotFound();

            ViewBag.Aktivnosti = await _context.Aktivnosts.ToListAsync();
            ViewBag.PrehrambeneNamirnice = await _context.Prehrambenanamirnicas.ToListAsync();

            return View(korisnik);
        }

        [HttpPost]
        public async Task<IActionResult> AddProvedbaAktivnostAjax([FromForm] Provedbatjakt model)
        {
            var korisnikIdClaim = User.FindFirst("KorisnikId")?.Value;
            if (korisnikIdClaim == null || !int.TryParse(korisnikIdClaim, out int korisnikId))
                return Unauthorized();

            model.KorisnikId = korisnikId;
            if (ModelState.IsValid)
            {
                _context.Provedbatjakts.Add(model);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> AddUnosPrehrambenaNamirnicaAjax([FromForm] Unosprehnam model)
        {
            var korisnikIdClaim = User.FindFirst("KorisnikId")?.Value;
            if (korisnikIdClaim == null || !int.TryParse(korisnikIdClaim, out int korisnikId))
            {
                return Unauthorized();
            }

            model.KorisnikId = korisnikId;

            if (ModelState.IsValid)
            {
                _context.Unosprehnams.Add(model);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProvedbaAktivnostAjax(int id)
        {
            var korisnikIdClaim = User.FindFirst("KorisnikId")?.Value;
            if (korisnikIdClaim == null || !int.TryParse(korisnikIdClaim, out int korisnikId))
                return Unauthorized();
            var provedba = await _context.Provedbatjakts.FindAsync(id);
            if (provedba == null || provedba.KorisnikId != korisnikId)
                return NotFound();
            _context.Provedbatjakts.Remove(provedba);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUnosPrehrambenaNamirnicaAjax(int id)
        {
            var korisnikIdClaim = User.FindFirst("KorisnikId")?.Value;
            if (korisnikIdClaim == null || !int.TryParse(korisnikIdClaim, out int korisnikId))
                return Unauthorized();
            var unos = await _context.Unosprehnams.FindAsync(id);
            if (unos == null || unos.KorisnikId != korisnikId)
                return NotFound();
            _context.Unosprehnams.Remove(unos);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProvedbaAktivnostAjax([FromForm] Provedbatjakt model)
        {
            var korisnikIdClaim = User.FindFirst("KorisnikId")?.Value;
            if (korisnikIdClaim == null || !int.TryParse(korisnikIdClaim, out int korisnikId))
                return Unauthorized();
            var existing = await _context.Provedbatjakts.FindAsync(model.ProvedbaTjAktId);
            if (existing == null || existing.KorisnikId != korisnikId)
                return NotFound();
            existing.Trajanje = model.Trajanje;
            existing.Datum = model.Datum;
            existing.AktivnostId = model.AktivnostId;
            if (ModelState.IsValid)
            {
                _context.Update(existing);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUnosPrehrambenaNamirnicaAjax([FromForm] Unosprehnam model)
        {
            var korisnikIdClaim = User.FindFirst("KorisnikId")?.Value;
            if (korisnikIdClaim == null || !int.TryParse(korisnikIdClaim, out int korisnikId))
                return Unauthorized();
            var existing = await _context.Unosprehnams.FindAsync(model.UnosPrehNamId);
            if (existing == null || existing.KorisnikId != korisnikId)
                return NotFound();
            existing.Kolicina = model.Kolicina;
            existing.Datum = model.Datum;
            existing.HranaId = model.HranaId;
            if (ModelState.IsValid)
            {
                _context.Update(existing);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }
    }
}
