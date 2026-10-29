using System.ComponentModel.DataAnnotations;

namespace AutoSzervizApp.Models;

public class Munkalap
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Rendszám")]
    public string Rendszam { get; set; } = "";

    [Required]
    [Display(Name = "Gépjárműtípus")]
    public int JarmuTipusId { get; set; }
    public JarmuTipus? JarmuTipus { get; set; }

    [Required]
    [Display(Name = "Ügyfél")]
    public int UgyfelId { get; set; }
    public Ugyfel? Ugyfel { get; set; }

    [Required]
    [Display(Name = "Leírás / Hibajelenség")]
    public string Leiras { get; set; } = "";

    [Display(Name = "Felvétel dátuma")]
    public DateTime Letrehozva { get; set; } = DateTime.Now;
}