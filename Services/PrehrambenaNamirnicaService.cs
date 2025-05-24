using CalBal.Data.Interfaces;
using CalBal.Models;
using CalBal.Services.Interfaces;

namespace CalBal.Services
{
    public class PrehrambenaNamirnicaService : IPrehrambenaNamirnicaService
    {
        private readonly IPrehrambenaNamirnicaRepository _repository;

        public PrehrambenaNamirnicaService(IPrehrambenaNamirnicaRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Prehrambenanamirnica>> DohvatiSveAsync() => _repository.GetAllAsync();
        public Task<Prehrambenanamirnica?> DohvatiPoIdAsync(int id) => _repository.GetByIdAsync(id);
        public Task DodajAsync(Prehrambenanamirnica namirnica) => _repository.AddAsync(namirnica);
        public Task UrediAsync(Prehrambenanamirnica namirnica) => _repository.UpdateAsync(namirnica);
        public Task ObrisiAsync(int id) => _repository.DeleteAsync(id);
        public Task<bool> PostojiAsync(int id) => _repository.ExistsAsync(id);
    }
}
