using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using HotelReservationSystem.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using HotelReservationSystem.Core.Domain.Enums;

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

        await SeedOpinionsAsync(serviceProvider);
    }

    private static async Task SeedOpinionsAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<HotelDbContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<Guest>>();

        if (await context.Opinions.AnyAsync())
        {
            return; 
        }

        var guestsData = new[]
        {
            new { Email = "adam.nowak@example.com", Phone = "111222333", First = "Adam", Last = "Nowak" },
            new { Email = "anna.kowalska@example.com", Phone = "222333444", First = "Anna", Last = "Kowalska" },
            new { Email = "jan.wiacek@example.com", Phone = "333444555", First = "Jan", Last = "Wiącek" },
            new { Email = "maria.duda@example.com", Phone = "444555666", First = "Maria", Last = "Duda" },
            new { Email = "piotr.zielinski@example.com", Phone = "555666777", First = "Piotr", Last = "Zieliński" }
        };

        var guests = new List<Guest>();
        foreach (var gd in guestsData)
        {
            var guest = await userManager.FindByEmailAsync(gd.Email);
            if (guest == null)
            {
                guest = new Guest(gd.Email, gd.Phone, gd.First, gd.Last)
                {
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(guest, "Demo123!");
                await userManager.AddToRoleAsync(guest, "Guest");
            }
            guests.Add(guest);
        }

        var rooms = await context.Rooms.OrderBy(r => r.Id).Take(5).ToListAsync();
        if (rooms.Count < 5)
        {
            return; 
        }

        var opinionsData = new[]
        {
            new { Rating = 5.0, Comment = "Wspaniały hotel! Obsługa była niesamowicie pomocna, a pokój czysty i bardzo luksusowy. Na pewno wrócę!" },
            new { Rating = 4.0, Comment = "Bardzo udany pobyt. Smaczne śniadania i świetna strefa SPA. Jedyny minus to lekki hałas z ulicy." },
            new { Rating = 5.0, Comment = "Najlepszy hotel w jakim byłam. Widok z okna zapiera dech w piersiach, a jedzenie w restauracji to czysta poezja." },
            new { Rating = 3.0, Comment = "Pobyt poprawny. Pokój ładny, ale obsługa w recepcji mogłaby być bardziej uprzejma." },
            new { Rating = 5.0, Comment = "Rewelacja! Czysto, nowocześnie i w świetnej lokalizacji. Bardzo wygodne łóżka. Polecam każdemu." }
        };

        for (int i = 0; i < 5; i++)
        {
            var guest = guests[i];
            var room = rooms[i];
            var data = opinionsData[i];

            var reservation = new Reservation(
                DateTime.Today,
                DateTime.Today.AddDays(2),
                2,
                room.PricePerNight * 2,
                "Brak",
                ReservationStatus.Completed,
                "Demo completed stay",
                room.Id,
                guest.Id
            );
            context.Reservations.Add(reservation);
            await context.SaveChangesAsync();

            var opinion = new Opinion(data.Rating, data.Comment, reservation.Id, guest.Id);
            context.Opinions.Add(opinion);
        }
        await context.SaveChangesAsync();
    }
}
