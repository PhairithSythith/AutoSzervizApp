using Microsoft.AspNetCore.Identity;
using AutoSzervizApp.Models;

namespace AutoSzervizApp.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(
    ApplicationDbContext context,
    RoleManager<IdentityRole> szerepkorKezelo,
    UserManager<IdentityUser> felhasznaloKezelo)
    {
        string[] szerepkorok = { "Admin", "Employee" };

        foreach (string szerepkor in szerepkorok)
        {
            if (!await szerepkorKezelo.RoleExistsAsync(szerepkor))
            {
                await szerepkorKezelo.CreateAsync(
                    new IdentityRole(szerepkor));
            }
        }

        var admin = await felhasznaloKezelo.FindByEmailAsync("admin@gmail.com");

        if (admin != null)
        {
            var token = await felhasznaloKezelo.GeneratePasswordResetTokenAsync(admin);
            var resetEredmeny = await felhasznaloKezelo.ResetPasswordAsync(admin, token, "AdminJelszo123!");

            if (!resetEredmeny.Succeeded)
            {
                throw new Exception(string.Join("; ", resetEredmeny.Errors.Select(e => e.Description)));
            }

            if (!await felhasznaloKezelo.IsInRoleAsync(admin, "Admin"))
            {
                await felhasznaloKezelo.AddToRoleAsync(admin, "Admin");
            }
        }

        var szervizTipusok = new[]
        {
            new SzervizTipus
            {
                Nev = "Olajcsere",
                Leiras = "Motorolaj és szűrők cseréje",
                Alapar = 25000
            },
            new SzervizTipus
            {
                Nev = "Fékjavítás",
                Leiras = "Fékbetétek és tárcsák ellenőrzése, cseréje",
                Alapar = 45000
            },
            new SzervizTipus
            {
                Nev = "Műszaki vizsga",
                Leiras = "Műszaki vizsgára való felkészítés és vizsgáztatás",
                Alapar = 35000
            }
        };

        await context.SzervizTipusok.AddRangeAsync(szervizTipusok);
        await context.SaveChangesAsync();
            }
}