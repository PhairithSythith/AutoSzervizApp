using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AutoSzervizApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoSzervizApp.Data;

namespace AutoSzervizApp.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }
    [Authorize]
    public async Task<IActionResult> Index()
    {
        ViewBag.UgyfelekSzama = await _context.Ugyfelek.CountAsync();
        ViewBag.JarmuvekSzama = await _context.Jarmuvek.CountAsync();

        ViewBag.AktivMunkalapokSzama = await _context.Munkalapok
            .CountAsync(m =>
                m.Statusz == MunkalapStatusz.Bejegyezve ||
                m.Statusz == MunkalapStatusz.Folyamatban);

        ViewBag.OsszesBevetel = await _context.Munkalapok
            .Where(m => m.Statusz == MunkalapStatusz.Befejezve)
            .SumAsync(m => (decimal?)m.Koltseg) ?? 0;

        var legutobbiMunkalapok = await _context.Munkalapok
            .AsNoTracking()
            .Include(m => m.JarmuTipus)
            .Include(m => m.Ugyfel)
            .OrderByDescending(m => m.Letrehozva)
            .Take(5)
            .ToListAsync();

        return View(legutobbiMunkalapok);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
