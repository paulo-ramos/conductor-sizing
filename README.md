# Conductor Sizing - Motor de Dimensionamento de Condutores Elétricos

Sistema de cálculo profissional para dimensionamento de condutores elétricos de baixa tensão seguindo rigorosamente a norma **ABNT NBR 5410:2004**.

## 🚀 Stack Técnica

- **Framework**: .NET 8 (C#)
- **Interface**: Blazor Web App (InteractiveServer)
- **Arquitetura**: Clean Architecture / DDD simplificado
- **Padrões**: Strategy Pattern (isolação), Specification Pattern (validações), Singleton Pattern (gerenciamento de estado)
- **UI**: Tailwind CSS (via CDN)
- **Persistência**: Em memória (Singleton thread-safe com ConcurrentDictionary)
- **Ambiente**: Dev Containers + Podman + VS Code (compatível com Fedora + SELinux)

## ✨ Funcionalidades

- ✅ **Dimensionamento Automático** conforme NBR 5410:2004
- ✅ **Cálculo de Corrente de Projeto** (Ib = P / (U × FP × η))
- ✅ **Fatores de Correção** (FCT, FCA, FRT)
- ✅ **Capacidade de Condução** (Tabelas 36 e 37)
- ✅ **Verificação de Queda de Tensão** (limite de 4%)
- ✅ **Condutor de Proteção** (PE - Tabela 58)
- ✅ **Histórico em Memória** thread-safe
- ✅ **Interface Moderna** e responsiva
- ✅ **Tempo Real** com Blazor InteractiveServer

## 🏗️ Arquitetura do Projeto

```
conductor-sizing/
├── .devcontainer/              # Configuração Dev Container + Podman
│   ├── devcontainer.json       # Config VS Code (flag :Z para SELinux)
│   └── Dockerfile              # Imagem .NET 8 + usuário vscode
├── src/
│   ├── ConductorSizing.Domain/          # Entidades, Enums, Interfaces
│   ├── ConductorSizing.Application/     # Serviços, Strategies, Specifications
│   │   └── Services/
│   │       ├── DimensionamentoService.cs
│   │       └── DimensionamentoStateService.cs  # ⚡ Singleton thread-safe
│   ├── ConductorSizing.Infrastructure/  # Tabelas NBR, Relatórios
│   └── ConductorSizing.Web/             # Blazor InteractiveServer
│       ├── Components/
│       │   ├── Pages/
│       │   │   ├── Home.razor                  # Página inicial
│       │   │   ├── Dimensionamento.razor       # Cálculo interativo
│       │   │   └── Historico.razor             # Histórico em memória
│       │   └── Layout/
│       │       └── MainLayout.razor            # Layout com Tailwind
│       └── Models/
│           └── DadosEntradaForm.cs             # Model mutável para formulário
├── Dockerfile                  # Build multi-stage para Podman
├── docker-compose.yml          # Compose para deploy local
├── run-podman.sh              # Script de execução rápida
└── stop-podman.sh             # Script de cleanup
```

## 🐧 Configuração do Ambiente (Fedora + Podman + SELinux)

### Pré-requisitos

1. **Podman** instalado:
```bash
sudo dnf install podman podman-compose
```

2. **VS Code** com extensão Dev Containers:
```bash
code --install-extension ms-vscode-remote.remote-containers
```

3. **Configurar Podman como Docker alternativo** (VS Code settings.json):
```json
{
  "dev.containers.dockerPath": "podman",
  "dev.containers.dockerComposePath": "podman-compose"
}
```

### ⚠️ Importante: SELinux e Flag :Z

O Fedora utiliza SELinux por padrão. O arquivo `devcontainer.json` já está configurado com a flag `:Z` nos mounts para relabeling automático, evitando erros de "Permission Denied".

## 🚀 Como Executar

### Opção 1: Dev Container (Recomendado)

1. Abra o projeto no VS Code
2. Pressione `F1` e selecione: **Dev Containers: Reopen in Container**
3. Aguarde o build e inicialização
4. Execute: `dotnet run --project src/ConductorSizing.Web`
5. Acesse: http://localhost:8080

### Opção 2: Podman (Container Local)

```bash
# Build e Run com script
chmod +x run-podman.sh
./run-podman.sh

# Ou manualmente
podman build -t conductor-sizing:latest .
podman run -d -p 8080:8080 --name conductor-sizing conductor-sizing:latest

# Acessar logs
podman logs -f conductor-sizing

# Parar e remover
./stop-podman.sh
```

### Opção 3: Execução Local (sem container)

```bash
# Restaurar dependências
dotnet restore

# Executar aplicação
dotnet run --project src/ConductorSizing.Web

# Acesse: http://localhost:8080
```

## 📱 Usando a Interface Web

1. **Página Inicial** (`/`): Visão geral das funcionalidades
2. **Dimensionamento** (`/dimensionamento`): 
   - Preencha os dados do circuito
   - Clique em "Calcular Dimensionamento"
   - Visualize os resultados em tempo real
3. **Histórico** (`/historico`): 
   - Veja todos os dimensionamentos realizados
   - Gerencie o histórico em memória

## 🧪 Gerenciamento de Estado

O sistema utiliza um serviço **Singleton thread-safe** (`DimensionamentoStateService`) para gerenciar o estado em memória:

```csharp
// Registrado em Program.cs
builder.Services.AddSingleton<DimensionamentoStateService>(sp => 
    new DimensionamentoStateService(maxHistorico: 200));
```

**Características:**
- ✅ Thread-safe com `ConcurrentDictionary`
- ✅ Limite configurável de histórico (evita vazamento de memória)
- ✅ Limpeza automática dos registros mais antigos
- ✅ Busca e filtragem por identificação de circuito

## 📊 Fluxo de Cálculo (NBR 5410:2004)

1. **Corrente de Projeto**: Ib = P / (U × FP × η)
2. **Fatores de Correção**: FCT, FRT, FCA
3. **Capacidade de Condução**: Iz' = Iz × FCT × FRT × FCA ≥ Ib
4. **Queda de Tensão**: ΔU ≤ 4%
5. **Condutor de Proteção**: PE conforme Tabela 58

## 🎨 Interface Visual

- **Design Moderno**: Tailwind CSS com gradientes e animações
- **Responsivo**: Mobile-first com breakpoints otimizados
- **Feedback Visual**: Cards coloridos conforme status (aprovado/reprovado)
- **Tempo Real**: Atualizações instantâneas via Blazor SignalR

## 🔧 Comandos Úteis

```bash
# Build
dotnet build

# Limpar
dotnet clean

# Restaurar dependências
dotnet restore

# Verificar erros
dotnet build --no-restore

# Build da imagem Docker
podman build -t conductor-sizing:latest .

# Executar container
podman run -d -p 8080:8080 conductor-sizing:latest

# Ver logs do container
podman logs -f conductor-sizing

# Parar container
podman stop conductor-sizing

# Remover container
podman rm conductor-sizing
```

## 📝 Tecnologias Utilizadas

- **.NET 8**: Framework principal
- **Blazor Server**: Renderização interativa em tempo real
- **SignalR**: Comunicação bidirecional WebSocket
- **Tailwind CSS**: Framework CSS utilitário
- **ConcurrentDictionary**: Thread-safety para estado compartilhado
- **Clean Architecture**: Separação de responsabilidades
- **Strategy Pattern**: Seleção dinâmica de estratégias de isolação
- **Specification Pattern**: Validações reutilizáveis

## 🚢 Deploy

### Container Registry

```bash
# Tag para registry
podman tag conductor-sizing:latest registry.example.com/conductor-sizing:latest

# Push para registry
podman push registry.example.com/conductor-sizing:latest
```

### Kubernetes

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: conductor-sizing
spec:
  replicas: 3
  selector:
    matchLabels:
      app: conductor-sizing
  template:
    metadata:
      labels:
        app: conductor-sizing
    spec:
      containers:
      - name: conductor-sizing
        image: conductor-sizing:latest
        ports:
        - containerPort: 8080
        env:
        - name: ASPNETCORE_ENVIRONMENT
          value: "Production"
```

## 📄 Licença

Este projeto é um sistema educacional para dimensionamento de condutores elétricos conforme NBR 5410:2004.

## 👨‍💻 Desenvolvimento

Desenvolvido com .NET 8, Blazor Server e Tailwind CSS para oferecer uma experiência moderna e profissional no dimensionamento de instalações elétricas.

---

**Nota**: Sistema em conformidade com ABNT NBR 5410:2004 - Instalações Elétricas de Baixa Tensão

Geração de PDF otimizado para mobile usando **QuestPDF** (compatível com Linux via SkiaSharp).

## 🧪 Execução

```bash
# Restaurar dependências
dotnet restore

# Compilar
dotnet build

# Executar console app
dotnet run --project src/ConductorSizing.Console

# Executar testes
dotnet test
```

## 📚 Referências

- ABNT NBR 5410:2004 - Instalações elétricas de baixa tensão
- Clean Architecture (Robert C. Martin)
- Domain-Driven Design (Eric Evans)

## 📝 Licença

Projeto educacional/profissional para dimensionamento de condutores elétricos.
