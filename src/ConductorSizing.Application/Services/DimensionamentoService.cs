using ConductorSizing.Application.Data;
using ConductorSizing.Application.Specifications;
using ConductorSizing.Application.Strategies;
using ConductorSizing.Domain.Enums;
using ConductorSizing.Domain.Interfaces;
using ConductorSizing.Domain.Models;

namespace ConductorSizing.Application.Services;

/// <summary>
/// Serviço coordenador para dimensionamento de condutores elétricos
/// Implementa todo o fluxo da NBR 5410:2004
/// </summary>
public class DimensionamentoService : IDimensionamentoService
{
    /// <summary>
    /// Executa o dimensionamento completo do condutor seguindo NBR 5410:2004
    /// </summary>
    public ResultadoDimensionamento Calcular(DadosEntrada dados)
    {
        var mensagens = new List<string>();
        
        // Validação: Número de condutores carregados
        if (dados.NumeroCondutoresCarregados != 2 && dados.NumeroCondutoresCarregados != 3)
        {
            mensagens.Add($"ERRO: Número de condutores carregados inválido ({dados.NumeroCondutoresCarregados}).");
            mensagens.Add("A NBR 5410 (Tabelas 36/37) considera apenas 2 ou 3 condutores carregados:");
            mensagens.Add("  • 2 condutores = Monofásico (Fase + Neutro)");
            mensagens.Add("  • 3 condutores = Trifásico (3 Fases ou 2 Fases + Neutro)");
            
            return new ResultadoDimensionamento
            {
                IdentificacaoCircuito = dados.IdentificacaoCircuito,
                CorrenteProjetoIb = 0,
                FatorCorrecaoTemperatura = 1,
                FatorResistividadeSolo = 1,
                FatorCorrecaoAgrupamento = 1,
                SecaoNominalFase = 0,
                CapacidadeTabelaIz = 0,
                CapacidadeCorrigidaIzLinha = 0,
                QuedaTensaoPercentual = 0,
                SecaoNominalPE = 0,
                TipoMetal = dados.TipoMetal,
                TipoIsolacao = dados.TipoIsolacao,
                Aprovado = false,
                Mensagens = mensagens,
                DadosEntrada = dados
            };
        }
        
        // PASSO 1: Calcular corrente de projeto (Ib)
        var correnteIb = CalcularCorrenteProjeto(dados);
        
        // PASSO 2: Obter fatores de correção
        var fct = ObterFatorCorrecaoTemperatura(dados);
        var frt = ObterFatorResistividadeSolo(dados);
        var fca = ObterFatorCorrecaoAgrupamento(dados);
        
        // PASSO 3: Selecionar estratégia e encontrar bitola adequada
        var strategy = CapacidadeConducaoStrategyFactory.Criar(dados.TipoIsolacao);
        var bitolasDisponiveis = strategy.ObterBitolasDisponiveis();
        
        // PASSO 4: Aplicar specifications e encontrar bitola válida
        var bitolaMinSpec = new BitolaMinima25mm2Specification(ehCircuitoForca: true);
        var quedaTensaoSpec = new QuedaTensaoMaxima4Specification();
        
        double? secaoSelecionada = null;
        double capacidadeTabelaIz = 0;
        double capacidadeCorrigidaIzLinha = 0;
        double quedaTensaoPercentual = 0;
        
        foreach (var bitola in bitolasDisponiveis)
        {
            // Verificar bitola mínima
            if (!bitolaMinSpec.IsSatisfiedBy(bitola))
                continue;
            
            // Obter capacidade de condução da tabela
            var iz = strategy.ObterCapacidadeConducao(
                bitola, 
                dados.MetodoInstalacao, 
                dados.NumeroCondutoresCarregados
            );
            
            if (iz == null)
                continue;
            
            // Calcular capacidade corrigida: Iz' = Iz × FCT × FRT × FCA
            var izLinha = iz.Value * fct * frt * fca;
            
            // Verificar se atende à corrente de projeto
            if (izLinha < correnteIb)
                continue;
            
            // Verificar critério de queda de tensão
            var context = new DimensionamentoContext
            {
                DadosEntrada = dados,
                CorrenteProjetoIb = correnteIb,
                SecaoNominalMm2 = bitola
            };
            
            var quedaTensao = QuedaTensaoMaxima4Specification.CalcularQuedaTensao(context);
            
            if (!quedaTensaoSpec.IsSatisfiedBy(context))
            {
                mensagens.Add($"Bitola {bitola} mm² foi rejeitada: queda de tensão de {quedaTensao:F2}% excede 4%");
                continue;
            }
            
            // Bitola encontrada!
            secaoSelecionada = bitola;
            capacidadeTabelaIz = iz.Value;
            capacidadeCorrigidaIzLinha = izLinha;
            quedaTensaoPercentual = quedaTensao;
            break;
        }
        
        // Validar se encontrou uma bitola válida
        if (secaoSelecionada == null)
        {
            mensagens.Add("ERRO: Nenhuma bitola comercial atende a todos os critérios da NBR 5410");
            
            return new ResultadoDimensionamento
            {
                IdentificacaoCircuito = dados.IdentificacaoCircuito,
                CorrenteProjetoIb = correnteIb,
                FatorCorrecaoTemperatura = fct,
                FatorResistividadeSolo = frt,
                FatorCorrecaoAgrupamento = fca,
                SecaoNominalFase = 0,
                CapacidadeTabelaIz = 0,
                CapacidadeCorrigidaIzLinha = 0,
                QuedaTensaoPercentual = 0,
                SecaoNominalPE = 0,
                TipoMetal = dados.TipoMetal,
                TipoIsolacao = dados.TipoIsolacao,
                Aprovado = false,
                Mensagens = mensagens,
                DadosEntrada = dados
            };
        }
        
        // PASSO 5: Calcular condutor de proteção (PE)
        var secaoPE = Tabela58CondutorPE.CalcularSecaoPE(secaoSelecionada.Value);
        var secaoPEComercial = Tabela58CondutorPE.AjustarBitolaComercialPE(secaoPE);
        
        // Adicionar mensagens informativas
        mensagens.Add($"Bitola selecionada: {secaoSelecionada.Value} mm² (atende todos os critérios)");
        mensagens.Add($"Capacidade de condução corrigida: {capacidadeCorrigidaIzLinha:F2} A ≥ {correnteIb:F2} A (Ib)");
        mensagens.Add($"Queda de tensão: {quedaTensaoPercentual:F2}% ≤ 4.0%");
        mensagens.Add($"Condutor PE: {secaoPEComercial} mm²");
        
        return new ResultadoDimensionamento
        {
            IdentificacaoCircuito = dados.IdentificacaoCircuito,
            CorrenteProjetoIb = correnteIb,
            FatorCorrecaoTemperatura = fct,
            FatorResistividadeSolo = frt,
            FatorCorrecaoAgrupamento = fca,
            SecaoNominalFase = secaoSelecionada.Value,
            CapacidadeTabelaIz = capacidadeTabelaIz,
            CapacidadeCorrigidaIzLinha = capacidadeCorrigidaIzLinha,
            QuedaTensaoPercentual = quedaTensaoPercentual,
            SecaoNominalPE = secaoPEComercial,
            TipoMetal = dados.TipoMetal,
            TipoIsolacao = dados.TipoIsolacao,
            Aprovado = true,
            Mensagens = mensagens,
            DadosEntrada = dados
        };
    }
    
    /// <summary>
    /// Calcula a corrente de projeto: Ib = P / (U × FP × η)
    /// </summary>
    private double CalcularCorrenteProjeto(DadosEntrada dados)
    {
        return dados.PotenciaAtivaWatts / 
               (dados.TensaoVolts * dados.FatorPotencia * dados.Rendimento);
    }
    
    /// <summary>
    /// Obtém o fator de correção de temperatura (FCT)
    /// </summary>
    private double ObterFatorCorrecaoTemperatura(DadosEntrada dados)
    {
        return Tabela40TemperaturaFCT.ObterFator(
            dados.TemperaturaAmbienteCelsius, 
            dados.TipoIsolacao
        );
    }
    
    /// <summary>
    /// Obtém o fator de resistividade térmica do solo (FRT)
    /// Retorna 1.0 para instalações não subterrâneas
    /// </summary>
    private double ObterFatorResistividadeSolo(DadosEntrada dados)
    {
        if (dados.TipoLinha == TipoLinha.NaoSubterranea)
            return 1.0;
        
        var resistividade = dados.ResistividadeTermicaSoloKmW ?? 2.5; // Valor padrão NBR 5410
        return TabelaResistividadeSoloFRT.ObterFator(resistividade);
    }
    
    /// <summary>
    /// Obtém o fator de correção de agrupamento (FCA)
    /// </summary>
    private double ObterFatorCorrecaoAgrupamento(DadosEntrada dados)
    {
        return TabelaAgrupamentoFCA.ObterFator(
            dados.NumeroCircuitosAgrupados, 
            dados.MetodoInstalacao
        );
    }
}
