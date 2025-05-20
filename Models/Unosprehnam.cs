using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CalBal.Models;

public partial class Unosprehnam : IValidatableObject
{
    public int UnosPrehNamId { get; set; }

    [Range(1, 999.99, ErrorMessage = "Quantity must be between 1 and 999.99 (pieces or grams).")]
    public decimal Kolicina { get; set; }

    public DateOnly Datum { get; set; }

    public int KorisnikId { get; set; }

    public int HranaId { get; set; }

    [ValidateNever]
    public virtual Prehrambenanamirnica Hrana { get; set; } = null!;

    [ValidateNever]
    public virtual Korisnik Korisnik { get; set; } = null!;

    [NotMapped]
    public decimal UneseneKalorije { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var context = (CalbalContext)validationContext.GetService(typeof(CalbalContext));
        if (context == null)
            yield break;

        // Get all food entries for this user and date, excluding this record if updating
        var dailyEntries = context.Unosprehnams
            .Include(u => u.Hrana)
            .Where(u => u.KorisnikId == this.KorisnikId && u.Datum == this.Datum && u.UnosPrehNamId != this.UnosPrehNamId)
            .ToList();

        // Calculate total calories for the day (excluding this entry)
        decimal totalCalories = dailyEntries.Sum(u => u.Kolicina * (u.Hrana?.Kalorije ?? 0));

        var hrana = context.Prehrambenanamirnicas.Find(this.HranaId);
        // Add calories for the current entry
        var thisCalories = this.Kolicina * (hrana?.Kalorije ?? 0);
        if (totalCalories + thisCalories > 10000)
        {
            yield return new ValidationResult(
                "Total daily calorie intake cannot exceed 10,000 kcal.",
                new[] { nameof(Kolicina) });
        }
    }
}
