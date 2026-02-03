using HotelReservationSystem.Core.Domain.Enums;

namespace HotelReservationSystem.Core.Domain.Entities;

public sealed partial class Reservation
{
    private static void ValidateInput(
        DateTime arrivalDate,
        DateTime departureDate,
        int numberOfGuests,
        decimal totalPrice,
        string additionalRequests)
    {
        if (arrivalDate < DateTime.Today)
            throw new ArgumentException("Arrival date cannot be in the past", nameof(arrivalDate));
        
        if (departureDate <= arrivalDate)
            throw new ArgumentException("Departure date must be after arrival date", nameof(departureDate));
        
        if ((departureDate - arrivalDate).Days > 30)
            throw new ArgumentException("Reservation cannot exceed 30 days", nameof(departureDate));
        
        if (numberOfGuests <= 0)
            throw new ArgumentException("Number of guests must be greater than 0", nameof(numberOfGuests));
        
        if (numberOfGuests > 10)
            throw new ArgumentException("Number of guests cannot exceed 10", nameof(numberOfGuests));
        
        if (totalPrice <= 0)
            throw new ArgumentException("Total price must be greater than 0", nameof(totalPrice));
        
        if (totalPrice > 100000)
            throw new ArgumentException("Total price cannot exceed 100000", nameof(totalPrice));

        if (!string.IsNullOrEmpty(additionalRequests) && additionalRequests.Length > 500)
            throw new ArgumentException("Additional requests cannot exceed 500 characters", nameof(additionalRequests));
    }

    public void UpdateStatus(ReservationStatus newStatus, string reason = "")
    {
        if (newStatus == Status)
            return;

        if (newStatus == ReservationStatus.Cancelled && string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Cancellation reason is required", nameof(reason));

        Status = newStatus;
        Reason = reason ?? string.Empty;
    }

    public bool CanBeCancelled()
    {
        return Status == ReservationStatus.Pending || Status == ReservationStatus.Confirmed;
    }

    public bool IsActive()
    {
        return Status == ReservationStatus.Confirmed && 
               DateTime.Today >= ArrivalDate && 
               DateTime.Today <= DepartureDate;
    }

    public int GetStayDuration()
    {
        return (DepartureDate - ArrivalDate).Days;
    }

    public void SetPayment(Payment payment)
    {
        Payment = payment;
        PaymentId = payment.Id;
    }
}