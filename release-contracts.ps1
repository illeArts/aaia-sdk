<#
.SYNOPSIS
    AAIA.Shared.Contracts — vollautomatischer Release-Workflow

.DESCRIPTION
    Bumpt die Version, committet + taggt aaia-sdk, wartet bis NuGet
    die neue Version indexiert hat, und updated dann alle .csproj im
    Universal-AAIA-Monorepo auf einen Schlag.

.PARAMETER Bump
    patch  (2.0.0 → 2.0.1) — Bugfix / optionale Property          [Default]
    minor  (2.0.0 → 2.1.0) — Neues Interface oder neues DTO
    major  (2.0.0 → 3.0.0) — Breaking Change

.PARAMETER Message
    Optionaler Commit-Message-Override für den aaia-sdk-Commit.

.EXAMPLE
    .\release-contracts.ps1
    .\release-contracts.ps1 -Bump minor
    .\release-contracts.ps1 -Bump patch -Message "feat: add INewInterface"

#>

param(
    [ValidateSet("patch", "minor", "major")]
    [string]$Bump = "patch",

    [string]$Message = ""
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

# ── Pfade ──────────────────────────────────────────────────────────────────────
$SdkRoot     = "H:\AAIAGitHub\aaia-sdk"
$MonorepoRoot = "C:\Users\Andre Iljaschow\OneDrive\Dokumente\Codex\AndreAIAgent\Universal AAIA"
$Csproj      = "$SdkRoot\src\AAIA.Shared.Contracts\AAIA.Shared.Contracts.csproj"

# ── Hilfsfunktionen ────────────────────────────────────────────────────────────
function Write-Step([string]$text) {
    Write-Host "`n── $text" -ForegroundColor Cyan
}

function Write-OK([string]$text) {
    Write-Host "  ✓ $text" -ForegroundColor Green
}

function Write-Warn([string]$text) {
    Write-Host "  ⚠  $text" -ForegroundColor Yellow
}

# ── 1 · Aktuelle Version lesen ─────────────────────────────────────────────────
Write-Step "Lese aktuelle Version aus .csproj"

$content = Get-Content $Csproj -Raw -Encoding UTF8
$match   = [regex]::Match($content, '<Version>([0-9]+\.[0-9]+\.[0-9]+)</Version>')

if (-not $match.Success) {
    throw "Konnte <Version> nicht in $Csproj finden."
}

$current = $match.Groups[1].Value
$parts   = $current.Split('.')
[int]$maj = $parts[0]
[int]$min = $parts[1]
[int]$pat = $parts[2]

switch ($Bump) {
    "major" { $maj++; $min = 0; $pat = 0 }
    "minor" { $min++;            $pat = 0 }
    "patch" {                    $pat++   }
}

$NewVersion = "$maj.$min.$pat"
Write-OK "$current  →  $NewVersion  ($Bump)"

# ── 2 · Version in .csproj setzen ─────────────────────────────────────────────
Write-Step "Version in .csproj setzen"

$updated = $content -replace '<Version>[0-9]+\.[0-9]+\.[0-9]+</Version>',
                              "<Version>$NewVersion</Version>"
Set-Content -Path $Csproj -Value $updated -Encoding UTF8 -NoNewline
Write-OK "AAIA.Shared.Contracts.csproj aktualisiert"

# ── 3 · aaia-sdk committen & pushen ───────────────────────────────────────────
Write-Step "aaia-sdk: commit + push"

Push-Location $SdkRoot
try {
    $commitMsg = if ($Message) { $Message } else { "chore: bump AAIA.Shared.Contracts to $NewVersion" }
    git add -A
    git commit -m $commitMsg
    git push
    Write-OK "Committed + pushed"
} finally {
    Pop-Location
}

# ── 4 · Tag pushen → GitHub Actions → NuGet.org ───────────────────────────────
Write-Step "Tag v$NewVersion pushen (löst NuGet-Publish aus)"

Push-Location $SdkRoot
try {
    git tag "v$NewVersion"
    git push origin "v$NewVersion"
    Write-OK "Tag v$NewVersion gepusht → GitHub Actions läuft jetzt"
} finally {
    Pop-Location
}

# ── 5 · Warten bis NuGet indexiert hat ────────────────────────────────────────
Write-Step "Warte auf NuGet-Indexierung von v$NewVersion ..."

$nugetUrl    = "https://api.nuget.org/v3/registration5-semver1/aaia.shared.contracts/index.json"
$timeoutSecs = 300   # 5 Minuten
$pollSecs    = 15
$elapsed     = 0
$found       = $false

while ($elapsed -lt $timeoutSecs) {
    Start-Sleep -Seconds $pollSecs
    $elapsed += $pollSecs

    try {
        $reg      = Invoke-RestMethod $nugetUrl -ErrorAction Stop
        $versions = $reg.items |
                    ForEach-Object { $_.items } |
                    ForEach-Object { $_.catalogEntry.version }

        if ($versions -contains $NewVersion) {
            Write-OK "NuGet hat v$NewVersion indexiert  ($elapsed s)"
            $found = $true
            break
        }
    } catch {
        # Network-Fehler ignorieren, einfach weiter warten
    }

    Write-Host "  ... noch nicht verfügbar ($elapsed / $timeoutSecs s)" -ForegroundColor DarkGray
}

if (-not $found) {
    Write-Warn "NuGet hat v$NewVersion nach $timeoutSecs s noch nicht indexiert."
    $ans = Read-Host "  Trotzdem mit dem Monorepo-Update weitermachen? (j/n)"
    if ($ans -notin @("j", "J", "y", "Y")) {
        Write-Host "`nAbgebrochen. Starte das Skript erneut wenn NuGet bereit ist." -ForegroundColor Red
        exit 1
    }
}

# ── 6 · Alle .csproj im Monorepo updaten ──────────────────────────────────────
Write-Step "Universal AAIA: alle .csproj auf v$NewVersion setzen"

$count = 0
Get-ChildItem -Path $MonorepoRoot -Recurse -Filter "*.csproj" | ForEach-Object {
    $c = Get-Content $_.FullName -Raw -Encoding UTF8
    if ($c -match 'Include="AAIA\.Shared\.Contracts"') {
        $newC = $c -replace '(Include="AAIA\.Shared\.Contracts"\s+Version=")[^"]*(")',
                            "`${1}$NewVersion`$2"
        Set-Content -Path $_.FullName -Value $newC -Encoding UTF8 -NoNewline
        $count++
        Write-OK $_.Name
    }
}

Write-OK "$count .csproj-Dateien aktualisiert"

# ── 7 · Monorepo committen & pushen ───────────────────────────────────────────
Write-Step "Universal AAIA: commit + push"

Push-Location $MonorepoRoot
try {
    if (Test-Path ".git\index.lock") { Remove-Item ".git\index.lock" -Force }
    git add -A
    git commit -m "chore: update AAIA.Shared.Contracts to $NewVersion"
    git push origin main
    Write-OK "Committed + pushed"
} finally {
    Pop-Location
}

# ── Fertig ────────────────────────────────────────────────────────────────────
Write-Host "`n✅  AAIA.Shared.Contracts v$NewVersion ist live!" -ForegroundColor Green
Write-Host "   NuGet:  https://www.nuget.org/packages/AAIA.Shared.Contracts/$NewVersion"
Write-Host "   GitHub: https://github.com/illeArts/aaia-sdk/releases/tag/v$NewVersion"
