using CalBal.Models;
using System.Threading.Tasks;

namespace CalBal.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool IsSuccess, string? ErrorMessage)> RegisterAsync(Korisnik korisnik, string lozinka);
        Task<(bool IsSuccess, Korisnik? User)> LoginAsync(string email, string lozinka);
    }
}
