Set-Location -Path "./tests/Cms.AspNetCore.JsonLocalizer.Tests"

dotnet test `
  /p:CollectCoverage=true `
  /p:CoverletOutputFormat=cobertura `
  /p:CoverletOutput=./TestResults/

reportgenerator `
  -reports:"./TestResults/coverage.cobertura.xml" `
  -targetdir:"./TestResults/coverage-report" `
  -reporttypes:Html

Write-Host "Relatório de cobertura gerado em ./TestResults/coverage-report/index.html"

