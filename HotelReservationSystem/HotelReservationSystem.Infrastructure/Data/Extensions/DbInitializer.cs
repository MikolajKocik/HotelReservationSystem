using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using HotelReservationSystem.Core.Domain.Entities;

namespace HotelReservationSystem.Infrastructure.Data.Extensions;

public static class DbInitializer
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        UserManager<Guest> userManager = serviceProvider.GetRequiredService<UserManager<Guest>>();
        IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();

        string[] roles = { "Recepcionist", "Manager", "Guest" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        var recepcionistEmail = configuration["StaffSettings:Recepcionist:Email"]!;
        var recepcionistPhone = configuration["StaffSettings:Recepcionist:PhoneNumber"]!;
        var recepcionistFirstName = configuration["StaffSettings:Recepcionist:FirstName"]!;
        var recepcionistLastName = configuration["StaffSettings:Recepcionist:LastName"]!;
        var recepcionistPassword = configuration["StaffSettings:Recepcionist:Password"]!;

        Guest? user = await userManager.FindByEmailAsync(recepcionistEmail);
        if (user == null)
        {
            var newUser = new Guest(recepcionistEmail, recepcionistPhone, recepcionistFirstName, recepcionistLastName)
            {
                EmailConfirmed = true
            };

            IdentityResult result = await userManager.CreateAsync(newUser, recepcionistPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newUser, "Recepcionist");
            }
        }

        // only as demo 
        var managerEmail = configuration["StaffSettings:Manager:Email"]!;
        var managerPhone = configuration["StaffSettings:Manager:PhoneNumber"]!;
        var managerFirstName = configuration["StaffSettings:Manager:FirstName"]!;
        var managerLastName = configuration["StaffSettings:Manager:LastName"]!;
        var managerPassword = configuration["StaffSettings:Manager:Password"]!;

        Guest? manager = await userManager.FindByEmailAsync(managerEmail);
        if (manager == null)
        {
            var newManager = new Guest(managerEmail, managerPhone, managerFirstName, managerLastName)
            {
                EmailConfirmed = true
            };

            IdentityResult result = await userManager.CreateAsync(newManager, managerPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newManager, "Manager");
            }
        }
    }
}
