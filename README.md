# InTicket 🎟️

A robust **RESTful API** for managing football match ticket bookings. InTicket handles the full lifecycle of ticket management — from user registration and authentication to match scheduling, seat booking, delegation, and Stripe-integrated payment processing.

---

## Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [API Endpoints](#api-endpoints)
- [Testing](#testing)
- [Domain Model](#domain-model)

---

## Overview

InTicket is built on **Clean Architecture** principles, separating concerns across distinct layers (Domain, Application, Infrastructure, Persistence, API). It uses the **CQRS** pattern via MediatR so every operation is a discrete, testable command or query handler.

Key capabilities:
- Secure JWT-based auth with OTP email confirmation and refresh tokens
- Fan-priority booking windows (home/away team fans get early access)
- Pessimistic locking to prevent double-booking under concurrent load
- Ticket delegation — book on behalf of another user
- Stripe payment integration with webhook handling
- Paginated, filterable payment history per user

---

## Architecture

```
InTicket.sln
├── InTicket.Api           → ASP.NET Core Web API (controllers, startup)
├── InTicket.Application   → CQRS handlers, DTOs, contracts (interfaces)
├── InTicket.Domain        → Entities, core business rules
├── InTicket.Infrastructure→ External services (Email via MailKit, Stripe)
├── InTicket.Persistence   → EF Core DbContext, migrations, repositories
└── InTicket.Tests         → xUnit unit tests (per feature)
```

The dependency flow is strictly inward — outer layers depend on inner ones, never the reverse.

```
Api → Application → Domain
Persistence → Application
Infrastructure → Application
```

---

## Features

### 🔐 Authentication
- **Register** — creates account with first/last name, national ID, and email; sends OTP for confirmation
- **Email Confirmation** — validates OTP sent to email before allowing login
- **Resend OTP** — re-sends confirmation code when needed
- **Login** — authenticates via email or username; returns JWT + refresh token
- **Refresh Token** — silently renews expired access tokens
- **Forgot / Reset Password** — OTP-based secure password reset flow
- **Logout** — invalidates the refresh token server-side

### 🏟️ Match Management *(Admin only)*
- Create, activate, and delete matches
- Browse all matches (public) or by ID
- Admin-only activation opens booking for a match

### 🎟️ Ticket Booking
- Book up to **5 tickets** per request
- Supports **fan-priority mode** — during the priority window, only fans of the respective team can book their side's seats
- **Pessimistic locking** on ticket rows prevents race conditions under high load
- Booking a ticket for someone else requires a **delegation relationship**

### 🤝 Delegation (Profile)
- Grant another user (by national ID) the ability to book on your behalf
- View, create, and revoke delegations
- A user can only be the delegator in one active delegation at a time

### 💳 Payments
- Initiates a **Stripe PaymentIntent** for a confirmed booking
- Stripe **webhook** updates payment status on success, failure, or cancellation
- Query paginated payment history (filterable by paid/expired status)
- Retrieve a single payment by ID (ownership enforced)

---

## Tech Stack

| Layer | Technology |
|---|---|
| Runtime | .NET 9 |
| Web Framework | ASP.NET Core |
| ORM | Entity Framework Core |
| Database | SQL Server |
| Auth | ASP.NET Core Identity + JWT Bearer |
| CQRS / Mediator | MediatR |
| Object Mapping | AutoMapper |
| Email | MailKit + Brevo SMTP |
| Payments | Stripe.NET |
| Testing | xUnit, Moq |

---

## Project Structure

```
InTicket.Application/
├── Contracts/              # Repository & service interfaces
├── Feauters/
│   ├── Authentication/     # Register, Login, OTP, Refresh, Logout
│   ├── Booking/            # BookMatchTickets
│   ├── Matches/            # Create, Activate, Delete, Get
│   └── Profile/
│       ├── Commands/       # AddDelegate, DeleteDelegation
│       └── Queries/        # GetMyDelegation, GetMyPayments, GetPayment
├── Responses/              # PagedResult<T>
└── ResourceParameters/     # Filtering/pagination parameters

InTicket.Domain/
└── Entites/
    ├── ApplicationUser.cs  # Extended IdentityUser
    ├── Match.cs
    ├── MatchTicket.cs
    ├── Ticket.cs
    ├── Delegation.cs
    ├── Payment.cs
    ├── RefreshToken.cs
    └── Team.cs

InTicket.Tests/
├── Features/
│   ├── Authentication/     # 6 handler test classes
│   ├── Booking/            # BookMatchTickets handler tests
│   └── Profile/            # 5 handler test classes
└── Helpers/
    └── MockHelpers.cs      # Shared UserManager / SignInManager mocks
```

---

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local or remote)
- A [Stripe](https://stripe.com) account (test mode keys are fine)
- An SMTP provider (Brevo/Sendinblue recommended; any SMTP works)

### 1. Clone the repository

```bash
git clone https://github.com/your-username/InTicket.git
cd InTicket
```

### 2. Configure settings

Copy `appsettings.json` and fill in your values (see [Configuration](#configuration) below):

```bash
cp InTicket.Api/appsettings.json InTicket.Api/appsettings.Development.json
```

### 3. Apply database migrations

```bash
dotnet ef database update --project InTicket.Persistence --startup-project InTicket.Api
```

### 4. Run the API

```bash
dotnet run --project InTicket.Api
```

Swagger UI will be available at `https://localhost:{port}/swagger`.

---

## Configuration

Edit `InTicket.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=InTicketDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "A-secret-key-at-least-32-characters-long",
    "Issuer": "InTicketAPI",
    "Audience": "InTicketClient",
    "Lifetime": 5
  },
  "EmailSettings": {
    "SenderName": "InTicket",
    "SenderEmail": "your@email.com",
    "SmtpServer": "smtp-relay.brevo.com",
    "SmtpPort": "587",
    "SmtpUsername": "your-smtp-username",
    "SmtpPassword": "your-smtp-password"
  },
  "Stripe": {
    "SecretKey": "sk_test_...",
    "PublishableKey": "pk_test_...",
    "WebhookSecret": "whsec_..."
  }
}
```

> **Never commit real credentials.** Use environment variables or [.NET User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) in development.

---

## API Endpoints

### Authentication — `api/auth`

| Method | Route | Auth | Description |
|---|---|---|---|
| `POST` | `/register` | Public | Create a new account |
| `POST` | `/login` | Public | Login and receive JWT + refresh token |
| `POST` | `/confirm_email` | Public | Verify OTP from registration email |
| `POST` | `/resend_confirmation_otp` | Public | Re-send email confirmation OTP |
| `POST` | `/forgot_password` | Public | Request a password-reset OTP |
| `POST` | `/reset_password` | Public | Reset password using OTP |
| `POST` | `/refresh_token` | Public | Exchange refresh token for new JWT |
| `POST` | `/logout` | Bearer | Invalidate refresh token |

### Matches — `api/matches`

| Method | Route | Auth | Description |
|---|---|---|---|
| `GET` | `/` | Public | List all matches (with filters) |
| `GET` | `/{id}` | Public | Get a single match by ID |
| `POST` | `/` | Admin | Create a new match |
| `PUT` | `/{id}/activate` | Admin | Open booking for a match |
| `DELETE` | `/{id}` | Admin | Delete a match |

### Booking — `api/booking`

| Method | Route | Auth | Description |
|---|---|---|---|
| `POST` | `/{matchId}/book` | Bearer | Book up to 5 tickets for a match |
| `POST` | `/{matchId}/complete_payment` | Bearer | Initiate Stripe payment for a booking |
| `POST` | `/webhook` | Public | Stripe webhook for payment events |

### Profile — `api/profile`

| Method | Route | Auth | Description |
|---|---|---|---|
| `GET` | `/delegation` | Bearer | View current delegation |
| `POST` | `/delegation` | Bearer | Delegate booking rights (by national ID) |
| `DELETE` | `/delegation/{id}` | Bearer | Remove an existing delegation |
| `GET` | `/payments` | Bearer | Paginated payment history |
| `GET` | `/payments/{paymentId}` | Bearer | Get a single payment by ID |

---

## Testing

The test suite uses **xUnit** with **Moq** for mocking. Tests are organised by feature, mirroring the application layer structure.

```
InTicket.Tests/
├── Features/
│   ├── Authentication/        # Register, Login, ForgotPassword, ResetPassword,
│   │                          #   EmailConfirmation, ResendOtp handlers
│   ├── Booking/               # BookMatchTickets (including fan-priority,
│   │                          #   delegation checks, locking, rollback)
│   ├── Matches/               # CreateMatch, ActivateMatch, DeleteMatch,
│   │                          #   GetAllMatches, GetMatchById handlers
│   └── Profile/               # AddDelegate, DeleteDelegation,
│                              #   GetMyDelegations, GetPayments, GetPayment
└── Helpers/
    ├── MockHelpers.cs         # Factory methods for UserManager / SignInManager mocks
    ├── BookingHandlerMocks.cs / BookingTestData.cs
    ├── MatchHandlerMocks.cs   / MatchTestData.cs
    └── ProfileHandlerMocks.cs / ProfileTestData.cs
```

Each feature folder contains:
- **`*HandlerMocks.cs`** — wires up Mock repositories and instantiates the real handler under test
- **`*TestData.cs`** — static factory methods that create consistent domain objects and request DTOs
- **`*Tests.cs`** — the actual `[Fact]` test methods (Arrange / Act / Assert)

### Run all tests

```bash
dotnet test
```

### Run tests for a specific feature

```bash
dotnet test --filter FullyQualifiedName~InTicket.Tests.Features.Profile
dotnet test --filter FullyQualifiedName~InTicket.Tests.Features.Booking
dotnet test --filter FullyQualifiedName~InTicket.Tests.Features.Matches
dotnet test --filter FullyQualifiedName~InTicket.Tests.Features.Authentication
```

---

## Domain Model

```
ApplicationUser (IdentityUser)
 ├── NationalId, FirstName, LastName, InTicketId
 ├── FavoriteTeam (→ Team)
 ├── DelegationGiven (→ Delegation)       # one delegation the user can give
 ├── DelegationsReceived (→ Delegation[]) # delegations granted to this user
 ├── Tickets (→ Ticket[])
 └── Payments (→ Payment[])

Match
 ├── HomeTeam / AwayTeam (→ Team)
 ├── BookingStatus (Open | FanPriority | Closed)
 └── MatchTickets (→ MatchTicket[])

MatchTicket
 ├── Class (FirstClass_Left/Right, SecondClass_Left/Right, ...)
 ├── isHomeTeam
 └── IsLocked / LockedByUserId

Delegation
 ├── DelegatorId → ApplicationUser
 └── DelegateId  → ApplicationUser

Payment
 ├── UserId → ApplicationUser
 ├── TicketIds (List<Guid>)
 ├── Price, ExpirationDate, Done
 └── PaymentIntentId (Stripe)
```

---

## License

This project is licensed under the MIT License.
