# 🚀 Quick Start - Conductor Sizing

## ⚡ Início Rápido em 3 Passos

### 1️⃣ Dev Container (Recomendado para desenvolvimento)

```bash
# Abra o projeto no VS Code
code /home/paulo/Projects/conductor-sizing

# Pressione F1 e selecione:
# "Dev Containers: Reopen in Container"

# Aguarde o build (primeira vez ~2 min)

# Execute:
dotnet run --project src/ConductorSizing.Web

# Acesse: http://localhost:8080
```

### 2️⃣ Podman/Docker (Rápido para testar)

```bash
cd /home/paulo/Projects/conductor-sizing

# Executar com script
./run-podman.sh

# Ou manualmente
podman build -t conductor-sizing .
podman run -d -p 8080:8080 --name conductor-sizing conductor-sizing:latest

# Acesse: http://localhost:8080
```

### 3️⃣ Local (Sem container)

```bash
cd /home/paulo/Projects/conductor-sizing

# Restaurar e executar
dotnet restore
dotnet run --project src/ConductorSizing.Web

# Acesse: http://localhost:8080
```

## 📱 Usando a Interface

### Dimensionamento (página principal)

1. **Preencha os dados do circuito:**
   - Identificação (ex: "Circuito Tomadas Cozinha")
   - Potência ativa (Watts)
   - Tensão (127V ou 220V)
   - Fator de potência (0.8 - 0.95)
   - Rendimento (0.9 - 0.98)
   - Comprimento do circuito (metros)

2. **Configure os parâmetros:**
   - Tipo de metal (Cobre/Alumínio)
   - Tipo de isolação (PVC/XLPE/EPR)
   - Tipo de linha (B1/C/D)
   - Temperatura ambiente
   - Número de circuitos agrupados
   - Resistividade do solo (se subterrânea)

3. **Clique em "Calcular Dimensionamento"**

4. **Veja os resultados:**
   - ✅ Verde = Aprovado
   - ❌ Vermelho = Reprovado
   - Bitola sugerida
   - Corrente de projeto
   - Capacidade de condução
   - Queda de tensão
   - Condutor PE

### Histórico

1. Acesse a página "Histórico"
2. Visualize todos os dimensionamentos realizados
3. Busque por identificação
4. Remova itens indesejados

## 🛠️ Comandos Úteis

### Build
```bash
dotnet build
```

### Executar
```bash
dotnet run --project src/ConductorSizing.Web
```

### Limpar
```bash
dotnet clean
```

### Watch (auto-reload)
```bash
dotnet watch --project src/ConductorSizing.Web
```

### Container
```bash
# Build
podman build -t conductor-sizing .

# Run
podman run -d -p 8080:8080 conductor-sizing

# Logs
podman logs -f conductor-sizing

# Stop
podman stop conductor-sizing

# Remove
podman rm conductor-sizing
```

## 📚 Estrutura de Pastas

```
conductor-sizing/
├── src/
│   ├── ConductorSizing.Domain/        # Entidades e interfaces
│   ├── ConductorSizing.Application/   # Serviços e lógica
│   ├── ConductorSizing.Infrastructure/# Tabelas NBR
│   └── ConductorSizing.Web/           # Interface Blazor
├── README.md                          # Documentação completa
├── DEPLOY.md                          # Guia de deploy
├── CHANGELOG.md                       # Histórico de mudanças
├── QUICKSTART.md                      # Este arquivo
├── Dockerfile                         # Container config
├── docker-compose.yml                 # Orchestration
├── run-podman.sh                      # Script de execução
└── stop-podman.sh                     # Script de cleanup
```

## 🎯 Exemplo de Cálculo

**Entrada:**
- Identificação: "Tomadas Sala"
- Potência: 3000 W
- Tensão: 220 V
- FP: 0.92
- Rendimento: 0.95
- Comprimento: 25 m
- Metal: Cobre
- Isolação: PVC
- Linha: B1
- Temp. Ambiente: 30°C
- Agrupamento: 3 circuitos
- Solo: 2.5 K.m/W

**Resultado Esperado:**
- Corrente: ~15.6 A
- Bitola: 2.5 mm²
- Capacidade: ~24 A
- Queda de tensão: ~2.5%
- Status: ✅ Aprovado

## 🐛 Troubleshooting Rápido

### Porta 8080 já em uso
```bash
# Verificar o que está usando
sudo ss -tlnp | grep 8080

# Matar processo
sudo kill -9 <PID>

# Ou usar outra porta
export ASPNETCORE_URLS="http://0.0.0.0:8081"
dotnet run --project src/ConductorSizing.Web
```

### Permissão negada no Podman (SELinux)
```bash
# Usar flag :Z nos volumes
podman run -v /path:/app:Z ...

# Ou desabilitar SELinux temporariamente
sudo setenforce 0
```

### Build falha
```bash
# Limpar tudo
dotman clean
rm -rf */bin */obj

# Restaurar e rebuild
dotnet restore
dotnet build
```

### Container não inicia
```bash
# Ver logs detalhados
podman logs conductor-sizing

# Executar interativamente
podman run -it --rm conductor-sizing
```

## 📞 Precisa de Ajuda?

1. **Documentação Completa:** [README.md](README.md)
2. **Deploy:** [DEPLOY.md](DEPLOY.md)
3. **Histórico:** [CHANGELOG.md](CHANGELOG.md)

## ✨ Features Principais

- ✅ Cálculo automático NBR 5410:2004
- ✅ Interface moderna com Tailwind CSS
- ✅ Tempo real com Blazor InteractiveServer
- ✅ Histórico em memória thread-safe
- ✅ Containerizado (Podman/Docker)
- ✅ Dev Container pronto
- ✅ Clean Architecture
- ✅ Design responsivo

## 🎉 Pronto para Usar!

A aplicação está 100% funcional e documentada.

**Escolha seu método preferido e comece a calcular!** 🚀
