using System;
using System.Collections.Generic;
using CalBal.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


namespace CalBal.Models;

public partial class CalbalContext : DbContext
{
    private readonly IConfiguration _configuration;

    public CalbalContext(DbContextOptions<CalbalContext> options, IConfiguration configuration)
    : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<Aktivnost> Aktivnosts { get; set; }

    public virtual DbSet<Cilj> Ciljs { get; set; }

    public virtual DbSet<Korisnik> Korisniks { get; set; }

    public virtual DbSet<Korisnikciljpreporuka> Korisnikciljpreporukas { get; set; }

    public virtual DbSet<Prehrambenanamirnica> Prehrambenanamirnicas { get; set; }

    public virtual DbSet<Preporuka> Preporukas { get; set; }

    public virtual DbSet<Provedbatjakt> Provedbatjakts { get; set; }

    public virtual DbSet<Unosprehnam> Unosprehnams { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum("razina_enum", new[] { "niza", "srednja", "visa", "niska", "visoka" });

        modelBuilder.Entity<Aktivnost>(entity =>
        {
            entity.HasKey(e => e.AktivnostId).HasName("aktivnost_pkey");

            entity.ToTable("aktivnost");

            entity.Property(e => e.AktivnostId).HasColumnName("aktivnost_id");
            entity.Property(e => e.Naziv)
                .HasMaxLength(50)
                .HasColumnName("naziv");
            entity.Property(e => e.Potrosnja)
                .HasPrecision(5, 2)
                .HasColumnName("potrosnja");
        });

        modelBuilder.Entity<Cilj>(entity =>
        {
            entity.HasKey(e => e.CiljId).HasName("cilj_pkey");

            entity.ToTable("cilj");

            entity.Property(e => e.CiljId).HasColumnName("cilj_id");
            entity.Property(e => e.CiljTezina)
                .HasPrecision(5, 2)
                .HasColumnName("cilj_tezina");
            entity.Property(e => e.DatumPostavljen).HasColumnName("datum_postavljen");
            entity.Property(e => e.DatumZavrsen).HasColumnName("datum_zavrsen");
            entity.Property(e => e.Opis)
                .HasMaxLength(100)
                .HasColumnName("opis");
            entity.Property(e => e.PocTezina)
                .HasPrecision(5, 2)
                .HasColumnName("poc_tezina");
        });

        modelBuilder.Entity<Korisnik>(entity =>
        {
            entity.HasKey(e => e.KorisnikId).HasName("korisnik_pkey");

            entity.ToTable("korisnik");

            entity.HasIndex(e => e.Email, "korisnik_email_key").IsUnique();

            entity.Property(e => e.KorisnikId).HasColumnName("korisnik_id");
            entity.Property(e => e.DatumRodenja).HasColumnName("datum_rodenja");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Ime)
                .HasMaxLength(50)
                .HasColumnName("ime");
            entity.Property(e => e.Lozinka)
                .HasMaxLength(255)
                .HasColumnName("lozinka");
            entity.Property(e => e.Prezime)
                .HasMaxLength(50)
                .HasColumnName("prezime");
            entity.Property(e => e.Tezina)
                .HasPrecision(5, 2)
                .HasColumnName("tezina");
            entity.Property(e => e.Visina)
                .HasPrecision(5, 2)
                .HasColumnName("visina");
            entity.Property(e => e.RazinaOvlasti)
                .HasColumnName("razina_ovlasti")
                .HasColumnType("razina_enum");
        });

        modelBuilder.Entity<Korisnikciljpreporuka>(entity =>
        {
            entity.HasKey(e => e.KorisnikCiljPreporukaId).HasName("korisnikciljpreporuka_pkey");

            entity.ToTable("korisnikciljpreporuka");

            entity.Property(e => e.KorisnikCiljPreporukaId).HasColumnName("korisnik_cilj_preporuka_id");
            entity.Property(e => e.CiljId).HasColumnName("cilj_id");
            entity.Property(e => e.Datum).HasColumnName("datum");
            entity.Property(e => e.KorisnikId).HasColumnName("korisnik_id");
            entity.Property(e => e.PreporukaId).HasColumnName("preporuka_id");

            entity.HasOne(d => d.Cilj).WithMany(p => p.Korisnikciljpreporukas)
                .HasForeignKey(d => d.CiljId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("korisnikciljpreporuka_cilj_id_fkey");

            entity.HasOne(d => d.Korisnik).WithMany(p => p.Korisnikciljpreporukas)
                .HasForeignKey(d => d.KorisnikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("korisnikciljpreporuka_korisnik_id_fkey");

            entity.HasOne(d => d.Preporuka).WithMany(p => p.Korisnikciljpreporukas)
                .HasForeignKey(d => d.PreporukaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("korisnikciljpreporuka_preporuka_id_fkey");
        });

        modelBuilder.Entity<Prehrambenanamirnica>(entity =>
        {
            entity.HasKey(e => e.PrehrambenaNamirnicaId).HasName("prehrambenanamirnica_pkey");

            entity.ToTable("prehrambenanamirnica");

            entity.Property(e => e.PrehrambenaNamirnicaId).HasColumnName("prehrambena_namirnica_id");
            entity.Property(e => e.Kalorije)
                .HasPrecision(6, 2)
                .HasColumnName("kalorije");
            entity.Property(e => e.Masti)
                .HasPrecision(5, 2)
                .HasColumnName("masti");
            entity.Property(e => e.Naziv)
                .HasMaxLength(50)
                .HasColumnName("naziv");
            entity.Property(e => e.Proteini)
                .HasPrecision(5, 2)
                .HasColumnName("proteini");
            entity.Property(e => e.Ugljikohidrati)
                .HasPrecision(5, 2)
                .HasColumnName("ugljikohidrati");
        });

        modelBuilder.Entity<Preporuka>(entity =>
        {
            entity.HasKey(e => e.PreporukaId).HasName("preporuka_pkey");

            entity.ToTable("preporuka");

            entity.Property(e => e.PreporukaId).HasColumnName("preporuka_id");
            entity.Property(e => e.DnevniDeficitSuficit)
                .HasPrecision(6, 2)
                .HasColumnName("dnevni_deficit_suficit");
            entity.Property(e => e.Sadrzaj)
                .HasMaxLength(100)
                .HasColumnName("sadrzaj");
        });

        modelBuilder.Entity<Provedbatjakt>(entity =>
        {
            entity.HasKey(e => e.ProvedbaTjAktId).HasName("provedbatjakt_pkey");

            entity.ToTable("provedbatjakt");

            entity.Property(e => e.ProvedbaTjAktId).HasColumnName("provedba_tj_akt_id");
            entity.Property(e => e.AktivnostId).HasColumnName("aktivnost_id");
            entity.Property(e => e.Datum).HasColumnName("datum");
            entity.Property(e => e.KorisnikId).HasColumnName("korisnik_id");
            entity.Property(e => e.Trajanje).HasColumnName("trajanje");

            entity.HasOne(d => d.Aktivnost).WithMany(p => p.Provedbatjakts)
                .HasForeignKey(d => d.AktivnostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("provedbatjakt_aktivnost_id_fkey");

            entity.HasOne(d => d.Korisnik).WithMany(p => p.Provedbatjakts)
                .HasForeignKey(d => d.KorisnikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("provedbatjakt_korisnik_id_fkey");
        });

        modelBuilder.Entity<Unosprehnam>(entity =>
        {
            entity.HasKey(e => e.UnosPrehNamId).HasName("unosprehnam_pkey");

            entity.ToTable("unosprehnam");

            entity.Property(e => e.UnosPrehNamId).HasColumnName("unos_preh_nam_id");
            entity.Property(e => e.Datum).HasColumnName("datum");
            entity.Property(e => e.HranaId).HasColumnName("hrana_id");
            entity.Property(e => e.Kolicina)
                .HasPrecision(6, 2)
                .HasColumnName("kolicina");
            entity.Property(e => e.KorisnikId).HasColumnName("korisnik_id");

            entity.HasOne(d => d.Hrana).WithMany(p => p.Unosprehnams)
                .HasForeignKey(d => d.HranaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("unosprehnam_hrana_id_fkey");

            entity.HasOne(d => d.Korisnik).WithMany(p => p.Unosprehnams)
                .HasForeignKey(d => d.KorisnikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("unosprehnam_korisnik_id_fkey");
        });

        base.OnModelCreating(modelBuilder);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
