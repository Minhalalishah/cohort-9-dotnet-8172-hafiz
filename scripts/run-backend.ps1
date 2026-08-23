Set-Location "$PSScriptRoot\..\backend"
dotnet restore
dotnet ef database update --project TaskManagement.Api
dotnet run --project TaskManagement.Api
