using HotelReservationSystem.Core.Domain.Enums;

namespace HotelReservationSystem.Core.Domain.Entities;

public sealed partial class Room
{
    private Room() 
    {
        _reservations = new List<Reservation>();
    } 

    public Room(string number, RoomType type, decimal pricePerNight, string currency, string? imagePath = null)
    {
        ValidateInput(number, pricePerNight);
        
        this.Id = Random.Shared.Next(1, int.MaxValue);
        this.Number = number;
        this.Type = type;
        this.PricePerNight = pricePerNight;
        this.IsAvailable = true;
        this.ImagePath = imagePath;
        this.CreatedAt = DateTime.UtcNow;
        this.Currency = currency;
        this._reservations = new List<Reservation>();
    }

    public int Id { get; private set; }
    public string Number { get; private set; } = default!;
    public RoomType Type { get; private set; }
    public decimal PricePerNight { get; private set; }
    public string Currency { get; private set; } = default!;
    public bool IsAvailable { get; private set; }
    public string? ImagePath { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private readonly List<Reservation> _reservations = new List<Reservation>();
    public IReadOnlyCollection<Reservation> Reservations => this._reservations.AsReadOnly();
}