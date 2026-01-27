using HotelReservationSystem.Core.Domain.Enums;

namespace HotelReservationSystem.Core.Domain.Entities;

public sealed partial class Payment
{
    private static void ValidateInput(string method, decimal amount, string stripePaymentIntentId)
    {
        if (string.IsNullOrWhiteSpace(method))
            throw new ArgumentException("Payment method cannot be empty", nameof(method));
        
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than 0", nameof(amount));
        
        if (amount > 100000)
            throw new ArgumentException("Amount cannot exceed 100000", nameof(amount));
        
        if (string.IsNullOrWhiteSpace(stripePaymentIntentId))
            throw new ArgumentException("Stripe payment intent ID cannot be empty", nameof(stripePaymentIntentId));
    }

    public void MarkAsPaid()
    {
        if (this.Status == PaymentStatus.Paid)
            throw new InvalidOperationException("Payment is already marked as paid");
        
        if (this.Status == PaymentStatus.Failed)
            throw new InvalidOperationException("Cannot mark failed payment as paid");

        this.Status = PaymentStatus.Paid;
        this.CompletedAt = DateTime.UtcNow;
    }

    public void MarkAsFailed()
    {
        if (this.Status == PaymentStatus.Paid)
            throw new InvalidOperationException("Cannot mark paid payment as failed");

        this.Status = PaymentStatus.Failed;
        this.CompletedAt = DateTime.UtcNow;
    }

    public void MarkAsRefunded()
    {
        if (this.Status != PaymentStatus.Paid)
            throw new InvalidOperationException("Can only refund paid payments");

        this.Status = PaymentStatus.Refunded;
    }

    public bool IsPending => this.Status == PaymentStatus.Pending;
    public bool IsPaid => this.Status == PaymentStatus.Paid;
    public bool HasFailed => this.Status == PaymentStatus.Failed;
}