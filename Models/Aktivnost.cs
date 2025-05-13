using System;
using System.Collections.Generic;

namespace CalBal.Models;

public partial class Aktivnost
{
    public int AktivnostId { get; set; }

    public string Naziv { get; set; } = null!;

    public decimal Potrosnja { get; set; }

    public virtual ICollection<Provedbatjakt> Provedbatjakts { get; set; } = new List<Provedbatjakt>();
}
