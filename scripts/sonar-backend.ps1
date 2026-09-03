$ErrorActionPreference = "Stop"

$SonarUrl = if ($env:SONAR_HOST_URL) { $env:SONAR_HOST_URL } else { "http://localhost:9000" }

if (-not $env:SONAR_TOKEN) {
    Write-Error "SONAR_TOKEN is not set. In SonarQube, create a token under My Account > Security, then run: `$env:SONAR_TOKEN='your-token'"
}

$repoRoot = Split-Path -Parent $PSScriptRoot
$solution = Join-Path $repoRoot "backend/TaskManagement.sln"
$coverage = Join-Path $repoRoot "backend/TaskManagement.Tests/TestResults/coverage.opencover.xml"

if (-not (Get-Command dotnet-sonarscanner -ErrorAction SilentlyContinue)) {
    Write-Host "Installing SonarScanner for .NET..."
    dotnet tool install --global dotnet-sonarscanner
    $env:PATH += ";$env:USERPROFILE\.dotnet\tools"
}

Write-Host "Starting SonarQube analysis for ASP.NET Core backend..."
dotnet-sonarscanner begin `
    /k:"task-management-tool-backend" `
    /n:"Task Management Tool - Backend" `
    /d:sonar.host.url="$SonarUrl" `
    /d:sonar.token="$env:SONAR_TOKEN" `
    /d:sonar.cs.opencover.reportsPaths="$coverage" `
    /d:sonar.exclusions="**/bin/**,**/obj/**,**/Migrations/**" `
    /d:sonar.coverage.exclusions="**/Program.cs,**/Migrations/**"

try {
    dotnet restore $solution
    dotnet build $solution --no-restore

    $testResults = Join-Path $repoRoot "backend/TaskManagement.Tests/TestResults"
    if (Test-Path $testResults) { Remove-Item $testResults -Recurse -Force }

    dotnet test $solution --no-build `
        --collect:"XPlat Code Coverage" `
        --results-directory $testResults `
        -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=opencover

    $generated = Get-ChildItem $testResults -Recurse -Filter "coverage.opencover.xml" | Select-Object -First 1
    if ($generated -and $generated.FullName -ne $coverage) {
        Copy-Item $generated.FullName $coverage -Force
    }
}
finally {
    dotnet-sonarscanner end /d:sonar.token="$env:SONAR_TOKEN"
}

Write-Host "Backend analysis submitted. Open $SonarUrl and view 'Task Management Tool - Backend'."
