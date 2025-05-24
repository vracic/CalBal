using CalBal.Models;
using CalBal.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalBal.Data
{
    public class KorisnikRepository : IKorisnikRepository
    {
        private readonly CalbalContext _context;

        public KorisnikRepository(CalbalContext context)
        {
            _context = context;
        }

        public async Task<List<Korisnik>> GetAllAsync()
        {
            return await _context.Korisniks.ToListAsync();
        }

        public async Task<Korisnik?> GetByIdAsync(int id)
        {
            return await _context.Korisniks.FindAsync(id);
        }

        public async Task AddAsync(Korisnik korisnik)
        {
            _context.Add(korisnik);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Korisnik korisnik)
        {
            _context.Update(korisnik);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Korisnik korisnik)
        {
            _context.Remove(korisnik);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id) =>
            await _context.Korisniks.AnyAsync(e => e.KorisnikId == id);

        public async Task<Korisnik> GetKorisnikWithDetailsAsync(int korisnikId) =>
            await _context.Korisniks
                .Include(k => k.Provedbatjakts)
                    .ThenInclude(p => p.Aktivnost)
                .Include(k => k.Unosprehnams)
                    .ThenInclude(u => u.Hrana)
                .FirstOrDefaultAsync(k => k.KorisnikId == korisnikId);

        public async Task<List<Aktivnost>> GetAllAktivnostiAsync() =>
            await _context.Aktivnosts.ToListAsync();

        public async Task<List<Prehrambenanamirnica>> GetAllNamirniceAsync() => 
            await _context.Prehrambenanamirnicas.ToListAsync();

        public async Task<Korisnik?> GetByEmailAsync(string email) =>
            await _context.Korisniks.FirstOrDefaultAsync(k => k.Email == email);

        public async Task<bool> ExistsByEmailAsync(string email) =>
            await _context.Korisniks.AnyAsync(k => k.Email == email);

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();


    }
}

