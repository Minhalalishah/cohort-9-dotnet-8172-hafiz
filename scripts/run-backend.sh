#!/usr/bin/env bash
cd "$(dirname "$0")/../backend"
dotnet restore
dotnet ef database update --project TaskManagement.Api
dotnet run --project TaskManagement.Api
