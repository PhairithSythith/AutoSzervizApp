using System.ComponentModel.DataAnnotations;

namespace AutoSzervizApp.Models;

public enum MunkalapStatusz
{
    [Display(Name = "Bejegyezve")]
    Bejegyezve,

    [Display(Name = "Folyamatban")]
    Folyamatban,

    [Display(Name = "Befejezve")]
    Befejezve,

    [Display(Name = "Törölve")]
    Torolve
}
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
    [Display(Name = "Utolsó módosítás")]
    public DateTime? UtoljaraModositva { get; set; }

    [Display(Name = "Utoljára módosította")]
    public string? UtoljaraModositotta { get; set; }

    [Display(Name = "Várható / Elkészülési dátum")]
    public DateTime? BefejezveDatuma { get; set; }

    [Display(Name = "Munkadíj / Összköltség (Ft)")]
    [Range(0, 10000000)]
    public decimal Koltseg { get; set; }

    [Display(Name = "Státusz")]
    public MunkalapStatusz Statusz { get; set; } = MunkalapStatusz.Bejegyezve;

    public ICollection<MunkalapTetel> Tetelek { get; set; } = new List<MunkalapTetel>();    
}