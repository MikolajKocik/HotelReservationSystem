using HotelReservationSystem.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservationSystem.Infrastructure.Data.Configuration;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Number)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(r => r.Type)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(r => r.PricePerNight)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(r => r.IsAvailable)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(r => r.ImagePath)
            .HasMaxLength(500);

        builder.Property(r => r.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(r => r.Number)
            .IsUnique();

        builder.HasIndex(r => r.Type);
        builder.HasIndex(r => r.IsAvailable);
        builder.HasIndex(r => r.PricePerNight);
    }
}