using AutoSzervizApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "hu", "en" };

    options.SetDefaultCulture("hu")
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("AlapAdatbazisKapcsolat")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context =
        services.GetRequiredService<ApplicationDbContext>();

    await context.Database.OpenConnectionAsync();

    var kapcsolat = context.Database.GetDbConnection();

    await using var parancs = kapcsolat.CreateCommand();

    // SQLite verzió
    parancs.CommandText = "SELECT sqlite_version();";
    Console.WriteLine($"SQLite verzió: {await parancs.ExecuteScalarAsync()}");

    // Integritásellenőrzés
    parancs.CommandText = "PRAGMA integrity_check;";
    Console.WriteLine($"Integritás: {await parancs.ExecuteScalarAsync()}");

    // Szerepkörök lekérdezése
    parancs.CommandText = "SELECT COUNT(*) FROM AspNetRoles;";
    Console.WriteLine($"Szerepkörök száma: {await parancs.ExecuteScalarAsync()}");

    Console.WriteLine(
    $"Adatbázis elérési útja: {context.Database.GetDbConnection().DataSource}"
    );    

    context.Database.Migrate();

    var szerepkorKezelo =
        services.GetRequiredService<RoleManager<IdentityRole>>();

    var felhasznaloKezelo =
        services.GetRequiredService<UserManager<IdentityUser>>();

    await DbInitializer.InitializeAsync(
        context,
        szerepkorKezelo,
        felhasznaloKezelo);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
