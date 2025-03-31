using HotelReservationSystem.Models.Domain;
using HotelReservationSystem.Repositories.EF;
using HotelReservationSystem.Repositories.Interfaces;
using HotelReservationSystem.Services.Interfaces;
using HotelReservationSystem.Services;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void Seed(this HotelDbContext context)
        {

            if (!context.Rooms.Any()) // tylko jeśli nie ma pokojów
            {
                var rooms = new[]
                {
                    new Room { Number = "101", Type = "Single", PricePerNight = 100, IsAvailable = true },
                    new Room { Number = "102", Type = "Double", PricePerNight = 150, IsAvailable = true },
                    new Room { Number = "103", Type = "Single", PricePerNight = 100, IsAvailable = true },
                    new Room { Number = "104", Type = "Double", PricePerNight = 150, IsAvailable = true },
                    new Room { Number = "105", Type = "Single", PricePerNight = 100, IsAvailable = true },
                    new Room { Number = "106", Type = "Double", PricePerNight = 150, IsAvailable = true },
                    new Room { Number = "107", Type = "Single", PricePerNight = 100, IsAvailable = true },
                    new Room { Number = "108", Type = "Double", PricePerNight = 150, IsAvailable = true },
                    new Room { Number = "109", Type = "Single", PricePerNight = 100, IsAvailable = true },
                    new Room { Number = "110", Type = "Double", PricePerNight = 150, IsAvailable = true }
                };
                context.Rooms.AddRange(rooms);
                context.SaveChanges();
            }
        }

        public static void AddDbContextBasedServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<HotelDbContext>(options =>
                options.UseSqlServer(configuration["ConnectionString"]));

            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IStripeService, StripeService>();

        }
    }
}
