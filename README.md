# Hotel Reservation System - ASP.NET Core MVC

A web application supporting a hotel reservation system, created in the MVC architecture. It allows you to make room reservations, manage them by receptionists and generate reports by the manager.

---

## ✨ Features

- Creating and canceling reservations
- Confirming reservations by the receptionist / manager
- List of reservations, managing room availability
- Handling online payments (Stripe)
- Authorization roles: Guest, Receptionist, Manager
- Generating reports and transactions

---

## 🚀 Technology

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core (code-first)
- Microsoft SQL Server
- Identity (authentication, authorization)
- Stripe API (test payments)
- FluentValidation (.NET)
- Bootstrap 5

---

## 💳 Stripe - configuration

### 1. Create a test account at: https://dashboard.stripe.com/register

### 2. Get keys:
- `STRIPE_API_KEY` (secret key, starts with `sk_test_...`)
- `STRIPE_PUBLISHABLE_KEY` (public key, starts with `pk_test_...`)

### 3. Set them as environment variables:
#### Windows (CMD or PowerShell):
```bash
setx STRIPE_API_KEY "sk_test_..."
setx STRIPE_PUBLISHABLE_KEY "pk_test_..."
```

#### Linux / macOS:
```bash
export STRIPE_API_KEY="sk_test_..."
export STRIPE_PUBLISHABLE_KEY="pk_test_..."
```

**Note:** These keys are required for the payment view to work properly.

---

## 🔧 Local configuration

### 1. Requirements:
- .NET SDK 8+
- SQL Server
- Stripe test account

### 2. Startup:
```bash
dotnet ef database update
```
```bash
dotnet run
```

---

## 📝 Form validation

The project uses FluentValidation to validate data in forms. Validators are automatically registered by:
```csharp
builder.Services.AddValidatorsFromAssemblyContaining<ReservationValidator>();
```

---

## 🚧 Role and user seeding

When the application starts, test users are created:

- **Receptionist**
- login: `recepcja@hotel.pl`
- password: `Test123!`

- **Manager**
- login: `manager@hotel.pl`
- password: `Test123!`

---

## 🏨 Roles and permissions

| Role | Capabilities |
|--------------|-----------------|
| Guest | Creating reservations |
| Receptionist | Confirmation, cancellation, panel, guest list |
| Manager | Everything + reports + availability management |

---

## 📊 Reports and Transactions

- Occupancy and Revenue Report: available only for `Manager` role
- Payment Transaction List (Stripe)

---

## 📒 Directory Structure

```
HotelReservationSystem/
├── Controllers/
├── Data/
│ ├── Configuration/
│ └── Extensions/
├── Migrations/
├── Models/
│ ├── Domain/
│ ├── Dtos/
│ └── ViewModels/
├── Repositories/
│ ├── EF/
│ └── Interfaces/
├── Services/
│ ├── Interfaces/
│ ├── GuestService.cs
│ ├── ReportService.cs
│ ├── ReservationService.cs
│ └── StripeService.cs
├── Validators/
├── Views/
│ ├── Account/
│ ├── Guest/
│ ├── Home/
│ ├── Payment/
│ ├── Reports/
│ ├── Reservation/
│ └── Shared/
├── wwwroot/
├── appsettings.json
└── Program.cs
```

---

## ⚠️ Warning

- Do not include real Stripe keys in your code!

---

## 💳 Sample test Stripe card number:
```
Number: 4242 4242 4242 4242
Date: anytime in the future
CVV: 123
```
