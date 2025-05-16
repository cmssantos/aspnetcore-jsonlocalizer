param(
    [Parameter(Mandatory = $true)][string]$ApiKey,
    [string]$Version,
    [string]$Suffix
)

$dirBuildPath = "Directory.Build.props"

if (-not (Test-Path $dirBuildPath)) {
    Write-Error "❌ Arquivo $dirBuildPath não encontrado."
    exit 1
}

$propsContent = Get-Content $dirBuildPath

# --- Version -----------------------------------------------------------
if (-not $Version) {
    $versionMatch = [regex]::Match($propsContent -join "`n", '<Version>(.*?)</Version>')
    if (-not $versionMatch.Success) {
        Write-Error "❌ Tag <Version> não encontrada no $dirBuildPath"
        exit 1
    }
    $Version = $versionMatch.Groups[1].Value
}

# --- VersionSuffix -----------------------------------------------------
if (-not $Suffix) {
    $suffixMatch = [regex]::Match($propsContent -join "`n", '<VersionSuffix>(.*?)</VersionSuffix>')
    if (-not $suffixMatch.Success) {
        Write-Error "❌ Tag <VersionSuffix> não encontrada no $dirBuildPath"
        exit 1
    }
    $Suffix = $suffixMatch.Groups[1].Value
}

# --- BuildNumber -------------------------------------------------------
$buildMatch = [regex]::Match($propsContent -join "`n", '<BuildNumber>(\d+)</BuildNumber>')
if (-not $buildMatch.Success) {
    Write-Error "❌ Tag <BuildNumber> não encontrada no $dirBuildPath"
    exit 1
}
$buildNumber = [int]$buildMatch.Groups[1].Value + 1

# Atualiza apenas o BuildNumber
$updatedProps = $propsContent -replace '<BuildNumber>\d+</BuildNumber>', "<BuildNumber>$buildNumber</BuildNumber>"
Set-Content $dirBuildPath $updatedProps

# --- Versão final ------------------------------------------------------
if ($Suffix) {
    $fullVersion = "$Version-$Suffix.$buildNumber"
}
else {
    $fullVersion = "$Version.$buildNumber"
}

Write-Host "🔧 Versão final: $fullVersion"

# Restore, Build, Test --------------------------------------------------
dotnet restore
dotnet build --configuration Release --no-restore
dotnet test  --configuration Release --no-build

# Pack ------------------------------------------------------------------
$packOutput = ".\nupkgs"
if (-not (Test-Path $packOutput)) {
    New-Item -ItemType Directory -Path $packOutput | Out-Null
}

dotnet pack --configuration Release --no-build --output $packOutput /p:PackageVersion=$fullVersion

# Push ------------------------------------------------------------------
$packagePath = Get-ChildItem $packOutput -Filter "*.nupkg" | Sort-Object LastWriteTime -Descending | Select-Object -First 1

if ($null -eq $packagePath) {
    Write-Error "❌ Nenhum pacote .nupkg encontrado na pasta $packOutput"
    exit 1
}

dotnet nuget push $packagePath.FullName --api-key $ApiKey --source https://api.nuget.org/v3/index.json
Write-Host "✅ Pacote enviado com sucesso: $($packagePath.Name)"
