namespace HotelReservationSystem.Web.ViewModels.Room;

public sealed record RoomSelectViewModel
{
    public int Id { get; init; }
    public string Number { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public decimal PricePerNight { get; init; }
    public bool IsAvailable { get; init; }
}
