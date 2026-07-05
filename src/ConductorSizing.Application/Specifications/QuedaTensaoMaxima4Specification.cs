using ConductorSizing.Application.Data;
using ConductorSizing.Domain.Interfaces;
using ConductorSizing.Domain.Models;

namespace ConductorSizing.Application.Specifications;

/// <summary>
/// Specification para validar o critério de queda de tensão máxima de 4%
/// Conforme NBR 5410:2004
/// </summary>
public class QuedaTensaoMaxima4Specification : ISpecification<DimensionamentoContext>
{
    private const double QuedaTensaoMaximaPercentual = 4.0;
    
    public string MensagemErro => 
        $"Queda de tensão excede o limite máximo de {QuedaTensaoMaximaPercentual}% (NBR 5410:2004)";
    
    /// <summary>
    /// Verifica se a queda de tensão está dentro do limite de 4%
    /// </summary>
    /// <param name="context">Contexto com os dados do dimensionamento</param>
    /// <returns>True se atende, False caso contrário</returns>
    public bool IsSatisfiedBy(DimensionamentoContext context)
    {
        var quedaTensao = CalcularQuedaTensao(context);
        return quedaTensao <= QuedaTensaoMaximaPercentual;
    }
    
    /// <summary>
    /// Calcula a queda de tensão em percentual
    /// ΔU% = (2 × ρ × L × FP × Ib) / (S × U) × 100
    /// Onde:
    /// - ρ (rho) = resistividade elétrica do condutor [Ω.mm²/m]
    /// - L = comprimento do circuito [m]
    /// - FP = fator de potência
    /// - Ib = corrente de projeto [A]
    /// - S = seção do condutor [mm²]
    /// - U = tensão [V]
    /// </summary>
    public static double CalcularQuedaTensao(DimensionamentoContext context)
    {
        var resistividade = ResistividadeEletrica.Obter(
            context.DadosEntrada.TipoMetal, 
            context.DadosEntrada.TipoIsolacao
        );
        
        var quedaTensaoVolts = (2.0 * resistividade * context.DadosEntrada.ComprimentoMetros * 
                                context.DadosEntrada.FatorPotencia * context.CorrenteProjetoIb) / 
                               context.SecaoNominalMm2;
        
        var quedaTensaoPercentual = (quedaTensaoVolts / context.DadosEntrada.TensaoVolts) * 100.0;
        
        return quedaTensaoPercentual;
    }
}

/// <summary>
/// Contexto para validação de dimensionamento
/// Contém os dados necessários para as specifications
/// </summary>
public record DimensionamentoContext
{
    public required DadosEntrada DadosEntrada { get; init; }
    public required double CorrenteProjetoIb { get; init; }
    public required double SecaoNominalMm2 { get; init; }
}
