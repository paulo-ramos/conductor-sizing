using ConductorSizing.Domain.Enums;

namespace ConductorSizing.Application.Data;

/// <summary>
/// Constantes de resistividade elétrica dos condutores
/// Valores em Ω.mm²/m (Ohm × milímetro quadrado por metro) a 20°C
/// </summary>
public static class ResistividadeEletrica
{
    /// <summary>
    /// Resistividade do COBRE com isolação PVC (70°C)
    /// Valor típico: 0.01786 Ω.mm²/m
    /// </summary>
    public const double CobrePVC = 0.01786;
    
    /// <summary>
    /// Resistividade do COBRE com isolação XLPE/EPR (90°C)
    /// Valor típico: 0.0173 Ω.mm²/m
    /// Menor devido à temperatura de trabalho mais alta
    /// </summary>
    public const double CobreXLPE = 0.0173;
    
    /// <summary>
    /// Resistividade do ALUMÍNIO com isolação PVC (70°C)
    /// Valor típico: 0.0293 Ω.mm²/m
    /// </summary>
    public const double AluminioPVC = 0.0293;
    
    /// <summary>
    /// Resistividade do ALUMÍNIO com isolação XLPE/EPR (90°C)
    /// Valor típico: 0.0283 Ω.mm²/m
    /// </summary>
    public const double AluminioXLPE = 0.0283;
    
    /// <summary>
    /// Obtém a resistividade elétrica baseada no tipo de metal e isolação
    /// </summary>
    /// <param name="tipoMetal">Tipo de metal (Cobre ou Alumínio)</param>
    /// <param name="tipoIsolacao">Tipo de isolação (PVC ou XLPE/EPR)</param>
    /// <returns>Resistividade em Ω.mm²/m</returns>
    public static double Obter(TipoMetal tipoMetal, TipoIsolacao tipoIsolacao)
    {
        return (tipoMetal, tipoIsolacao) switch
        {
            (TipoMetal.Cobre, TipoIsolacao.PVC) => CobrePVC,
            (TipoMetal.Cobre, TipoIsolacao.XLPE_EPR) => CobreXLPE,
            (TipoMetal.Aluminio, TipoIsolacao.PVC) => AluminioPVC,
            (TipoMetal.Aluminio, TipoIsolacao.XLPE_EPR) => AluminioXLPE,
            _ => CobrePVC // Valor padrão
        };
    }
}
