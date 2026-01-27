using HotelReservationSystem.Core.Domain.Enums;

namespace HotelReservationSystem.Application.Dtos.Reservation;

public class ReservationDto
{
    public string Id { get; set; } = default!;
    public DateTime ArrivalDate { get; set; }
    public DateTime DepartureDate { get; set; }
    public int NumberOfGuests { get; set; }
    public decimal TotalPrice { get; set; }
    public string AdditionalRequests { get; set; } = default!;
    public ReservationStatus Status { get; set; }
    public string Reason { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    
    // Related entities
    public int RoomId { get; set; }
    public string RoomNumber { get; set; } = default!;
    public RoomType RoomType { get; set; }
    public decimal RoomPricePerNight { get; set; }
    
    public string GuestId { get; set; } = default!;
    public string GuestFirstName { get; set; } = default!;
    public string GuestLastName { get; set; } = default!;
    public string GuestEmail { get; set; } = default!;
    
    public int? PaymentId { get; set; }
    public PaymentStatus? PaymentStatus { get; set; }
}