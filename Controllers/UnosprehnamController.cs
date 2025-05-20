using Microsoft.AspNetCore.Mvc;
using CalBal.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Security.Claims;

namespace CalBal.Controllers
{
    public class UnosprehnamController : Controller
    {
        private readonly CalbalContext _context;

        public UnosprehnamController(CalbalContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] Unosprehnam model)
        {
            var korisnikIdClaim = User.FindFirst("KorisnikId")?.Value;
            if (korisnikIdClaim == null || !int.TryParse(korisnikIdClaim, out int korisnikId))
                return Unauthorized();

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
        public async Task<IActionResult> Update([FromForm] Unosprehnam model)
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

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
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
    }
}