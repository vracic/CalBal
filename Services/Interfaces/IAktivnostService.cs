using CalBal.Data.Interfaces;
using CalBal.Models;

namespace CalBal.Services.Interfaces
{
    public interface IAktivnostService
    {
        public Task<List<Aktivnost>> DohvatiSveAsync();
        public Task<Aktivnost?> DohvatiPoIdAsync(int id);
        public Task DodajAsync(Aktivnost aktivnost);
        public Task UrediAsync(Aktivnost aktivnost);
        public Task ObrisiAsync(int id);
        public Task<bool> PostojiAsync(int id);
    }
}
