namespace HotelReservationSystem.Application.Dtos.Room;

public sealed record RoomAvailabilityDto
{
    public int Id { get; init; }
    public string Number { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public decimal PricePerNight { get; init; }
    public string Currency { get; init; } = string.Empty;
    public string StatusLabel { get; init; } = string.Empty;
    public string Text { get; init; } = string.Empty;
}
