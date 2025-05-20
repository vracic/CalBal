// Services/KorisnikService.cs
using CalBal.Models;

namespace CalBal.Services
{
    public class KorisnikService
    {
        public decimal IzracunajPotroseneKalorije(Provedbatjakt p)
        {
            // (Trajanje u minutama * potrosnja po satu) / 60
            return p.Trajanje * (p.Aktivnost?.Potrosnja ?? 0) / 60.0m;
        }

        public decimal IzracunajUneseneKalorije(Unosprehnam u)
        {
            // Kolicina * kalorije po jedinici
            return u.Kolicina * (u.Hrana?.Kalorije ?? 0);
        }
    }
}