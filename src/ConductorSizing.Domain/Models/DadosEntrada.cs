using ConductorSizing.Domain.Enums;

namespace ConductorSizing.Domain.Models;

/// <summary>
/// Dados de entrada para o dimensionamento do condutor elétrico
/// </summary>
public record DadosEntrada
{
    /// <summary>
    /// Identificação do circuito (ex: "Circuito 5 - Compressor")
    /// </summary>
    public required string IdentificacaoCircuito { get; init; }
    
    /// <summary>
    /// Potência ativa em Watts (P)
    /// </summary>
    public required double PotenciaAtivaWatts { get; init; }
    
    /// <summary>
    /// Tensão em Volts (U) - Ex: 127V, 220V, 380V
    /// </summary>
    public required double TensaoVolts { get; init; }
    
    /// <summary>
    /// Comprimento do circuito em metros (L)
    /// </summary>
    public required double ComprimentoMetros { get; init; }
    
    /// <summary>
    /// Fator de potência (FP) - Valor entre 0 e 1 (ex: 0.92)
    /// </summary>
    public required double FatorPotencia { get; init; }
    
    /// <summary>
    /// Rendimento (η - eta) - Valor entre 0 e 1 (ex: 0.85 para motores)
    /// Para cargas resistivas puras, usar 1.0
    /// </summary>
    public required double Rendimento { get; init; }
    
    /// <summary>
    /// Tipo de metal condutor (Cobre ou Alumínio)
    /// </summary>
    public required TipoMetal TipoMetal { get; init; }
    
    /// <summary>
    /// Tipo de isolação do condutor (PVC 70°C ou XLPE/EPR 90°C)
    /// </summary>
    public required TipoIsolacao TipoIsolacao { get; init; }
    
    /// <summary>
    /// Método de instalação conforme NBR 5410
    /// Exemplos: "B1", "A1", "C", "D", "E", "F", "G"
    /// </summary>
    public required string MetodoInstalacao { get; init; }
    
    /// <summary>
    /// Número de condutores carregados no circuito
    /// 2 condutores: Monofásico (F+N) ou CC
    /// 3 condutores: Trifásico (3F) ou Trifásico com Neutro (3F+N)
    /// </summary>
    public required int NumeroCondutoresCarregados { get; init; }
    
    /// <summary>
    /// Temperatura ambiente em graus Celsius (para linhas não subterrâneas)
    /// ou temperatura do solo (para linhas subterrâneas)
    /// Exemplos: 25°C, 30°C, 35°C, 40°C
    /// </summary>
    public required int TemperaturaAmbienteCelsius { get; init; }
    
    /// <summary>
    /// Número de circuitos agrupados ou cabos multipolares agrupados
    /// Mínimo: 1 (circuito isolado)
    /// </summary>
    public required int NumeroCircuitosAgrupados { get; init; }
    
    /// <summary>
    /// Tipo de linha (aérea/não subterrânea ou subterrânea)
    /// </summary>
    public required TipoLinha TipoLinha { get; init; }
    
    /// <summary>
    /// Resistividade térmica do solo em K.m/W (apenas para linhas subterrâneas)
    /// Valores típicos: 1.0 (solo úmido), 2.5 (solo normal), 3.0 (solo seco)
    /// NBR 5410 considera 2.5 K.m/W como valor padrão
    /// Null ou 0 para linhas não subterrâneas
    /// </summary>
    public double? ResistividadeTermicaSoloKmW { get; init; }
}
