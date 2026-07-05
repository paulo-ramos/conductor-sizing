# Conductor Sizing - Instruções de Uso

## 🚀 Como Executar o Projeto

### 1. Usando Dev Container (Recomendado - Fedora + Podman)

#### Pré-requisitos
- Podman instalado: `sudo dnf install podman podman-compose`
- VS Code com extensão Dev Containers

#### Passos
1. Abra o projeto no VS Code
2. Pressione `F1` e selecione: **Dev Containers: Reopen in Container**
3. Aguarde o build da imagem e inicialização do container
4. O ambiente estará pronto com .NET 8 SDK e todas as dependências

### 2. Executando Localmente (sem container)

#### Pré-requisitos
- .NET 8 SDK instalado

#### Passos
```bash
# Restaurar dependências
dotnet restore

# Compilar a solution
dotnet build

# Executar aplicação console (exemplo)
dotnet run --project src/ConductorSizing.Console

# Executar aplicação Blazor Web
dotnet run --project src/ConductorSizing.Web
```

## 📱 Aplicação Blazor Web

### Acessando a Interface Web
Após executar o projeto Blazor:
```bash
dotnet run --project src/ConductorSizing.Web
```

Acesse no navegador:
- **HTTP**: http://localhost:5000
- **HTTPS**: https://localhost:5001

### Usando a Interface
1. Navegue até **Dimensionamento** no menu lateral
2. Preencha os dados do circuito:
   - Identificação do circuito
   - Potência (W), Tensão (V), Comprimento (m)
   - Fator de Potência e Rendimento
   - Tipo de metal e isolação
   - Método de instalação e condutores carregados
   - Temperatura ambiente e circuitos agrupados
3. Clique em **"Calcular Dimensionamento"**
4. O resultado aparecerá à direita com:
   - Bitola do condutor fase e PE
   - Corrente de projeto
   - Fatores de correção
   - Queda de tensão
5. Clique em **"Gerar Relatório PDF"** para baixar o relatório

## 📄 Geração de Relatórios PDF

O sistema utiliza **QuestPDF** para gerar relatórios otimizados para mobile:
- Formato A5 (ideal para WhatsApp)
- Margens reduzidas
- Design responsivo e limpo
- Compatível com Linux (SkiaSharp nativo)

## 🧪 Testes

```bash
# Executar todos os testes
dotnet test

# Executar testes com cobertura
dotnet test /p:CollectCoverage=true
```

## 🏗️ Estrutura do Projeto

```
conductor-sizing/
├── .devcontainer/              # Configuração Dev Container + Podman
│   ├── devcontainer.json       # Config VS Code (flag :Z para SELinux)
│   └── Dockerfile              # Imagem .NET 8 + usuário vscode
├── src/
│   ├── ConductorSizing.Domain/          # Entidades, Enums, Interfaces
│   │   ├── Enums/                       # TipoMetal, TipoIsolacao, TipoLinha
│   │   ├── Models/                      # DadosEntrada, ResultadoDimensionamento
│   │   └── Interfaces/                  # ISpecification, ICapacidadeConducaoStrategy
│   ├── ConductorSizing.Application/     # Serviços, Strategies, Specifications
│   │   ├── Services/                    # DimensionamentoService
│   │   ├── Strategies/                  # IsolacaoPvcStrategy, IsolacaoXlpeEprStrategy
│   │   └── Specifications/              # BitolaMinima, QuedaTensao
│   ├── ConductorSizing.Infrastructure/  # Tabelas NBR, Repositórios
│   │   ├── Data/                        # Tabelas 36, 37, 40, 58
│   │   └── Reports/                     # RelatorioCondutorPDF
│   ├── ConductorSizing.Web/             # Aplicação Blazor Server
│   │   └── Components/Pages/            # Dimensionamento.razor
│   └── ConductorSizing.Console/         # Aplicação Console (exemplo)
├── tests/
│   └── ConductorSizing.Tests/           # Testes Unitários
└── ConductorSizing.sln
```

## 🎯 Exemplo de Uso (Console)

```csharp
var dados = new DadosEntrada
{
    IdentificacaoCircuito = "Circuito 5 - Ar-Condicionado 12.000 BTU",
    PotenciaAtivaWatts = 1320.0,
    TensaoVolts = 220.0,
    ComprimentoMetros = 25.0,
    FatorPotencia = 0.92,
    Rendimento = 0.85,
    TipoMetal = TipoMetal.Cobre,
    TipoIsolacao = TipoIsolacao.PVC,
    MetodoInstalacao = "B1",
    NumeroCondutoresCarregados = 2,
    TemperaturaAmbienteCelsius = 35,
    NumeroCircuitosAgrupados = 3,
    TipoLinha = TipoLinha.NaoSubterranea
};

var servico = new DimensionamentoService();
var resultado = servico.Calcular(dados);

Console.WriteLine($"Condutor Fase: {resultado.SecaoNominalFase} mm²");
Console.WriteLine($"Condutor PE: {resultado.SecaoNominalPE} mm²");
Console.WriteLine($"Queda de Tensão: {resultado.QuedaTensaoPercentual:F2}%");

// Gerar PDF
RelatorioCondutorPDF.Gerar(resultado, "relatorio.pdf");
```

## 📚 Referências Técnicas

### Tabelas da NBR 5410:2004 Implementadas
- **Tabela 36**: Capacidade de condução - PVC (70°C)
- **Tabela 37**: Capacidade de condução - XLPE/EPR (90°C)
- **Tabela 40**: Fatores de correção por temperatura (FCT)
- **Tabela 42/43**: Fatores de correção por agrupamento (FCA)
- **Tabela 58**: Dimensionamento do condutor de proteção (PE)

### Critérios de Dimensionamento
1. **Corrente de Projeto**: Ib = P / (U × FP × η)
2. **Fatores de Correção**: FCT × FRT × FCA
3. **Capacidade Corrigida**: Iz' = Iz × FCT × FRT × FCA ≥ Ib
4. **Bitola Mínima**: 2.5 mm² para circuitos de força/TUE
5. **Queda de Tensão**: ΔU ≤ 4%
6. **Condutor PE**: Conforme Tabela 58

## 🔧 Troubleshooting

### Erro de permissão no Dev Container (SELinux)
- Verifique se a flag `:Z` está presente no mount do `devcontainer.json`
- Exemplo: `"source=${localWorkspaceFolder},target=/workspace,type=bind,Z"`

### Erro ao gerar PDF
- Certifique-se que as dependências nativas do SkiaSharp estão instaladas
- No Dockerfile já estão incluídas: `libfontconfig1`, `libfreetype6`, `libharfbuzz0b`

### Porta já em uso (Blazor)
```bash
# Mudar porta manualmente
dotnet run --project src/ConductorSizing.Web --urls "http://localhost:5050;https://localhost:5051"
```

## 📞 Suporte

Para dúvidas técnicas ou problemas:
- Verifique o README.md principal
- Consulte a documentação da NBR 5410:2004
- Revise os logs de erro no console

---

**Nota**: Este sistema foi desenvolvido para fins educacionais e profissionais. 
Sempre valide os resultados com um engenheiro eletricista qualificado antes da implementação em campo.
