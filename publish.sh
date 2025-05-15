#./publish.sh 1.0.0 development SUA_API_KEY

#!/bin/bash

set -e

if [ -z "$1" ] || [ -z "$3" ]; then
  echo "Usage: ./publish.sh <version> [suffix] <api_key>"
  echo "Example: ./publish.sh 1.0.6 beta YOUR_API_KEY"
  exit 1
fi

VERSION=$1
if [[ "$3" == "" ]]; then
  API_KEY=$2
  SUFFIX=""
else
  SUFFIX=$2
  API_KEY=$3
fi

BUILD_FILE="./buildnumber.txt"

# Ler build number ou iniciar em 0
if [ -f "$BUILD_FILE" ]; then
  BUILD_NUMBER=$(cat $BUILD_FILE)
else
  BUILD_NUMBER=0
fi

# Incrementar build number
BUILD_NUMBER=$((BUILD_NUMBER+1))
echo $BUILD_NUMBER > $BUILD_FILE

# Montar versão final
if [ -n "$SUFFIX" ]; then
  FULL_VERSION="${VERSION}-${SUFFIX}.${BUILD_NUMBER}"
else
  FULL_VERSION="${VERSION}.${BUILD_NUMBER}"
fi

echo "Nova versão completa: $FULL_VERSION"

# Atualizar Directory.Build.props
sed -i "s|<Version>.*</Version>|<Version>$FULL_VERSION</Version>|" Directory.Build.props
echo "Versão atualizada no Directory.Build.props."

# Restore
dotnet restore

# Build
dotnet build --configuration Release --no-restore

# Test
dotnet test --configuration Release --no-build

# Pack
PACK_OUTPUT="./nupkgs"
mkdir -p $PACK_OUTPUT
dotnet pack --configuration Release --no-build --output $PACK_OUTPUT

# Push
PACKAGE_PATH=$(ls -t $PACK_OUTPUT/*.nupkg | head -n 1)
if [ -z "$PACKAGE_PATH" ]; then
  echo "Nenhum pacote .nupkg encontrado em $PACK_OUTPUT"
  exit 1
fi

dotnet nuget push "$PACKAGE_PATH" --api-key $API_KEY --source https://api.nuget.org/v3/index.json

echo "Pacote enviado com sucesso: $(basename $PACKAGE_PATH)"
