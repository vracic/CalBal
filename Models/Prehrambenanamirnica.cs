using System;
using System.Collections.Generic;

namespace CalBal.Models;

public partial class Prehrambenanamirnica
{
    public int PrehrambenaNamirnicaId { get; set; }

    public string Naziv { get; set; } = null!;

    public decimal Kalorije { get; set; }

    public decimal? Proteini { get; set; }

    public decimal? Masti { get; set; }

    public decimal? Ugljikohidrati { get; set; }

    public virtual ICollection<Unosprehnam> Unosprehnams { get; set; } = new List<Unosprehnam>();
}
