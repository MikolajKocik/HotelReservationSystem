using HotelReservationSystem.Infrastructure.Data;
using HotelReservationSystem.Infrastructure.Data.Extensions;
using HotelReservationSystem.Infrastructure.Extensions;
using HotelReservationSystem.Application.Extensions;
using HotelReservationSystem.Web.Extensions;
using HotelReservationSystem.Web.Configuration;
using Microsoft.EntityFrameworkCore;
using HotelReservationSystem.Web.Middleware.MiddlewareExtensions;
using HotelReservationSystem.MCP.Server;
using HotelReservationSystem.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<StaffSettings>(builder.Configuration.GetSection("StaffSettings"));

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddWebServices();

builder.Services.AddHttpContextAccessor();
builder.Services.AddHotelMcpServer(builder.Configuration);
builder.Services.AddScoped<IAgentService, AgentService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<HotelDbContext>();
    try
    {
        context.Database.Migrate();
        context.Seed();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during database migration/seeding: {ex.Message}");
        throw;
    }
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await DbInitializer.SeedRolesAsync(services);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during role seeding: {ex.Message}");
        throw;
    }
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
