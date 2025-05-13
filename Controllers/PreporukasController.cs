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
    public class PreporukasController : Controller
    {
        private readonly CalbalContext _context;

        public PreporukasController(CalbalContext context)
        {
            _context = context;
        }

        // GET: Preporukas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Preporukas.ToListAsync());
        }

        // GET: Preporukas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var preporuka = await _context.Preporukas
                .FirstOrDefaultAsync(m => m.PreporukaId == id);
            if (preporuka == null)
            {
                return NotFound();
            }

            return View(preporuka);
        }

        // GET: Preporukas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Preporukas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PreporukaId,Sadrzaj,DnevniDeficitSuficit")] Preporuka preporuka)
        {
            if (ModelState.IsValid)
            {
                _context.Add(preporuka);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(preporuka);
        }

        // GET: Preporukas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var preporuka = await _context.Preporukas.FindAsync(id);
            if (preporuka == null)
            {
                return NotFound();
            }
            return View(preporuka);
        }

        // POST: Preporukas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PreporukaId,Sadrzaj,DnevniDeficitSuficit")] Preporuka preporuka)
        {
            if (id != preporuka.PreporukaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(preporuka);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PreporukaExists(preporuka.PreporukaId))
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
            return View(preporuka);
        }

        // GET: Preporukas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var preporuka = await _context.Preporukas
                .FirstOrDefaultAsync(m => m.PreporukaId == id);
            if (preporuka == null)
            {
                return NotFound();
            }

            return View(preporuka);
        }

        // POST: Preporukas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var preporuka = await _context.Preporukas.FindAsync(id);
            if (preporuka != null)
            {
                _context.Preporukas.Remove(preporuka);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PreporukaExists(int id)
        {
            return _context.Preporukas.Any(e => e.PreporukaId == id);
        }
    }
}
