using AutoSzervizApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
}