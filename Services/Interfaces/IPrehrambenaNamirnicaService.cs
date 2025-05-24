using CalBal.Models;
using NuGet.Protocol.Core.Types;

namespace CalBal.Services.Interfaces
{
    public interface IPrehrambenaNamirnicaService
    {
        public Task<List<Prehrambenanamirnica>> DohvatiSveAsync();
        public Task<Prehrambenanamirnica?> DohvatiPoIdAsync(int id);
        public Task DodajAsync(Prehrambenanamirnica namirnica);
        public Task UrediAsync(Prehrambenanamirnica namirnica);
        public Task ObrisiAsync(int id);
        public Task<bool> PostojiAsync(int id);
    }
}
