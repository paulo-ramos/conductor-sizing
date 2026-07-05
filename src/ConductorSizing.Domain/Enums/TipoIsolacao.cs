namespace ConductorSizing.Domain.Enums;

/// <summary>
/// Tipo de isolação do condutor conforme NBR 5410
/// </summary>
public enum TipoIsolacao
{
    /// <summary>
    /// Isolação em PVC - Temperatura máxima 70°C
    /// Tabela 36 da NBR 5410
    /// </summary>
    PVC = 1,
    
    /// <summary>
    /// Isolação em XLPE (Polietileno Reticulado) ou EPR (Borracha Etileno-Propileno)
    /// Temperatura máxima 90°C
    /// Tabela 37 da NBR 5410
    /// </summary>
    XLPE_EPR = 2
}
