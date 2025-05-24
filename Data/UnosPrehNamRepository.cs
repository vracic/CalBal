using CalBal.Models;
using CalBal.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CalBal.Data
{
    public class UnosPrehNamRepository : IUnosPrehNamRepository
    {
        private readonly CalbalContext _context;

        public UnosPrehNamRepository(CalbalContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Unosprehnam model)
        {
            await _context.Unosprehnams.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task<Unosprehnam?> GetByIdAsync(int id)
        {
            return await _context.Unosprehnams.FindAsync(id);
        }

        public async Task UpdateAsync(Unosprehnam model)
        {
            _context.Unosprehnams.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Unosprehnam model)
        {
            _context.Unosprehnams.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Unosprehnam>> GetDailyEntriesAsync(int korisnikId, DateOnly datum, int? excludeId = null)
        {
            var query = _context.Unosprehnams
                .Include(u => u.Hrana)
                .Where(u => u.KorisnikId == korisnikId && u.Datum == datum);

            if (excludeId.HasValue)
                query = query.Where(u => u.UnosPrehNamId != excludeId.Value);

            return await query.ToListAsync();
        }
        public async Task<Prehrambenanamirnica> GetPrehrambenaNamirnicaByIdAsync(int id)
        {
            return await _context.Prehrambenanamirnicas.FindAsync(id) ?? throw new KeyNotFoundException("Prehrambena namirnica nije pronađena.");
        }
    }
}
