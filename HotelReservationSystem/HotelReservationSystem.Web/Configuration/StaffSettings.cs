namespace HotelReservationSystem.Web.Configuration;

public sealed class StaffSettings
{
    public List<string> ExcludedEmails { get; set; } = new();
    public StaffAccount Recepcionist { get; set; } = new();
    public StaffAccount Manager { get; set; } = new();
}

public sealed class StaffAccount
{
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
