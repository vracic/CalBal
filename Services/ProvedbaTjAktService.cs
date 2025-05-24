using CalBal.Models;
using CalBal.Data.Interfaces;
using System.Threading.Tasks;

namespace CalBal.Services
{
    public class ProvedbaTjAktService
    {
        private readonly IProvedbaTjAktRepository _repository;

        public ProvedbaTjAktService(IProvedbaTjAktRepository repository)
        {
            _repository = repository;
        }

        public async Task<(bool Success, string? ErrorMessage)> AddAsync(Provedbatjakt model)
        {
            // Dohvati trenutno trajanje za korisnika i datum
            var totalDuration = await _repository.GetTotalDurationForDateAsync(model.KorisnikId, model.Datum);

            if (totalDuration + model.Trajanje > 1440)
                return (false, "Ukupno trajanje aktivnosti za dan ne smije prelaziti 1440 minuta (24 sata).");

            await _repository.AddAsync(model);
            return (true, null);
        }

        public Task<Provedbatjakt?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public async Task<(bool Success, string? ErrorMessage)> UpdateAsync(Provedbatjakt model)
        {
            var existing = await _repository.GetByIdAsync(model.ProvedbaTjAktId);
            if (existing == null)
                return (false, "Zapis nije pronađen.");

            if (existing.KorisnikId != model.KorisnikId)
                return (false, "Nemate ovlasti za uređivanje ovog zapisa.");

            var totalDuration = await _repository.GetTotalDurationForDateExceptAsync(model.KorisnikId, model.Datum, model.ProvedbaTjAktId);

            if (totalDuration + model.Trajanje > 1440)
                return (false, "Ukupno trajanje aktivnosti za dan ne smije prelaziti 1440 minuta (24 sata).");

            existing.Trajanje = model.Trajanje;
            existing.Datum = model.Datum;
            existing.AktivnostId = model.AktivnostId;

            await _repository.UpdateAsync(existing);
            return (true, null);
        }

        public Task DeleteAsync(Provedbatjakt model) => _repository.DeleteAsync(model);
    }
}
