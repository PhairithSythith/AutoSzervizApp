using System.ComponentModel.DataAnnotations;

namespace AutoSzervizApp.Models;

public class SzervizTipus
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Szolgáltatás neve")]
    public string Nev { get; set; } = "";

    [Display(Name = "Leírás")]
    public string? Leiras { get; set; }

    [Required]
    [Range(0, 10000000)]
    [Display(Name = "Alapár (Ft)")]
    public decimal Alapar { get; set; }
}