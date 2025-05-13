using System;
using System.Collections.Generic;

namespace CalBal.Models;

public partial class Preporuka
{
    public int PreporukaId { get; set; }

    public string Sadrzaj { get; set; } = null!;

    public decimal DnevniDeficitSuficit { get; set; }

    public virtual ICollection<Korisnikciljpreporuka> Korisnikciljpreporukas { get; set; } = new List<Korisnikciljpreporuka>();
}
