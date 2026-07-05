#!/bin/bash

echo "⚡ Conductor Sizing - Iniciando aplicação..."
echo ""

# Verificar se o .NET está instalado
if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET SDK não encontrado!"
    exit 1
fi

echo "✅ .NET SDK encontrado:"
dotnet --version
echo ""

# Restaurar dependências da solução
echo "📦 Restaurando dependências..."
dotnet restore ConductorSizing.sln

if [ $? -ne 0 ]; then
    echo "❌ Erro ao restaurar dependências"
    exit 1
fi

echo ""
echo "🔨 Compilando aplicação..."
dotnet build ConductorSizing.sln -c Release

if [ $? -ne 0 ]; then
    echo "❌ Erro ao compilar aplicação"
    exit 1
fi

echo ""
echo "✅ Compilação concluída!"
echo ""
echo "🚀 Iniciando Conductor Sizing..."
echo ""

# Usar a porta fornecida pelo Railway ou padrão 8080
PORT="${PORT:-8080}"
export ASPNETCORE_URLS="http://0.0.0.0:$PORT"
export ASPNETCORE_ENVIRONMENT="Production"

echo "📍 Servidor rodando na porta $PORT"
echo "📍 ASPNETCORE_URLS=$ASPNETCORE_URLS"
echo ""

# Executar ESPECIFICAMENTE o projeto Web
cd src/ConductorSizing.Web
dotnet run --no-build -c Release --urls "http://0.0.0.0:$PORT"
