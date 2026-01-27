using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace HotelReservationSystem.Infrastructure.Data.Extensions;

public static class DbInitializer
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        UserManager<IdentityUser> userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        string[] roles = { "Recepcionist", "Manager", "Guest" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        IdentityUser? user = await userManager.FindByEmailAsync("recepcja@hotel.pl");
        if (user == null)
        {
            var newUser = new IdentityUser
            {
                UserName = "recepcja@hotel.pl",
                Email = "recepcja@hotel.pl",
                EmailConfirmed = true
            };

            IdentityResult result = await userManager.CreateAsync(newUser, "Test123!");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newUser, "Recepcionist");
            }
        }

        IdentityUser? manager = await userManager.FindByEmailAsync("manager@hotel.pl");
        if (manager == null)
        {
            var newManager = new IdentityUser
            {
                UserName = "manager@hotel.pl",
                Email = "manager@hotel.pl",
                EmailConfirmed = true
            };

            IdentityResult result = await userManager.CreateAsync(newManager, "Test123!");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newManager, "Manager");
            }
        }
    }
}
