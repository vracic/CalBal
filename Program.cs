using CalBal.Models;
using CalBal.Models.Enums;
using CalBal.Data;
using CalBal.Data.Interfaces;
using CalBal.Services;
using CalBal.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString("CalbalDatabase"));
dataSourceBuilder.MapEnum<RazinaOvlasti>("razina_enum");
var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<CalbalContext>(options => options.UseNpgsql(dataSource));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Korisniks/Login";
        options.LogoutPath = "/Korisniks/Logout";
    });

builder.Services.AddScoped<IKorisnikService, KorisnikService>();
builder.Services.AddScoped<IAktivnostService, AktivnostService>();
builder.Services.AddScoped<IPrehrambenaNamirnicaService, PrehrambenaNamirnicaService>();
builder.Services.AddScoped<IProvedbaTjAktService, ProvedbaTjAktService>();
builder.Services.AddScoped<IUnosPrehNamService, UnosPrehNamService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IKorisnikRepository, KorisnikRepository>();
builder.Services.AddScoped<IAktivnostRepository, AktivnostRepository>();
builder.Services.AddScoped<IPrehrambenaNamirnicaRepository, PrehrambenaNamirnicaRepository>();
builder.Services.AddScoped<IProvedbaTjAktRepository, ProvedbaTjAktRepository>();
builder.Services.AddScoped<IUnosPrehNamRepository, UnosPrehNamRepository>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("NiskaRazina", policy =>
        policy.RequireClaim("RazinaOvlasti", "niska", "srednja", "visoka"));

    options.AddPolicy("SrednjaRazina", policy =>
        policy.RequireClaim("RazinaOvlasti", "srednja", "visoka"));

    options.AddPolicy("VisokaRazina", policy =>
        policy.RequireClaim("RazinaOvlasti", "visoka"));
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Auth/Login";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

namespace CalBal
{
    public partial class Program { }
}
