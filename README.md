# NeuroPath

[![CI](https://github.com/ninoleladze/NeuroPath/actions/workflows/ci.yml/badge.svg)](https://github.com/ninoleladze/NeuroPath/actions/workflows/ci.yml)

NeuroPath is the backend for a brain-training and learning platform. Users work through training plans and cognitive exercises, take quizzes and courses, learn about brain regions and neurotransmitters, track their mood and progress, earn achievements and certificates, and compete on leaderboards and in weekly challenges.

## Features

- **Learning**: course modules with lessons, articles, quizzes, daily tips and an FAQ
- **Brain knowledge base**: brain areas and regions, neurotransmitters and cognitive skills
- **Training**: exercises with hints and reviews, training plans, daily sessions and skill assessments
- **Progress**: progress snapshots, mood logs, an activity log and reports
- **Gamification**: points ledger, streaks, achievements, certificates, leaderboards, weekly challenges and a shop
- **Social**: friends, teams, direct messages, comments, bookmarks and notifications
- **Support**: support tickets and feedback
- **Accounts**: registration with e-mail verification, password reset, refresh-token rotation and self-service account deletion

The full endpoint list with access rules is in [docs/API.md](docs/API.md).

## Tech stack

- .NET 10, ASP.NET Core Web API
- ASP.NET Core Identity with JWT access tokens and rotating refresh tokens
- EF Core 10 with SQL Server
- FluentValidation, MailKit, Serilog, Swagger
- xUnit, `WebApplicationFactory` and SQLite for tests

## Project structure

```
NeuroPath.API/     Program.cs, controllers, middleware, health checks, background jobs
NeuroPath.Core/    Domain models, DTOs, interfaces, services, validators, settings
NeuroPath.Data/    DbContext, entity configurations, migrations, seed data
NeuroPath.Tests/   Unit, HTTP, authorization-matrix and concurrency tests
docs/              API reference
```

Dependencies flow `API → Core`, `API → Data` and `Data → Core`. Core depends only on the `INeuroPathDbContext` abstraction.

## Running locally

Prerequisites: the .NET 10 SDK and SQL Server (LocalDB is fine on Windows).

1. Set the required secrets. They are intentionally empty in `appsettings.json`:

   ```bash
   dotnet user-secrets set "JwtSettings:SecretKey" "<random value, at least 32 characters>" --project NeuroPath.API
   dotnet user-secrets set "Seed:AdminPassword" "<strong password>" --project NeuroPath.API
   dotnet user-secrets set "SmtpSettings:Username" "<smtp user>" --project NeuroPath.API
   dotnet user-secrets set "SmtpSettings:Password" "<smtp password>" --project NeuroPath.API
   ```

   To develop without SMTP, set `SmtpSettings:LogEmailsInsteadOfSending` to `true`. E-mails are then written to the log.

2. Run the API:

   ```bash
   dotnet run --project NeuroPath.API
   ```

   In Development, pending migrations are applied on startup, the admin account from `Seed:AdminEmail` is created, and Swagger is available at `/swagger`.

3. To apply migrations manually, or after adding one:

   ```bash
   dotnet ef database update --project NeuroPath.Data --startup-project NeuroPath.API
   ```

## Running with Docker

The compose file starts SQL Server and the API, applies migrations and seeds the admin account.

```bash
cp .env.example .env
docker compose up --build
```

Fill in `.env` first:

| Variable | Required | Notes |
|---|---|---|
| `MSSQL_SA_PASSWORD` | yes | At least 8 characters with upper case, lower case, a digit and a symbol. Do not use `;`, because the value goes into the connection string |
| `JWT_SECRET_KEY` | yes | At least 32 random characters |
| `ADMIN_PASSWORD` | yes | Password for the seeded admin account |
| `ADMIN_EMAIL` | no | Defaults to `admin@neuropath.com` |
| `API_PORT` | no | Host port, defaults to `8080` |
| `CLIENT_BASE_URL` | no | Front-end origin for CORS and e-mail links |
| `SMTP_USERNAME`, `SMTP_PASSWORD` | no | Needed only when real e-mails are sent |
| `SMTP_LOG_ONLY` | no | Defaults to `true`, which logs e-mails instead of sending them |

The API listens on `http://localhost:8080`. The database, uploads, logs and data-protection keys are kept in named volumes. Behind a reverse proxy, add the proxy addresses to `ForwardedHeaders:KnownProxies` or `ForwardedHeaders:KnownNetworks` so client IPs and HTTPS are detected correctly.

## Configuration

| Section | Purpose |
|---|---|
| `ConnectionStrings:DefaultConnection` | SQL Server connection |
| `JwtSettings` | Signing key, issuer, audience and token lifetimes |
| `Seed` | Admin account and whether to apply migrations outside Development |
| `ClientApp` | Front-end URL and allowed CORS origins |
| `SmtpSettings` | Outgoing e-mail |
| `FileUploadSettings` | Upload folder, public path, size limits and allowed extensions |
| `RateLimitSettings` | Global, auth and upload rate limits |
| `Gamification` | Daily point cap, scored repeats per exercise and minimum exercise duration |
| `BackgroundJobs` | Maintenance interval, snapshots, token retention and streak reset |
| `ForwardedHeaders` | Trusted proxies and networks |
| `CacheSettings` | Cache lifetimes for reference data |

Any value can be overridden with an environment variable that uses `__` as the separator, for example `JwtSettings__SecretKey`.

## Health checks

| Endpoint | Meaning |
|---|---|
| `GET /health` | Liveness: the process is up. It does not touch the database |
| `GET /health/ready` | Readiness: the database is reachable |

## Security

- Access tokens are checked against the user's security stamp, so logging out, changing the password, blocking a user or deleting an account invalidates existing tokens immediately.
- Refresh tokens are stored hashed and rotated on every use. Reusing an old token outside a short grace period is rejected.
- Login, registration and password-reset endpoints have their own rate limit. All other requests are limited per user or per IP.
- Responses carry `X-Content-Type-Options`, `X-Frame-Options`, `Referrer-Policy`, `Permissions-Policy` and `Cross-Origin-Opener-Policy`. API responses also get a strict `Content-Security-Policy` and `Cache-Control: no-store`.
- Uploaded files are checked by content signature as well as extension. They are stored under random names, and PDFs are served as attachments.

## Tests

```bash
dotnet test
```

Tests run against SQLite and need no SQL Server. They cover services, validation, the HTTP pipeline, an authorization matrix over every endpoint, and concurrent requests such as parallel token refreshes, quiz submissions and achievement checks. Stop a running API before testing, because it locks the build output.

## Continuous integration

GitHub Actions ([.github/workflows/ci.yml](.github/workflows/ci.yml)) restores, builds and tests the solution on every push and pull request to `main`, uploads the test results, and builds the Docker image.
