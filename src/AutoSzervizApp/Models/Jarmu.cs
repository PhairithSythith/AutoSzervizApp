using System.ComponentModel.DataAnnotations;

namespace AutoSzervizApp.Models;

public class Jarmu
{
    public int Id { get; set; }

    [Required]
    public string Rendszam { get; set; } = "";

    public int JarmuTipusId { get; set; }
    public JarmuTipus? JarmuTipus { get; set; }

    public int UgyfelId { get; set; }
    public Ugyfel? Ugyfel { get; set; }
}