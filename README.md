# Task Management System

REST API for managing personal and team tasks, built with **ASP.NET Core 9**, **Minimal APIs**, and **Clean Architecture**.

**Repository:** [github.com/shamilalavasani/TaskManagementSystem](https://github.com/shamilalavasani/TaskManagementSystem)

**Portfolio notes:** [docs/RELEASE.md](docs/RELEASE.md)

## Features

- Minimal API endpoints with Swagger (Development only)
- Clean Architecture: API, Application, Domain, Infrastructure
- JWT authentication and ASP.NET Core Identity
- Role-based authorization: `Admin`, `Manager`, `User`
- Ownership-based access for todos (users see only their own tasks unless Admin/Manager)
- Categories with role-gated create/update/delete
- FluentValidation on request DTOs
- Pagination, filtering, search, and sorting for todos and categories
- Global exception handling middleware
- Request/response logging middleware
- Serilog logging to console and SQL Server
- Unit tests (xUnit, Moq, FluentAssertions)

## Tech stack

| Layer | Technologies |
|-------|----------------|
| Runtime | .NET 9 |
| API | ASP.NET Core Minimal APIs, Swashbuckle |
| Data | EF Core 9, SQL Server |
| Auth | ASP.NET Core Identity, JWT Bearer |
| Validation | FluentValidation |
| Logging | Serilog (Console + MSSqlServer sink) |
| Tests | xUnit, Moq, FluentAssertions |

## Solution structure

```
TaskManagementSystem/
├── TaskManagement.API/              # HTTP host, endpoints, middleware, Swagger
├── TaskManagement.Application/      # Services, DTOs, validators, repository contracts
├── TaskManagement.Domain/           # Entities and enums (no framework dependencies)
├── TaskManagement.Infrastructure/   # EF Core, Identity, repositories, migrations
└── TaskManagement.Tests/            # Unit tests for Domain and Application
```

### Layer responsibilities

| Project | Responsibility |
|---------|----------------|
| **Domain** | `TodoItem`, `Category`, business rules (status transitions, validation) |
| **Application** | Use cases, DTOs, FluentValidation, custom exceptions |
| **Infrastructure** | `AppDbContext`, repositories, `AuthService`, JWT token generation |
| **API** | Route mapping, authorization policies, middleware, DI composition |

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB, Express, or full instance)
- EF Core tools (optional, for migrations):

  ```bash
  dotnet tool install --global dotnet-ef
  ```

## Local setup (Windows)

| Language | Guide |
|----------|--------|
| English | [docs/SETUP.md](docs/SETUP.md) |
| Persian | [docs/SETUP-FA.md](docs/SETUP-FA.md) |

One-command setup from solution root:

```powershell
.\scripts\setup-local.ps1
cd TaskManagement.API
dotnet run
```

Copy [.env.example](.env.example) for required environment variable names.

## Quick start

### 1. Clone and restore

```bash
git clone https://github.com/shamilalavasani/TaskManagementSystem.git
cd TaskManagementSystem
dotnet restore
```

### 2. Configure JWT secret (required)

The signing key must **not** be committed. Set it via User Secrets from the API project:

```bash
cd TaskManagement.API
dotnet user-secrets set "JwtSettings:Key" "YOUR_LONG_RANDOM_SECRET_AT_LEAST_32_CHARS"
```

For production, use environment variables or your host's secret store instead of User Secrets.

### 3. Database connection

Default connection string in `TaskManagement.API/appsettings.json`:

```
Server=.;Database=TaskManagementDb;Trusted_Connection=True;TrustServerCertificate=True
```

Override for your environment in `appsettings.Development.json`, User Secrets, or environment variables:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=...;Database=...;..."
```

### 4. Apply migrations

From the solution root:

```bash
dotnet ef database update --project TaskManagement.Infrastructure --startup-project TaskManagement.API
```

### 5. Run the API

```bash
cd TaskManagement.API
dotnet run
```

- HTTP: `http://localhost:5013`
- HTTPS: `https://localhost:7205`
- Swagger UI (Development): `https://localhost:7205/swagger`

On startup, default roles (`Admin`, `Manager`, `User`) are seeded automatically.

## Configuration

| Setting | Location | Description |
|---------|----------|-------------|
| `ConnectionStrings:DefaultConnection` | `appsettings.json` / secrets / env | SQL Server |
| `JwtSettings:Key` | User Secrets / env | JWT signing key (**required**) |
| `JwtSettings:Issuer` | `appsettings.json` | Token issuer (`TaskManagementApp`) |
| `JwtSettings:Audience` | `appsettings.json` | Token audience (`TaskManagementUsers`) |
| `JwtSettings:ExpireMinutes` | `appsettings.json` | Token lifetime (default: 60) |
| `Serilog` | `appsettings.json` | Console + SQL table `Logs` |

## Authentication and authorization

### Register and login

| Method | Path | Auth |
|--------|------|------|
| POST | `/auth/register` | Public |
| POST | `/auth/login` | Public |

Response includes `accessToken`, `expireAt`, and `email`. Send the token on protected routes:

```
Authorization: Bearer <token>
```

New users are assigned the **User** role by default.

### Roles

| Role | Todos | Categories |
|------|-------|------------|
| **User** | Own todos only | Read only |
| **Manager** | All todos | Create, update |
| **Admin** | All todos | Create, update, delete |

### Protected route policies

- Most todo and category routes require `UserOrAbove`.
- Category create/update: `CanManageCategories` (Manager, Admin).
- Category delete: `CanDeleteCategories` (Admin only).

## API reference

Base URL: `/` (see launch settings for host/port).

### Todos (`/todos`) — requires JWT

| Method | Path | Description |
|--------|------|-------------|
| GET | `/todos` | List with pagination, filters, search, sort |
| GET | `/todos/{id}` | Get by id (ownership enforced for User) |
| POST | `/todos` | Create (owner = current user) |
| PUT | `/todos/{id}` | Update details and status |
| PATCH | `/todos/{id}/status` | Update completion status only |
| DELETE | `/todos/{id}` | Delete (ownership enforced for User) |
| GET | `/todos/overdue` | Overdue items |
| GET | `/todos/due-next-7-days` | Items due within 7 days |

**Query parameters** (`GET /todos`): `pageNumber`, `pageSize`, `status`, `dueBefore`, `dueAfter`, `search`, `sortBy`, `sortDirection`.

### Categories (`/categories`) — requires JWT

| Method | Path | Auth policy |
|--------|------|-------------|
| GET | `/categories` | UserOrAbove |
| GET | `/categories/{id}` | UserOrAbove |
| POST | `/categories` | CanManageCategories |
| PUT | `/categories/{id}` | CanManageCategories |
| DELETE | `/categories/{id}` | CanDeleteCategories |

## Todo status workflow

```
Pending → InProgress | Cancelled
InProgress → Completed | Cancelled
Completed / Cancelled → (no further changes)
```

## Build and test

```bash
dotnet build -c Release
dotnet test -c Release
```

## EF Core migrations

Add a new migration:

```bash
dotnet ef migrations add <MigrationName> --project TaskManagement.Infrastructure --startup-project TaskManagement.API
```

Apply to database:

```bash
dotnet ef database update --project TaskManagement.Infrastructure --startup-project TaskManagement.API
```

## Health check

```
GET /health
```

## Docker (optional)

```bash
docker compose up --build
```

API: `http://localhost:8080` — run EF migrations from the host against `localhost,1433` before first use (see [docs/SETUP.md](docs/SETUP.md) or [docs/SETUP-FA.md](docs/SETUP-FA.md)).

## Portfolio

This repo is a backend portfolio sample. Scope and demo features are summarized in [docs/RELEASE.md](docs/RELEASE.md).

## Author

**Shamila Lavasani**

- GitHub: [shamilalavasani](https://github.com/shamilalavasani)
- Project: [TaskManagementSystem](https://github.com/shamilalavasani/TaskManagementSystem)
