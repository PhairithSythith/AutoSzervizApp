using System.ComponentModel.DataAnnotations;

namespace AutoSzervizApp.Models;

public class Ugyfel
{
    public int Id { get; set; }

    [Required]
    public string Nev { get; set; } = "";

    [Required]
    public string Telefon { get; set; } = "";

    [Display(Name = "E-mail-cím")]
    public string EmailCim { get; set; } = "";

    public ICollection<Jarmu> Jarmuvek { get; set; } = new List<Jarmu>();
}