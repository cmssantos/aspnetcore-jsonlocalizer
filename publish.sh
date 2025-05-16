#!/bin/bash

# Exemplo de uso:
# ./publish.sh <API_KEY> [VERSION] [SUFFIX]

set -e

API_KEY=$1
CUSTOM_VERSION=$2
CUSTOM_SUFFIX=$3
DIR_BUILD_PATH="Directory.Build.props"

if [ -z "$API_KEY" ]; then
  echo "❌ API key não fornecida."
  echo "Uso: ./publish.sh <API_KEY> [VERSION] [SUFFIX]"
  exit 1
fi

if [ ! -f "$DIR_BUILD_PATH" ]; then
  echo "❌ Arquivo $DIR_BUILD_PATH não encontrado."
  exit 1
fi

PROPS_CONTENT=$(cat "$DIR_BUILD_PATH")

# Extrair valores existentes
CURRENT_VERSION=$(echo "$PROPS_CONTENT" | grep -oP '(?<=<Version>).*?(?=</Version>)')
CURRENT_SUFFIX=$(echo "$PROPS_CONTENT" | grep -oP '(?<=<VersionSuffix>).*?(?=</VersionSuffix>)')
CURRENT_BUILD=$(echo "$PROPS_CONTENT" | grep -oP '(?<=<BuildNumber>)[0-9]+(?=</BuildNumber>)')

if [ -z "$CURRENT_VERSION" ] || [ -z "$CURRENT_SUFFIX" ] || [ -z "$CURRENT_BUILD" ]; then
  echo "❌ Tags <Version>, <VersionSuffix> ou <BuildNumber> não encontradas no $DIR_BUILD_PATH"
  exit 1
fi

# Usar parâmetros se fornecidos, senão usar os valores do arquivo
VERSION="${CUSTOM_VERSION:-$CURRENT_VERSION}"
SUFFIX="${CUSTOM_SUFFIX:-$CURRENT_SUFFIX}"
BUILD_NUMBER=$((CURRENT_BUILD + 1))

# Atualizar <BuildNumber> no props
UPDATED_PROPS=$(echo "$PROPS_CONTENT" | sed -E "s|<BuildNumber>[0-9]+</BuildNumber>|<BuildNumber>$BUILD_NUMBER</BuildNumber>|")
echo "$UPDATED_PROPS" > "$DIR_BUILD_PATH"

# Construir versão final
if [ -n "$SUFFIX" ]; then
  FULL_VERSION="$VERSION-$SUFFIX.$BUILD_NUMBER"
else
  FULL_VERSION="$VERSION.$BUILD_NUMBER"
fi

echo "🔧 Versão final: $FULL_VERSION"

# Restore, Build, Test
dotnet restore
dotnet build --configuration Release --no-restore
dotnet test --configuration Release --no-build

# Pack
PACK_OUTPUT="./nupkgs"
mkdir -p "$PACK_OUTPUT"

dotnet pack --configuration Release --no-build --output "$PACK_OUTPUT" /p:PackageVersion="$FULL_VERSION"

# Encontrar último pacote
PACKAGE_PATH=$(ls -t "$PACK_OUTPUT"/*.nupkg | head -n 1)

if [ ! -f "$PACKAGE_PATH" ]; then
  echo "❌ Nenhum pacote .nupkg encontrado em $PACK_OUTPUT"
  exit 1
fi

# Push
dotnet nuget push "$PACKAGE_PATH" --api-key "$API_KEY" --source https://api.nuget.org/v3/index.json

echo "✅ Pacote enviado com sucesso: $(basename "$PACKAGE_PATH")"
