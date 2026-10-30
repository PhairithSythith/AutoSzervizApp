using AutoSzervizApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoSzervizApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutoSzervizApp.Controllers;

[Authorize]
public class JarmuvekController : Controller
{
    private readonly ApplicationDbContext _context;

    public JarmuvekController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var jarmuvek = await _context.Jarmuvek
            .Include(j => j.Ugyfel)
            .Include(j => j.JarmuTipus)
            .AsNoTracking()
            .ToListAsync();

        return View(jarmuvek);
    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create()
    {
        await ListaadatokBetolteseAsync();
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Jarmu jarmu)
    {
        if (!ModelState.IsValid)
        {
            await ListaadatokBetolteseAsync();
            return View(jarmu);
        }

        _context.Jarmuvek.Add(jarmu);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var jarmu = await _context.Jarmuvek.FindAsync(id);

        if (jarmu == null)
        {
            return NotFound();
        }

        await ListaadatokBetolteseAsync();
        return View(jarmu);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,Rendszam,JarmuTipusId,UgyfelId")] Jarmu jarmu)
    {
        if (id != jarmu.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            await ListaadatokBetolteseAsync();
            return View(jarmu);
        }

        _context.Jarmuvek.Update(jarmu);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private async Task ListaadatokBetolteseAsync()
    {
        var ugyfelek = await _context.Ugyfelek
            .OrderBy(u => u.Nev)
            .ToListAsync();

        var tipusok = await _context.JarmuTipusok
            .Select(t => new
            {
                t.Id,
                Megnevezes = t.Marka + " " + t.Modell + " (" + t.GyartasiEv + ")"
            })
            .ToListAsync();

        ViewData["UgyfelId"] = new SelectList(ugyfelek, "Id", "Nev");
        ViewData["JarmuTipusId"] = new SelectList(tipusok, "Id", "Megnevezes");
    }
}