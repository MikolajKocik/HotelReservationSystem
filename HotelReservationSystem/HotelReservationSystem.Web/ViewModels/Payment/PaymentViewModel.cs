namespace HotelReservationSystem.Web.ViewModels;

public class PaymentViewModel
{
    public string ReservationId { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string ClientSecret { get; set; } = string.Empty;
    public string PublishableKey { get; set; } = string.Empty;
    public string Currency { get; set; } = "PLN";
}