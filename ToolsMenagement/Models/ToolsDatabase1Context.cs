using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ToolsMenagement.Models;

namespace ToolsMenagement.Models;

public partial class ToolsDatabase1Context : DbContext
{
    public ToolsDatabase1Context()
    {
    }

    public ToolsDatabase1Context(DbContextOptions<ToolsDatabase1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Kategorium> Kategoria { get; set; }

    public virtual DbSet<Magazyn> Magazyns { get; set; }

    public virtual DbSet<NarzedziaTechnologium> NarzedziaTechnologia { get; set; }

    public virtual DbSet<Narzedzie> Narzedzies { get; set; }

    public virtual DbSet<Rejestracja> Rejestracjas { get; set; }

    public virtual DbSet<Technologium> Technologia { get; set; }

    public virtual DbSet<Zlecenie> Zlecenies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-IKCEP9LL\\SQLEXPRESS;Initial Catalog=Tools_Database1;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Kategorium>(entity =>
        {
            entity.HasKey(e => e.IdKategorii).HasName("PK__Kategori__E2A569286A5C94E6");

            entity.Property(e => e.IdKategorii).HasColumnName("Id_Kategorii");
            entity.Property(e => e.MaterialWykonania).HasColumnName("Material_wykonania");
            entity.Property(e => e.Opis).HasMaxLength(32);
            entity.Property(e => e.Przeznaczenie).HasMaxLength(32);
        });

        modelBuilder.Entity<Magazyn>(entity =>
        {
            entity.HasKey(e => e.PozycjaMagazynowa).HasName("PK__Magazyn__78B0461F242BAAD7");

            entity.ToTable("Magazyn");

            entity.Property(e => e.PozycjaMagazynowa).HasColumnName("Pozycja_magazynowa");
            entity.Property(e => e.CyklRegeneracji).HasColumnName("Cykl_regeneracji");
            entity.Property(e => e.IdNarzedzia).HasColumnName("Id_narzedzia");

            entity.HasOne(d => d.IdNarzedziaNavigation).WithMany(p => p.Magazyns)
                .HasForeignKey(d => d.IdNarzedzia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rST_MagazynNarzedzie");
        });

        modelBuilder.Entity<NarzedziaTechnologium>(entity =>
        {
            entity.HasKey(e => new { e.IdNarzedzia, e.IdTechnologi }).HasName("PK__Narzedzi__6ED7F3184B816AF5");

            entity.ToTable("Narzedzia_Technologia");

            entity.Property(e => e.IdNarzedzia).HasColumnName("Id_narzedzia");
            entity.Property(e => e.IdTechnologi).HasColumnName("Id_technologi");
            entity.Property(e => e.CzasPracy).HasColumnName("Czas_pracy");

            entity.HasOne(d => d.IdNarzedziaNavigation).WithMany(p => p.NarzedziaTechnologia)
                .HasForeignKey(d => d.IdNarzedzia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rST_Narzedzia_TechnologiaNarzedzie");

            entity.HasOne(d => d.IdTechnologiNavigation).WithMany(p => p.NarzedziaTechnologia)
                .HasForeignKey(d => d.IdTechnologi)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rST_Narzedzia_TechnologiaTechnologia");
        });

        modelBuilder.Entity<Narzedzie>(entity =>
        {
            entity.HasKey(e => e.IdNarzedzia).HasName("PK__Narzedzi__B019BBBC628096CC");

            entity.ToTable("Narzedzie");

            entity.Property(e => e.IdNarzedzia).HasColumnName("Id_narzedzia");
            entity.Property(e => e.IdKategorii).HasColumnName("Id_Kategorii");
            entity.Property(e => e.Nazwa).HasMaxLength(64);

            entity.HasOne(d => d.IdKategoriiNavigation).WithMany(p => p.Narzedzies)
                .HasForeignKey(d => d.IdKategorii)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rST_NarzedzieKategoria");
        });

        modelBuilder.Entity<Rejestracja>(entity =>
        {
            entity.HasKey(e => e.IdPozycji).HasName("PK__Rejestra__04BB3C818C437FEF");

            entity.ToTable("Rejestracja");

            entity.Property(e => e.IdPozycji).HasColumnName("Id_pozycji");
            entity.Property(e => e.DataWykonania)
                .HasColumnType("date")
                .HasColumnName("Data_wykonania");
            entity.Property(e => e.IdZlecenia).HasColumnName("Id_zlecenia");
            entity.Property(e => e.Wykonal).HasMaxLength(1);

            entity.HasOne(d => d.IdZleceniaNavigation).WithMany(p => p.Rejestracjas)
                .HasForeignKey(d => d.IdZlecenia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rST_ZlecenieRejestracja");
        });

        modelBuilder.Entity<Technologium>(entity =>
        {
            entity.HasKey(e => e.IdTechnologi).HasName("PK__Technolo__ECE48A4CFDB3801A");

            entity.Property(e => e.IdTechnologi).HasColumnName("Id_technologi");
            entity.Property(e => e.DataUtworzenia)
                .HasColumnType("datetime")
                .HasColumnName("Data_utworzenia");
            entity.Property(e => e.Opis).HasMaxLength(100);
        });

        modelBuilder.Entity<Zlecenie>(entity =>
        {
            entity.HasKey(e => e.IdZlecenia).HasName("PK__Zlecenie__4FF1DFCEB8CCD08B");

            entity.ToTable("Zlecenie");

            entity.Property(e => e.IdZlecenia).HasColumnName("Id_zlecenia");
            entity.Property(e => e.IdTechnologi).HasColumnName("Id_technologi");

            entity.HasOne(d => d.IdTechnologiNavigation).WithMany(p => p.Zlecenies)
                .HasForeignKey(d => d.IdTechnologi)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rST_ZlecenieTechnologia");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
