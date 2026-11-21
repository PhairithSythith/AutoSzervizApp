using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AutoSzervizApp.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class LoginModel : PageModel
{
    private readonly SignInManager<IdentityUser> _signInManager;

    public LoginModel(SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string ReturnUrl { get; set; } = "/";

    public class InputModel
    {
        [Required(ErrorMessage = "Az e-mail-cím megadása kötelező.")]
        [EmailAddress(ErrorMessage = "Érvényes e-mail-címet adj meg.")]
        [Display(Name = "E-mail-cím")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "A jelszó megadása kötelező.")]
        [DataType(DataType.Password)]
        [Display(Name = "Jelszó")]
        public string Password { get; set; } = "";

        [Display(Name = "Bejelentkezve maradok")]
        public bool RememberMe { get; set; }
    }

    public void OnGet(string? returnUrl = null)
    {
        ReturnUrl = returnUrl ?? Url.Content("~/");
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        ReturnUrl = returnUrl ?? Url.Content("~/");

        if (!ModelState.IsValid)
            return Page();

        var result = await _signInManager.PasswordSignInAsync(
            Input.Email,
            Input.Password,
            Input.RememberMe,
            lockoutOnFailure: false);

        if (result.Succeeded)
            return LocalRedirect(ReturnUrl);

        if (result.RequiresTwoFactor)
            return RedirectToPage("./LoginWith2fa",
                new { ReturnUrl, Input.RememberMe });

        if (result.IsLockedOut)
            return RedirectToPage("./Lockout");

        ModelState.AddModelError(
            string.Empty,
            "Hibás e-mail-cím vagy jelszó.");

        return Page();
    }
}