using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace AutoSzervizApp.Models
{
    public enum JavaslatAllapot
    {
        Fuggoben = 0,
        Elfogadva = 1,
        Elutasitva = 2
    }

    public class SzervizTipusJavaslat
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A szolgáltatás nevének megadása kötelező!")]
        [StringLength(100, ErrorMessage = "A név legfeljebb 100 karakter lehet.")]
        [Display(Name = "Szolgáltatás neve")]
        public string Nev { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "A leírás legfeljebb 1000 karakter lehet.")]
        [Display(Name = "Leírás")]
        public string? Leiras { get; set; }

        [Required(ErrorMessage = "A javasolt ár megadása kötelező!")]
        [Range(0, 10000000, ErrorMessage = "Érvényes összeget adj meg!")]
        [Display(Name = "Javasolt ár (Ft)")]
        public decimal JavasoltAlapar { get; set; }

        [Required]
        public string JavasloFelhasznaloId { get; set; } = string.Empty;
        [ForeignKey(nameof(JavasloFelhasznaloId))]
        public IdentityUser? JavasloFelhasznalo { get; set; }


        [Display(Name = "Javaslat állapota")]
        public JavaslatAllapot Allapot { get; set; } = JavaslatAllapot.Fuggoben;

        public DateTime Letrehozva { get; set; } = DateTime.UtcNow;

        public DateTime? Elbiralva { get; set; }

        public string? ElbiraloFelhasznaloId { get; set; }
        [ForeignKey(nameof(ElbiraloFelhasznaloId))]
        public IdentityUser? ElbiraloFelhasznalo { get; set; }


        [StringLength(500, ErrorMessage = "Az indoklás legfeljebb 500 karakter lehet.")]
        [Display(Name = "Admin megjegyzése")]
        public string? AdminMegjegyzes { get; set; }
    }
}
