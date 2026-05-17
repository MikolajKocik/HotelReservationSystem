using HotelReservationSystem.Application.CQRS.Abstractions;
using HotelReservationSystem.Application.CQRS.Reservations.Commands;
using HotelReservationSystem.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Security.Claims;

namespace HotelReservationSystem.MCP.Server.Tools;

public sealed class ReceptionTools
{
    private readonly ICQRSMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly INotificationQueueService _notificationQueueService;

    public ReceptionTools(
        ICQRSMediator mediator,
        IHttpContextAccessor httpContextAccessor,
        INotificationQueueService notificationQueueService) 
    {
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
        _notificationQueueService = notificationQueueService;
    }

    [McpServerTool(Name = "notify_staff")]
    [Description("Wysyła pilne zgłoszenie do personelu hotelowego. Używać tylko w przypadku próśb gości wymagających interwencji (np. brak ręczników, sprzątanie).")]
    public async Task NotifyStaffAsync(
        [Description("Treść zgłoszenia")] string message,
        [Description("Kategoria: 'housekeeping', 'reception', 'technical'")] string category,
        CancellationToken cancellationToken)
    {
        await _notificationQueueService.SendStaffNotificationAsync(message, category, cancellationToken);
    }

    [McpServerTool(Name = "book_room")]
    [Description("Tworzy rezerwację pokoju dla gościa.")]
    public async Task<string> BookRoomAsync(
        [Description("Data przyjazdu (YYYY-MM-DD)")] DateTime arrival,
        [Description("Data wyjazdu (YYYY-MM-DD)")] DateTime departure,
        [Description("ID pokoju")] int roomId,
        [Description("Liczba gości")] int guests)
    {
        ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;
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
            var result = await _mediator.SendAsync(command);
            return $"SUKCES: Rezerwacja została utworzona dla {email}. Numer potwierdzenia: {result}";
        }
        catch (Exception ex)
        {
            return $"Nie udało się dokonać rezerwacji: {ex.Message}";
        }
    }
}
