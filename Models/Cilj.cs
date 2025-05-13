using System;
using System.Collections.Generic;

namespace CalBal.Models;

public partial class Cilj
{
    public int CiljId { get; set; }

    public string Opis { get; set; } = null!;

    public decimal PocTezina { get; set; }

    public decimal CiljTezina { get; set; }

    public DateOnly DatumPostavljen { get; set; }

    public DateOnly? DatumZavrsen { get; set; }

    public virtual ICollection<Korisnikciljpreporuka> Korisnikciljpreporukas { get; set; } = new List<Korisnikciljpreporuka>();
}
