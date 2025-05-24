using CalBal.Models;
using System.Threading.Tasks;

namespace CalBal.Services.Interfaces
{
    public interface IProvedbaTjAktService
    {
        Task<(bool Success, string? ErrorMessage)> AddAsync(Provedbatjakt model);
        Task<Provedbatjakt?> GetByIdAsync(int id);
        public Task<(bool Success, string? ErrorMessage)> UpdateAsync(Provedbatjakt model);
        Task DeleteAsync(Provedbatjakt model);
    }
}
