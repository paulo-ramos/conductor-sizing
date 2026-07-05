namespace ConductorSizing.Domain.Interfaces;

/// <summary>
/// Specification Pattern: Interface para especificações de validação
/// </summary>
/// <typeparam name="T">Tipo do objeto a ser validado</typeparam>
public interface ISpecification<in T>
{
    /// <summary>
    /// Verifica se o objeto satisfaz a especificação
    /// </summary>
    /// <param name="obj">Objeto a ser validado</param>
    /// <returns>True se satisfaz, False caso contrário</returns>
    bool IsSatisfiedBy(T obj);
    
    /// <summary>
    /// Mensagem descritiva da validação quando não satisfeita
    /// </summary>
    string MensagemErro { get; }
}
