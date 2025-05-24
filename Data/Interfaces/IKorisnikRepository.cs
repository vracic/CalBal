using CalBal.Models;

namespace CalBal.Data.Interfaces
{
    public interface IKorisnikRepository
    {
        Task<List<Korisnik>> GetAllAsync();
        Task<Korisnik?> GetByIdAsync(int id);
        Task AddAsync(Korisnik korisnik);
        Task UpdateAsync(Korisnik korisnik);
        Task DeleteAsync(Korisnik korisnik);
        Task<bool> ExistsAsync(int id);
        Task<Korisnik> GetKorisnikWithDetailsAsync(int korisnikId);
        Task<List<Aktivnost>> GetAllAktivnostiAsync();
        Task<List<Prehrambenanamirnica>> GetAllNamirniceAsync();
        Task<Korisnik?> GetByEmailAsync(string email);
        Task<bool> ExistsByEmailAsync(string email);
        Task SaveChangesAsync();
    }
}
