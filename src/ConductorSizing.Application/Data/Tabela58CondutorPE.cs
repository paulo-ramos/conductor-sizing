namespace ConductorSizing.Application.Data;

/// <summary>
/// Tabela 58 da NBR 5410:2004 - Seção mínima do condutor de proteção (PE)
/// </summary>
public static class Tabela58CondutorPE
{
    /// <summary>
    /// Calcula a seção do condutor de proteção (PE) baseado na seção do condutor fase
    /// Conforme Tabela 58 da NBR 5410:2004
    /// </summary>
    /// <param name="secaoFaseMm2">Seção nominal do condutor fase em mm²</param>
    /// <returns>Seção nominal do condutor de proteção (PE) em mm²</returns>
    public static double CalcularSecaoPE(double secaoFaseMm2)
    {
        // Regra da Tabela 58:
        // S ≤ 16 mm² → SPE = S (mesma seção da fase)
        if (secaoFaseMm2 <= 16.0)
        {
            return secaoFaseMm2;
        }
        
        // 16 < S ≤ 35 mm² → SPE = 16 mm²
        if (secaoFaseMm2 <= 35.0)
        {
            return 16.0;
        }
        
        // S > 35 mm² → SPE = S / 2 (metade da seção da fase)
        return secaoFaseMm2 / 2.0;
    }
    
    /// <summary>
    /// Ajusta a seção do PE para a bitola comercial mais próxima (arredonda para cima)
    /// </summary>
    /// <param name="secaoPECalculada">Seção calculada do PE em mm²</param>
    /// <returns>Seção comercial mais próxima em mm²</returns>
    public static double AjustarBitolaComercialPE(double secaoPECalculada)
    {
        // Bitolas comerciais padrão
        double[] bitolasComerciais = 
        { 
            0.5, 0.75, 1.0, 1.5, 2.5, 4.0, 6.0, 10.0, 
            16.0, 25.0, 35.0, 50.0, 70.0, 95.0, 
            120.0, 150.0, 185.0, 240.0, 300.0, 400.0 
        };
        
        // Retorna a menor bitola comercial que seja maior ou igual à calculada
        return bitolasComerciais.FirstOrDefault(b => b >= secaoPECalculada, secaoPECalculada);
    }
}
