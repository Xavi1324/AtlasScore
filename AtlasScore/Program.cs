using Application.Interfaces.IServices;
using Application.Services;
using Microsoft.AspNetCore.Localization;
using Persistence;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersitence(builder.Configuration);

builder.Services.AddScoped<IPaisService, PaisService>();
builder.Services.AddScoped<IMacroindicadorService, MacroindicadorService>();
builder.Services.AddScoped<IIndicadorPorPaisService, IndicadorPorPaisService>();
builder.Services.AddScoped<ITasaRetornoService, TasaRetornoService>();







// Add services to the container.
builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
var defaultCulture = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(defaultCulture),
    SupportedCultures = new[] { defaultCulture },
    SupportedUICultures = new[] { defaultCulture }
};

app.UseRequestLocalization(localizationOptions);


app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();



app.Run();
