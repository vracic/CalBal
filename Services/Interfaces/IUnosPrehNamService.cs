using CalBal.Models;
using System.Threading.Tasks;

namespace CalBal.Services.Interfaces
{
    public interface IUnosPrehNamService
    {
        Task<(bool Success, string? ErrorMessage)> AddAsync(Unosprehnam model);
        Task<(bool Success, string? ErrorMessage)> UpdateAsync(Unosprehnam model);
        Task<Unosprehnam?> GetByIdAsync(int id);
        Task DeleteAsync(Unosprehnam model);
    }
}
