using HotelReservationSystem.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservationSystem.Infrastructure.Data.Configuration
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasKey(r => r.Id);
            
            builder.Property(r => r.Id)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(r => r.ArrivalDate)
                .IsRequired();

            builder.Property(r => r.DepartureDate)
                .IsRequired();

            builder.Property(r => r.NumberOfGuests)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(r => r.TotalPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(r => r.AdditionalRequests)
                .HasMaxLength(1000);

            builder.Property(r => r.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(r => r.Reason)
                .HasMaxLength(500);

            builder.Property(r => r.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(r => r.GuestId)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasOne(r => r.Guest)
                .WithMany()
                .HasForeignKey(r => r.GuestId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Room)
                .WithMany()
                .HasForeignKey(r => r.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Payment)
                .WithOne(p => p.Reservation)
                .HasForeignKey<Payment>(p => p.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(r => r.GuestId);
            builder.HasIndex(r => r.RoomId);
            builder.HasIndex(r => r.ArrivalDate);
            builder.HasIndex(r => r.Status);
        }
    }
}
