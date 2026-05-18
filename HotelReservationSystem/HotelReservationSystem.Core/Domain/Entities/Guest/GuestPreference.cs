namespace HotelReservationSystem.Core.Domain.Entities.GuestPref;

public sealed class GuestPreference
{
    public string Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty; 
    public string Value { get; private set; } = string.Empty;   
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public GuestPreference(string email, string category, string value)
    {
        Id = $"{Guid.NewGuid()}";
        Email = email;
        Category = category;
        Value = value;
        CreatedAt = DateTime.UtcNow;
    }
}