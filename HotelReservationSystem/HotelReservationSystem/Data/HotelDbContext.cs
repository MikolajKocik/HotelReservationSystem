using HotelReservationSystem.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HotelReservationSystem.Data
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options) { }

        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }
}
