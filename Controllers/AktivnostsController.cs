using CalBal.Models;
using CalBal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CalBal.Controllers
{
    public class AktivnostsController : Controller
    {
        private readonly AktivnostService _service;

        public AktivnostsController(AktivnostService service)
        {
            _service = service;
        }

        [Authorize(Policy = "NiskaRazina")]
        public async Task<IActionResult> Index()
        {
            var aktivnosti = await _service.DohvatiSveAsync();
            return View(aktivnosti);
        }

        [Authorize(Policy = "VisokaRazina")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var aktivnost = await _service.DohvatiPoIdAsync(id.Value);
            if (aktivnost == null) return NotFound();

            return View(aktivnost);
        }

        [Authorize(Policy = "VisokaRazina")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "VisokaRazina")]
        public async Task<IActionResult> Create([Bind("AktivnostId,Naziv,Potrosnja")] Aktivnost aktivnost)
        {
            if (ModelState.IsValid)
            {
                await _service.DodajAsync(aktivnost);
                return RedirectToAction(nameof(Index));
            }
            return View(aktivnost);
        }

        [Authorize(Policy = "VisokaRazina")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var aktivnost = await _service.DohvatiPoIdAsync(id.Value);
            if (aktivnost == null) return NotFound();

            return View(aktivnost);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "VisokaRazina")]
        public async Task<IActionResult> Edit(int id, [Bind("AktivnostId,Naziv,Potrosnja")] Aktivnost aktivnost)
        {
            if (id != aktivnost.AktivnostId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.UrediAsync(aktivnost);
                }
                catch
                {
                    if (!await _service.PostojiAsync(id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(aktivnost);
        }

        [Authorize(Policy = "VisokaRazina")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var aktivnost = await _service.DohvatiPoIdAsync(id.Value);
            if (aktivnost == null) return NotFound();

            return View(aktivnost);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "VisokaRazina")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.ObrisiAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
