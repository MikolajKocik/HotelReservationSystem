using System.Collections.Generic;
using HotelReservationSystem.Infrastructure.Data;
using HotelReservationSystem.Infrastructure.Data.Extensions;
using HotelReservationSystem.Infrastructure.Extensions;
using HotelReservationSystem.Application.Extensions;
using HotelReservationSystem.Web.Extensions;
using Microsoft.EntityFrameworkCore;
using HotelReservationSystem.Web.Middleware.MiddlewareExtensions;

var builder = WebApplication.CreateBuilder(args);

// Ensure a DB connection string exists; if missing add a sensible local default for development
var config = builder.Configuration;
var conn = config.GetConnectionString("Default") ?? config.GetConnectionString("ConnectionString") ?? config["ConnectionString"];
if (string.IsNullOrWhiteSpace(conn))
{
    var defaults = new Dictionary<string, string>
    {
        ["ConnectionStrings:Default"] = "Server=(localdb)\\mssqllocaldb;Database=HotelReservationSystem;Trusted_Connection=True;MultipleActiveResultSets=true"
    };
    builder.Configuration.AddInMemoryCollection(defaults);
}

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddWebServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<HotelDbContext>();
    context.Database.Migrate();
    context.Seed(); 
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbInitializer.SeedRolesAsync(services);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseErrorHandlingMiddleware();
app.UseCultureHandling();

app.UseRouting();
app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
