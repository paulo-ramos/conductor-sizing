using ConductorSizing.Application.Data;
using ConductorSizing.Domain.Interfaces;

namespace ConductorSizing.Application.Strategies;

/// <summary>
/// Estratégia para cálculo de capacidade de condução com isolação PVC (70°C)
/// Utiliza Tabela 36 da NBR 5410:2004
/// </summary>
public class IsolacaoPvcStrategy : ICapacidadeConducaoStrategy
{
    /// <summary>
    /// Obtém a capacidade de condução de corrente da Tabela 36 (PVC)
    /// </summary>
    public double? ObterCapacidadeConducao(double secaoMm2, string metodoInstalacao, int numeroCondutoresCarregados)
    {
        if (!Tabela36PVC.CapacidadeConducao.TryGetValue(secaoMm2, out var metodos))
        {
            return null;
        }
        
        var chave = Tabela36PVC.ObterChave(metodoInstalacao, numeroCondutoresCarregados);
        
        return metodos.TryGetValue(chave, out var corrente) ? corrente : null;
    }
    
    /// <summary>
    /// Lista todas as bitolas comerciais disponíveis na Tabela 36
    /// </summary>
    public List<double> ObterBitolasDisponiveis()
    {
        return Tabela36PVC.CapacidadeConducao.Keys.OrderBy(k => k).ToList();
    }
}
