using CalBal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalBal.Data.Interfaces
{
    public interface IUnosPrehNamRepository
    {
        Task AddAsync(Unosprehnam model);
        Task<Unosprehnam?> GetByIdAsync(int id);
        Task UpdateAsync(Unosprehnam model);
        Task DeleteAsync(Unosprehnam model);
        Task<List<Unosprehnam>> GetDailyEntriesAsync(int korisnikId, DateOnly datum, int? excludeId = null);
        Task<Prehrambenanamirnica> GetPrehrambenaNamirnicaByIdAsync(int id);
    }
}
