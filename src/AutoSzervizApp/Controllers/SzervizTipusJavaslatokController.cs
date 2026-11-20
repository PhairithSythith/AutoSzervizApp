using AutoSzervizApp.Data;
using AutoSzervizApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoSzervizApp.Controllers
{
    [Authorize]
    public class SzervizTipusJavaslatokController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _felhasznaloKezelo;

        public SzervizTipusJavaslatokController(
            ApplicationDbContext context,
            UserManager<IdentityUser> felhasznaloKezelo)
        {
            _context = context;
            _felhasznaloKezelo = felhasznaloKezelo;
        }

        // A bejelentkezett felhasználó saját javaslatainak listája
        public async Task<IActionResult> Index()
        {
            var felhasznaloId = _felhasznaloKezelo.GetUserId(User);

            var javaslatok = await _context.SzervizTipusJavaslatok
                .Where(j => j.JavasloFelhasznaloId == felhasznaloId)
                .OrderByDescending(j => j.Letrehozva)
                .ToListAsync();

            return View(javaslatok);
        }

        // Új javaslat űrlapja
        [HttpGet]
        public IActionResult Letrehozas()
        {
            return View();
        }

        // Új javaslat mentése
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Letrehozas([Bind("Nev,Leiras,JavasoltAlapar")] SzervizTipusJavaslat javaslat)
        {
            javaslat.JavasloFelhasznaloId = _felhasznaloKezelo.GetUserId(User)!;

            ModelState.Remove(nameof(javaslat.JavasloFelhasznaloId));

            if (!ModelState.IsValid)
            {
                return View(javaslat);
            }
            javaslat.Id = 0;
            javaslat.Allapot = JavaslatAllapot.Fuggoben;
            javaslat.Letrehozva = DateTime.UtcNow;
            javaslat.Elbiralva = null;
            javaslat.ElbiraloFelhasznaloId = null;
            javaslat.AdminMegjegyzes = null;

            _context.SzervizTipusJavaslatok.Add(javaslat);
            await _context.SaveChangesAsync();

            TempData["Siker"] = "A szolgáltatástípus-javaslatodat sikeresen beküldtük.";
            return RedirectToAction(nameof(Index));
        }

        // Az adminisztrátor az összes beérkezett javaslatot megtekintheti
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminIndex()
        {
            var javaslatok = await _context.SzervizTipusJavaslatok
                .Include(j => j.JavasloFelhasznalo)
                .Include(j => j.ElbiraloFelhasznalo)
                .OrderBy(j => j.Allapot)
                .ThenByDescending(j => j.Letrehozva)
                .ToListAsync();

            return View(javaslatok);
        }

        // Az adminisztrátor elfogadhatja vagy elutasíthatja a javaslatot
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Elbiralas(
            int id,
            JavaslatAllapot allapot,
            string? adminMegjegyzes)
        {
            if (allapot != JavaslatAllapot.Elfogadva &&
                allapot != JavaslatAllapot.Elutasitva)
            {
                return BadRequest();
            }

            var javaslat = await _context.SzervizTipusJavaslatok
                .FirstOrDefaultAsync(j => j.Id == id);

            if (javaslat == null)
            {
                return NotFound();
            }

            if (javaslat.Allapot != JavaslatAllapot.Fuggoben)
            {
                TempData["Hiba"] = "Ezt a javaslatot már elbírálták.";
                return RedirectToAction(nameof(AdminIndex));
            }

            if (allapot == JavaslatAllapot.Elfogadva)
            {
                var szolgaltatas = new SzervizTipus
                {
                    Nev = javaslat.Nev,
                    Leiras = javaslat.Leiras,
                    Alapar = javaslat.JavasoltAlapar
                };

                _context.SzervizTipusok.Add(szolgaltatas);
            }

            javaslat.Allapot = allapot;
            javaslat.Elbiralva = DateTime.UtcNow;
            javaslat.ElbiraloFelhasznaloId = _felhasznaloKezelo.GetUserId(User);
            javaslat.AdminMegjegyzes = adminMegjegyzes;

            await _context.SaveChangesAsync();

            TempData["Siker"] = allapot == JavaslatAllapot.Elfogadva
                ? "A javaslatot elfogadtad, a szolgáltatás létrejött."
                : "A javaslatot elutasítottad.";

            return RedirectToAction(nameof(AdminIndex));
        }
    }
}

