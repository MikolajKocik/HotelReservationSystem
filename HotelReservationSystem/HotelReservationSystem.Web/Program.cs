using FluentValidation;
using FluentValidation.AspNetCore;
using HotelReservationSystem.Infrastructure.Data;
using HotelReservationSystem.Infrastructure.Data.Extensions;
using HotelReservationSystem.Infrastructure.Extensions;
using HotelReservationSystem.Application.Interfaces;
using HotelReservationSystem.Application.UseCases;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContextBasedServices(builder.Configuration);

// Register CQRS Services
builder.Services.AddCQRSServices();

// Register remaining Application Services
builder.Services.AddScoped<IStripeService, StripeService>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<HotelDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<HotelDbContext>();
    context.Database.Migrate();
    context.Seed(); // extension seed method
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbInitializer.SeedRolesAndAdminAsync(services);
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
