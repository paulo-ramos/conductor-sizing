# Conductor Sizing - Motor de Dimensionamento de Condutores Elétricos

Sistema de cálculo para dimensionamento de condutores elétricos de baixa tensão seguindo rigorosamente a norma **ABNT NBR 5410:2004**.

## 🏗️ Arquitetura

- **Framework**: .NET 8 (C#)
- **Arquitetura**: Clean Architecture / DDD simplificado
- **Padrões**: Strategy Pattern (isolação), Specification Pattern (validações)
- **Ambiente**: Dev Containers + Podman + VS Code (compatível com Fedora + SELinux)

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

3. **Configurar Podman como Docker alternativo** (VS Code):
```bash
# Criar link simbólico (opcional, apenas se necessário)
sudo ln -s /usr/bin/podman /usr/bin/docker

# Ou configurar no VS Code settings.json:
{
  "dev.containers.dockerPath": "podman",
  "dev.containers.dockerComposePath": "podman-compose"
}
```

### ⚠️ Importante: SELinux e Flag :Z

O Fedora utiliza SELinux por padrão. O arquivo `devcontainer.json` já está configurado com a flag `:Z` nos mounts para relabeling automático:

```json
"mounts": [
  "source=${localWorkspaceFolder},target=/workspace,type=bind,Z"
]
```

**O que a flag `:Z` faz:**
- Relabela o contexto SELinux do volume para permitir acesso do container
- Evita erros de "Permission Denied" mesmo com usuário correto

### 🚀 Iniciando o Ambiente

1. Abra o projeto no VS Code
2. Pressione `F1` e selecione: **Dev Containers: Reopen in Container**
3. Aguarde o build da imagem e inicialização

## 📁 Estrutura do Projeto

```
conductor-sizing/
├── .devcontainer/
│   ├── devcontainer.json    # Configuração VS Code + Podman
│   └── Dockerfile            # Imagem .NET 8 com usuário vscode
├── src/
│   ├── ConductorSizing.Domain/          # Entidades, Enums, Interfaces
│   ├── ConductorSizing.Application/     # Serviços, Strategies, Specifications
│   ├── ConductorSizing.Infrastructure/  # Tabelas NBR, Repositórios
│   └── ConductorSizing.Console/         # Aplicação Console
├── tests/
│   └── ConductorSizing.Tests/           # Testes Unitários
└── ConductorSizing.sln
```

## 🔌 Fluxo de Cálculo (NBR 5410:2004)

### 1. Corrente de Projeto (Ib)
```
Ib = P / (U × FP × η)
```

### 2. Fatores de Correção
- **FCT**: Temperatura ambiente/solo e tipo de isolação
- **FRT**: Resistividade térmica do solo (se subterrânea)
- **FCA**: Agrupamento de circuitos

### 3. Capacidade de Condução (Strategy Pattern)
- **Iz'** = Iz × FCT × FRT × FCA ≥ Ib
- Tabela 36 (PVC) ou Tabela 37 (XLPE/EPR)
- Bitola mínima: 2,5 mm² para circuitos de força

### 4. Queda de Tensão (Specification Pattern)
- **ΔU** ≤ 4%
- ΔU = (2 × ρ × L × FP × Ib) / S

### 5. Condutor de Proteção (PE)
- Tabela 58 da NBR 5410

## 📊 Relatórios

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
