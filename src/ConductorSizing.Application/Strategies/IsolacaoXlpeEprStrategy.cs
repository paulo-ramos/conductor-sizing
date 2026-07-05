using ConductorSizing.Application.Data;
using ConductorSizing.Domain.Interfaces;

namespace ConductorSizing.Application.Strategies;

/// <summary>
/// Estratégia para cálculo de capacidade de condução com isolação XLPE/EPR (90°C)
/// Utiliza Tabela 37 da NBR 5410:2004
/// </summary>
public class IsolacaoXlpeEprStrategy : ICapacidadeConducaoStrategy
{
    /// <summary>
    /// Obtém a capacidade de condução de corrente da Tabela 37 (XLPE/EPR)
    /// </summary>
    public double? ObterCapacidadeConducao(double secaoMm2, string metodoInstalacao, int numeroCondutoresCarregados)
    {
        if (!Tabela37XLPE.CapacidadeConducao.TryGetValue(secaoMm2, out var metodos))
        {
            return null;
        }
        
        var chave = Tabela37XLPE.ObterChave(metodoInstalacao, numeroCondutoresCarregados);
        
        return metodos.TryGetValue(chave, out var corrente) ? corrente : null;
    }
    
    /// <summary>
    /// Lista todas as bitolas comerciais disponíveis na Tabela 37
    /// </summary>
    public List<double> ObterBitolasDisponiveis()
    {
        return Tabela37XLPE.CapacidadeConducao.Keys.OrderBy(k => k).ToList();
    }
}
