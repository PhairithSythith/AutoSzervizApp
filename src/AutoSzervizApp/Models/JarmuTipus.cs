using System.ComponentModel.DataAnnotations;

namespace AutoSzervizApp.Models;

public class JarmuTipus
{
    public int Id { get; set; }

    [Required]
    public string Marka { get; set; } = "";

    [Required]
    public string Modell { get; set; } = "";

    [Range(1900, 2100)]
    public int GyartasiEv { get; set; }
}