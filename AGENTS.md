# InTicket — AI Agent Guide

## Project Overview

**InTicket** is a .NET 9 RESTful API for football match ticket bookings. Built with Clean Architecture + CQRS (MediatR), it handles registration (OTP email), JWT auth with refresh tokens, fan-priority booking with pessimistic locking, ticket delegation, and Stripe payments.

## Quick Start

```bash
dotnet restore
dotnet build
dotnet test
# Configure connection string + secrets in appsettings.Development.json
dotnet run --project InTicket.Api
```

## Solution Structure (6 projects)

| Project | Layer | Purpose |
|---|---|---|
| `InTicket.Api` | Presentation | ASP.NET Core controllers, middleware, startup |
| `InTicket.Application` | Application | CQRS commands/queries, handlers, DTOs, service interfaces |
| `InTicket.Domain` | Domain | Entities, enums, core business rules (no external dependencies) |
| `InTicket.Infrastructure` | Infrastructure | MailKit email, Stripe payment implementations |
| `InTicket.Persistence` | Persistence | EF Core DbContext, migrations, repository implementations |
| `InTicket.Tests` | Tests | xUnit + Moq + FluentAssertions (handler-per-feature) |

## Architecture Rules

- **Dependency flow**: `Api → Application → Domain` and `Persistence/Infrastructure → Application`. Domain has zero external dependencies.
- **CQRS**: Every operation is a MediatR `IRequest<T>` + `IRequestHandler<TRequest, TResponse>`.
- **Repository pattern**: Interfaces in `Application/Contracts/`, implementations in `Persistence/Repositories/`.
- **Unit of Work**: Transaction management via `IBaseRepository.BeginTransactionAsync()`. `SaveChangesAsync` should only be called once per transaction by the handler — the repo's `AddAsync` should NOT auto-save.
- **Exceptions in handlers**: Catch exceptions, log via `ILogger<T>`, roll back transaction, return a failure response — never return `null`.

## Key Conventions

### Naming

| Convention | Rule | Example |
|---|---|---|
| Namespaces | Match folder structure (note: current typos exist) | `InTicket.Application.Features.Bookings` |
| Folders | `Features/`, `Entities/`, `Persistence/`, `Infrastructure/` | (currently misspelled as `Feauters/`, `Entites/`, `Presistance/`, `Infrasructure/`) |
| C# properties | PascalCase | `IsUsed`, `IsRevoked`, `TicketStatus` |
| Method names | PascalCase, descriptive | `ChangeTicketStatus` (not `ChangeTicKetStatus`) |
| Route constraints | Single colon | `{id:guid}` (not `{id::guid}`) |

### CQRS Pattern

```
Request/Command (IRequest<TResponse>)
  ↓
Handler (IRequestHandler<TRequest, TResponse>)
  ↓
Repository calls / Service calls
  ↓
Response/Result DTO
```

### Tests

- **Framework**: xUnit v3 + Moq + FluentAssertions
- **Organization**: Mirror `Application/Features/` structure in `Tests/Features/`
- **Pattern**: One test class per handler, one helper mock class per feature area
- **Mock helpers**: `MockHelpers.cs` provides `MockUserManager()` and `MockSignInManager()` factories
- **Run**: `dotnet test` or `dotnet test --filter FullyQualifiedName~InTicket.Tests.Features.{FeatureName}`

## Important Known Issues

- **Security critical**: `IdentityService.cs:19` has `ValidateLifetime = false` — expired JWTs are accepted.
- **Typos across codebase**: Folder names `Feauters/` (→`Features/`), `Entites/` (→`Entities/`), `Presistance/` (→`Persistence/`), `Infrasructure/` (→`Infrastructure/`). These are in namespaces and `using` statements everywhere.
- **Race condition**: `BookMatchTicketsRequestHandler` calls `AnyUserHasBooked` twice (outside and inside transaction) — TOCTOU bug.
- **Overtime**: `RefreshTokenRepository.MarkRefreshTokenAsRevokedAsync` sets `isUsed = true` instead of `isRevoked = true`.
- **Save twice**: `BaseRepository.AddAsync` internally calls `SaveChangesAsync`, and handlers also call it — causes duplicate round-trips.
- **Weak crypto**: `new Random()` used for OTP and refresh token generation.
- **No OTP rate limiting**: `OtpAttempts`/`LastOtpAttemptAt` fields exist on `ApplicationUser` but are never checked.
- **No indexes**: Missing `HasIndex()` on frequently queried columns (`MatchId`, `UserId`, `Token`, `PaymentIntentId`).

## Tech Stack

| Category | Technology |
|---|---|
| Runtime | .NET 9 |
| ORM | EF Core 9 + SQL Server |
| Auth | ASP.NET Core Identity + JWT Bearer |
| CQRS | MediatR 9 |
| Mapping | AutoMapper 12 |
| Email | MailKit + Brevo SMTP |
| Payments | Stripe.NET 50 |
| Tests | xUnit v3 + Moq + FluentAssertions |
| API Docs | Swashbuckle (Swagger) |

## Configuration

Key sections in `appsettings.json`:
- `ConnectionStrings:DefaultConnection` — SQL Server
- `Jwt:Key` — signing key (min 32 chars)
- `EmailSettings` — SMTP for Brevo
- `Stripe` — SecretKey, PublishableKey, WebhookSecret

Secrets should NOT be committed — use `dotnet user-secrets` or environment variables.

## Building & Testing

```bash
dotnet build                # Build all projects
dotnet test                 # Run all tests
dotnet test --no-build      # Re-run without rebuild
dotnet ef migrations add {Name} --project InTicket.Persistence --startup-project InTicket.Api
dotnet ef database update --project InTicket.Persistence --startup-project InTicket.Api
```
