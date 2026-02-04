using HotelReservationSystem.Infrastructure.Data;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for payment entities
/// </summary>
public sealed class PaymentRepository : IPaymentRepository
{
    private readonly HotelDbContext context;

    public PaymentRepository(HotelDbContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Gets all payments with related data
    /// </summary>
    public async Task<List<Payment>> GetAllAsync(CancellationToken cancellationToken = default)
        => await this.context.Payments
            .AsNoTracking()
            .Include(p => p.Reservation)
                .ThenInclude(r => r!.Guest)
            .Include(p => p.Reservation)
                .ThenInclude(r => r!.Room)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);

    /// <summary>
    /// Gets a payment by its Stripe payment intent ID
    /// </summary>
    public async Task<Payment?> GetByStripePaymentIntentIdAsync(string stripePaymentIntentId, CancellationToken cancellationToken = default)
        => await this.context.Payments
            .FirstOrDefaultAsync(p => p.StripePaymentIntentId == stripePaymentIntentId, cancellationToken);

    /// <summary>
    /// Creates a new payment
    /// </summary>
    public async Task<int> CreateAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        this.context.Payments.Add(payment);
        await this.context.SaveChangesAsync(cancellationToken);
        return payment.Id;
    }

    /// <summary>
    /// Updates an existing payment
    /// </summary>
    public async Task UpdateAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        this.context.Payments.Update(payment);
        await this.context.SaveChangesAsync(cancellationToken);
    }
}