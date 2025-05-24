using CalBal.Models;
using CalBal.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CalBal.Data
{
    public class PrehrambenaNamirnicaRepository : IPrehrambenaNamirnicaRepository
    {
        private readonly CalbalContext _context;

        public PrehrambenaNamirnicaRepository(CalbalContext context)
        {
            _context = context;
        }

        public async Task<List<Prehrambenanamirnica>> GetAllAsync()
        {
            return await _context.Prehrambenanamirnicas.ToListAsync();
        }

        public async Task<Prehrambenanamirnica?> GetByIdAsync(int id)
        {
            return await _context.Prehrambenanamirnicas.FindAsync(id);
        }

        public async Task AddAsync(Prehrambenanamirnica namirnica)
        {
            _context.Prehrambenanamirnicas.Add(namirnica);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Prehrambenanamirnica namirnica)
        {
            _context.Prehrambenanamirnicas.Update(namirnica);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var namirnica = await _context.Prehrambenanamirnicas.FindAsync(id);
            if (namirnica != null)
            {
                _context.Prehrambenanamirnicas.Remove(namirnica);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Prehrambenanamirnicas.AnyAsync(a => a.PrehrambenaNamirnicaId == id);
        }
    }
}
