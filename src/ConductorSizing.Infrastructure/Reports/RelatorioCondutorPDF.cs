using ConductorSizing.Domain.Enums;
using ConductorSizing.Domain.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ConductorSizing.Infrastructure.Reports;

/// <summary>
/// Gerador de relatório PDF educativo otimizado para visualização mobile
/// Padrão de cores SENAI com explicações detalhadas dos cálculos
/// </summary>
public class RelatorioCondutorPDF
{
    // Cores SENAI
    private static readonly string CorSenaiVermelho = "#E30613";
    private static readonly string CorSenaiVermelhoEscuro = "#B60510";
    private static readonly string CorSenaiCinza = "#58595B";
    
    public static void Gerar(ResultadoDimensionamento resultado, string caminhoArquivo)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        Document.Create(container => CriarDocumento(container, resultado)).GeneratePdf(caminhoArquivo);
    }
    
    public static byte[] GerarBytes(ResultadoDimensionamento resultado)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        return Document.Create(container => CriarDocumento(container, resultado)).GeneratePdf();
    }
    
    private static void CriarDocumento(IDocumentContainer container, ResultadoDimensionamento resultado)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(20);
            page.PageColor(Colors.White);
            page.DefaultTextStyle(x => x.FontSize(9).FontFamily("Arial"));
            
            page.Header().Element(c => CriarCabecalho(c, resultado));
            page.Content().Element(c => CriarConteudo(c, resultado));
            page.Footer().AlignCenter().Text(text =>
            {
                text.Span("Relatório gerado em ").FontSize(7).FontColor(Colors.Grey.Medium);
                text.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm")).FontSize(7).Bold();
                text.Span(" | NBR 5410:2004").FontSize(7).FontColor(Colors.Grey.Medium);
            });
        });
    }
    
    private static void CriarCabecalho(IContainer container, ResultadoDimensionamento resultado)
    {
        container.Column(column =>
        {
            // Faixa vermelha SENAI
            column.Item().Background(CorSenaiVermelho).Padding(12).Column(col =>
            {
                col.Item().Text("RELATÓRIO TÉCNICO DE DIMENSIONAMENTO")
                    .FontSize(14).Bold().FontColor(Colors.White);
                col.Item().Text("Condutores Elétricos de Baixa Tensão")
                    .FontSize(9).FontColor(Colors.White);
            });
            
            // Status
            column.Item().PaddingTop(8).Row(row =>
            {
                row.RelativeItem().Column(c =>
                {
                    c.Item().Text("CIRCUITO").FontSize(8).FontColor(Colors.Grey.Darken1);
                    c.Item().Text(resultado.IdentificacaoCircuito).FontSize(10).Bold();
                });
                
                row.ConstantItem(120).AlignRight().Background(resultado.Aprovado ? Colors.Green.Medium : Colors.Red.Medium)
                    .Padding(6).AlignCenter()
                    .Text(resultado.Aprovado ? "APROVADO" : "REPROVADO")
                    .FontSize(10).Bold().FontColor(Colors.White);
            });
            
            column.Item().PaddingTop(5).LineHorizontal(1).LineColor(CorSenaiVermelho);
        });
    }
    
    private static void CriarConteudo(IContainer container, ResultadoDimensionamento resultado)
    {
        container.PaddingTop(10).Column(column =>
        {
            column.Spacing(10);
            
            // Dados de Entrada
            column.Item().Element(c => CriarDadosEntrada(c, resultado));
            
            // Se o circuito foi reprovado (bitola = 0), mostrar apenas os erros
            if (!resultado.Aprovado && resultado.SecaoNominalFase == 0)
            {
                column.Item().Background(Colors.Red.Lighten4).Border(2).BorderColor(Colors.Red.Medium).Padding(15).Column(erro =>
                {
                    erro.Item().Text("ERRO NO DIMENSIONAMENTO").FontSize(14).Bold().FontColor(Colors.Red.Darken2);
                    erro.Item().PaddingTop(8).Text("Não foi possível dimensionar o condutor. Verifique os dados de entrada:")
                        .FontSize(10);
                    
                    foreach (var msg in resultado.Mensagens)
                    {
                        erro.Item().PaddingTop(4).Text($"• {msg}").FontSize(9);
                    }
                });
                return;
            }
            
            // PASSO 1: Corrente de Projeto
            column.Item().Element(c => CriarPasso1(c, resultado));
            
            // PASSO 2: Capacidade de Condução
            column.Item().Element(c => CriarPasso2(c, resultado));
            
            // PASSO 3: Corrente Corrigida
            column.Item().Element(c => CriarPasso3(c, resultado));
            
            // PASSO 4: Queda de Tensão
            column.Item().Element(c => CriarPasso4(c, resultado));
            
            // PASSO 5: Condutor de Proteção
            column.Item().Element(c => CriarPasso5(c, resultado));
            
            // Quebra de página antes do resultado final
            column.Item().PageBreak();
            
            // Resultado Final
            column.Item().Element(c => CriarResultadoFinal(c, resultado));
        });
    }
    
    private static void CriarDadosEntrada(IContainer container, ResultadoDimensionamento resultado)
    {
        container.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(8).Column(column =>
        {
            column.Item().Background(Colors.Grey.Lighten3).Padding(5)
                .Text("DADOS DE ENTRADA").FontSize(10).Bold().FontColor(CorSenaiCinza);
            
            column.Item().PaddingTop(5).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(1);
                });
                
                // Linha 1
                table.Cell().Text("Potência:").FontSize(8).FontColor(Colors.Grey.Darken1);
                table.Cell().Text($"{resultado.DadosEntrada.PotenciaAtivaWatts:F0} W").FontSize(8).Bold();
                table.Cell().Text("Tensão:").FontSize(8).FontColor(Colors.Grey.Darken1);
                table.Cell().Text($"{resultado.DadosEntrada.TensaoVolts:F0} V").FontSize(8).Bold();
                
                // Linha 2
                table.Cell().Text("Comprimento:").FontSize(8).FontColor(Colors.Grey.Darken1);
                table.Cell().Text($"{resultado.DadosEntrada.ComprimentoMetros:F1} m").FontSize(8).Bold();
                table.Cell().Text("FP:").FontSize(8).FontColor(Colors.Grey.Darken1);
                table.Cell().Text($"{resultado.DadosEntrada.FatorPotencia:F2}").FontSize(8).Bold();
                
                // Linha 3
                table.Cell().Text("Metal:").FontSize(8).FontColor(Colors.Grey.Darken1);
                table.Cell().Text(resultado.DadosEntrada.TipoMetal.ToString()).FontSize(8).Bold();
                table.Cell().Text("Isolação:").FontSize(8).FontColor(Colors.Grey.Darken1);
                table.Cell().Text(resultado.DadosEntrada.TipoIsolacao.ToString().Replace("_", "/")).FontSize(8).Bold();
                
                // Linha 4
                table.Cell().Text("Método:").FontSize(8).FontColor(Colors.Grey.Darken1);
                table.Cell().Text(resultado.DadosEntrada.MetodoInstalacao).FontSize(8).Bold();
                table.Cell().Text("Condutores:").FontSize(8).FontColor(Colors.Grey.Darken1);
                table.Cell().Text($"{resultado.DadosEntrada.NumeroCondutoresCarregados}").FontSize(8).Bold();
            });
        });
    }
    
    private static void CriarPasso1(IContainer container, ResultadoDimensionamento resultado)
    {
        var dados = resultado.DadosEntrada;
        
        container.Border(1).BorderColor(CorSenaiVermelho).Column(column =>
        {
            // Título
            column.Item().Background(CorSenaiVermelho).Padding(8)
                .Text("PASSO 1 - CORRENTE DE PROJETO (Ib)").FontSize(11).Bold().FontColor(Colors.White);
            
            column.Item().Padding(10).Column(c =>
            {
                // Explicação
                c.Item().Text(text =>
                {
                    text.Span("A corrente de projeto (Ib) é a corrente que o circuito deve transportar em condições normais de funcionamento. ")
                        .FontSize(8);
                text.Span("É calculada dividindo a potência ativa pela tensão, fator de potência e rendimento do equipamento.")
                    .FontSize(8);
            });
            
            // Fórmula
            c.Item().PaddingTop(6).Background(Colors.Grey.Lighten4).Padding(6).Column(formula =>
            {
                formula.Item().Text("FÓRMULA:").FontSize(8).Bold().FontColor(CorSenaiVermelhoEscuro);
                formula.Item().Text("Ib = P / (U × FP × η)").FontSize(9).Bold().FontFamily("Courier New");
                formula.Item().PaddingTop(3).Text("Onde:").FontSize(7).Italic();
                formula.Item().Text($"P = {dados.PotenciaAtivaWatts:F0} W (Potência ativa)").FontSize(7);
                formula.Item().Text($"U = {dados.TensaoVolts:F0} V (Tensão)").FontSize(7);
                formula.Item().Text($"FP = {dados.FatorPotencia:F2} (Fator de potência)").FontSize(7);
                formula.Item().Text($"η = {dados.Rendimento:F2} (Rendimento)").FontSize(7);
            });
            
            // Cálculo
            c.Item().PaddingTop(6).Text(text =>
            {
                text.Span("CÁLCULO: ").FontSize(8).Bold().FontColor(CorSenaiVermelhoEscuro);
                text.Span($"Ib = {dados.PotenciaAtivaWatts:F0} / ({dados.TensaoVolts:F0} × {dados.FatorPotencia:F2} × {dados.Rendimento:F2}) = ")
                    .FontSize(8).FontFamily("Courier New");
                text.Span($"{resultado.CorrenteProjetoIb:F3} A").FontSize(9).Bold().FontColor(CorSenaiVermelho);
            });
            
            // Conclusão
            c.Item().PaddingTop(6).Background(Colors.Blue.Lighten5).Padding(6).Text(text =>
            {
                text.Span("CONCLUSÃO: ").FontSize(8).Bold().FontColor(Colors.Blue.Darken2);
                text.Span($"O circuito necessita transportar uma corrente de {resultado.CorrenteProjetoIb:F2} A em condições normais de operação.")
                    .FontSize(8);
            });
            });
        });
    }
    
    private static void CriarPasso2(IContainer container, ResultadoDimensionamento resultado)
    {
        var tabelaNome = resultado.TipoIsolacao == TipoIsolacao.PVC ? "Tabela 36" : "Tabela 37";
        var tempMaxima = resultado.TipoIsolacao == TipoIsolacao.PVC ? "70°C" : "90°C";
        
        container.Border(1).BorderColor(CorSenaiVermelho).Column(column =>
        {
            column.Item().Background(CorSenaiVermelho).Padding(8)
                .Text("PASSO 2 - CAPACIDADE DE CONDUÇÃO (Iz)").FontSize(11).Bold().FontColor(Colors.White);
            
            column.Item().Padding(10).Column(c =>
            {
                c.Item().Text(text =>
                {
                    text.Span($"A capacidade de condução de corrente (Iz) é obtida da {tabelaNome} da NBR 5410:2004, ")
                        .FontSize(8);
                    text.Span($"que especifica as capacidades para condutores com isolação {resultado.TipoIsolacao} ")
                        .FontSize(8);
                    text.Span($"(temperatura máxima no condutor {tempMaxima}). ")
                        .FontSize(8);
                });
                
                c.Item().PaddingTop(4).Text(text =>
                {
                    text.Span($"Para o método de instalação {resultado.DadosEntrada.MetodoInstalacao} ")
                        .FontSize(8);
                    text.Span($"com {resultado.DadosEntrada.NumeroCondutoresCarregados} condutores carregados, ")
                        .FontSize(8);
                    text.Span($"a bitola de {resultado.SecaoNominalFase} mm² suporta:")
                        .FontSize(8).Bold();
                });
                
                // Aviso sobre bitola mínima
                if (resultado.SecaoNominalFase >= 2.5)
                {
                    c.Item().PaddingTop(6).Background(Colors.Orange.Lighten4).BorderLeft(3).BorderColor(Colors.Orange.Darken2).Padding(6).Column(aviso =>
                    {
                        aviso.Item().Text("⚠ BITOLA MÍNIMA CONFORME NBR 5410").FontSize(8).Bold().FontColor(Colors.Orange.Darken3);
                        aviso.Item().PaddingTop(2).Text(text =>
                        {
                            text.Span("A Tabela 47 da NBR 5410:2004 estabelece que circuitos de ")
                                .FontSize(7);
                            text.Span("força (tomadas de uso específico - TUE)")
                                .FontSize(7).Bold();
                            text.Span(" devem ter seção mínima de ")
                                .FontSize(7);
                            text.Span("2,5 mm²")
                                .FontSize(7).Bold().FontColor(CorSenaiVermelho);
                            text.Span(", independentemente do cálculo de corrente. Circuitos de iluminação podem usar 1,5 mm².")
                                .FontSize(7);
                        });
                    });
                }
                
                c.Item().PaddingTop(6).Background(Colors.Grey.Lighten4).Padding(6).Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text($"TABELA UTILIZADA:").FontSize(8).Bold().FontColor(CorSenaiVermelhoEscuro);
                        col.Item().Text($"{tabelaNome} - NBR 5410:2004").FontSize(8);
                        col.Item().Text($"Isolação: {resultado.TipoIsolacao} ({tempMaxima})").FontSize(7).Italic();
                    });
                    
                    row.ConstantItem(100).AlignRight().Column(col =>
                    {
                        col.Item().Text("Iz =").FontSize(8).FontColor(Colors.Grey.Darken1);
                        col.Item().Text($"{resultado.CapacidadeTabelaIz:F1} A").FontSize(12).Bold().FontColor(CorSenaiVermelho);
                    });
                });
                
                c.Item().PaddingTop(6).Background(Colors.Blue.Lighten5).Padding(6).Text(text =>
                {
                    text.Span("CONCLUSÃO: ").FontSize(8).Bold().FontColor(Colors.Blue.Darken2);
                    text.Span($"Segundo a {tabelaNome}, um condutor de {resultado.TipoMetal} com seção de {resultado.SecaoNominalFase} mm² ")
                        .FontSize(8);
                    text.Span($"instalado pelo método {resultado.DadosEntrada.MetodoInstalacao} suporta {resultado.CapacidadeTabelaIz:F1} A. ")
                        .FontSize(8);
                    text.Span("Porém, este valor deve ser corrigido por fatores ambientais e de instalação.")
                        .FontSize(8).Italic();
                });
            });
        });
    }
    
    private static void CriarPasso3(IContainer container, ResultadoDimensionamento resultado)
    {
        var dados = resultado.DadosEntrada;
        var usaFRT = dados.TipoLinha == TipoLinha.Subterranea;
        
        container.Border(1).BorderColor(CorSenaiVermelho).Column(column =>
        {
            column.Item().Background(CorSenaiVermelho).Padding(8)
                .Text("PASSO 3 - CORRENTE CORRIGIDA (Iz')").FontSize(11).Bold().FontColor(Colors.White);
            
            column.Item().Padding(10).Column(c =>
            {
                c.Item().Text("A capacidade de condução deve ser corrigida por três fatores:")
                    .FontSize(8);
                
                // FCT
                c.Item().PaddingTop(6).Border(1).BorderColor(Colors.Grey.Lighten2).Padding(6).Column(col =>
                {
                    col.Item().Text("a) FATOR DE CORREÇÃO DE TEMPERATURA (FCT)").FontSize(9).Bold().FontColor(CorSenaiVermelhoEscuro);
                    col.Item().PaddingTop(3).Text(text =>
                    {
                        text.Span($"Tabela 40 da NBR 5410. Temperatura ambiente: {dados.TemperaturaAmbienteCelsius}°C. ")
                            .FontSize(8);
                        text.Span($"Para isolação {resultado.TipoIsolacao}, o fator de correção é ")
                            .FontSize(8);
                        text.Span($"FCT = {resultado.FatorCorrecaoTemperatura:F3}").FontSize(8).Bold().FontColor(CorSenaiVermelho);
                    });
                });
                
                // FRT
                c.Item().PaddingTop(4).Border(1).BorderColor(Colors.Grey.Lighten2).Padding(6).Column(col =>
                {
                    col.Item().Text("b) FATOR DE RESISTIVIDADE TÉRMICA DO SOLO (FRT)").FontSize(9).Bold().FontColor(CorSenaiVermelhoEscuro);
                    col.Item().PaddingTop(3).Text(text =>
                    {
                        if (usaFRT)
                        {
                            var resistividade = dados.ResistividadeTermicaSoloKmW ?? 2.5;
                            text.Span($"Linha subterrânea. Resistividade do solo: {resistividade} K.m/W. ")
                                .FontSize(8);
                            text.Span($"FRT = {resultado.FatorResistividadeSolo:F3}").FontSize(8).Bold().FontColor(CorSenaiVermelho);
                        }
                        else
                        {
                            text.Span("Linha NÃO subterrânea. ")
                                .FontSize(8).Bold();
                            text.Span("Portanto, FRT = 1.0 (não há correção por resistividade do solo).")
                                .FontSize(8);
                        }
                    });
                });
                
                // FCA
                c.Item().PaddingTop(4).Border(1).BorderColor(Colors.Grey.Lighten2).Padding(6).Column(col =>
                {
                    col.Item().Text("c) FATOR DE CORREÇÃO DE AGRUPAMENTO (FCA)").FontSize(9).Bold().FontColor(CorSenaiVermelhoEscuro);
                    col.Item().PaddingTop(3).Text(text =>
                    {
                        text.Span($"Tabelas 42/43 da NBR 5410. ")
                            .FontSize(8);
                        text.Span($"Com {dados.NumeroCircuitosAgrupados} circuito(s) agrupado(s), ")
                            .FontSize(8);
                        text.Span($"FCA = {resultado.FatorCorrecaoAgrupamento:F3}").FontSize(8).Bold().FontColor(CorSenaiVermelho);
                    });
                });
                
                // Fórmula final
                c.Item().PaddingTop(8).Background(Colors.Grey.Lighten4).Padding(6).Column(col =>
                {
                    col.Item().Text("FÓRMULA DA CORRENTE CORRIGIDA:").FontSize(8).Bold().FontColor(CorSenaiVermelhoEscuro);
                    col.Item().Text("Iz' = Iz × FCT × FRT × FCA").FontSize(9).Bold().FontFamily("Courier New");
                    col.Item().PaddingTop(4).Text($"Iz' = {resultado.CapacidadeTabelaIz:F1} × {resultado.FatorCorrecaoTemperatura:F3} × {resultado.FatorResistividadeSolo:F3} × {resultado.FatorCorrecaoAgrupamento:F3}")
                        .FontSize(8).FontFamily("Courier New");
                    col.Item().Text($"Iz' = {resultado.CapacidadeCorrigidaIzLinha:F3} A")
                        .FontSize(10).Bold().FontColor(CorSenaiVermelho);
                });
                
                // Conclusão
                c.Item().PaddingTop(6).Background(Colors.Blue.Lighten5).Padding(6).Column(col =>
                {
                    col.Item().Text("CONCLUSÃO:").FontSize(8).Bold().FontColor(Colors.Blue.Darken2);
                    col.Item().PaddingTop(2).Text(text =>
                    {
                        text.Span($"Com os fatores de correção aplicados, o cabo de {resultado.SecaoNominalFase} mm² passou a suportar ")
                            .FontSize(8);
                        text.Span($"{resultado.CapacidadeCorrigidaIzLinha:F2} A (Iz'), ")
                            .FontSize(8).Bold();
                        text.Span($"que é {'{'}{(resultado.CapacidadeCorrigidaIzLinha >= resultado.CorrenteProjetoIb ? "MAIOR" : "MENOR")}{'}'} ")
                            .FontSize(8).Bold().FontColor(resultado.CapacidadeCorrigidaIzLinha >= resultado.CorrenteProjetoIb ? Colors.Green.Darken2 : Colors.Red.Darken2);
                        text.Span($"que a corrente de projeto {resultado.CorrenteProjetoIb:F2} A (Ib).")
                            .FontSize(8);
                    });
                    
                    if (resultado.SecaoNominalFase >= 2.5)
                    {
                        col.Item().PaddingTop(4).Text(text =>
                        {
                            text.Span("IMPORTANTE: ").FontSize(8).Bold().FontColor(CorSenaiVermelho);
                            text.Span("A bitola mínima de 2,5 mm² foi aplicada conforme Tabela 47 da NBR 5410 (circuitos de força/TUE). ")
                                .FontSize(8);
                            text.Span("Mesmo que o cálculo de corrente permita bitolas menores, esta seção mínima é obrigatória.")
                                .FontSize(8).Italic();
                        });
                    }
                });
            });
        });
    }
    
    private static void CriarPasso4(IContainer container, ResultadoDimensionamento resultado)
    {
        var dados = resultado.DadosEntrada;
        var quedaTensaoVolts = (resultado.QuedaTensaoPercentual / 100.0) * dados.TensaoVolts;
        var quedaMaximaVolts = dados.TensaoVolts * 0.04;
        var rho = resultado.TipoIsolacao == TipoIsolacao.PVC ? 0.01786 : 0.0173;
        
        container.Border(1).BorderColor(CorSenaiVermelho).Column(column =>
        {
            column.Item().Background(CorSenaiVermelho).Padding(8)
                .Text("PASSO 4 - QUEDA DE TENSÃO (ΔU)").FontSize(11).Bold().FontColor(Colors.White);
            
            column.Item().Padding(10).Column(c =>
            {
                c.Item().Text(text =>
                {
                    text.Span("A NBR 5410 estabelece que a queda de tensão entre a origem da instalação e qualquer ponto de utilização ")
                        .FontSize(8);
                    text.Span("não deve ser superior a 4% da tensão nominal. ")
                        .FontSize(8).Bold();
                    text.Span("Este critério garante o funcionamento adequado dos equipamentos.")
                        .FontSize(8);
                });
                
                // Cálculo da queda máxima permitida
                c.Item().PaddingTop(6).Background(Colors.Orange.Lighten4).Padding(6).Column(maxima =>
                {
                    maxima.Item().Text("QUEDA MÁXIMA PERMITIDA (4%):").FontSize(8).Bold().FontColor(Colors.Orange.Darken3);
                    maxima.Item().PaddingTop(2).Text($"ΔU máxima = {dados.TensaoVolts:F0} V × 0,04 = {quedaMaximaVolts:F2} V")
                        .FontSize(9).FontFamily("Courier New").FontColor(Colors.Orange.Darken2);
                    maxima.Item().Text($"Portanto, a queda não pode ultrapassar {quedaMaximaVolts:F2} V neste circuito.")
                        .FontSize(7).Italic();
                });
                
                // FÓRMULA EM VOLTS
                c.Item().PaddingTop(6).Background(Colors.Grey.Lighten4).Padding(6).Column(col =>
                {
                    col.Item().Text("1) FÓRMULA DA QUEDA DE TENSÃO (em Volts):").FontSize(8).Bold().FontColor(CorSenaiVermelhoEscuro);
                    col.Item().Text("ΔU (V) = (2 × ρ × L × FP × Ib) / S").FontSize(9).Bold().FontFamily("Courier New");
                    col.Item().PaddingTop(3).Text("Onde:").FontSize(7).Italic();
                    col.Item().Text($"ρ = {rho:F5} Ω.mm²/m (Resistividade do {resultado.TipoMetal} com isolação {resultado.TipoIsolacao})").FontSize(7);
                    col.Item().Text($"L = {dados.ComprimentoMetros:F1} m (Comprimento do circuito)").FontSize(7);
                    col.Item().Text($"FP = {dados.FatorPotencia:F2} (Fator de potência)").FontSize(7);
                    col.Item().Text($"Ib = {resultado.CorrenteProjetoIb:F3} A (Corrente de projeto)").FontSize(7);
                    col.Item().Text($"S = {resultado.SecaoNominalFase} mm² (Seção do condutor)").FontSize(7);
                });
                
                // Cálculo em Volts
                c.Item().PaddingTop(4).Column(calc =>
                {
                    calc.Item().Text("CÁLCULO EM VOLTS:").FontSize(8).Bold().FontColor(CorSenaiVermelhoEscuro);
                    calc.Item().Text($"ΔU (V) = (2 × {rho:F5} × {dados.ComprimentoMetros:F1} × {dados.FatorPotencia:F2} × {resultado.CorrenteProjetoIb:F3}) / {resultado.SecaoNominalFase}")
                        .FontSize(7).FontFamily("Courier New");
                    calc.Item().Text($"ΔU = {quedaTensaoVolts:F2} V")
                        .FontSize(10).Bold().FontColor(CorSenaiVermelho);
                });
                
                // FÓRMULA EM PERCENTUAL
                c.Item().PaddingTop(6).Background(Colors.Grey.Lighten4).Padding(6).Column(col =>
                {
                    col.Item().Text("2) CONVERSÃO PARA PERCENTUAL:").FontSize(8).Bold().FontColor(CorSenaiVermelhoEscuro);
                    col.Item().Text("ΔU (%) = (ΔU em V / U) × 100").FontSize(9).Bold().FontFamily("Courier New");
                    col.Item().Text($"U = {dados.TensaoVolts:F0} V (Tensão nominal)").FontSize(7).Italic();
                });
                
                // Cálculo em Percentual
                c.Item().PaddingTop(4).Column(calc =>
                {
                    calc.Item().Text("CÁLCULO EM PERCENTUAL:").FontSize(8).Bold().FontColor(CorSenaiVermelhoEscuro);
                    calc.Item().Text($"ΔU (%) = ({quedaTensaoVolts:F2} / {dados.TensaoVolts:F0}) × 100")
                        .FontSize(7).FontFamily("Courier New");
                    calc.Item().Text($"ΔU = {resultado.QuedaTensaoPercentual:F2}%")
                        .FontSize(10).Bold().FontColor(CorSenaiVermelho);
                });
                
                // Comparação
                c.Item().PaddingTop(6).Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("VERIFICAÇÃO:").FontSize(8).Bold().FontColor(CorSenaiVermelhoEscuro);
                        col.Item().Text($"Queda calculada: {quedaTensaoVolts:F2} V ({resultado.QuedaTensaoPercentual:F2}%)")
                            .FontSize(8).Bold();
                        col.Item().Text($"Limite máximo: {quedaMaximaVolts:F2} V (4,0%)")
                            .FontSize(8);
                    });
                    
                    row.ConstantItem(80).AlignRight().Background(resultado.QuedaTensaoPercentual <= 4.0 ? Colors.Green.Medium : Colors.Red.Medium)
                        .Padding(4).AlignCenter()
                        .Text(resultado.QuedaTensaoPercentual <= 4.0 ? "OK" : "ACIMA")
                        .FontSize(9).Bold().FontColor(Colors.White);
                });
                
                c.Item().PaddingTop(6).Background(Colors.Blue.Lighten5).Padding(6).Text(text =>
                {
                    text.Span("CONCLUSÃO: ").FontSize(8).Bold().FontColor(Colors.Blue.Darken2);
                    text.Span($"A queda de tensão calculada para o condutor de {resultado.SecaoNominalFase} mm² é {quedaTensaoVolts:F2} V ({resultado.QuedaTensaoPercentual:F2}%), ")
                        .FontSize(8);
                    
                    if (resultado.QuedaTensaoPercentual <= 4.0)
                    {
                        text.Span("que está DENTRO do limite máximo de 4% estabelecido pela NBR 5410. ")
                            .FontSize(8).Bold().FontColor(Colors.Green.Darken2);
                        text.Span("Portanto, o condutor é adequado também sob este critério.")
                            .FontSize(8);
                    }
                    else
                    {
                        text.Span("que está ACIMA do limite máximo de 4%. ")
                            .FontSize(8).Bold().FontColor(Colors.Red.Darken2);
                        text.Span("Seria necessário aumentar a seção do condutor.")
                            .FontSize(8);
                    }
                });
            });
        });
    }
    
    private static void CriarPasso5(IContainer container, ResultadoDimensionamento resultado)
    {
        var criterio = resultado.SecaoNominalFase <= 16 ? "S ≤ 16 mm²" :
                      resultado.SecaoNominalFase <= 35 ? "16 < S ≤ 35 mm²" :
                      "S > 35 mm²";
        
        var regra = resultado.SecaoNominalFase <= 16 ? "SPE = S (mesma seção da fase)" :
                   resultado.SecaoNominalFase <= 35 ? "SPE = 16 mm²" :
                   "SPE = S / 2 (metade da seção da fase)";
        
        container.Border(1).BorderColor(CorSenaiVermelho).Column(column =>
        {
            column.Item().Background(CorSenaiVermelho).Padding(8)
                .Text("PASSO 5 - CONDUTOR DE PROTEÇÃO (PE)").FontSize(11).Bold().FontColor(Colors.White);
            
            column.Item().Padding(10).Column(c =>
            {
                c.Item().Text(text =>
                {
                    text.Span("O condutor de proteção (PE) é dimensionado conforme a ")
                        .FontSize(8);
                    text.Span("Tabela 58 da NBR 5410:2004").FontSize(8).Bold();
                    text.Span(", que estabelece a seção mínima do PE em função da seção dos condutores fase.")
                        .FontSize(8);
                });
                
                c.Item().PaddingTop(6).Background(Colors.Grey.Lighten4).Padding(6).Column(col =>
                {
                    col.Item().Text("TABELA 58 - NBR 5410:2004").FontSize(8).Bold().FontColor(CorSenaiVermelhoEscuro);
                    col.Item().PaddingTop(3).Row(row =>
                    {
                        row.RelativeItem().Column(r =>
                        {
                            r.Item().Text($"Seção da fase (S): {resultado.SecaoNominalFase} mm²").FontSize(8);
                            r.Item().Text($"Critério aplicável: {criterio}").FontSize(8).Italic();
                            r.Item().Text($"Regra: {regra}").FontSize(8).Bold().FontColor(CorSenaiVermelho);
                        });
                    });
                });
                
                c.Item().PaddingTop(6).Row(row =>
                {
                    row.RelativeItem().Text($"Seção calculada do PE: {resultado.SecaoNominalPE} mm²")
                        .FontSize(9).Bold();
                    
                    row.ConstantItem(80).AlignRight().Background(Colors.Green.Medium)
                        .Padding(4).AlignCenter()
                        .Text($"{resultado.SecaoNominalPE} mm²")
                        .FontSize(11).Bold().FontColor(Colors.White);
                });
                
                c.Item().PaddingTop(6).Background(Colors.Blue.Lighten5).Padding(6).Text(text =>
                {
                    text.Span("CONCLUSÃO: ").FontSize(8).Bold().FontColor(Colors.Blue.Darken2);
                    text.Span($"De acordo com a Tabela 58, para um condutor fase de {resultado.SecaoNominalFase} mm², ")
                        .FontSize(8);
                    text.Span($"o condutor de proteção (PE) deve ter seção de {resultado.SecaoNominalPE} mm². ")
                        .FontSize(8).Bold();
                    text.Span("Esta seção garante a proteção adequada contra choques elétricos e correntes de falta.")
                        .FontSize(8);
                });
            });
        });
    }
    
    private static void CriarResultadoFinal(IContainer container, ResultadoDimensionamento resultado)
    {
        container.Background(CorSenaiVermelho).Padding(12).Column(column =>
        {
            column.Item().Text("RESULTADO FINAL DO DIMENSIONAMENTO")
                .FontSize(12).Bold().FontColor(Colors.White).AlignCenter();
            
            column.Item().PaddingTop(8).Row(row =>
            {
                row.RelativeItem().Background(Colors.White).Padding(10).AlignCenter().Column(c =>
                {
                    c.Item().Text("CONDUTOR FASE").FontSize(9).FontColor(Colors.Grey.Darken1);
                    c.Item().Text($"{resultado.SecaoNominalFase} mm²")
                        .FontSize(22).Bold().FontColor(CorSenaiVermelho);
                    c.Item().Text($"{resultado.TipoMetal} - {resultado.TipoIsolacao}").FontSize(8).Italic();
                });
                
                row.ConstantItem(20);
                
                row.RelativeItem().Background(Colors.White).Padding(10).AlignCenter().Column(c =>
                {
                    c.Item().Text("CONDUTOR PE").FontSize(9).FontColor(Colors.Grey.Darken1);
                    c.Item().Text($"{resultado.SecaoNominalPE} mm²")
                        .FontSize(22).Bold().FontColor(Colors.Green.Darken2);
                    c.Item().Text("Proteção").FontSize(8).Italic();
                });
            });
            
            column.Item().PaddingTop(8).Background(Colors.White).Padding(6).Text(text =>
            {
                text.Span("[OK] ").FontSize(9).Bold().FontColor(Colors.Green.Darken2);
                text.Span($"Iz' ({resultado.CapacidadeCorrigidaIzLinha:F2} A) ≥ Ib ({resultado.CorrenteProjetoIb:F2} A) | ")
                    .FontSize(8);
                text.Span($"ΔU ({resultado.QuedaTensaoPercentual:F2}%) ≤ 4% | ")
                    .FontSize(8);
                text.Span($"Bitola mínima atendida")
                    .FontSize(8);
            });
        });
    }
}

