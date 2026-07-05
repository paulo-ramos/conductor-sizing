# 📚 Índice de Documentação - Conductor Sizing

## 🎯 Documentos Disponíveis

### 1. 📖 README.md - Documentação Principal
**O que contém:**
- Stack técnica completa
- Funcionalidades da aplicação
- Arquitetura do projeto
- Instruções de configuração (Dev Container, Podman, Local)
- Guia de uso da interface
- Gerenciamento de estado
- Fluxo de cálculo NBR 5410:2004
- Comandos úteis
- Informações de deploy

**Quando usar:**
- Primeira leitura do projeto
- Entender a arquitetura
- Configurar ambiente de desenvolvimento
- Referência completa

---

### 2. 🚀 QUICKSTART.md - Início Rápido
**O que contém:**
- 3 métodos de execução rápida
- Passo a passo para começar imediatamente
- Comandos essenciais
- Exemplo prático de cálculo
- Troubleshooting básico
- Features principais

**Quando usar:**
- Primeira vez usando a aplicação
- Precisa executar rapidamente
- Guia prático e direto
- Comandos mais usados

---

### 3. 🚢 DEPLOY.md - Guia de Deployment
**O que contém:**
- Deploy com Podman/Docker
- Deploy com Docker Compose
- Deploy no Kubernetes (manifests completos)
- Ingress e Load Balancer
- Deploy em servidor Linux (systemd)
- Nginx como reverse proxy
- HTTPS com Let's Encrypt
- Monitoramento (Prometheus/Grafana)
- Variáveis de ambiente
- Backup e restore
- CI/CD pipeline exemplo

**Quando usar:**
- Colocar em produção
- Deploy em Kubernetes
- Configurar proxy reverso
- Setup de monitoramento
- Automatizar CI/CD

---

### 4. 🔧 TROUBLESHOOTING.md - Solução de Problemas
**O que contém:**
- Problemas de compilação
- Problemas com containers
- Problemas de rede
- Problemas de memória
- Problemas de interface
- Problemas de permissão (SELinux)
- Performance issues
- Debugging avançado
- Checklist de diagnóstico
- Comandos de teste

**Quando usar:**
- Algo não está funcionando
- Erro de compilação
- Container não inicia
- Aplicação lenta
- Problemas de conectividade
- SELinux bloqueando acesso

---

### 5. 📝 CHANGELOG.md - Histórico de Mudanças
**O que contém:**
- Sumário completo da refatoração
- Antes vs Depois
- Arquivos removidos
- Nova arquitetura implementada
- Interface moderna
- Gerenciamento de estado thread-safe
- Containerização
- Documentação criada
- Próximos passos sugeridos

**Quando usar:**
- Entender o que foi modificado
- Ver evolução do projeto
- Comparar versões
- Planejar próximas features

---

### 6. 📋 INSTRUCOES.md - (Documento Original)
**O que contém:**
- Instruções técnicas da NBR 5410:2004
- Tabelas de referência
- Fórmulas de cálculo
- Regras de dimensionamento

**Quando usar:**
- Entender cálculos elétricos
- Consultar norma técnica
- Validar resultados

---

## 🗺️ Fluxo de Leitura Recomendado

### Para Desenvolvedores (Primeira Vez)
1. **README.md** - Entender o projeto
2. **QUICKSTART.md** - Executar localmente
3. **CHANGELOG.md** - Ver mudanças implementadas
4. **TROUBLESHOOTING.md** - Resolver problemas

### Para DevOps/Deploy
1. **README.md** - Contexto geral
2. **DEPLOY.md** - Instruções completas de deploy
3. **TROUBLESHOOTING.md** - Resolver problemas específicos

### Para Usuários Finais
1. **QUICKSTART.md** - Como usar rapidamente
2. **README.md** - Referência completa
3. **INSTRUCOES.md** - Entender cálculos

### Para Manutenção/Debug
1. **TROUBLESHOOTING.md** - Diagnosticar problema
2. **README.md** - Arquitetura e configuração
3. **CHANGELOG.md** - Entender implementação

---

## 📂 Estrutura de Arquivos

```
conductor-sizing/
├── 📖 README.md              # Documentação principal (leia primeiro!)
├── 🚀 QUICKSTART.md          # Início rápido
├── 🚢 DEPLOY.md              # Guia de deploy completo
├── 🔧 TROUBLESHOOTING.md     # Solução de problemas
├── 📝 CHANGELOG.md           # Histórico de mudanças
├── 📋 INSTRUCOES.md          # NBR 5410:2004 (original)
├── 📚 DOCS.md                # Este arquivo (índice)
│
├── .devcontainer/            # Configuração Dev Container
│   ├── devcontainer.json
│   └── Dockerfile
│
├── src/                      # Código-fonte
│   ├── ConductorSizing.Domain/
│   ├── ConductorSizing.Application/
│   ├── ConductorSizing.Infrastructure/
│   └── ConductorSizing.Web/
│
├── Dockerfile                # Build de produção
├── docker-compose.yml        # Orchestration
├── run-podman.sh            # Script de execução
├── stop-podman.sh           # Script de cleanup
└── ConductorSizing.sln      # Solution file
```

---

## 🔍 Busca Rápida por Tópico

### Executar a Aplicação
- QUICKSTART.md → "Início Rápido em 3 Passos"
- README.md → "Como Executar"

### Problemas de Compilação
- TROUBLESHOOTING.md → "Problemas de Compilação"

### Deploy em Produção
- DEPLOY.md → "Deploy com Podman/Docker"
- DEPLOY.md → "Deploy no Kubernetes"

### Configurar HTTPS
- DEPLOY.md → "HTTPS com Let's Encrypt"
- DEPLOY.md → "Nginx como Reverse Proxy"

### Entender Arquitetura
- README.md → "Arquitetura do Projeto"
- CHANGELOG.md → "Arquitetura Refatorada"

### Problemas de Memória
- TROUBLESHOOTING.md → "Problemas de Memória"

### Configurar Monitoramento
- DEPLOY.md → "Monitoramento"

### SELinux/Permissões
- TROUBLESHOOTING.md → "Problemas de Permissão (Linux)"
- README.md → "Importante: SELinux e Flag :Z"

### Performance
- TROUBLESHOOTING.md → "Problemas de Performance"

### Cálculos Elétricos
- INSTRUCOES.md → Tabelas e fórmulas NBR 5410
- README.md → "Fluxo de Cálculo (NBR 5410:2004)"

---

## 💡 Dicas

### Leitura Sequencial (Recomendado)
```
README → QUICKSTART → DEPLOY → TROUBLESHOOTING → CHANGELOG
```

### Leitura por Necessidade
- **Preciso executar agora:** QUICKSTART.md
- **Preciso fazer deploy:** DEPLOY.md
- **Algo quebrou:** TROUBLESHOOTING.md
- **Preciso entender tudo:** README.md
- **O que mudou:** CHANGELOG.md

### Comandos Rápidos
- **Local:** `dotnet run --project src/ConductorSizing.Web`
- **Container:** `./run-podman.sh`
- **Logs:** `podman logs -f conductor-sizing`
- **Build:** `dotnet build`

---

## 📞 Ordem de Consulta para Problemas

1. **TROUBLESHOOTING.md** - Procure seu erro específico
2. **README.md** - Revise configuração e arquitetura
3. **QUICKSTART.md** - Tente o método mais simples
4. **CHANGELOG.md** - Veja se algo mudou recentemente

---

## ✅ Checklist de Documentos Lidos

- [ ] README.md - Documentação geral
- [ ] QUICKSTART.md - Início rápido
- [ ] DEPLOY.md - Deploy em produção
- [ ] TROUBLESHOOTING.md - Problemas comuns
- [ ] CHANGELOG.md - O que foi feito
- [ ] DOCS.md - Este índice

---

## 🎯 Sumário Executivo

**Conductor Sizing** é uma aplicação web moderna para dimensionamento de condutores elétricos conforme NBR 5410:2004.

**Stack:**
- .NET 8 + Blazor InteractiveServer
- Tailwind CSS
- Clean Architecture
- Thread-safe Singleton para estado em memória
- Containerizado (Podman/Docker)

**Documentação:**
- 6 arquivos de documentação completa
- Guias práticos e teóricos
- Troubleshooting detalhado
- Exemplos de deploy

**Status:**
✅ 100% funcional e pronto para uso

---

**Comece por [QUICKSTART.md](QUICKSTART.md) para executar em 3 passos!** 🚀
