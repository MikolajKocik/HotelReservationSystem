using HotelReservationSystem.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservationSystem.Infrastructure.Data.Configuration;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();

        builder.Property(p => p.Method)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(p => p.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.StripePaymentIntentId)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(p => p.CompletedAt);

        builder.Property(p => p.ReservationId)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(p => p.StripePaymentIntentId)
            .IsUnique();

        builder.HasIndex(p => p.ReservationId)
            .IsUnique();

        builder.HasIndex(p => p.Status);
        builder.HasIndex(p => p.CreatedAt);
    }
}