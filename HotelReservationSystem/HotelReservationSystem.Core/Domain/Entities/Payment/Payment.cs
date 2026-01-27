using HotelReservationSystem.Core.Domain.Enums;

namespace HotelReservationSystem.Core.Domain.Entities;

public sealed partial class Payment
{
    private Payment() 
    {
        this.ReservationId = string.Empty;
    } 

    public Payment(string method, decimal amount, string stripePaymentIntentId, string reservationId)
    {
        ValidateInput(method, amount, stripePaymentIntentId);
        
        this.Id = Random.Shared.Next(1, int.MaxValue);
        this.Method = method;
        this.Amount = amount;
        this.StripePaymentIntentId = stripePaymentIntentId;
        this.ReservationId = reservationId;
        this.Status = PaymentStatus.Pending;
        this.CreatedAt = DateTime.UtcNow;
    }

    public int Id { get; private set; }
    public string Method { get; private set; } = default!;
    public PaymentStatus Status { get; private set; }
    public decimal Amount { get; private set; }
    public string StripePaymentIntentId { get; private set; } = default!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    public string ReservationId { get; private set; }
    public Reservation Reservation { get; set; } = default!;
}