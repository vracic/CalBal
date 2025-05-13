using CalBal.Models.Enums;
using System;
using System.Collections.Generic;

namespace CalBal.Models;

public partial class Korisnik
{
    public int KorisnikId { get; set; }

    public string Ime { get; set; } = null!;

    public string Prezime { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Lozinka { get; set; } = null!;

    public DateOnly? DatumRodenja { get; set; }

    public decimal? Visina { get; set; }

    public decimal? Tezina { get; set; }

    public RazinaOvlasti RazinaOvlasti { get; set; }

    public virtual ICollection<Korisnikciljpreporuka> Korisnikciljpreporukas { get; set; } = new List<Korisnikciljpreporuka>();

    public virtual ICollection<Provedbatjakt> Provedbatjakts { get; set; } = new List<Provedbatjakt>();

    public virtual ICollection<Unosprehnam> Unosprehnams { get; set; } = new List<Unosprehnam>();
}
