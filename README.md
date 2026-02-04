# HotelReservationSystem

`HotelReservationSystem` to aplikacja ASP.NET Core (net8.0) do zarządzania rezerwacjami hotelowymi. Projekt stosuje CQRS, EF Core oraz wydzielone warstwy: domenę, logikę aplikacyjną, infrastrukturę i warstwę webową.

Główne moduły:

- `HotelReservationSystem.Core` — model domenowy i interfejsy
- `HotelReservationSystem.Application` — logika aplikacyjna, komendy/handlery, serwis Stripe
- `HotelReservationSystem.Infrastructure` — repozytoria, migracje EF, integracje z zewnętrznymi serwisami
- `HotelReservationSystem.Web` — interfejs użytkownika (MVC/Razor), API i webhooki
- `HotelReservationSystem.Workers` — zadania tła (kolejki, przetwarzanie asynchroniczne)

## Funkcjonalności 

- Obsługa rezerwacji: formularze, kody rabatowe, uwagi, zgoda na przetwarzanie danych
- Zarządzanie gośćmi i pokojami
- Integracja z Stripe (sesje/PaymentIntent, webhooki)
- Mechanizm CQRS + background processing dla operacji długotrwałych

## Wymagania

- .NET 8 SDK
- SQL Server (lub inny provider ustawiony w `appsettings.json`)
- Konto Stripe (klucze API)

## Konfiguracja

Konfiguracja znajduje się w `appsettings.json` / `appsettings.Development.json` lub w zmiennych środowiskowych. Wrażliwe wartości nie powinny być śledzone w repozytorium.

Najważniejsze klucze:

- `ConnectionStrings:Default`
- `Stripe:SecretKey`, `Stripe:PublishableKey`, `Stripe:WebhookSecret`

## Szybkie uruchomienie 

1. Przywróć zależności i zbuduj rozwiązanie:

```bash
dotnet restore "HotelReservationSystem/HotelReservationSystem.sln"
dotnet build "HotelReservationSystem/HotelReservationSystem.sln" -clp:Summary
```

2. Zastosuj migracje EF:

```bash
dotnet ef database update --project HotelReservationSystem/HotelReservationSystem.Infrastructure --startup-project HotelReservationSystem/HotelReservationSystem.Web
```

3. Uruchom aplikację Web:

```bash
dotnet run --project HotelReservationSystem/HotelReservationSystem.Web
```

4. Uruchom testy:

```bash
dotnet test HotelReservationSystem/HotelReservationSystem.Tests/HotelReservationSystem.Tests.csproj
```