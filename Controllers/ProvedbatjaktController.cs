using CalBal.Models;
using CalBal.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CalBal.Controllers
{
    public class ProvedbatjaktController : Controller
    {
        private readonly ProvedbaTjAktService _service;

        public ProvedbatjaktController(ProvedbaTjAktService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] Provedbatjakt model)
        {
            var korisnikIdClaim = User.FindFirst("KorisnikId")?.Value;
            if (korisnikIdClaim == null || !int.TryParse(korisnikIdClaim, out int korisnikId))
                return Unauthorized();

            model.KorisnikId = korisnikId;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (Success, ErrorMessage) = await _service.AddAsync(model);

            if (!Success)
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var korisnikIdClaim = User.FindFirst("KorisnikId")?.Value;
            if (korisnikIdClaim == null || !int.TryParse(korisnikIdClaim, out int korisnikId))
                return Unauthorized();

            var provedba = await _service.GetByIdAsync(id);
            if (provedba == null || provedba.KorisnikId != korisnikId)
                return NotFound();

            await _service.DeleteAsync(provedba);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm] Provedbatjakt model)
        {
            var korisnikIdClaim = User.FindFirst("KorisnikId")?.Value;
            if (korisnikIdClaim == null || !int.TryParse(korisnikIdClaim, out int korisnikId))
                return Unauthorized();

            var existing = await _service.GetByIdAsync(model.ProvedbaTjAktId);
            if (existing == null || existing.KorisnikId != korisnikId)
                return NotFound();

            existing.Trajanje = model.Trajanje;
            existing.Datum = model.Datum;
            existing.AktivnostId = model.AktivnostId;

            if (ModelState.IsValid)
            {
                await _service.UpdateAsync(existing);
                return Ok();
            }
            return BadRequest(ModelState);
        }
    }
}
