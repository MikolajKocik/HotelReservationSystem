namespace HotelReservationSystem.Core.Domain.Entities;

public sealed partial class Guest
{
    private Guest() 
    {
        this.Id = string.Empty;
        _reservations = new List<Reservation>();
    }

    public Guest(string firstName, string lastName, string email, string phoneNumber)
    {
        ValidateInput(firstName, lastName, email, phoneNumber);
        
        this.Id = $"guest_{Guid.NewGuid()}";
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Email = email;
        this.PhoneNumber = phoneNumber;
        this.CreatedAt = DateTime.UtcNow;
        _reservations = new List<Reservation>();
    }

    public string Id { get; private set; } = default!;
    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string PhoneNumber { get; private set; } = default!;
    public DateTime CreatedAt { get; private set; }

    private readonly List<Reservation> _reservations = new List<Reservation>();
    public IReadOnlyCollection<Reservation> Reservations => _reservations.AsReadOnly();
}