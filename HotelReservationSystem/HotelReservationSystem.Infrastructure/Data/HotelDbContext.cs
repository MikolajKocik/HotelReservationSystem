using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Entities.GuestPref;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Infrastructure.Data;

public sealed class HotelDbContext : IdentityDbContext<Guest>
{
    public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options) { }

    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Opinion> Opinions { get; set; }
    public DbSet<GuestPreference> GuestPreferences { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
