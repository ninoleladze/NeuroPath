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
NeuroPath.API/    Program.cs (DI, auth, rate limiting, Swagger), exception-handling middleware
NeuroPath.Core/   Domain models, DTOs, interfaces, 35 services, settings
NeuroPath.Data/   DbContext and seed data
```

## Getting Started

```bash
dotnet ef migrations add InitialCreate --project NeuroPath.Data --startup-project NeuroPath.API
dotnet ef database update --project NeuroPath.Data --startup-project NeuroPath.API
dotnet run --project NeuroPath.API
```

Set `JwtSettings:SecretKey` and the `SmtpSettings` in `NeuroPath.API/appsettings.json` (or with user secrets) before running.
