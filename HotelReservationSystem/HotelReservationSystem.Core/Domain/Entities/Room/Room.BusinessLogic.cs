using System.Globalization;
using HotelReservationSystem.Core.Domain.Enums;

namespace HotelReservationSystem.Core.Domain.Entities;

public sealed partial class Room
{
    private static void ValidateInput(string number, decimal pricePerNight)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException("Room number cannot be empty", nameof(number));
        
        if (number.Length > 10)
            throw new ArgumentException("Room number cannot exceed 10 characters", nameof(number));
        
        if (pricePerNight <= 0)
            throw new ArgumentException("Price per night must be greater than 0", nameof(pricePerNight));
        
        if (pricePerNight > 10000)
            throw new ArgumentException("Price per night cannot exceed 10000", nameof(pricePerNight));
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new ArgumentException("Price must be greater than 0", nameof(newPrice));
        
        if (newPrice > 10000)
            throw new ArgumentException("Price cannot exceed 10000", nameof(newPrice));

        this.PricePerNight = newPrice;
    }

    public void SetAvailability(bool isAvailable)
    {
        this.IsAvailable = isAvailable;
    }

    public void UpdateImage(string? imagePath)
    {
        this.ImagePath = imagePath;
    }

    public bool CanBeReserved(DateTime arrivalDate, DateTime departureDate)
    {
        if (!this.IsAvailable) return false;
        
        return !this.Reservations.Any(r => 
            r.Status == ReservationStatus.Confirmed &&
            arrivalDate < r.DepartureDate && 
            departureDate > r.ArrivalDate);
    }
}