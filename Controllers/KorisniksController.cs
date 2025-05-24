using CalBal.Models;
using CalBal.Models.Enums;
using CalBal.Services.Interfaces;
using CalBal.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CalBal.Controllers
{
    public class KorisniksController : Controller
    {
        private readonly IKorisnikService _korisnikService;

        public KorisniksController(IKorisnikService korisnikService)
        {
            _korisnikService = korisnikService;
        }

        // GET: Korisniks
        [Authorize(Policy = "VisokaRazina")]
        public async Task<IActionResult> Index()
        {
            var korisnici = await _korisnikService.DohvatiSveKorisnikeAsync();
            return View(korisnici);
        }

        // GET: Korisniks/Create
        [Authorize(Policy = "VisokaRazina")]
        public IActionResult Create()
        {
            ViewBag.RazinaOvlasti = new SelectList(Enum.GetValues(typeof(RazinaOvlasti)));
            return View();
        }

        // POST: Korisniks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "VisokaRazina")]
        public async Task<IActionResult> Create([Bind("KorisnikId,Ime,Prezime,Email,Lozinka,DatumRodenja,Visina,Tezina,RazinaOvlasti")] Korisnik korisnik)
        {
            if (ModelState.IsValid)
            {
                await _korisnikService.DodajKorisnikaAsync(korisnik);
                return RedirectToAction(nameof(Index));
            }
            return View(korisnik);
        }

        // GET: Korisniks/Edit/5
        [Authorize(Policy = "VisokaRazina")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnik = await _korisnikService.DohvatiKorisnikaPoIdAsync(id.Value);
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
                    var existing = await _korisnikService.DohvatiKorisnikaPoIdAsync(id);
                    if (existing == null)
                        return NotFound();

                    if (!isAdmin)
                    {
                        korisnik.RazinaOvlasti = existing.RazinaOvlasti;
                    }

                    await _korisnikService.AzurirajKorisnikaAsync(korisnik);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _korisnikService.KorisnikPostojiAsync(korisnik.KorisnikId))
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
        [Authorize(Policy = "VisokaRazina")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnik = await _korisnikService.DohvatiKorisnikaPoIdAsync(id.Value);
            if (korisnik == null)
            {
                return NotFound();
            }

            return View(korisnik);
        }

        // POST: Korisniks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "VisokaRazina")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var korisnik = await _korisnikService.DohvatiKorisnikaPoIdAsync(id);
            if (korisnik != null)
            {
                await _korisnikService.ObrisiKorisnikaAsync(korisnik);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Korisniks/MasterDetail
        [Authorize(Policy = "NiskaRazina")]
        public async Task<IActionResult> MasterDetail(string activitySearch, string foodSearch)
        {
            var korisnikIdClaim = User.FindFirst("KorisnikId")?.Value;
            if (korisnikIdClaim == null)
                return RedirectToAction("Login", "Auth");

            if (!int.TryParse(korisnikIdClaim, out int korisnikId))
                return NotFound();

            var korisnik = await _korisnikService.DohvatiKorisnikaSaDetaljimaFiltriranoAsync(korisnikId, activitySearch, foodSearch);
            if (korisnik == null)
                return NotFound();

            var viewModel = new KorisnikMasterDetailViewModel
            {
                Korisnik = korisnik,
                SveAktivnosti = await _korisnikService.DohvatiSveAktivnostiAsync(),
                SveNamirnice = await _korisnikService.DohvatiSveNamirniceAsync(),
                ActivitySearch = activitySearch,
                FoodSearch = foodSearch
            };

            return View(viewModel);
        }

    }
}
