#!/bin/bash

set -e

PROJECT_KEY="aspnetcore-jsonlocalizer"
COVERAGE_PATH="./TestResults/coverage.cobertura.xml"
SONAR_HOST_URL="https://meuservidor.sonarqube.com"

if [ -z "$SONAR_TOKEN" ]; then
  echo "ERRO: variável SONAR_TOKEN não está definida."
  exit 1
fi

dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=$COVERAGE_PATH

dotnet sonarscanner begin /k:"$PROJECT_KEY" /d:sonar.login="$SONAR_TOKEN" /d:sonar.host.url="$SONAR_HOST_URL" /d:sonar.cs.opencover.reportsPaths="$COVERAGE_PATH"

dotnet build

dotnet sonarscanner end /d:sonar.login="$SONAR_TOKEN"

echo "Análise Sonar concluída com sucesso!"
