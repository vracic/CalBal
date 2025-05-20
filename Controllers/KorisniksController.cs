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
using CalBal.Services;

namespace CalBal.Controllers
{
    public class KorisniksController : Controller
    {
        private readonly CalbalContext _context;
        private readonly KorisnikService _korisnikService;

        public KorisniksController(CalbalContext context, KorisnikService korisnikService)
        {
            _context = context;
            _korisnikService = korisnikService;
        }

        // GET: Korisniks
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated || User.FindFirst("RazinaOvlasti")?.Value != Models.Enums.RazinaOvlasti.visoka.ToString())
            {
                return RedirectToAction("Login", "Auth");
            }
            return View(await _context.Korisniks.ToListAsync());
        }

        // GET: Korisniks/Create
        public IActionResult Create()
        {
            if (!User.Identity.IsAuthenticated || User.FindFirst("RazinaOvlasti")?.Value != Models.Enums.RazinaOvlasti.visoka.ToString())
            {
                return RedirectToAction("Login", "Auth");
            }
            ViewBag.RazinaOvlasti = new SelectList(Enum.GetValues(typeof(RazinaOvlasti)));
            return View();
        }

        // POST: Korisniks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KorisnikId,Ime,Prezime,Email,Lozinka,DatumRodenja,Visina,Tezina,RazinaOvlasti")] Korisnik korisnik)
        {
            if (!User.Identity.IsAuthenticated || User.FindFirst("RazinaOvlasti")?.Value != Models.Enums.RazinaOvlasti.visoka.ToString())
            {
                return RedirectToAction("Login", "Auth");
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
                return RedirectToAction("Login", "Auth");
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("KorisnikId,Ime,Prezime,Email,Lozinka,DatumRodenja,Visina,Tezina,RazinaOvlasti")] Korisnik korisnik)
        {
            var isAdmin = User.FindFirst("RazinaOvlasti")?.Value == Models.Enums.RazinaOvlasti.visoka.ToString();

            if (!User.Identity.IsAuthenticated || (!isAdmin && User.FindFirst("KorisnikId")?.Value != id.ToString()))
            {
                return RedirectToAction("Login", "Auth");
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
                return RedirectToAction("Login", "Auth");
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
                return RedirectToAction("Login", "Auth");
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

        // GET: Korisniks/MasterDetail
        public async Task<IActionResult> MasterDetail(string activitySearch, string foodSearch)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Auth");

            var korisnikIdClaim = User.FindFirst("KorisnikId")?.Value;
            if (korisnikIdClaim == null)
                return RedirectToAction("Login", "Auth");

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

            // Filter Provedbatjakts by activity name
            if (!string.IsNullOrWhiteSpace(activitySearch))
            {
                korisnik.Provedbatjakts = korisnik.Provedbatjakts
                    .Where(p => p.Aktivnost != null && p.Aktivnost.Naziv.Contains(activitySearch, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Filter Unosprehnams by food name
            if (!string.IsNullOrWhiteSpace(foodSearch))
            {
                korisnik.Unosprehnams = korisnik.Unosprehnams
                    .Where(u => u.Hrana != null && u.Hrana.Naziv.Contains(foodSearch, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            foreach (var p in korisnik.Provedbatjakts)
                p.PotroseneKalorije = _korisnikService.IzracunajPotroseneKalorije(p);

            foreach (var u in korisnik.Unosprehnams)
                u.UneseneKalorije = _korisnikService.IzracunajUneseneKalorije(u);

            ViewBag.Aktivnosti = await _context.Aktivnosts.ToListAsync();
            ViewBag.PrehrambeneNamirnice = await _context.Prehrambenanamirnicas.ToListAsync();
            ViewBag.ActivitySearch = activitySearch;
            ViewBag.FoodSearch = foodSearch;

            return View(korisnik);
        }
    }
}
