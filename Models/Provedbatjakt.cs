using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CalBal.Models;

public partial class Provedbatjakt : IValidatableObject
{
    public int ProvedbaTjAktId { get; set; }

    [Range(1, 720, ErrorMessage = "Duration must be between 1 and 720 minutes (12 hours).")]
    public int Trajanje { get; set; }

    public DateOnly Datum { get; set; }

    public int KorisnikId { get; set; }

    public int AktivnostId { get; set; }

    [ValidateNever]
    public virtual Aktivnost Aktivnost { get; set; } = null!;

    [ValidateNever]
    public virtual Korisnik Korisnik { get; set; } = null!;

    [NotMapped]
    public decimal PotroseneKalorije { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var context = (CalbalContext)validationContext.GetService(typeof(CalbalContext));
        if (context == null)
            yield break;

        // Sum all durations for this user and date, excluding this record if updating
        var totalDuration = context.Provedbatjakts
            .Where(a => a.KorisnikId == this.KorisnikId && a.Datum == this.Datum && a.ProvedbaTjAktId != this.ProvedbaTjAktId)
            .Sum(a => (int?)a.Trajanje) ?? 0;

        if (totalDuration + this.Trajanje > 1440)
        {
            yield return new ValidationResult(
                "Total activity duration for the day cannot exceed 1,440 minutes (24 hours).",
                new[] { nameof(Trajanje) });
        }
    }
}
