using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using HotelReservationSystem.Infrastructure.Repositories; 
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HotelReservationSystem.Application.Interfaces;

namespace HotelReservationSystem.Infrastructure.Data.Extensions;

public static class ServiceCollectionExtensions
{
    public static void Seed(this HotelDbContext context)
    {
        if (!context.Rooms.Any())
        {
            var rooms = new[]
            {
                    new Room("1", RoomType.Single, 550, "pln"),
                    new Room("2", RoomType.Double, 700, "pln"),
                    new Room("3", RoomType.Single, 550, "pln"),
                    new Room("4", RoomType.Double, 700, "pln"),
                    new Room("5", RoomType.Single, 550, "pln"),
                    new Room("6", RoomType.Double, 700, "pln"),
                    new Room("7", RoomType.Single, 550, "pln"),
                    new Room("8", RoomType.Double, 700, "pln"),
                    new Room("9", RoomType.Single, 550, "pln"),
                    new Room("10", RoomType.Double, 700, "pln")
                };
            context.Rooms.AddRange(rooms);
            context.SaveChanges();
        }
    }

    public static void AddDbContextBasedServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default")
            ?? configuration.GetConnectionString("ConnectionString")
            ?? configuration["ConnectionString"];
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Database connection string is not configured. Set 'ConnectionStrings:Default' or 'ConnectionString' in configuration.");
        }
        services.AddDbContext<HotelDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IGuestRepository, GuestRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IOpinionRepository, OpinionRepository>();

        services.AddMemoryCache();
        services.AddSingleton<ReservationQueue>();
        services.AddSingleton<IReservationQueue>(sp => sp.GetRequiredService<ReservationQueue>());

        services.AddScoped<IReservationRepository, ReservationRepository>(sp =>
        {
            HotelDbContext ctx = sp.GetRequiredService<HotelDbContext>();
            IMemoryCache cache = sp.GetRequiredService<IMemoryCache>();
            return new ReservationRepository(ctx, cache);
        });
    }
}