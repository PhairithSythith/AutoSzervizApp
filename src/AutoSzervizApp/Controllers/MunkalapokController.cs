using AutoSzervizApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
}