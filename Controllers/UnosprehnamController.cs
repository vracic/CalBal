using CalBal.Models;
using CalBal.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CalBal.Controllers
{
    public class UnosprehnamController : Controller
    {
        private readonly UnosPrehNamService _service;

        public UnosprehnamController(UnosPrehNamService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] Unosprehnam model)
        {
            var korisnikIdClaim = User.FindFirst("KorisnikId")?.Value;
            if (korisnikIdClaim == null || !int.TryParse(korisnikIdClaim, out int korisnikId))
                return Unauthorized();

            model.KorisnikId = korisnikId;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, errorMessage) = await _service.AddAsync(model);
            if (!success)
            {
                ModelState.AddModelError(nameof(model.Kolicina), errorMessage ?? "Validation failed.");
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm] Unosprehnam model)
        {
            var korisnikIdClaim = User.FindFirst("KorisnikId")?.Value;
            if (korisnikIdClaim == null || !int.TryParse(korisnikIdClaim, out int korisnikId))
                return Unauthorized();

            var existing = await _service.GetByIdAsync(model.UnosPrehNamId);
            if (existing == null || existing.KorisnikId != korisnikId)
                return NotFound();

            existing.Kolicina = model.Kolicina;
            existing.Datum = model.Datum;
            existing.HranaId = model.HranaId;

            if (ModelState.IsValid)
            {
                await _service.UpdateAsync(existing);
                return Ok();
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var korisnikIdClaim = User.FindFirst("KorisnikId")?.Value;
            if (korisnikIdClaim == null || !int.TryParse(korisnikIdClaim, out int korisnikId))
                return Unauthorized();

            var existing = await _service.GetByIdAsync(id);
            if (existing == null || existing.KorisnikId != korisnikId)
                return NotFound();

            await _service.DeleteAsync(existing);
            return Ok();
        }
    }
}
