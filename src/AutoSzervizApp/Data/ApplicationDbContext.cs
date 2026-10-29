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
}