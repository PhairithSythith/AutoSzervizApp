using AutoSzervizApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AutoSzervizApp.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Ugyfel> Ugyfelek => Set<Ugyfel>();
    public DbSet<Jarmu> Jarmuvek => Set<Jarmu>();
    public DbSet<JarmuTipus> JarmuTipusok => Set<JarmuTipus>();
    public DbSet<SzervizTipus> SzervizTipusok => Set<SzervizTipus>();
    public DbSet<Munkalap> Munkalapok => Set<Munkalap>();
    public DbSet<MunkalapTetel> MunkalapTetelek => Set<MunkalapTetel>();
    public DbSet<SzervizTipusJavaslat> SzervizTipusJavaslatok
    => Set<SzervizTipusJavaslat>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SzervizTipusJavaslat>()
            .HasOne(j => j.JavasloFelhasznalo)
            .WithMany()
            .HasForeignKey(j => j.JavasloFelhasznaloId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SzervizTipusJavaslat>()
            .HasOne(j => j.ElbiraloFelhasznalo)
            .WithMany()
            .HasForeignKey(j => j.ElbiraloFelhasznaloId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

