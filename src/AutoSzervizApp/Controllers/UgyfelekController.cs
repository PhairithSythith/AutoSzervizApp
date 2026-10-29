using AutoSzervizApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoSzervizApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace AutoSzervizApp.Controllers;

[Authorize]
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
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var ugyfel = await _context.Ugyfelek.FindAsync(id);

        if (ugyfel == null)
        {
            return NotFound();
        }

    return View(ugyfel);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,Nev,Telefon,EmailCim")] Ugyfel ugyfel)
    {
        if (id != ugyfel.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(ugyfel);
        }

        _context.Ugyfelek.Update(ugyfel);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var ugyfel = await _context.Ugyfelek
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);

        if (ugyfel == null)
        {
            return NotFound();
        }

        return View(ugyfel);
    }

    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var ugyfel = await _context.Ugyfelek.FindAsync(id);

        if (ugyfel != null)
        {
            _context.Ugyfelek.Remove(ugyfel);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}