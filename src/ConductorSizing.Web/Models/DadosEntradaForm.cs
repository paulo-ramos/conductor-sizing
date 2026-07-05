using ConductorSizing.Domain.Enums;

namespace ConductorSizing.Web.Models;

/// <summary>
/// ViewModel mutável para o formulário Blazor de dimensionamento
/// </summary>
public class DadosEntradaForm
{
    public string IdentificacaoCircuito { get; set; } = string.Empty;
    public double PotenciaAtivaWatts { get; set; }
    public double TensaoVolts { get; set; }
    public double ComprimentoMetros { get; set; }
    public double FatorPotencia { get; set; }
    public double Rendimento { get; set; }
    public TipoMetal TipoMetal { get; set; }
    public TipoIsolacao TipoIsolacao { get; set; }
    public string MetodoInstalacao { get; set; } = string.Empty;
    public int NumeroCondutoresCarregados { get; set; }
    public int TemperaturaAmbienteCelsius { get; set; }
    public int NumeroCircuitosAgrupados { get; set; }
    public TipoLinha TipoLinha { get; set; }
    public double? ResistividadeTermicaSoloKmW { get; set; }
}
