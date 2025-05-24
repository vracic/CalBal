using CalBal.Models;
using CalBal.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

namespace CalBal.Services
{
    public class KorisnikService
    {
        private readonly IKorisnikRepository _repository;

        public KorisnikService(IKorisnikRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Korisnik>> DohvatiSveKorisnikeAsync() => _repository.GetAllAsync();

        public Task<Korisnik?> DohvatiKorisnikaPoIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task DodajKorisnikaAsync(Korisnik korisnik) => _repository.AddAsync(korisnik);

        public Task AzurirajKorisnikaAsync(Korisnik korisnik) => _repository.UpdateAsync(korisnik);

        public Task ObrisiKorisnikaAsync(Korisnik korisnik) => _repository.DeleteAsync(korisnik);

        public Task<bool> KorisnikPostojiAsync(int id) => _repository.ExistsAsync(id);

        public async Task<Korisnik> DohvatiKorisnikaSaDetaljimaFiltriranoAsync(int korisnikId, string activitySearch, string foodSearch)
        {
            var korisnik = await _repository.GetKorisnikWithDetailsAsync(korisnikId);
            if (korisnik == null)
                return null;

            if (!string.IsNullOrWhiteSpace(activitySearch))
            {
                korisnik.Provedbatjakts = korisnik.Provedbatjakts
                    .Where(p => p.Aktivnost != null &&
                                p.Aktivnost.Naziv.Contains(activitySearch, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(foodSearch))
            {
                korisnik.Unosprehnams = korisnik.Unosprehnams
                    .Where(u => u.Hrana != null &&
                                u.Hrana.Naziv.Contains(foodSearch, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            foreach (var p in korisnik.Provedbatjakts)
                p.PotroseneKalorije = IzracunajPotroseneKalorije(p);

            foreach (var u in korisnik.Unosprehnams)
                u.UneseneKalorije = IzracunajUneseneKalorije(u);

            return korisnik;
        }

        public decimal IzracunajPotroseneKalorije(Provedbatjakt p)
        {
            return p.Trajanje * (p.Aktivnost?.Potrosnja ?? 0) / 60.0m;
        }

        public decimal IzracunajUneseneKalorije(Unosprehnam u)
        {
            return u.Kolicina * (u.Hrana?.Kalorije ?? 0);
        }

        public async Task<List<Aktivnost>> DohvatiSveAktivnostiAsync()
        {
            return await _repository.GetAllAktivnostiAsync();
        }

        public async Task<List<Prehrambenanamirnica>> DohvatiSveNamirniceAsync()
        {
            return await _repository.GetAllNamirniceAsync();
        }
    }

}