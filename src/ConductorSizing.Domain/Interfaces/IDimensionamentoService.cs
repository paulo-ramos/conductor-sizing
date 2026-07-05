using ConductorSizing.Domain.Models;

namespace ConductorSizing.Domain.Interfaces;

/// <summary>
/// Interface para o serviço de dimensionamento de condutores
/// </summary>
public interface IDimensionamentoService
{
    /// <summary>
    /// Executa o dimensionamento completo do condutor elétrico conforme NBR 5410:2004
    /// </summary>
    /// <param name="dados">Dados de entrada do circuito</param>
    /// <returns>Resultado completo do dimensionamento</returns>
    ResultadoDimensionamento Calcular(DadosEntrada dados);
}
