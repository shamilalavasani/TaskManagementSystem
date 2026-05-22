# Portfolio sample — release notes

This repository is a **resume / portfolio example**: a Task Management REST API built with ASP.NET Core 9 and Clean Architecture. It is meant to show how I design and implement a backend service, not to serve as a tutorial or a long-term product roadmap.

**Live repo:** https://github.com/shamilalavasani/TaskManagementSystem

---

## What this project demonstrates

- Layered solution: API, Application, Domain, Infrastructure, Tests  
- Minimal APIs with JWT authentication and ASP.NET Core Identity  
- Role-based authorization (`Admin`, `Manager`, `User`) and per-user todo ownership  
- Categories with stricter rules for create, update, and delete  
- FluentValidation on request DTOs  
- Global exception handling and request logging middleware  
- EF Core 9 with SQL Server and migrations  
- Pagination, filtering, search, and sorting on todos  
- Unit tests (xUnit, Moq) for domain and application services  
- Health endpoint (`GET /health`)  
- Docker support and documented local setup  

---

## How to run locally

See [SETUP.md](SETUP.md) (English) or [SETUP-FA.md](SETUP-FA.md) (Persian).

Quick path from the solution root:

```powershell
.\scripts\setup-local.ps1
cd TaskManagement.API
dotnet run
```

Swagger (Development): `https://localhost:7205/swagger`

---

## Snapshot version

Treated as a **stable portfolio snapshot** suitable for review by recruiters or interviewers. No separate product releases are planned in this repository.

---

## Author

**Shamila Lavasani**

- https://github.com/shamilalavasani  
- https://github.com/shamilalavasani/TaskManagementSystem  
