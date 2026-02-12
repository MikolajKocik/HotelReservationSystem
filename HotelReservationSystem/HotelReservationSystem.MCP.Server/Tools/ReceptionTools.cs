using HotelReservationSystem.Application.CQRS.Abstractions;
using HotelReservationSystem.Application.CQRS.Reservations.Commands;
using Microsoft.AspNetCore.Http;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Security.Claims;

namespace HotelReservationSystem.MCP.Server.Tools;

public class ReceptionTools
{
    private readonly ICQRSMediator mediator;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly HttpClient slackClient;

    public ReceptionTools(
        ICQRSMediator mediator,
        IHttpContextAccessor httpContextAccessor,
        IHttpClientFactory httpClientFactory
        )
    {
        this.mediator = mediator;
        this.httpContextAccessor = httpContextAccessor;
        this.slackClient = httpClientFactory.CreateClient("SlackClient");
    }

    [McpServerTool(Name = "notify_staff")]
    [Description("Wysyła pilne zgłoszenie do personelu hotelowego. Używać tylko w przypadku próśb gości wymagających interwencji (np. brak ręczników, sprzątanie).")]
    public async Task<string> NotifyStaffAsync(
        [Description("Treść zgłoszenia")] string message,
        [Description("Kategoria: 'housekeeping', 'reception', 'technical'")] string category)
    {
        var payload = new
        {
            text = $"[ZGŁOSZENIE] Kategoria: {category.ToUpper()} | Treść: {message}"
        };

        var response = await this.slackClient.PostAsJsonAsync("", payload);

        return response.IsSuccessStatusCode
            ? "Zgłoszenie zostało przekazane do personelu"
            : "Wystąpił błąd komunikacji z systemem powiadomień";
    }

    [McpServerTool(Name = "book_room")]
    [Description("Tworzy rezerwację pokoju dla gościa.")]
    public async Task<string> BookRoomAsync(
        [Description("Data przyjazdu (YYYY-MM-DD)")] DateTime arrival,
        [Description("Data wyjazdu (YYYY-MM-DD)")] DateTime departure,
        [Description("ID pokoju")] int roomId,
        [Description("Liczba gości")] int guests)
    {
        ClaimsPrincipal? user = this.httpContextAccessor.HttpContext?.User;
        if (user == null || !user.Identity!.IsAuthenticated)
        {
            return "BŁĄD: Użytkownik nie jest zalogowany. Poproś użytkownika o zalogowanie się, aby dokonać rezerwacji.";
        }

        string email = user.FindFirst(ClaimTypes.Email)?.Value ?? user.Identity.Name!;
        string firstName = user.FindFirst(ClaimTypes.GivenName)?.Value ?? "Gość";
        string lastName = user.FindFirst(ClaimTypes.Surname)?.Value ?? "Online";
        string phone = user.FindFirst(ClaimTypes.MobilePhone)?.Value ?? "123456789";

        var command = new CreateReservationCommand(
            ArrivalDate: arrival,
            DepartureDate: departure,
            RoomId: roomId,
            GuestFirstName: firstName,
            GuestLastName: lastName,
            GuestEmail: email,       
            GuestPhoneNumber: phone, 
            DiscountCode: null,
            AdditionalRequests: "Rezerwacja przez Agenta AI",
            AcceptPrivacy: true,
            NumberOfGuests: guests
        );

        try
        {
            var result = await this.mediator.SendAsync(command);
            return $"SUKCES: Rezerwacja została utworzona dla {email}. Numer potwierdzenia: {result}";
        }
        catch (Exception ex)
        {
            return $"Nie udało się dokonać rezerwacji: {ex.Message}";
        }
    }
}
