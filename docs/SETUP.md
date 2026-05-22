# Local setup (Windows)

Step-by-step guide to run the API on your machine.

## Prerequisites

1. [.NET 9 SDK](https://dotnet.microsoft.com/download)
2. SQL Server (or LocalDB)
3. Optional: EF Core CLI

```powershell
dotnet tool install --global dotnet-ef
```

## Quick setup (recommended)

From the solution root (where `TaskManagementSystem.sln` is):

```powershell
.\scripts\setup-local.ps1
```

Then run the API:

```powershell
cd TaskManagement.API
dotnet run
```

Swagger UI:

```
https://localhost:7205/swagger
```

## Manual setup

### Step 1 — JWT secret

```powershell
cd TaskManagement.API
dotnet user-secrets set "JwtSettings:Key" "LocalDev_JWT_Secret_Key_At_Least_32_Chars!"
```

The key must be at least 32 characters.

### Step 2 — Database connection

Edit the connection string in:

```
TaskManagement.API/appsettings.json
```

Default example:

```
Server=.;Database=TaskManagementDb;Trusted_Connection=True;TrustServerCertificate=True
```

### Step 3 — Apply migrations

From the solution root:

```powershell
dotnet ef database update --project TaskManagement.Infrastructure --startup-project TaskManagement.API
```

### Step 4 — Run

```powershell
cd TaskManagement.API
dotnet run
```

## First test in Swagger

1. `POST /auth/register` — create a user
2. Copy the token from the response
3. Click **Authorize** — value: `Bearer <token>`
4. `POST /categories` — requires **Manager** or **Admin** role
5. `POST /todos` — create a task

New users get the **User** role and only see their own todos.

## Docker (optional)

```powershell
docker compose up --build
```

Before the first request, apply migrations from Windows (SQL on port 1433):

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=TaskManagementDb;User Id=sa;Password=Your_Strong_Password_123!;TrustServerCertificate=True"
dotnet ef database update --project TaskManagement.Infrastructure --startup-project TaskManagement.API
```

API in Docker:

```
http://localhost:8080/swagger
```

## Health check

```
GET /health
```

Should return **Healthy**.

## Frontend

This repository is API-only. You can add a separate React or Blazor app that calls the same endpoints with JWT.

## Troubleshooting

| Issue | Fix |
|-------|-----|
| `JwtSettings:Key` error | Run `setup-local.ps1` or set User Secrets |
| SQL connection error | Start SQL Server and fix the connection string |
| 403 on categories | **User** role is read-only; use **Manager** or **Admin** |
