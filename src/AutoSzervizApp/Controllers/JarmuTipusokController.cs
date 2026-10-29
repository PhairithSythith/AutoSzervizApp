using AutoSzervizApp.Models;
using AutoSzervizApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoSzervizApp.Controllers;

[Authorize(Roles = "Admin")]
public class JarmuTipusokController : Controller
{
    private readonly ApplicationDbContext _context;

    public JarmuTipusokController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var jarmuTipusok = await _context.JarmuTipusok
            .AsNoTracking()
            .OrderBy(j => j.Marka)
            .ThenBy(j => j.Modell)
            .ToListAsync();

        return View(jarmuTipusok);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(JarmuTipus jarmuTipus)
    {
        if (!ModelState.IsValid)
        {
            return View(jarmuTipus);
        }

        _context.JarmuTipusok.Add(jarmuTipus);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}