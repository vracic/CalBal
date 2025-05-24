using CalBal.Models;
using System.Collections.Generic;

namespace CalBal.ViewModels
{
    public class KorisnikMasterDetailViewModel
    {
        public Korisnik Korisnik { get; set; }

        public List<Aktivnost> SveAktivnosti { get; set; }

        public List<Prehrambenanamirnica> SveNamirnice { get; set; }

        public string ActivitySearch { get; set; }

        public string FoodSearch { get; set; }
    }
}
