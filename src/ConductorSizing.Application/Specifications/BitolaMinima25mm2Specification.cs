using ConductorSizing.Domain.Interfaces;

namespace ConductorSizing.Application.Specifications;

/// <summary>
/// Specification para validar bitola mínima de 2.5 mm² para circuitos de força/TUE
/// Conforme NBR 5410:2004 - Tabela 47
/// </summary>
public class BitolaMinima25mm2Specification : ISpecification<double>
{
    private const double BitolaMinima = 2.5;
    private readonly bool _ehCircuitoForca;
    
    public string MensagemErro => 
        $"Bitola mínima para circuitos de força/TUE é {BitolaMinima} mm² (NBR 5410 - Tabela 47)";
    
    /// <summary>
    /// Construtor da specification
    /// </summary>
    /// <param name="ehCircuitoForca">Indica se é um circuito de força/TUE (true) ou iluminação (false)</param>
    public BitolaMinima25mm2Specification(bool ehCircuitoForca = true)
    {
        _ehCircuitoForca = ehCircuitoForca;
    }
    
    /// <summary>
    /// Verifica se a bitola selecionada atende ao requisito mínimo
    /// </summary>
    /// <param name="secaoMm2">Seção nominal em mm²</param>
    /// <returns>True se atende, False caso contrário</returns>
    public bool IsSatisfiedBy(double secaoMm2)
    {
        // Para circuitos de força/TUE, a bitola mínima é 2.5 mm²
        if (_ehCircuitoForca)
        {
            return secaoMm2 >= BitolaMinima;
        }
        
        // Para iluminação, pode ser menor (ex: 1.5 mm²)
        return true;
    }
}
