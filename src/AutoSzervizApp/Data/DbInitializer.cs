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

        var ujAdmin = await felhasznaloKezelo.FindByEmailAsync("admin2@gmail.com");

        if (ujAdmin == null)
        {
            ujAdmin = new IdentityUser
            {
                UserName = "admin2@gmail.com",
                Email = "admin2@gmail.com",
                EmailConfirmed = true
            };

            var eredmeny = await felhasznaloKezelo.CreateAsync(
                ujAdmin,
                "AdminUj123!"
            );

            if (!eredmeny.Succeeded)
            {
                throw new Exception(
                    string.Join("; ", eredmeny.Errors.Select(e => e.Description))
                );
            }
        }

        if (!await felhasznaloKezelo.IsInRoleAsync(ujAdmin, "Admin"))
        {
            var szerepkorEredmeny =
                await felhasznaloKezelo.AddToRoleAsync(ujAdmin, "Admin");

            if (!szerepkorEredmeny.Succeeded)
            {
                throw new Exception(
                    string.Join("; ", szerepkorEredmeny.Errors.Select(e => e.Description))
                );
            }
        }

        Console.WriteLine("Új adminfiók ellenőrizve!");
        if (!await felhasznaloKezelo.IsInRoleAsync(ujAdmin, "Admin"))
        {
            await felhasznaloKezelo.AddToRoleAsync(ujAdmin, "Admin");
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