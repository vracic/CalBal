using CalBal.Models;
using CalBal.Services.Interfaces;
using CalBal.Data.Interfaces;

namespace CalBal.Services
{
    public class AktivnostService : IAktivnostService
    {
        private readonly IAktivnostRepository _repository;

        public AktivnostService(IAktivnostRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Aktivnost>> DohvatiSveAsync() => _repository.GetAllAsync();
        public Task<Aktivnost?> DohvatiPoIdAsync(int id) => _repository.GetByIdAsync(id);
        public Task DodajAsync(Aktivnost aktivnost) => _repository.AddAsync(aktivnost);
        public Task UrediAsync(Aktivnost aktivnost) => _repository.UpdateAsync(aktivnost);
        public Task ObrisiAsync(int id) => _repository.DeleteAsync(id);
        public Task<bool> PostojiAsync(int id) => _repository.ExistsAsync(id);
    }
}
