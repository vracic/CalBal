using CalBal.Models;
using CalBal.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using CalBal.Services.Interfaces;

public class AuthService : IAuthService
{
    private readonly IKorisnikRepository _repository;
    private readonly PasswordHasher<Korisnik> _hasher = new();

    public AuthService(IKorisnikRepository repository)
    {
        _repository = repository;
    }

    public async Task<(bool IsSuccess, string? ErrorMessage)> RegisterAsync(Korisnik korisnik, string lozinka)
    {
        if (await _repository.ExistsByEmailAsync(korisnik.Email))
            return (false, "Email already registered.");

        korisnik.Lozinka = _hasher.HashPassword(korisnik, lozinka);
        korisnik.RazinaOvlasti = CalBal.Models.Enums.RazinaOvlasti.niska;

        await _repository.AddAsync(korisnik);
        await _repository.SaveChangesAsync();

        return (true, null);
    }

    public async Task<(bool IsSuccess, Korisnik? User)> LoginAsync(string email, string lozinka)
    {
        var korisnik = await _repository.GetByEmailAsync(email);
        if (korisnik == null)
            return (false, null);

        var result = _hasher.VerifyHashedPassword(korisnik, korisnik.Lozinka, lozinka);
        if (result == PasswordVerificationResult.Failed)
            return (false, null);

        return (true, korisnik);
    }
}
