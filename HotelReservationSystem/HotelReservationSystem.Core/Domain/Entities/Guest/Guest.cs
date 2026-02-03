using Microsoft.AspNetCore.Identity;

namespace HotelReservationSystem.Core.Domain.Entities;

public sealed partial class Guest : IdentityUser
{
    public Guest() : base()
    {
        this.CreatedAt = DateTime.UtcNow;
    }

    public Guest(string email, string phoneNumber, string? firstName = null, string? lastName = null) : this()
    {
        ValidateInput(email, phoneNumber);
        
        this.Id = $"guest_{Guid.NewGuid()}";

        if (!string.IsNullOrWhiteSpace(firstName))
            this.FirstName = firstName;
        if (!string.IsNullOrWhiteSpace(lastName))
            this.LastName = lastName;   
            
        this.Email = email;
        this.UserName = email;
        this.PhoneNumber = phoneNumber;

    } 

    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;    
    public DateTime CreatedAt { get; private set; }


    private readonly List<Reservation> _reservations = new List<Reservation>();
    public IReadOnlyCollection<Reservation> Reservations => this._reservations.AsReadOnly();
}