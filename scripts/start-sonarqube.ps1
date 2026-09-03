```powershell
$ErrorActionPreference = "Stop"

try {
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "       Starting SonarQube Server" -ForegroundColor Cyan
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host ""

    # Move from scripts folder to project root
    Set-Location "$PSScriptRoot\.."

    Write-Host "Project Root:" -ForegroundColor Yellow
    Write-Host (Get-Location)
    Write-Host ""

    # Check Docker
    Write-Host "Checking Docker..." -ForegroundColor Cyan

    if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
        throw "Docker is not installed or is not available in PATH."
    }

    # Check Docker Engine
    docker info | Out-Null

    if ($LASTEXITCODE -ne 0) {
        throw "Docker is not running. Please start Docker Desktop and try again."
    }

    Write-Host "Docker is running." -ForegroundColor Green
    Write-Host ""

    # Start SonarQube
    Write-Host "Starting SonarQube..." -ForegroundColor Cyan

    docker compose up -d sonarqube

    if ($LASTEXITCODE -ne 0) {
        throw "Docker Compose failed to start SonarQube."
    }

    Write-Host ""
    Write-Host "SonarQube started successfully!" -ForegroundColor Green
    Write-Host ""
    Write-Host "SonarQube URL:" -ForegroundColor Yellow
    Write-Host "http://localhost:9000" -ForegroundColor White
    Write-Host ""

    # Show container status
    Write-Host "Container Status:" -ForegroundColor Cyan
    docker compose ps sonarqube

    Write-Host ""
    Write-Host "Please wait 30-60 seconds for SonarQube to become ready." -ForegroundColor Yellow
}
catch {
    Write-Host ""
    Write-Host "========================================" -ForegroundColor Red
    Write-Host "       SonarQube Startup Failed" -ForegroundColor Red
    Write-Host "========================================" -ForegroundColor Red
    Write-Host ""
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    Write-Host "Troubleshooting:" -ForegroundColor Yellow
    Write-Host "1. Make sure Docker Desktop is running."
    Write-Host "2. Make sure docker-compose.yml exists in the project root."
    Write-Host "3. Run: docker compose ps"
    Write-Host "4. Run: docker logs tasktool-sonarqube"
    Write-Host ""
    exit 1
}
```
