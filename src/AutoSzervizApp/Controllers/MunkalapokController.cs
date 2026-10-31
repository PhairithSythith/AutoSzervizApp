using AutoSzervizApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoSzervizApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutoSzervizApp.Controllers;

[Authorize]
public class MunkalapokController : Controller
{
    private readonly ApplicationDbContext _context;

    public MunkalapokController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var munkalapok = await _context.Munkalapok
            .Include(m => m.Ugyfel)
            .Include(m => m.JarmuTipus)
            .AsNoTracking()
            .OrderByDescending(m => m.Letrehozva)
            .ToListAsync();

        return View(munkalapok);
    }
    [Authorize(Roles = "Admin,Employee")]
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await ListaadatokBetolteseAsync();
        return View();
    }

    [Authorize(Roles = "Admin,Employee")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Munkalap munkalap)
    {
        if (!ModelState.IsValid)
        {
            await ListaadatokBetolteseAsync();
            return View(munkalap);
        }

        _context.Munkalapok.Add(munkalap);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var munkalap = await _context.Munkalapok
            .Include(m => m.Ugyfel)
            .Include(m => m.JarmuTipus)
            .Include(m => m.Tetelek)
                .ThenInclude(t => t.SzervizTipus)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);

        if (munkalap == null)
        {
            return NotFound();
        }

        return View(munkalap);
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