using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using HotelReservationSystem.Infrastructure.Repositories;
using HotelReservationSystem.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelReservationSystem.Infrastructure.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void Seed(this HotelDbContext context)
        {
            if (!context.Rooms.Any()) // tylko jeśli nie ma pokojów
            {
                var rooms = new[]
                {
                    new Room("101", RoomType.Single, 100),
                    new Room("102", RoomType.Double, 150),
                    new Room("103", RoomType.Single, 100),
                    new Room("104", RoomType.Double, 150),
                    new Room("105", RoomType.Single, 100),
                    new Room("106", RoomType.Double, 150),
                    new Room("107", RoomType.Single, 100),
                    new Room("108", RoomType.Double, 150),
                    new Room("109", RoomType.Single, 100),
                    new Room("110", RoomType.Double, 150)
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
            services.AddScoped<IGuestRepository, GuestRepository>();
        }
    }
}
