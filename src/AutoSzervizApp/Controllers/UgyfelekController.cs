using AutoSzervizApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoSzervizApp.Models;

namespace AutoSzervizApp.Controllers;

public class UgyfelekController : Controller
{
    private readonly ApplicationDbContext _context;

    public UgyfelekController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var ugyfelek = await _context.Ugyfelek
            .AsNoTracking()
            .ToListAsync();

        return View(ugyfelek);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Ugyfel ugyfel)
    {
        if (!ModelState.IsValid)
        {
            return View(ugyfel);
        }

        _context.Ugyfelek.Add(ugyfel);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}