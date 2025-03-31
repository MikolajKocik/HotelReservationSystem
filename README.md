# Hotel Reservation System - ASP.NET Core MVC

Aplikacja webowa wspierająca system rezerwacji hotelowych, stworzona w architekturze MVC. Umożliwia dokonywanie rezerwacji pokoi, zarządzanie nimi przez recepcjonistów oraz generowanie raportów przez kierownika.

---

## ✨ Funkcje

- Tworzenie i anulowanie rezerwacji
- Potwierdzanie rezerwacji przez recepcjonistę / kierownika
- Lista rezerwacji, zarządzanie dostępnością pokoi
- Obsługa płatności online (Stripe)
- Autoryzacja ról: Gość, Recepcjonista, Kierownik
- Generowanie raportów i transakcji

---

## 🚀 Technologia

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core (code-first)
- Microsoft SQL Server
- Identity (autentykacja, autoryzacja)
- Stripe API (testowe płatności)
- FluentValidation (.NET)
- Bootstrap 5

---

## 💳 Stripe - konfiguracja

### 1. Załóż konto testowe na: https://dashboard.stripe.com/register

### 2. Uzyskaj klucze:
- `STRIPE_API_KEY` (secret key, zaczyna się od `sk_test_...`)
- `STRIPE_PUBLISHABLE_KEY` (public key, zaczyna się od `pk_test_...`)

### 3. Ustaw je jako zmienne środowiskowe:
#### Windows (CMD lub PowerShell):
```bash
setx STRIPE_API_KEY "sk_test_..."
setx STRIPE_PUBLISHABLE_KEY "pk_test_..."
```

#### Linux / macOS:
```bash
export STRIPE_API_KEY="sk_test_..."
export STRIPE_PUBLISHABLE_KEY="pk_test_..."
```

**Uwaga:** Klucze te są wymagane do poprawnego działania widoku płatności.

---

## 🔧 Konfiguracja lokalna

### 1. Wymagania:
- .NET SDK 8+
- SQL Server
- Stripe test account

### 2. Uruchomienie:
```bash
dotnet ef database update
```
```bash
dotnet run
```

---

## 📝 Walidacja formularzy

W projekcie wykorzystano FluentValidation do walidacji danych w formularzach. Walidatory są automatycznie rejestrowane przez:
```csharp
builder.Services.AddValidatorsFromAssemblyContaining<ReservationValidator>();
```

---

## 🚧 Seedowanie ról i użytkowników

Przy starcie aplikacji tworzeni są użytkownicy testowi:

- **Recepcjonista**
  - login: `recepcja@hotel.pl`
  - hasło: `Test123!`

- **Kierownik**
  - login: `manager@hotel.pl`
  - hasło: `Test123!`

---

## 🏨 Role i uprawnienia

| Rola         | Możliwości |
|--------------|------------------|
| Gość        | Tworzenie rezerwacji |
| Recepcjonista| Potwierdzanie, anulowanie, panel, lista gości |
| Kierownik    | Wszystko + raporty + zarządzanie dostępnością |

---

## 📊 Raporty i transakcje

- Raport zajętości i przychodów: dostępne tylko dla roli `Kierownik`
- Lista transakcji płatności (Stripe)

---

## 📒 Struktura katalogów

```
HotelReservationSystem/
├── Controllers/
├── Data/
│   ├── Configuration/
│   └── Extensions/
├── Migrations/
├── Models/
│   ├── Domain/
│   ├── Dtos/
│   └── ViewModels/
├── Repositories/
│   ├── EF/
│   └── Interfaces/
├── Services/
│   ├── Interfaces/
│   ├── GuestService.cs
│   ├── ReportService.cs
│   ├── ReservationService.cs
│   └── StripeService.cs
├── Validators/
├── Views/
│   ├── Account/
│   ├── Guest/
│   ├── Home/
│   ├── Payment/
│   ├── Reports/
│   ├── Reservation/
│   └── Shared/
├── wwwroot/
├── appsettings.json
└── Program.cs
```

---


## ⚠️ Uwaga

- Nie umieszczaj prawdziwych kluczy Stripe w kodzie!

---

## 💳 Przykładowy testowy numer karty Stripe:
```
Numer: 4242 4242 4242 4242
Data: dowolna w przyszłości
CVV: 123
```

