using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;

namespace CalBal.Models;

public partial class Provedbatjakt
{
    public int ProvedbaTjAktId { get; set; }

    public int Trajanje { get; set; }

    public DateOnly Datum { get; set; }

    public int KorisnikId { get; set; }

    public int AktivnostId { get; set; }

    [ValidateNever]
    public virtual Aktivnost Aktivnost { get; set; } = null!;

    [ValidateNever]
    public virtual Korisnik Korisnik { get; set; } = null!;
}
