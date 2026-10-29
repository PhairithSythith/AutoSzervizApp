using Microsoft.AspNetCore.Identity;
using AutoSzervizApp.Models;

namespace AutoSzervizApp.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(
        ApplicationDbContext context,
        RoleManager<IdentityRole> szerepkorKezelo)
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

        if (context.SzervizTipusok.Any())
        {
            return;
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