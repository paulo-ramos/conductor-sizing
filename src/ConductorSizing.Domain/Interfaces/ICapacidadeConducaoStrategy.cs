namespace ConductorSizing.Domain.Interfaces;

/// <summary>
/// Strategy Pattern: Estratégia para cálculo de capacidade de condução de corrente
/// baseado no tipo de isolação (PVC, XLPE/EPR)
/// </summary>
public interface ICapacidadeConducaoStrategy
{
    /// <summary>
    /// Obtém a capacidade de condução de corrente (Iz) da tabela apropriada
    /// Tabela 36 (PVC) ou Tabela 37 (XLPE/EPR) da NBR 5410
    /// </summary>
    /// <param name="secaoMm2">Seção nominal em mm²</param>
    /// <param name="metodoInstalacao">Método de instalação (ex: B1, A1, C, D)</param>
    /// <param name="numeroCondutoresCarregados">2 ou 3 condutores carregados</param>
    /// <returns>Capacidade de condução em Ampères ou null se não encontrado</returns>
    double? ObterCapacidadeConducao(double secaoMm2, string metodoInstalacao, int numeroCondutoresCarregados);
    
    /// <summary>
    /// Lista todas as bitolas comerciais disponíveis na tabela
    /// </summary>
    /// <returns>Lista de bitolas em mm² em ordem crescente</returns>
    List<double> ObterBitolasDisponiveis();
}
