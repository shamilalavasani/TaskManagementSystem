# راه‌اندازی لوکال (ویندوز)

راهنمای گام‌به‌گام اجرای API روی کامپیوتر خودت.

## پیش‌نیاز

1. نصب [.NET 9 SDK](https://dotnet.microsoft.com/download)
2. SQL Server (یا LocalDB)
3. اختیاری: ابزار EF Core

```powershell
dotnet tool install --global dotnet-ef
```

## راه‌اندازی سریع (پیشنهادی)

از ریشه solution (جایی که `TaskManagementSystem.sln` است):

```powershell
.\scripts\setup-local.ps1
```

سپس API را اجرا کن:

```powershell
cd TaskManagement.API
dotnet run
```

Swagger:

```
https://localhost:7205/swagger
```

## راه‌اندازی دستی

### گام ۱ — کلید JWT

```powershell
cd TaskManagement.API
dotnet user-secrets set "JwtSettings:Key" "LocalDev_JWT_Secret_Key_At_Least_32_Chars!"
```

حداقل ۳۲ کاراکتر لازم است.

### گام ۲ — اتصال دیتابیس

فایل زیر را ویرایش کن:

```
TaskManagement.API/appsettings.json
```

مثال پیش‌فرض:

```
Server=.;Database=TaskManagementDb;Trusted_Connection=True;TrustServerCertificate=True
```

### گام ۳ — اعمال migration

از ریشه solution:

```powershell
dotnet ef database update --project TaskManagement.Infrastructure --startup-project TaskManagement.API
```

### گام ۴ — اجرا

```powershell
cd TaskManagement.API
dotnet run
```

## تست اول در Swagger

1. `POST /auth/register` — ثبت‌نام
2. توکن را از پاسخ کپی کن
3. **Authorize** — مقدار: `Bearer <token>`
4. `POST /categories` — فقط نقش **Manager** یا **Admin**
5. `POST /todos` — ساخت کار

کاربر جدید نقش **User** دارد و فقط کارهای خودش را می‌بیند.

## Docker (اختیاری)

```powershell
docker compose up --build
```

قبل از اولین درخواست، migration را از ویندوز بزن (SQL روی پورت 1433):

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=TaskManagementDb;User Id=sa;Password=Your_Strong_Password_123!;TrustServerCertificate=True"
dotnet ef database update --project TaskManagement.Infrastructure --startup-project TaskManagement.API
```

API در Docker:

```
http://localhost:8080/swagger
```

## Health check

```
GET /health
```

باید **Healthy** برگردد.

## فرانت‌اند

این مخزن فقط API است. می‌توانی React یا Blazor جدا بسازی و با JWT به همین endpointها وصل شوی.

## عیب‌یابی

| مشکل | راه‌حل |
|------|--------|
| خطای `JwtSettings:Key` | `setup-local.ps1` را اجرا کن یا User Secrets را تنظیم کن |
| خطای اتصال SQL | SQL Server را روشن کن و connection string را درست کن |
| 403 روی categories | نقش **User** فقط خواندن دارد؛ **Manager** یا **Admin** لازم است |

## نسخه انگلیسی

راهنمای انگلیسی: [SETUP.md](SETUP.md)
