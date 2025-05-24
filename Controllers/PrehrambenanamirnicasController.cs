using CalBal.Models;
using CalBal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CalBal.Controllers
{
    public class PrehrambenaNamirnicasController : Controller
    {
        private readonly PrehrambenaNamirnicaService _service;

        public PrehrambenaNamirnicasController(PrehrambenaNamirnicaService service)
        {
            _service = service;
        }

        [Authorize(Policy = "NiskaRazina")]
        public async Task<IActionResult> Index()
        {
            var namirnice = await _service.DohvatiSveAsync();
            return View(namirnice);
        }

        [Authorize(Policy = "VisokaRazina")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var namirnica = await _service.DohvatiPoIdAsync(id.Value);
            if (namirnica == null) return NotFound();

            return View(namirnica);
        }

        [Authorize(Policy = "VisokaRazina")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "VisokaRazina")]
        public async Task<IActionResult> Create([Bind("PrehrambenaNamirnicaId,Naziv,Kalorije,Proteini,Masti,Ugljikohidrati")] Prehrambenanamirnica namirnica)
        {
            if (ModelState.IsValid)
            {
                await _service.DodajAsync(namirnica);
                return RedirectToAction(nameof(Index));
            }
            return View(namirnica);
        }

        [Authorize(Policy = "VisokaRazina")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var namirnica = await _service.DohvatiPoIdAsync(id.Value);
            if (namirnica == null) return NotFound();

            return View(namirnica);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "VisokaRazina")]
        public async Task<IActionResult> Edit(int id, [Bind("PrehrambenaNamirnicaId,Naziv,Kalorije,Proteini,Masti,Ugljikohidrati")] Prehrambenanamirnica namirnica)
        {
            if (id != namirnica.PrehrambenaNamirnicaId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.UrediAsync(namirnica);
                }
                catch
                {
                    if (!await _service.PostojiAsync(id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(namirnica);
        }

        [Authorize(Policy = "VisokaRazina")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var namirnica = await _service.DohvatiPoIdAsync(id.Value);
            if (namirnica == null) return NotFound();

            return View(namirnica);
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
