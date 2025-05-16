# Como executar
#.\publish.ps1 -Version "1.0.0" -Suffix "development" -ApiKey "SUA_API_KEY"

param(
    [Parameter(Mandatory=$true)][string]$Version,
    [Parameter(Mandatory=$true)][string]$Suffix = "",
    [Parameter(Mandatory=$true)][string]$ApiKey
)

$buildFile = ".\buildnumber"

# Ler o build number atual ou iniciar em 0
if (Test-Path $buildFile) {
    $buildNumber = [int](Get-Content $buildFile)
} else {
    $buildNumber = 0
}

# Incrementar build number
$buildNumber++
Set-Content $buildFile $buildNumber

# Montar versão final
if ($Suffix -ne "") {
    $fullVersion = "$Version-$Suffix.$buildNumber"
} else {
    $fullVersion = "$Version.$buildNumber"
}

Write-Host "Nova versão completa: $fullVersion"

# Atualizar versão no Directory.Build.props
$dirBuildPath = "Directory.Build.props"
(Get-Content $dirBuildPath) -replace '<Version>.*</Version>', "<Version>$fullVersion</Version>" | Set-Content $dirBuildPath
Write-Host "Versão atualizada no $dirBuildPath."

# Restore
dotnet restore

# Build
dotnet build --configuration Release --no-restore

# Test
dotnet test --configuration Release --no-build

# Pack
$packOutput = ".\nupkgs"
if (-Not (Test-Path $packOutput)) {
    New-Item -ItemType Directory -Path $packOutput | Out-Null
}
dotnet pack --configuration Release --no-build --output $packOutput

# Push
$packagePath = Get-ChildItem -Path $packOutput -Filter "*.nupkg" | Sort-Object LastWriteTime -Descending | Select-Object -First 1

if ($packagePath -eq $null) {
    Write-Error "Nenhum pacote .nupkg encontrado na pasta $packOutput"
    exit 1
}

dotnet nuget push $packagePath.FullName --api-key $ApiKey --source https://api.nuget.org/v3/index.json

Write-Host "Pacote enviado com sucesso: $($packagePath.Name)"
