using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CalBal.Models;

namespace CalBal.Controllers
{
    public class PrehrambenanamirnicasController : Controller
    {
        private readonly CalbalContext _context;

        public PrehrambenanamirnicasController(CalbalContext context)
        {
            _context = context;
        }

        // GET: Prehrambenanamirnicas
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Auth");
            }
            return View(await _context.Prehrambenanamirnicas.ToListAsync());
        }

        // GET: Prehrambenanamirnicas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!User.Identity.IsAuthenticated || User.FindFirst("RazinaOvlasti")?.Value == Models.Enums.RazinaOvlasti.niska.ToString())
            {
                return RedirectToAction("Login", "Auth");
            }
            if (id == null)
            {
                return NotFound();
            }

            var prehrambenanamirnica = await _context.Prehrambenanamirnicas
                .FirstOrDefaultAsync(m => m.PrehrambenaNamirnicaId == id);
            if (prehrambenanamirnica == null)
            {
                return NotFound();
            }

            return View(prehrambenanamirnica);
        }

        // GET: Prehrambenanamirnicas/Create
        public IActionResult Create()
        {
            if (!User.Identity.IsAuthenticated || User.FindFirst("RazinaOvlasti")?.Value == Models.Enums.RazinaOvlasti.niska.ToString())
            {
                return RedirectToAction("Login", "Auth");
            }
            return View();
        }

        // POST: Prehrambenanamirnicas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PrehrambenaNamirnicaId,Naziv,Kalorije,Proteini,Masti,Ugljikohidrati")] Prehrambenanamirnica prehrambenanamirnica)
        {
            if (!User.Identity.IsAuthenticated || User.FindFirst("RazinaOvlasti")?.Value == Models.Enums.RazinaOvlasti.niska.ToString())
            {
                return RedirectToAction("Login", "Auth");
            }
            if (ModelState.IsValid)
            {
                _context.Add(prehrambenanamirnica);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(prehrambenanamirnica);
        }

        // GET: Prehrambenanamirnicas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!User.Identity.IsAuthenticated || User.FindFirst("RazinaOvlasti")?.Value == Models.Enums.RazinaOvlasti.niska.ToString())
            {
                return RedirectToAction("Login", "Auth");
            }
            if (id == null)
            {
                return NotFound();
            }

            var prehrambenanamirnica = await _context.Prehrambenanamirnicas.FindAsync(id);
            if (prehrambenanamirnica == null)
            {
                return NotFound();
            }
            return View(prehrambenanamirnica);
        }

        // POST: Prehrambenanamirnicas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PrehrambenaNamirnicaId,Naziv,Kalorije,Proteini,Masti,Ugljikohidrati")] Prehrambenanamirnica prehrambenanamirnica)
        {
            if (!User.Identity.IsAuthenticated || User.FindFirst("RazinaOvlasti")?.Value == Models.Enums.RazinaOvlasti.niska.ToString())
            {
                return RedirectToAction("Login", "Auth");
            }
            if (id != prehrambenanamirnica.PrehrambenaNamirnicaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prehrambenanamirnica);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrehrambenanamirnicaExists(prehrambenanamirnica.PrehrambenaNamirnicaId))
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
            return View(prehrambenanamirnica);
        }

        // GET: Prehrambenanamirnicas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!User.Identity.IsAuthenticated || User.FindFirst("RazinaOvlasti")?.Value == Models.Enums.RazinaOvlasti.niska.ToString())
            {
                return RedirectToAction("Login", "Auth");
            }
            if (id == null)
            {
                return NotFound();
            }

            var prehrambenanamirnica = await _context.Prehrambenanamirnicas
                .FirstOrDefaultAsync(m => m.PrehrambenaNamirnicaId == id);
            if (prehrambenanamirnica == null)
            {
                return NotFound();
            }

            return View(prehrambenanamirnica);
        }

        // POST: Prehrambenanamirnicas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!User.Identity.IsAuthenticated || User.FindFirst("RazinaOvlasti")?.Value == Models.Enums.RazinaOvlasti.niska.ToString())
            {
                return RedirectToAction("Login", "Auth");
            }
            var prehrambenanamirnica = await _context.Prehrambenanamirnicas.FindAsync(id);
            if (prehrambenanamirnica != null)
            {
                _context.Prehrambenanamirnicas.Remove(prehrambenanamirnica);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrehrambenanamirnicaExists(int id)
        {
            return _context.Prehrambenanamirnicas.Any(e => e.PrehrambenaNamirnicaId == id);
        }
    }
}
