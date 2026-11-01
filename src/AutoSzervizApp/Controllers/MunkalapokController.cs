using AutoSzervizApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoSzervizApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutoSzervizApp.Controllers;

[Authorize(Roles = "Admin,Employee")]
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

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var munkalap = await _context.Munkalapok.FindAsync(id);

        if (munkalap == null)
        {
            return NotFound();
        }

        await ListaadatokBetolteseAsync(
            munkalap.JarmuTipusId,
            munkalap.UgyfelId);

        return View(munkalap);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,Rendszam,JarmuTipusId,UgyfelId,Leiras,Koltseg,Statusz,BefejezveDatuma")]
        Munkalap munkalap)
    {
        if (id != munkalap.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            await ListaadatokBetolteseAsync(
                munkalap.JarmuTipusId,
                munkalap.UgyfelId);

            return View(munkalap);
        }

        var adatbazisbanMunkalap =
            await _context.Munkalapok.FindAsync(id);

        if (adatbazisbanMunkalap == null)
        {
            return NotFound();
        }

        adatbazisbanMunkalap.Rendszam = munkalap.Rendszam;
        adatbazisbanMunkalap.JarmuTipusId = munkalap.JarmuTipusId;
        adatbazisbanMunkalap.UgyfelId = munkalap.UgyfelId;
        adatbazisbanMunkalap.Leiras = munkalap.Leiras;
        adatbazisbanMunkalap.Koltseg = munkalap.Koltseg;
        adatbazisbanMunkalap.Statusz = munkalap.Statusz;
        adatbazisbanMunkalap.BefejezveDatuma = munkalap.BefejezveDatuma;
        adatbazisbanMunkalap.UtoljaraModositva = DateTime.Now;
        adatbazisbanMunkalap.UtoljaraModositotta = User.Identity?.Name;

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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AllapotFrissitese(
        int id,
        MunkalapStatusz ujStatusz)
    {
        if (!Enum.IsDefined(typeof(MunkalapStatusz), ujStatusz))
        {
            return BadRequest("Érvénytelen munkalapstátusz.");
        }

        var munkalap = await _context.Munkalapok.FindAsync(id);

        if (munkalap == null)
        {
            return NotFound();
        }

        munkalap.Statusz = ujStatusz;
        munkalap.UtoljaraModositva = DateTime.Now;
        munkalap.UtoljaraModositotta = User.Identity?.Name;

        await _context.SaveChangesAsync();

        TempData["SikerUzenet"] =
            $"A munkalap státusza sikeresen frissítve: {ujStatusz}";

        return RedirectToAction(nameof(Details), new { id });
    }

    public async Task<IActionResult> Print(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var munkalap = await _context.Munkalapok
            .Include(m => m.JarmuTipus)
            .Include(m => m.Ugyfel)
            .Include(m => m.Tetelek)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (munkalap == null)
        {
            return NotFound();
        }

        return View(munkalap);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var munkalap = await _context.Munkalapok
            .Include(m => m.JarmuTipus)
            .Include(m => m.Ugyfel)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (munkalap == null)
        {
            return NotFound();
        }

        return View(munkalap);
    }

    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var munkalap = await _context.Munkalapok.FindAsync(id);

        if (munkalap != null)
        {
            _context.Munkalapok.Remove(munkalap);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
    
    private async Task ListaadatokBetolteseAsync(
    int? kivalasztottTipusId = null,
    int? kivalasztottUgyfelId = null)
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

        ViewData["UgyfelId"] =
        new SelectList(ugyfelek, "Id", "Nev", kivalasztottUgyfelId);

        ViewData["JarmuTipusId"] =
        new SelectList(tipusok, "Id", "Megnevezes", kivalasztottTipusId);
    }
}