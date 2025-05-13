using System;
using System.Collections.Generic;

namespace CalBal.Models;

public partial class Korisnikciljpreporuka
{
    public int KorisnikCiljPreporukaId { get; set; }

    public DateOnly Datum { get; set; }

    public int KorisnikId { get; set; }

    public int CiljId { get; set; }

    public int PreporukaId { get; set; }

    public virtual Cilj Cilj { get; set; } = null!;

    public virtual Korisnik Korisnik { get; set; } = null!;

    public virtual Preporuka Preporuka { get; set; } = null!;
}
