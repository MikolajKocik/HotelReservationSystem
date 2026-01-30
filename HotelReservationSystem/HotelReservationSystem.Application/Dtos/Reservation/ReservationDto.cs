using HotelReservationSystem.Core.Domain.Enums;

namespace HotelReservationSystem.Application.Dtos.Reservation;

public record ReservationDto
{
    public string Id { get; init; } = string.Empty;
    public DateTime ArrivalDate { get; init; }
    public DateTime DepartureDate { get; init; }
    public int NumberOfGuests { get; init; }
    public decimal TotalPrice { get; init; }
    public string AdditionalRequests { get; init; } = string.Empty;
    public ReservationStatus Status { get; init; }
    public string Reason { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public string DiscountCode { get; init; } = string.Empty;

    public int RoomId { get; init; }
    public string RoomNumber { get; init; } = string.Empty;
    public RoomType RoomType { get; init; }
    public decimal RoomPricePerNight { get; init; }

    public string GuestId { get; init; } = string.Empty;
    public string GuestFirstName { get; init; } = string.Empty;
    public string GuestLastName { get; init; } = string.Empty;
    public string GuestEmail { get; init; } = string.Empty;
    public string GuestPhoneNumber { get; init; } = string.Empty;

    public int? PaymentId { get; init; }
    public PaymentStatus? PaymentStatus { get; init; }
}