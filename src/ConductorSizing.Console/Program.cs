using ConductorSizing.Application.Services;
using ConductorSizing.Domain.Enums;
using ConductorSizing.Domain.Models;
using ConductorSizing.Infrastructure.Reports;

Console.WriteLine("╔═══════════════════════════════════════════════════════════╗");
Console.WriteLine("║  DIMENSIONAMENTO DE CONDUTORES ELÉTRICOS - NBR 5410:2004 ║");
Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");
Console.WriteLine();

// Exemplo 1: Circuito de ar-condicionado
var dadosExemplo = new DadosEntrada
{
    IdentificacaoCircuito = "Circuito 5 - Ar-Condicionado Split 12.000 BTU",
    PotenciaAtivaWatts = 1320.0,     // Potência típica de um AC 12.000 BTU
    TensaoVolts = 220.0,              // Tensão 220V
    ComprimentoMetros = 25.0,         // 25 metros do quadro até o AC
    FatorPotencia = 0.92,             // FP típico de compressor
    Rendimento = 0.85,                // Rendimento típico
    TipoMetal = TipoMetal.Cobre,
    TipoIsolacao = TipoIsolacao.PVC,
    MetodoInstalacao = "B1",          // Cabo unipolar em eletroduto
    NumeroCondutoresCarregados = 2,   // Fase + Neutro (monofásico)
    TemperaturaAmbienteCelsius = 35,  // Temperatura ambiente 35°C
    NumeroCircuitosAgrupados = 3,     // 3 circuitos no mesmo eletroduto
    TipoLinha = TipoLinha.NaoSubterranea,
    ResistividadeTermicaSoloKmW = null
};

Console.WriteLine("🔧 DADOS DO EXEMPLO:");
Console.WriteLine($"   Circuito: {dadosExemplo.IdentificacaoCircuito}");
Console.WriteLine($"   Potência: {dadosExemplo.PotenciaAtivaWatts:F0} W");
Console.WriteLine($"   Tensão: {dadosExemplo.TensaoVolts:F0} V");
Console.WriteLine($"   Comprimento: {dadosExemplo.ComprimentoMetros:F1} m");
Console.WriteLine();

// Executar dimensionamento
Console.WriteLine("⚡ PROCESSANDO DIMENSIONAMENTO...");
Console.WriteLine();

var servico = new DimensionamentoService();
var resultado = servico.Calcular(dadosExemplo);

// Exibir resultados
Console.WriteLine("═══════════════════════ RESULTADOS ═══════════════════════");
Console.WriteLine();

if (resultado.Aprovado)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("✓ DIMENSIONAMENTO APROVADO");
    Console.ResetColor();
}
else
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("✗ DIMENSIONAMENTO REPROVADO");
    Console.ResetColor();
}

Console.WriteLine();
Console.WriteLine($"📊 Corrente de Projeto (Ib): {resultado.CorrenteProjetoIb:F2} A");
Console.WriteLine();
Console.WriteLine("🔧 Fatores de Correção:");
Console.WriteLine($"   • Temperatura (FCT): {resultado.FatorCorrecaoTemperatura:F3}");
Console.WriteLine($"   • Resistividade Solo (FRT): {resultado.FatorResistividadeSolo:F3}");
Console.WriteLine($"   • Agrupamento (FCA): {resultado.FatorCorrecaoAgrupamento:F3}");
Console.WriteLine();
Console.WriteLine($"📏 Capacidade de Condução:");
Console.WriteLine($"   • Iz (Tabela): {resultado.CapacidadeTabelaIz:F2} A");
Console.WriteLine($"   • Iz' (Corrigida): {resultado.CapacidadeCorrigidaIzLinha:F2} A");
Console.WriteLine();
Console.WriteLine($"📉 Queda de Tensão: {resultado.QuedaTensaoPercentual:F2}% (máx: 4.0%)");
Console.WriteLine();

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("╔════════════════════════════════════════╗");
Console.WriteLine($"║  CONDUTOR FASE: {resultado.SecaoNominalFase,4} mm²             ║");
Console.WriteLine($"║  CONDUTOR PE:   {resultado.SecaoNominalPE,4} mm²             ║");
Console.WriteLine("╚════════════════════════════════════════╝");
Console.ResetColor();
Console.WriteLine();

if (resultado.Mensagens.Any())
{
    Console.WriteLine("📝 OBSERVAÇÕES:");
    foreach (var mensagem in resultado.Mensagens)
    {
        Console.WriteLine($"   • {mensagem}");
    }
    Console.WriteLine();
}

// Gerar relatório PDF
try
{
    var caminhoRelatorio = Path.Combine(Directory.GetCurrentDirectory(), "relatorio_dimensionamento.pdf");
    RelatorioCondutorPDF.Gerar(resultado, caminhoRelatorio);
    
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"📄 Relatório PDF gerado: {caminhoRelatorio}");
    Console.ResetColor();
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"❌ Erro ao gerar PDF: {ex.Message}");
    Console.ResetColor();
}

Console.WriteLine();
Console.WriteLine("═══════════════════════════════════════════════════════════");
Console.WriteLine("Pressione qualquer tecla para sair...");
Console.ReadKey();
