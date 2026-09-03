$ErrorActionPreference = "Stop"

$SonarUrl = if ($env:SONAR_HOST_URL) { $env:SONAR_HOST_URL } else { "http://localhost:9000" }

if (-not $env:SONAR_TOKEN) {
    Write-Error "SONAR_TOKEN is not set. In SonarQube, create a token under My Account > Security, then run: `$env:SONAR_TOKEN='your-token'"
}

$repoRoot = Split-Path -Parent $PSScriptRoot
$frontend = Join-Path $repoRoot "frontend"
Push-Location $frontend
try {
    npm ci
    npm run build
    npx @sonar/scan `
        -Dsonar.host.url="$SonarUrl" `
        -Dsonar.token="$env:SONAR_TOKEN"
}
finally {
    Pop-Location
}

Write-Host "Frontend analysis submitted. Open $SonarUrl and view 'Task Management Tool - Frontend'."
