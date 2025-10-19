using Microsoft.EntityFrameworkCore;
using NewsPortal.Data;
using NewsPortal.Repositories.Implementations;
using NewsPortal.Repositories.Interfaces;
using NewsPortal.Services.Implementations;
using NewsPortal.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

string? connection = builder.Configuration.GetConnectionString("DatabaseConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connection));

builder.Services.AddScoped<INewsRepository, NewsRepository>();
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddControllersWithViews();




var app = builder.Build();

// јвто-миграци€: создаЄт таблицы, которых ещЄ нет, данные не тер€ютс€
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=News}/{action=Index}/{id?}");

app.Run();
