using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CalBal.Models;

public partial class Unosprehnam
{
    public int UnosPrehNamId { get; set; }

    public decimal Kolicina { get; set; }

    public DateOnly Datum { get; set; }

    public int KorisnikId { get; set; }

    public int HranaId { get; set; }

    [ValidateNever]
    public virtual Prehrambenanamirnica Hrana { get; set; } = null!;

    [ValidateNever]
    public virtual Korisnik Korisnik { get; set; } = null!;
}
