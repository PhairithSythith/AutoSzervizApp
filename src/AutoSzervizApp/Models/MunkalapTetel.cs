using System.ComponentModel.DataAnnotations;

namespace AutoSzervizApp.Models;

public enum TetelTipus
{
    Szolgaltatas,
    Alkatresz,
    Egyeb
}

public class MunkalapTetel
{
    public int Id { get; set; }

    [Required]
    public int MunkalapId { get; set; }
    public Munkalap? Munkalap { get; set; }

    public int? SzervizTipusId { get; set; }
    public SzervizTipus? SzervizTipus { get; set; }

    [Display(Name = "Tétel típusa")]
    public TetelTipus Tipus { get; set; }

    [Required]
    [Display(Name = "Megnevezés")]
    public string Megnevezes { get; set; } = "";

    [Required]
    [Range(0.01, 100000)]
    [Display(Name = "Mennyiség")]
    public decimal Mennyiseg { get; set; } = 1;

    [Required]
    [Range(0, 10000000)]
    [Display(Name = "Egységár (Ft)")]
    public decimal Egysegar { get; set; }

    [Display(Name = "Összesen (Ft)")]
    public decimal Osszesen => Mennyiseg * Egysegar; 
}