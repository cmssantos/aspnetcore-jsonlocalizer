$ErrorActionPreference = "Stop"

$projectKey = "aspnetcore-jsonlocalizer"
$testProjectPath = ".\tests\Cms.AspNetCore.JsonLocalizer.Tests"
$coverageDir = "$testProjectPath\TestResults"
$coverageFilePattern = "coverage*.opencover.xml"
$sonarToken = "sqp_a695923c26a448312349afea689f13fa71d1e302"
$sonarHostUrl = "http://10.0.0.10:9000"

if (-not $sonarToken) {
    Write-Error "Variável SONAR_TOKEN não está definida."
    exit 1
}

# Limpar a pasta TestResults para evitar arquivos antigos
if (Test-Path $coverageDir) {
    Remove-Item $coverageDir -Recurse -Force
}

# Rodar testes e gerar relatório de cobertura
Write-Host "Executando testes e gerando relatório de cobertura..."

Push-Location $testProjectPath
dotnet test `
    /p:CollectCoverage=true `
    /p:CoverletOutputFormat=opencover `
    /p:CoverletOutput=TestResults/
Pop-Location

# Verificar arquivo de cobertura gerado
$coverageFiles = Get-ChildItem -Path $coverageDir -Filter $coverageFilePattern -File

if ($coverageFiles.Count -eq 0) {
    Write-Error "Arquivo de cobertura não encontrado no diretório '$coverageDir'."
    exit 1
}

$coveragePath = $coverageFiles[0].FullName
Write-Host "Arquivo de cobertura encontrado: $coveragePath"

# Iniciar análise SonarQube
Write-Host "Iniciando análise SonarQube..."
dotnet sonarscanner begin `
    /k:$projectKey `
    /d:sonar.login=$sonarToken `
    /d:sonar.host.url=$sonarHostUrl `
    /d:sonar.cs.opencover.reportsPaths=$coveragePath

dotnet build

dotnet sonarscanner end /d:sonar.login=$sonarToken

Write-Host "Análise Sonar concluída com sucesso!"
