using CalBal.Models;

namespace CalBal.Data.Interfaces
{
    public interface IAktivnostRepository
    {
        Task<List<Aktivnost>> GetAllAsync();
        Task<Aktivnost?> GetByIdAsync(int id);
        Task AddAsync(Aktivnost aktivnost);
        Task UpdateAsync(Aktivnost aktivnost);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
