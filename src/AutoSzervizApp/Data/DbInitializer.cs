using Microsoft.AspNetCore.Identity;

namespace AutoSzervizApp.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(
        RoleManager<IdentityRole> szerepkorKezelo)
    {
        string[] szerepkorok = { "Admin", "Employee" };

        foreach (string szerepkor in szerepkorok)
        {
            if (!await szerepkorKezelo.RoleExistsAsync(szerepkor))
            {
                await szerepkorKezelo.CreateAsync(
                    new IdentityRole(szerepkor));
            }
        }
    }
}