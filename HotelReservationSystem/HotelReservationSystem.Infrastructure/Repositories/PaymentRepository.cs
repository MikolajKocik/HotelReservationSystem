using HotelReservationSystem.Infrastructure.Data;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for payment entities
/// </summary>
public class PaymentRepository : IPaymentRepository
{
    private readonly HotelDbContext context;

    public PaymentRepository(HotelDbContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Gets a payment by its Stripe payment intent ID
    /// </summary>
    public async Task<Payment?> GetByStripePaymentIntentIdAsync(string stripePaymentIntentId)
        => await context.Payments
            .FirstOrDefaultAsync(p => p.StripePaymentIntentId == stripePaymentIntentId);

    /// <summary>
    /// Creates a new payment
    /// </summary>
    public async Task<int> CreateAsync(Payment payment)
    {
        context.Payments.Add(payment);
        await context.SaveChangesAsync();
        return payment.Id;
    }

    /// <summary>
    /// Updates an existing payment
    /// </summary>
    public async Task UpdateAsync(Payment payment)
    {
        context.Payments.Update(payment);
        await context.SaveChangesAsync();
    }
}