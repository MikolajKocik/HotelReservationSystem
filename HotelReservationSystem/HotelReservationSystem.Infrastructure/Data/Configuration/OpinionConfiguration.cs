using HotelReservationSystem.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservationSystem.Infrastructure.Data.Configuration;

public class OpinionConfiguration : IEntityTypeConfiguration<Opinion>
{
    public void Configure(EntityTypeBuilder<Opinion> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(o => o.Rating)
            .IsRequired()
            .HasColumnType("float");

        builder.Property(o => o.Comment)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(o => o.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(o => o.ReservationId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(o => o.GuestId)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasOne(o => o.Reservation)
            .WithOne()
            .HasForeignKey<Opinion>(o => o.ReservationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(o => o.Guest)
            .WithMany()
            .HasForeignKey(o => o.GuestId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(o => o.ReservationId)
            .IsUnique();

        builder.HasIndex(o => o.GuestId);
        builder.HasIndex(o => o.Rating);
        builder.HasIndex(o => o.CreatedAt);
    }
}