using CalBal.Data.Interfaces;
using CalBal.Models;

namespace CalBal.Services.Interfaces
{
    public interface IKorisnikService
    {
        public Task<List<Korisnik>> DohvatiSveKorisnikeAsync();
        public Task<Korisnik?> DohvatiKorisnikaPoIdAsync(int id);
        public Task DodajKorisnikaAsync(Korisnik korisnik);
        public Task AzurirajKorisnikaAsync(Korisnik korisnik);
        public Task ObrisiKorisnikaAsync(Korisnik korisnik);
        public Task<bool> KorisnikPostojiAsync(int id);
        public Task<Korisnik> DohvatiKorisnikaSaDetaljimaFiltriranoAsync(int korisnikId, string activitySearch, string foodSearch);
        public decimal IzracunajPotroseneKalorije(Provedbatjakt p);
        public decimal IzracunajUneseneKalorije(Unosprehnam u);
        public Task<List<Aktivnost>> DohvatiSveAktivnostiAsync();
        public Task<List<Prehrambenanamirnica>> DohvatiSveNamirniceAsync();
    }
}
