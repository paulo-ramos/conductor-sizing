using ConductorSizing.Domain.Enums;

namespace ConductorSizing.Application.Data;

/// <summary>
/// Tabela 40 da NBR 5410:2004 - Fatores de correção para temperaturas ambientes diferentes de 30°C (ar) / 20°C (solo)
/// </summary>
public static class Tabela40TemperaturaFCT
{
    /// <summary>
    /// Fatores de correção por temperatura para isolação PVC (70°C)
    /// Key: Temperatura ambiente (°C)
    /// Value: Fator de correção (FCT)
    /// </summary>
    public static readonly Dictionary<int, double> FatorPVC = new()
    {
        { 10, 1.22 },
        { 15, 1.17 },
        { 20, 1.12 },
        { 25, 1.06 },
        { 30, 1.00 },  // Temperatura de referência
        { 35, 0.94 },
        { 40, 0.87 },
        { 45, 0.79 },
        { 50, 0.71 },
        { 55, 0.61 },
        { 60, 0.50 }
    };
    
    /// <summary>
    /// Fatores de correção por temperatura para isolação XLPE/EPR (90°C)
    /// Key: Temperatura ambiente (°C)
    /// Value: Fator de correção (FCT)
    /// </summary>
    public static readonly Dictionary<int, double> FatorXLPE = new()
    {
        { 10, 1.15 },
        { 15, 1.12 },
        { 20, 1.08 },
        { 25, 1.04 },
        { 30, 1.00 },  // Temperatura de referência
        { 35, 0.96 },
        { 40, 0.91 },
        { 45, 0.87 },
        { 50, 0.82 },
        { 55, 0.76 },
        { 60, 0.71 },
        { 65, 0.65 },
        { 70, 0.58 },
        { 75, 0.50 },
        { 80, 0.41 }
    };
    
    /// <summary>
    /// Obtém o fator de correção de temperatura baseado no tipo de isolação
    /// </summary>
    /// <param name="temperaturaAmbiente">Temperatura ambiente em °C</param>
    /// <param name="tipoIsolacao">Tipo de isolação do condutor</param>
    /// <returns>Fator de correção (FCT)</returns>
    public static double ObterFator(int temperaturaAmbiente, TipoIsolacao tipoIsolacao)
    {
        var tabela = tipoIsolacao == TipoIsolacao.PVC ? FatorPVC : FatorXLPE;
        
        if (tabela.TryGetValue(temperaturaAmbiente, out double fator))
        {
            return fator;
        }
        
        // Interpolar se temperatura exata não existir
        var temperaturasOrdenadas = tabela.Keys.OrderBy(k => k).ToList();
        
        if (temperaturaAmbiente < temperaturasOrdenadas.First())
            return tabela[temperaturasOrdenadas.First()];
        
        if (temperaturaAmbiente > temperaturasOrdenadas.Last())
            return tabela[temperaturasOrdenadas.Last()];
        
        // Interpolação linear
        var tempInferior = temperaturasOrdenadas.LastOrDefault(t => t < temperaturaAmbiente);
        var tempSuperior = temperaturasOrdenadas.FirstOrDefault(t => t > temperaturaAmbiente);
        
        if (tempInferior == 0 || tempSuperior == 0)
            return 1.0;
        
        var fatorInferior = tabela[tempInferior];
        var fatorSuperior = tabela[tempSuperior];
        
        // Interpolação linear: y = y1 + (x - x1) * (y2 - y1) / (x2 - x1)
        return fatorInferior + (temperaturaAmbiente - tempInferior) * (fatorSuperior - fatorInferior) / (tempSuperior - tempInferior);
    }
}
