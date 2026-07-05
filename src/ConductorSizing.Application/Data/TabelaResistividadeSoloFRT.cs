namespace ConductorSizing.Application.Data;

/// <summary>
/// Fatores de correção para resistividade térmica do solo (FRT)
/// Aplicável apenas para linhas subterrâneas
/// </summary>
public static class TabelaResistividadeSoloFRT
{
    /// <summary>
    /// Fatores de correção por resistividade térmica do solo
    /// Key: Resistividade térmica do solo em K.m/W
    /// Value: Fator de correção (FRT)
    /// Referência: resistividade de 2.5 K.m/W (valor padrão NBR 5410)
    /// </summary>
    public static readonly Dictionary<double, double> FatoresResistividade = new()
    {
        { 0.5, 1.28 },   // Solo muito úmido
        { 0.7, 1.20 },
        { 1.0, 1.18 },   // Solo úmido
        { 1.2, 1.14 },
        { 1.5, 1.10 },
        { 2.0, 1.05 },
        { 2.5, 1.00 },   // Valor de referência (solo normal)
        { 3.0, 0.96 },   // Solo seco
        { 3.5, 0.93 }    // Solo muito seco
    };
    
    /// <summary>
    /// Obtém o fator de correção de resistividade térmica do solo
    /// </summary>
    /// <param name="resistividadeKmW">Resistividade térmica do solo em K.m/W</param>
    /// <returns>Fator de correção (FRT)</returns>
    public static double ObterFator(double resistividadeKmW)
    {
        if (resistividadeKmW <= 0)
            return 1.0;
        
        if (FatoresResistividade.TryGetValue(resistividadeKmW, out double fator))
        {
            return fator;
        }
        
        // Interpolar se valor exato não existir
        var valoresOrdenados = FatoresResistividade.Keys.OrderBy(k => k).ToList();
        
        if (resistividadeKmW < valoresOrdenados.First())
            return FatoresResistividade[valoresOrdenados.First()];
        
        if (resistividadeKmW > valoresOrdenados.Last())
            return FatoresResistividade[valoresOrdenados.Last()];
        
        // Interpolação linear
        var resInferior = valoresOrdenados.LastOrDefault(r => r < resistividadeKmW);
        var resSuperior = valoresOrdenados.FirstOrDefault(r => r > resistividadeKmW);
        
        if (resInferior == 0 || resSuperior == 0)
            return 1.0;
        
        var fatorInferior = FatoresResistividade[resInferior];
        var fatorSuperior = FatoresResistividade[resSuperior];
        
        // Interpolação linear
        return fatorInferior + (resistividadeKmW - resInferior) * (fatorSuperior - fatorInferior) / (resSuperior - resInferior);
    }
}
