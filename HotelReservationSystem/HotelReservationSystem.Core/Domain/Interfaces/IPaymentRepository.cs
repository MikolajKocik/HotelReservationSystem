using HotelReservationSystem.Core.Domain.Entities;

namespace HotelReservationSystem.Core.Domain.Interfaces;

/// <summary>
/// Repository interface for managing payment entities
/// </summary>
public interface IPaymentRepository
{
    /// <summary>
    /// Gets a payment by its Stripe payment intent ID
    /// </summary>
    Task<Payment?> GetByStripePaymentIntentIdAsync(string stripePaymentIntentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new payment
    /// </summary>
    Task<int> CreateAsync(Payment payment, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing payment
    /// </summary>
    Task UpdateAsync(Payment payment, CancellationToken cancellationToken = default);
}