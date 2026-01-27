namespace HotelReservationSystem.Application.Dtos.Reservation;

public record PaymentInfoDto
{
    public string ReservationId { get; init; } = string.Empty;
    public decimal TotalAmount { get; init; }
    public string ClientSecret { get; init; } = string.Empty;
    public string PublishableKey { get; init; } = string.Empty;
    public string Currency { get; init; } = "PLN";
}