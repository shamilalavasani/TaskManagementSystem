# Release checklist (v1.0.0)

Use this document to take the project from “works locally” to a tagged **GitHub Release** you can deploy with confidence.

## Phase 1 — Must fix before release

### Security and authorization

- [ ] **Filter overdue / due-soon todos by owner**  
  `GET /todos/overdue` and `GET /todos/due-next-7-days` currently return all users’ tasks. Pass `userId` and `isAdminOrManager` into the service (same pattern as `GetAllTodoItemsAsync`) and filter in the repository.

- [ ] **Validate auth DTOs**  
  Add FluentValidation for `RegisterRequestDto` and `LoginRequestDto`, and apply `ValidationFilter` on `/auth/register` and `/auth/login`.

- [ ] **Require JWT key at startup**  
  Fail fast if `JwtSettings:Key` is missing or too short (avoid null reference at runtime).

### Configuration

- [ ] Add `appsettings.Production.json` with safe defaults (no local SQL Server string).
- [ ] Read secrets from environment variables in Production:
  - `ConnectionStrings__DefaultConnection`
  - `JwtSettings__Key`
- [ ] Document all required env vars in README.

### Database

- [ ] Verify all migrations apply cleanly on an empty database.
- [ ] Decide migration strategy for production (run on deploy vs. separate job).
- [ ] Optional: seed a default Admin user via a one-time script or documented SQL (roles already seed; users do not).

## Phase 2 — Quality gate

### Tests

- [ ] Run `dotnet test -c Release` and fix any failures.
- [ ] Add tests for overdue/due-soon ownership filtering (after fix).
- [ ] Optional: integration tests with `WebApplicationFactory` + Testcontainers SQL Server.

### Code health

- [ ] Remove empty placeholder folders or use them (e.g. `Application/Mappings`).
- [ ] Fix inconsistent file names (validators with trailing space in filename).
- [ ] Review Serilog SQL sink in Production (volume, retention, PII in logs).

## Phase 3 — Production readiness

### API behavior

- [ ] Confirm Swagger is **disabled** outside Development (already in `Program.cs`).
- [ ] Add health checks: `AddHealthChecks()` + `MapHealthChecks("/health")` (DB connectivity).
- [ ] Optional: API versioning or OpenAPI export for consumers.

### Observability

- [ ] Structured logging fields (correlation id in `LoggingMiddleware`).
- [ ] Production log level: Warning for `Microsoft.*`, Information for app.

### Containerization (recommended)

- [ ] Add multi-stage `Dockerfile` (build API, run on ASP.NET runtime).
- [ ] Add `docker-compose.yml` with API + SQL Server for local/staging.
- [ ] Document `docker compose up` in README.

## Phase 4 — Automation and governance

### Repository files

- [ ] Add `LICENSE` (MIT or your choice).
- [ ] Add `CHANGELOG.md` (Keep a Changelog format).
- [ ] Add `.github/workflows/ci.yml`:
  - `dotnet restore`
  - `dotnet build -c Release`
  - `dotnet test -c Release`
  - Optional: publish artifact

### Versioning

- [ ] Align version in one place (e.g. `<Version>1.0.0</Version>` in API csproj or `Directory.Build.props`).
- [ ] Tag release: `git tag -a v1.0.0 -m "First stable release"`
- [ ] Create GitHub Release from tag with notes from CHANGELOG.

## Phase 5 — Deploy

Pick one target and document it in README:

| Target | Typical steps |
|--------|----------------|
| **Azure App Service** | Publish API, Azure SQL, App Settings for connection + JWT |
| **IIS / Windows Server** | `dotnet publish`, install hosting bundle, set env vars |
| **Docker host / VPS** | Push image, run with env vars, run migrations before traffic |

### Pre-deploy smoke test

1. Register a user → receive JWT.
2. Create category (as Manager/Admin).
3. Create todo → list → update status → delete.
4. Confirm User cannot access another user’s todo (403).
5. Call `/health` (after implemented).

## Suggested timeline

| Week | Focus |
|------|--------|
| 1 | Phase 1 (security + production config) |
| 2 | Phase 2–3 (tests, Docker, health checks) |
| 3 | Phase 4 (CI, LICENSE, CHANGELOG, tag v1.0.0) |
| 4 | Phase 5 (deploy + smoke test + GitHub Release notes) |

## Definition of done for v1.0.0

- CI green on `main`
- All Phase 1 items complete
- README and RELEASE.md match actual behavior
- Tagged `v1.0.0` with published GitHub Release
- Production instance running with secrets outside source control
