# 🧠 Task Management System (ASP.NET Core Minimal API)

A clean and scalable **Task Management API** built with **ASP.NET Core (.NET 8)** using **Minimal APIs** and **Clean Architecture** principles.

---

## 🚀 Features

* ✅ Minimal API architecture
* ✅ Clean Architecture (API / Application / Domain / Infrastructure)
* ✅ JWT Authentication
* ✅ Role-based & Ownership-based Authorization
* ✅ Global Exception Handling (custom exceptions)
* ✅ Validation using FluentValidation
* ✅ Pagination & Filtering
* ✅ Category management
* ✅ Logging with Middleware

---

## 🧱 Project Structure

```
TaskManagement.API
TaskManagement.Application
TaskManagement.Domain
TaskManagement.Infrastructure
```

* **API** → Endpoints, Middleware, Extensions
* **Application** → Services, DTOs, Interfaces
* **Domain** → Entities, Enums
* **Infrastructure** → EF Core, Repositories, Identity

---

## 🔐 Authentication & Authorization

* JWT-based authentication
* Role-based access (Admin, Manager, User)
* Ownership-based access control (users can only access their own tasks)

---

## ⚙️ Configuration (Important)

This project uses **User Secrets** for sensitive data.

### ❗ You MUST set JWT Secret before running the project:

```bash
dotnet user-secrets set "JwtSettings:Key" "YOUR_SECRET_KEY_HERE"
```

---

## 🧪 Running the Project

1. Clone the repository
2. Set JWT secret (see above)
3. Update database:

```bash
dotnet ef database update
```

4. Run the project:

```bash
dotnet run
```

5. Open Swagger:

```
https://localhost:<port>/swagger
```

---

## 📌 API Highlights

### Todos

* Create Todo
* Get All (with pagination & filtering)
* Get By Id (ownership enforced)
* Update Todo
* Update Status
* Delete (ownership enforced)

### Categories

* Create / Update / Delete
* Unique name validation

---

## 🧠 Key Concepts Implemented

* Custom Exception Handling Middleware
* Claims-based User Context Extraction
* Reusable Ownership Validation
* Clean separation of concerns
* Extension methods for cleaner endpoints

---

## 🛠️ Tech Stack

* ASP.NET Core (.NET 8)
* Entity Framework Core
* SQL Server
* FluentValidation
* JWT Authentication
* Serilog

---

## 📈 Future Improvements

* Unit & Integration Tests
* Docker Support
* Caching (Redis)
* Background Jobs
* Advanced Authorization Policies

---

## 👩‍💻 Author

Shamila Lavasani
