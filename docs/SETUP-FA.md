# راه‌اندازی روی ویندوز (لوکال)

این راهنما فقط برای اجرا روی کامپیوتر خودت است.  
کد روی گیت‌هاب آپلود نمی‌شود؛ همان پوشه‌ای که الان داری را استفاده کن.

---

## پیش‌نیاز

1. نصب .NET 9 SDK  
2. نصب SQL Server (یا LocalDB)  
3. اختیاری: نصب ابزار EF  

```powershell
dotnet tool install --global dotnet-ef
```

---

## روش سریع (پیشنهادی)

در ریشه پروژه (همان جایی که فایل sln هست):

```powershell
.\scripts\setup-local.ps1
```

بعد API را اجرا کن:

```powershell
cd TaskManagement.API
dotnet run
```

Swagger:

```
https://localhost:7205/swagger
```

---

## روش دستی

### گام 1 — کلید JWT

```powershell
cd TaskManagement.API
dotnet user-secrets set "JwtSettings:Key" "LocalDev_JWT_Secret_Key_At_Least_32_Chars!"
```

حداقل 32 کاراکتر لازم است.

### گام 2 — اتصال دیتابیس

در فایل زیر رشته اتصال را با SQL خودت هماهنگ کن:

```
TaskManagement.API/appsettings.json
```

مثال پیش‌فرض:

```
Server=.;Database=TaskManagementDb;Trusted_Connection=True;TrustServerCertificate=True
```

### گام 3 — ساخت دیتابیس

از ریشه solution:

```powershell
dotnet ef database update --project TaskManagement.Infrastructure --startup-project TaskManagement.API
```

### گام 4 — اجرا

```powershell
cd TaskManagement.API
dotnet run
```

---

## تست اولیه در Swagger

1. `POST /auth/register` — ثبت‌نام  
2. توکن را کپی کن  
3. دکمه Authorize — مقدار: `Bearer <token>`  
4. `POST /categories` — فقط نقش Manager یا Admin  
5. `POST /todos` — ساخت کار  

کاربر تازه‌ثبت‌نام‌شده نقش User دارد و فقط کارهای خودش را می‌بیند.

---

## اجرا با Docker (اختیاری)

```powershell
docker compose up --build
```

قبل از اولین درخواست، migration را از ویندوز بزن (اتصال به پورت 1433):

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=TaskManagementDb;User Id=sa;Password=Your_Strong_Password_123!;TrustServerCertificate=True"
dotnet ef database update --project TaskManagement.Infrastructure --startup-project TaskManagement.API
```

API در Docker:

```
http://localhost:8080/swagger
```

---

## Health check

```
GET /health
```

باید وضعیت Healthy برگردد.

---

## فرانت‌اند

این مخزن فقط API است.  
می‌توانی بعداً React یا Blazor جدا بسازی و به همان آدرس‌ها درخواست بزنی.

---

## عیب‌یابی

| مشکل | کار |
|------|-----|
| خطای JwtSettings:Key | اسکریپت setup یا user-secrets را اجرا کن |
| خطای SQL | سرویس SQL را روشن کن و connection string را درست کن |
| 403 روی category | کاربر User فقط خواندن دارد؛ Manager/Admin لازم است |
