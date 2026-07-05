namespace ConductorSizing.Domain.Enums;

/// <summary>
/// Tipo de linha para instalação
/// </summary>
public enum TipoLinha
{
    /// <summary>
    /// Linha não subterrânea (aérea, em eletroduto aparente, etc.)
    /// Fator de resistividade térmica do solo (FRT) = 1.0
    /// </summary>
    NaoSubterranea = 1,
    
    /// <summary>
    /// Linha subterrânea (enterrada diretamente ou em eletroduto no solo)
    /// Aplica fator de resistividade térmica do solo (FRT) baseado em Tabela 40
    /// </summary>
    Subterranea = 2
}
