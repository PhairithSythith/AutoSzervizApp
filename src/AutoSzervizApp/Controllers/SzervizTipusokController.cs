using AutoSzervizApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoSzervizApp.Models;

namespace AutoSzervizApp.Controllers;

[Authorize]
public class SzervizTipusokController : Controller
{
    private readonly ApplicationDbContext _context;

    public SzervizTipusokController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var szervizTipusok = await _context.SzervizTipusok
            .AsNoTracking()
            .OrderBy(s => s.Nev)
            .ToListAsync();

        return View(szervizTipusok);
    }
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SzervizTipus szervizTipus)
    {
        if (!ModelState.IsValid)
        {
            return View(szervizTipus);
        }

        _context.SzervizTipusok.Add(szervizTipus);
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

        var szervizTipus = await _context.SzervizTipusok.FindAsync(id);

        if (szervizTipus == null)
        {
            return NotFound();
        }

        return View(szervizTipus);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,Nev,Leiras,Alapar")] SzervizTipus szervizTipus)
    {
        if (id != szervizTipus.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(szervizTipus);
        }

        _context.SzervizTipusok.Update(szervizTipus);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}