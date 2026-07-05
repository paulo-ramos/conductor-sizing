using ConductorSizing.Domain.Enums;

namespace ConductorSizing.Domain.Models;

/// <summary>
/// Resultado completo do dimensionamento do condutor elétrico conforme NBR 5410
/// </summary>
public record ResultadoDimensionamento
{
    /// <summary>
    /// Identificação do circuito analisado
    /// </summary>
    public required string IdentificacaoCircuito { get; init; }
    
    /// <summary>
    /// Corrente de projeto calculada (Ib) em Ampères
    /// Ib = P / (U × FP × η)
    /// </summary>
    public required double CorrenteProjetoIb { get; init; }
    
    /// <summary>
    /// Fator de correção de temperatura (FCT)
    /// </summary>
    public required double FatorCorrecaoTemperatura { get; init; }
    
    /// <summary>
    /// Fator de correção de resistividade térmica do solo (FRT)
    /// 1.0 para instalações não subterrâneas
    /// </summary>
    public required double FatorResistividadeSolo { get; init; }
    
    /// <summary>
    /// Fator de correção de agrupamento (FCA)
    /// </summary>
    public required double FatorCorrecaoAgrupamento { get; init; }
    
    /// <summary>
    /// Seção nominal selecionada em mm² para o condutor fase
    /// </summary>
    public required double SecaoNominalFase { get; init; }
    
    /// <summary>
    /// Capacidade de condução de corrente da tabela (Iz) em Ampères
    /// Valor tabelado antes das correções
    /// </summary>
    public required double CapacidadeTabelaIz { get; init; }
    
    /// <summary>
    /// Capacidade de condução real corrigida (Iz') em Ampères
    /// Iz' = Iz × FCT × FRT × FCA
    /// </summary>
    public required double CapacidadeCorrigidaIzLinha { get; init; }
    
    /// <summary>
    /// Queda de tensão calculada em percentual (ΔU%)
    /// </summary>
    public required double QuedaTensaoPercentual { get; init; }
    
    /// <summary>
    /// Seção nominal do condutor de proteção (PE) em mm²
    /// Calculada conforme Tabela 58 da NBR 5410
    /// </summary>
    public required double SecaoNominalPE { get; init; }
    
    /// <summary>
    /// Tipo de metal do condutor
    /// </summary>
    public required TipoMetal TipoMetal { get; init; }
    
    /// <summary>
    /// Tipo de isolação do condutor
    /// </summary>
    public required TipoIsolacao TipoIsolacao { get; init; }
    
    /// <summary>
    /// Indica se o dimensionamento atende a todos os critérios da NBR 5410
    /// </summary>
    public required bool Aprovado { get; init; }
    
    /// <summary>
    /// Mensagens de validação ou alertas
    /// </summary>
    public List<string> Mensagens { get; init; } = new();
    
    /// <summary>
    /// Dados de entrada utilizados no cálculo
    /// </summary>
    public required DadosEntrada DadosEntrada { get; init; }
}
