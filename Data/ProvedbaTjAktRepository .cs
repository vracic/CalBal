using CalBal.Models;
using CalBal.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CalBal.Data
{
    public class ProvedbaTjAktRepository : IProvedbaTjAktRepository
    {
        private readonly CalbalContext _context;

        public ProvedbaTjAktRepository(CalbalContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Provedbatjakt model)
        {
            await _context.Provedbatjakts.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task<Provedbatjakt?> GetByIdAsync(int id)
        {
            return await _context.Provedbatjakts.FindAsync(id);
        }

        public async Task UpdateAsync(Provedbatjakt model)
        {
            _context.Provedbatjakts.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Provedbatjakt model)
        {
            _context.Provedbatjakts.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetTotalDurationForDateExceptAsync(int korisnikId, DateOnly datum, int excludeId)
        {
            return await _context.Provedbatjakts
                .Where(a => a.KorisnikId == korisnikId && a.Datum == datum && a.ProvedbaTjAktId != excludeId)
                .SumAsync(a => (int?)a.Trajanje) ?? 0;
        }

        public async Task<int> GetTotalDurationForDateAsync(int korisnikId, DateOnly datum)
        {
            return await _context.Provedbatjakts
                .Where(a => a.KorisnikId == korisnikId && a.Datum == datum)
                .SumAsync(a => (int?)a.Trajanje) ?? 0;
        }

    }
}
