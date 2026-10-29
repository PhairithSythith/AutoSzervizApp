using AutoSzervizApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoSzervizApp.Controllers;

[Authorize]
public class JarmuvekController : Controller
{
    private readonly ApplicationDbContext _context;

    public JarmuvekController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var jarmuvek = await _context.Jarmuvek
            .Include(j => j.Ugyfel)
            .Include(j => j.JarmuTipus)
            .AsNoTracking()
            .ToListAsync();

        return View(jarmuvek);
    }
}