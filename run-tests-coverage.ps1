# generate-coverage.ps1

# Define o caminho para o projeto de testes
$testProjectPath = "./tests/Cms.AspNetCore.JsonLocalizer.Tests"

# Define a pasta onde os resultados da cobertura serão salvos
$coverageOutputDir = "$testProjectPath/TestResults"
$coverageReportDir = "$coverageOutputDir/coverage-report"
$coverageReportFile = "$coverageReportDir/index.html"

# Vai para o diretório do projeto de testes
Set-Location -Path $testProjectPath

# Executa os testes com cobertura no formato cobertura e salva no diretório definido
dotnet test `
  /p:CollectCoverage=true `
  /p:CoverletOutputFormat=cobertura `
  /p:CoverletOutput="$coverageOutputDir/coverage.cobertura.xml"

# Gera o relatório HTML a partir do arquivo XML de cobertura
reportgenerator `
  -reports:"$coverageOutputDir/coverage.cobertura.xml" `
  -targetdir:"$coverageReportDir" `
  -reporttypes:Html

Write-Host "Relatório de cobertura gerado em $coverageReportFile"

# Abre o relatório HTML automaticamente (Windows)
Start-Process $coverageReportFile

# Volta para o diretório raiz do repositório
Set-Location -Path "../.."
