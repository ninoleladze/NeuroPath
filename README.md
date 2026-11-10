# NeuroPath

Backend for **NeuroPath**, a brain-training and learning platform. Users work through training plans and cognitive exercises, learn about brain regions and neurotransmitters, track their mood and progress, earn achievements and certificates, and compete on leaderboards and weekly challenges.

> Status: work in progress. The domain model, data layer and service layer are in place; the HTTP controllers are not written yet.

## Domain

- **Learning** — course modules, articles, quizzes, daily tips, FAQ
- **Brain knowledge base** — brain areas and regions, neurotransmitters, cognitive skills
- **Training** — exercises, training plans, daily sessions, skill assessments
- **Progress** — progress snapshots, mood logs, activity log, reports
- **Gamification** — achievements, certificates, leaderboard, weekly challenges, shop
- **Social** — friendships, teams, direct messages, comments, bookmarks, notifications
- **Support** — support tickets and feedback

## Tech Stack

- **.NET 10**, ASP.NET Core Web API
- ASP.NET Core **Identity** + **JWT** authentication
- **EF Core** + SQL Server, database seeding
- AutoMapper, FluentValidation
- MailKit (e-mail), in-memory caching, rate limiting
- Serilog (console + rolling file logs), Swagger

## Project Structure

```
NeuroPath.API/    Program.cs (DI, auth, rate limiting, Swagger), middleware, validation filter
NeuroPath.Core/   Domain models, DTOs, interfaces, services, validators, settings
NeuroPath.Data/   DbContext, entity configurations, seed data
```

Dependency flow: `API → Core`, `API → Data`, `Data → Core`. Core depends only on the `INeuroPathDbContext` abstraction.

## Getting Started

1. Set the required secrets (they are intentionally empty in `appsettings.json`):

```bash
dotnet user-secrets set "JwtSettings:SecretKey" "<random value, at least 32 characters>" --project NeuroPath.API
dotnet user-secrets set "Seed:AdminPassword" "<strong password>" --project NeuroPath.API
dotnet user-secrets set "SmtpSettings:Username" "<smtp user>" --project NeuroPath.API
dotnet user-secrets set "SmtpSettings:Password" "<smtp password>" --project NeuroPath.API
```

2. Create the database:

```bash
dotnet ef migrations add InitialCreate --project NeuroPath.Data --startup-project NeuroPath.API
dotnet ef database update --project NeuroPath.Data --startup-project NeuroPath.API
```

3. Run the API:

```bash
dotnet run --project NeuroPath.API
```

In Development, pending migrations are applied on startup and Swagger is available at `/swagger`. A health endpoint is available at `/health`.
