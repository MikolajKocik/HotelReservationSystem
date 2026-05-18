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
    public async Task<string> NotifyStaffAsync(
        [Description("Numer pokoju gościa. UWAGA: Jeśli gość nie podał numeru pokoju, NIE WYWOŁUJ tego narzędzia. Zamiast tego zapytaj gościa o numer pokoju.")] 
        int roomId,
        [Description("Zwięzła treść zgłoszenia (max 2 zdania), np. 'Brak ręczników' lub 'Zepsuta klimatyzacja'.")] 
        string message,
        [Description("Kategoria zgłoszenia. DOZWOLONE WARTOŚCI TO TYLKO: 'housekeeping' (sprzątanie, braki), 'reception' (zapytania ogólne), 'technical' (usterki). Nie wolno używać innych wartości!")] 
        string category,
        CancellationToken cancellationToken)
    {
        await _notificationQueueService.SendStaffNotificationAsync(roomId, message, category, cancellationToken);
        return "Zgłoszenie wysłane, dziękujemy i prosimy o cierpliwość";
    }

    [McpServerTool(Name = "book_room")]
    [Description("Tworzy nową rezerwację pokoju dla gościa. Używaj tylko, gdy masz komplet danych i są dostępne pokoje.")]    
    public async Task<string> BookRoomAsync(
        [Description("Data przyjazdu w ścisłym formacie YYYY-MM-DD. Jeśli gość podał datę nieprecyzyjnie (np. 'jutro'), oblicz ją. Jeśli gość w ogóle nie podał daty przyjazdu, NIE WYWOŁUJ narzędzia i dopytaj o nią.")] 
        DateTime arrival,
        [Description("Data wyjazdu w ścisłym formacie YYYY-MM-DD. Jeśli gość nie podał daty wyjazdu, NIE WYWOŁUJ narzędzia i dopytaj o nią.")] 
        DateTime departure,
        [Description("ID pokoju w bazie danych (np. 1, 2, 3). Zobacz ostrzeżenie poniżej.")] 
        int roomId,
        [Description("Liczba gości (osób dorosłych i dzieci). Jeśli brak tej informacji, dopytaj.")] 
        int guests,
        CancellationToken cancellationToken)
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
            string result = await _mediator.SendAsync(command, cancellationToken);
            return $"SUKCES: Rezerwacja została utworzona dla {email}. Numer potwierdzenia: {result}";
        }
        catch (Exception ex)
        {
            return $"Nie udało się dokonać rezerwacji: {ex.Message}";
        }
    }
}
