using CalBal.Models;
using CalBal.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CalBal.Data
{
    public class AktivnostRepository : IAktivnostRepository
    {
        private readonly CalbalContext _context;

        public AktivnostRepository(CalbalContext context)
        {
            _context = context;
        }

        public async Task<List<Aktivnost>> GetAllAsync()
        {
            return await _context.Aktivnosts.ToListAsync();
        }

        public async Task<Aktivnost?> GetByIdAsync(int id)
        {
            return await _context.Aktivnosts.FindAsync(id);
        }

        public async Task AddAsync(Aktivnost aktivnost)
        {
            _context.Aktivnosts.Add(aktivnost);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Aktivnost aktivnost)
        {
            _context.Aktivnosts.Update(aktivnost);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var aktivnost = await _context.Aktivnosts.FindAsync(id);
            if (aktivnost != null)
            {
                _context.Aktivnosts.Remove(aktivnost);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Aktivnosts.AnyAsync(a => a.AktivnostId == id);
        }
    }
}
