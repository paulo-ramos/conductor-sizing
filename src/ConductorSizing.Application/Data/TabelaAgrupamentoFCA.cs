namespace ConductorSizing.Application.Data;

/// <summary>
/// Fatores de correção para agrupamento de circuitos (FCA)
/// Baseado em Tabelas 42 e 43 da NBR 5410:2004
/// </summary>
public static class TabelaAgrupamentoFCA
{
    /// <summary>
    /// Fatores de correção por agrupamento (simplificado)
    /// Key: Número de circuitos agrupados ou cabos multipolares
    /// Value: Fator de correção (FCA)
    /// 
    /// Aplicável para:
    /// - Cabos isolados ou cabos multipolares: em feixe ao ar livre ou sobre superfície
    /// - Uma camada sobre parede, piso ou bandeja não perfurada ou prateleira
    /// </summary>
    public static readonly Dictionary<int, double> FatoresAgrupamento = new()
    {
        { 1, 1.00 },   // 1 circuito (sem agrupamento)
        { 2, 0.80 },   // 2 circuitos
        { 3, 0.70 },   // 3 circuitos
        { 4, 0.65 },   // 4 circuitos
        { 5, 0.60 },   // 5 circuitos
        { 6, 0.57 },   // 6 circuitos
        { 7, 0.54 },   // 7 circuitos
        { 8, 0.52 },   // 8 circuitos
        { 9, 0.50 },   // 9 circuitos
        { 10, 0.48 },  // 10 circuitos
        { 11, 0.46 },  // 11 circuitos
        { 12, 0.45 },  // 12 circuitos
        { 13, 0.44 },  // 13 circuitos
        { 14, 0.43 },  // 14 circuitos
        { 15, 0.42 },  // 15 circuitos
        { 16, 0.41 },  // 16 circuitos
        { 17, 0.40 },  // 17 circuitos
        { 18, 0.40 },  // 18 circuitos
        { 19, 0.39 },  // 19 circuitos
        { 20, 0.39 }   // 20 ou mais circuitos
    };
    
    /// <summary>
    /// Obtém o fator de correção de agrupamento
    /// </summary>
    /// <param name="numeroCircuitos">Número de circuitos agrupados</param>
    /// <param name="metodoInstalacao">Método de instalação (opcional para ajustes futuros)</param>
    /// <returns>Fator de correção (FCA)</returns>
    public static double ObterFator(int numeroCircuitos, string? metodoInstalacao = null)
    {
        if (numeroCircuitos <= 0)
            return 1.0;
        
        // Se exceder 20 circuitos, usar o fator de 20+
        if (numeroCircuitos > 20)
            return FatoresAgrupamento[20];
        
        return FatoresAgrupamento.TryGetValue(numeroCircuitos, out double fator) ? fator : 1.0;
    }
}
