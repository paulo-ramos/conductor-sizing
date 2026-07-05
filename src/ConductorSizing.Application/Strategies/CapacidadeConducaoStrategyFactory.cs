using ConductorSizing.Domain.Enums;
using ConductorSizing.Domain.Interfaces;

namespace ConductorSizing.Application.Strategies;

/// <summary>
/// Factory para criar a estratégia apropriada baseada no tipo de isolação
/// </summary>
public static class CapacidadeConducaoStrategyFactory
{
    /// <summary>
    /// Cria a estratégia apropriada para o tipo de isolação
    /// </summary>
    /// <param name="tipoIsolacao">Tipo de isolação do condutor</param>
    /// <returns>Estratégia de capacidade de condução</returns>
    public static ICapacidadeConducaoStrategy Criar(TipoIsolacao tipoIsolacao)
    {
        return tipoIsolacao switch
        {
            TipoIsolacao.PVC => new IsolacaoPvcStrategy(),
            TipoIsolacao.XLPE_EPR => new IsolacaoXlpeEprStrategy(),
            _ => throw new ArgumentException($"Tipo de isolação não suportado: {tipoIsolacao}", nameof(tipoIsolacao))
        };
    }
}
