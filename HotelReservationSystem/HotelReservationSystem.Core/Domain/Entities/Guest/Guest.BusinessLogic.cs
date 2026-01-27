namespace HotelReservationSystem.Core.Domain.Entities;

public sealed partial class Guest
{
    public void UpdateContactInfo(string email, string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number cannot be empty", nameof(phoneNumber));

        this.Email = email;
        this.PhoneNumber = phoneNumber;
    }
}