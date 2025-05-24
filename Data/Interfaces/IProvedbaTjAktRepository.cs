using CalBal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalBal.Data.Interfaces
{
    public interface IProvedbaTjAktRepository
    {
        Task AddAsync(Provedbatjakt model);
        Task<Provedbatjakt?> GetByIdAsync(int id);
        Task UpdateAsync(Provedbatjakt model);
        Task DeleteAsync(Provedbatjakt model);
        Task<int> GetTotalDurationForDateExceptAsync(int korisnikId, DateOnly datum, int excludeId);
        Task<int> GetTotalDurationForDateAsync(int korisnikId, DateOnly datum);

    }
}
