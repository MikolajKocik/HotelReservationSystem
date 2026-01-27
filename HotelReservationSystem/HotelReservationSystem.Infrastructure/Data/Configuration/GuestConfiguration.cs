using HotelReservationSystem.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservationSystem.Infrastructure.Data.Configuration;

public class GuestConfiguration : IEntityTypeConfiguration<Guest>
{
    public void Configure(EntityTypeBuilder<Guest> builder)
    {
        builder.HasKey(g => g.Id);
        
        builder.Property(g => g.Id)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(g => g.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(g => g.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(g => g.Email)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(g => g.PhoneNumber)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(g => g.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(g => g.Email)
            .IsUnique();

        builder.HasIndex(g => g.CreatedAt);
    }
}