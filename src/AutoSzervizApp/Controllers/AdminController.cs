using AutoSzervizApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoSzervizApp.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<IdentityUser> _felhasznaloKezelo;

    public AdminController(UserManager<IdentityUser> felhasznaloKezelo)
    {
        _felhasznaloKezelo = felhasznaloKezelo;
    }

    public async Task<IActionResult> Index()
    {
        var felhasznalok = await _felhasznaloKezelo.Users
            .OrderBy(f => f.Email)
            .ToListAsync();

        var lista = new List<AdminFelhasznaloViewModel>();

        foreach (var felhasznalo in felhasznalok)
        {
            var szerepkorok =
                await _felhasznaloKezelo.GetRolesAsync(felhasznalo);

            lista.Add(new AdminFelhasznaloViewModel
            {
                Id = felhasznalo.Id,
                Email = felhasznalo.Email,
                Szerepkorok = string.Join(", ", szerepkorok)
            });
        }

        return View(lista);
    }
    [HttpGet]
    public IActionResult UjFelhasznalo()
    {
        return View(new AdminUjFelhasznaloViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UjFelhasznalo(
        AdminUjFelhasznaloViewModel modell)
    {
        if (!ModelState.IsValid)
        {
            return View(modell);
        }

        if (modell.Szerepkor != "Admin" &&
            modell.Szerepkor != "Employee")
        {
            ModelState.AddModelError("Szerepkor", "Érvénytelen szerepkör.");
            return View(modell);
        }

        var felhasznalo = new IdentityUser
        {
            UserName = modell.Email,
            Email = modell.Email,
            EmailConfirmed = true
        };

        var letrehozasEredmenye =
            await _felhasznaloKezelo.CreateAsync(felhasznalo, modell.Jelszo);

        if (!letrehozasEredmenye.Succeeded)
        {
            foreach (var hiba in letrehozasEredmenye.Errors)
            {
                ModelState.AddModelError(string.Empty, hiba.Description);
            }

            return View(modell);
        }

        var szerepkorEredmenye =
            await _felhasznaloKezelo.AddToRoleAsync(
                felhasznalo, modell.Szerepkor);

        if (!szerepkorEredmenye.Succeeded)
        {
            await _felhasznaloKezelo.DeleteAsync(felhasznalo);

            foreach (var hiba in szerepkorEredmenye.Errors)
            {
                ModelState.AddModelError(string.Empty, hiba.Description);
            }

            return View(modell);
        }

        TempData["Siker"] = "Az új dolgozói fiók sikeresen létrejött.";

        return RedirectToAction(nameof(Index));
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SzerepkorModositas(
        string id,
        string ujSzerepkor)
    {
        var felhasznalo = await _felhasznaloKezelo.FindByIdAsync(id);

        if (felhasznalo == null)
        {
            return NotFound();
        }

        if (ujSzerepkor != "Admin" &&
            ujSzerepkor != "Employee" &&
            ujSzerepkor != "")
        {
            return BadRequest();
        }

        var jelenlegiSzerepkorok =
            await _felhasznaloKezelo.GetRolesAsync(felhasznalo);

        if (felhasznalo.Id == _felhasznaloKezelo.GetUserId(User) &&
            jelenlegiSzerepkorok.Contains("Admin") &&
            ujSzerepkor != "Admin")
        {
            TempData["Hiba"] =
                "A saját Admin-jogosultságodat nem távolíthatod el!";
            return RedirectToAction(nameof(Index));
        }

        if (jelenlegiSzerepkorok.Contains("Admin") &&
            ujSzerepkor != "Admin")
        {
            var adminFelhasznalok =
                await _felhasznaloKezelo.GetUsersInRoleAsync("Admin");

            if (adminFelhasznalok.Count <= 1)
            {
                TempData["Hiba"] =
                    "Az utolsó Admin-jogosultság nem távolítható el!";
                return RedirectToAction(nameof(Index));
            }
        }

        var torlesEredmeny =
            await _felhasznaloKezelo.RemoveFromRolesAsync(
                felhasznalo, jelenlegiSzerepkorok);

        if (!torlesEredmeny.Succeeded)
        {
            TempData["Hiba"] =
                "A régi szerepkörök eltávolítása nem sikerült.";
            return RedirectToAction(nameof(Index));
        }

        if (!string.IsNullOrEmpty(ujSzerepkor))
        {
            var hozzaadasEredmeny =
                await _felhasznaloKezelo.AddToRoleAsync(
                    felhasznalo, ujSzerepkor);

            if (!hozzaadasEredmeny.Succeeded)
            {
                TempData["Hiba"] =
                    "Az új szerepkör beállítása nem sikerült.";
                return RedirectToAction(nameof(Index));
            }
        }

        TempData["Siker"] = "A felhasználó szerepköre sikeresen módosult.";

        return RedirectToAction(nameof(Index));
    }
}