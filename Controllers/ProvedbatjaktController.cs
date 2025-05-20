using Microsoft.AspNetCore.Mvc;
using CalBal.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Security.Claims;

namespace CalBal.Controllers
{
    public class ProvedbatjaktController : Controller
    {
        private readonly CalbalContext _context;

        public ProvedbatjaktController(CalbalContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] Provedbatjakt model)
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
        public async Task<IActionResult> Delete(int id)
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
        public async Task<IActionResult> Update([FromForm] Provedbatjakt model)
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
    }
}