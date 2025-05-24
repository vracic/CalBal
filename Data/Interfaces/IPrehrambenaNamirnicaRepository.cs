using CalBal.Models;

namespace CalBal.Data.Interfaces
{
    public interface IPrehrambenaNamirnicaRepository
    {
        Task<List<Prehrambenanamirnica>> GetAllAsync();
        Task<Prehrambenanamirnica?> GetByIdAsync(int id);
        Task AddAsync(Prehrambenanamirnica namirnica);
        Task UpdateAsync(Prehrambenanamirnica namirnica);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
