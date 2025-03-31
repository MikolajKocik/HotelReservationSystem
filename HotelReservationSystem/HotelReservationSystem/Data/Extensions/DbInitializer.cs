using Microsoft.AspNetCore.Identity;

namespace HotelReservationSystem.Data.Extensions
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string[] roles = { "Recepcjonista", "Kierownik" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Tworzenie użytkownika testowego (Recepcjonista)
            var user = await userManager.FindByEmailAsync("recepcja@hotel.pl");
            if (user == null)
            {
                var newUser = new IdentityUser
                {
                    UserName = "recepcja@hotel.pl",
                    Email = "recepcja@hotel.pl",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newUser, "Test123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, "Recepcjonista");
                }
            }

            // Tworzenie użytkownika testowego (Kierownik)
            var manager = await userManager.FindByEmailAsync("manager@hotel.pl");
            if (manager == null)
            {
                var newManager = new IdentityUser
                {
                    UserName = "manager@hotel.pl",
                    Email = "manager@hotel.pl",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newManager, "Test123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newManager, "Kierownik");
                }
            }
        }
    }

}
