using System.ComponentModel.DataAnnotations;

namespace AutoSzervizApp.Models;

public class AdminUjFelhasznaloViewModel
{
    [Required(ErrorMessage = "Az e-mail-cím megadása kötelező.")]
    [EmailAddress(ErrorMessage = "Adj meg egy érvényes e-mail-címet.")]
    [Display(Name = "E-mail-cím")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "A jelszó megadása kötelező.")]
    [StringLength(100, MinimumLength = 6,
        ErrorMessage = "A jelszónak legalább 6 karakter hosszúnak kell lennie.")]
    [DataType(DataType.Password)]
    [Display(Name = "Jelszó")]
    public string Jelszo { get; set; } = "";

    [Required(ErrorMessage = "A jelszó megerősítése kötelező.")]
    [DataType(DataType.Password)]
    [Compare(nameof(Jelszo), ErrorMessage = "A két jelszó nem egyezik.")]
    [Display(Name = "Jelszó megerősítése")]
    public string JelszoMegerositese { get; set; } = "";

    [Required]
    [RegularExpression("^(Admin|Employee)$",
        ErrorMessage = "Érvénytelen szerepkör.")]
    [Display(Name = "Szerepkör")]
    public string Szerepkor { get; set; } = "Employee";
}