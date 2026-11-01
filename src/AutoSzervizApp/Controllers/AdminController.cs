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
}