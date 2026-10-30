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
}