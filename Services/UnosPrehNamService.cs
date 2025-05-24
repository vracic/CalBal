using CalBal.Models;
using CalBal.Services.Interfaces;
using CalBal.Data.Interfaces;
using System.Threading.Tasks;

namespace CalBal.Services
{
    public class UnosPrehNamService : IUnosPrehNamService
    {
        private readonly IUnosPrehNamRepository _repository;
        public UnosPrehNamService(IUnosPrehNamRepository repository)
        {
            _repository = repository;
        }

        public async Task<(bool Success, string? ErrorMessage)> AddAsync(Unosprehnam model)
        {
            var dailyEntries = await _repository.GetDailyEntriesAsync(model.KorisnikId, model.Datum);
            decimal totalCalories = dailyEntries.Sum(u => u.Kolicina * (u.Hrana?.Kalorije ?? 0));
            var hrana = await _repository.GetPrehrambenaNamirnicaByIdAsync(model.HranaId);
            var thisCalories = model.Kolicina * hrana.Kalorije;

            if (totalCalories + thisCalories > 10000)
                return (false, "Maksimalni dnevni unos ne smije biti vise od 10,000 kcal.");

            await _repository.AddAsync(model);
            return (true, null);
        }

        public async Task<(bool Success, string? ErrorMessage)> UpdateAsync(Unosprehnam model)
        {
            var existing = await _repository.GetByIdAsync(model.UnosPrehNamId);
            if (existing == null)
                return (false, "Record not found.");

            var dailyEntries = await _repository.GetDailyEntriesAsync(model.KorisnikId, model.Datum, model.UnosPrehNamId);
            decimal totalCalories = dailyEntries.Sum(u => u.Kolicina * (u.Hrana?.Kalorije ?? 0));
            var hranaEntry = dailyEntries.FirstOrDefault(u => u.HranaId == model.HranaId);
            decimal thisCalories = model.Kolicina * (hranaEntry?.Hrana?.Kalorije ?? 0);

            if (totalCalories + thisCalories > 10000)
                return (false, "Maksimalni dnevni unos ne smije premasiti 10,000 kcal.");

            existing.Kolicina = model.Kolicina;
            existing.Datum = model.Datum;
            existing.HranaId = model.HranaId;

            await _repository.UpdateAsync(existing);
            return (true, null);
        }


        public Task<Unosprehnam?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);
        public Task DeleteAsync(Unosprehnam model) => _repository.DeleteAsync(model);

    }
}