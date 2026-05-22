# Run from solution root: .\scripts\setup-local.ps1
param(
    [string]$JwtKey = "LocalDev_JWT_Secret_Key_At_Least_32_Chars!"
)

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $PSScriptRoot
$apiPath = Join-Path $root "TaskManagement.API"

Write-Host "Restoring packages..."
dotnet restore (Join-Path $root "TaskManagementSystem.sln")

Write-Host "Setting JWT User Secret..."
Push-Location $apiPath
dotnet user-secrets set "JwtSettings:Key" $JwtKey
Pop-Location

Write-Host "Applying database migrations..."
dotnet ef database update `
    --project (Join-Path $root "TaskManagement.Infrastructure") `
    --startup-project $apiPath

Write-Host ""
Write-Host "Done. Start API with:"
Write-Host "  cd TaskManagement.API"
Write-Host "  dotnet run"
Write-Host ""
Write-Host "Swagger: https://localhost:7205/swagger"
