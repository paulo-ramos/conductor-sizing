namespace ConductorSizing.Application.Data;

/// <summary>
/// Tabela 37 da NBR 5410:2004 - Capacidades de condução de corrente em ampères
/// para condutores isolados em XLPE ou EPR, temperatura no condutor 90°C
/// Temperatura ambiente: 30°C (ar), 20°C (solo)
/// CONDUTOR: COBRE
/// </summary>
public static class Tabela37XLPE
{
    /// <summary>
    /// Estrutura de dados da tabela
    /// Key: Seção nominal (mm²)
    /// Value: Dictionary com método de instalação e número de condutores -> corrente (A)
    /// </summary>
    public static readonly Dictionary<double, Dictionary<string, double>> CapacidadeConducao = new()
    {
        // Seção 0.5 mm²
        {
            0.5, new Dictionary<string, double>
            {
                { "A1_2", 10 },
                { "A1_3", 9 },
                { "B1_2", 12 },
                { "B1_3", 11 }
            }
        },
        // Seção 0.75 mm²
        {
            0.75, new Dictionary<string, double>
            {
                { "A1_2", 12 },
                { "A1_3", 11 },
                { "B1_2", 15 },
                { "B1_3", 13 }
            }
        },
        // Seção 1.0 mm²
        {
            1.0, new Dictionary<string, double>
            {
                { "A1_2", 15 },
                { "A1_3", 13 },
                { "B1_2", 18 },
                { "B1_3", 16 }
            }
        },
        // Seção 1.5 mm²
        {
            1.5, new Dictionary<string, double>
            {
                { "A1_2", 19 },
                { "A1_3", 17 },
                { "B1_2", 23 },
                { "B1_3", 20 },
                { "C_2", 23 },
                { "C_3", 20 },
                { "D_2", 24 },
                { "D_3", 21 }
            }
        },
        // Seção 2.5 mm² - BITOLA MÍNIMA PARA CIRCUITOS DE FORÇA
        {
            2.5, new Dictionary<string, double>
            {
                { "A1_2", 26 },
                { "A1_3", 23 },
                { "B1_2", 31 },
                { "B1_3", 27 },
                { "C_2", 31 },
                { "C_3", 27 },
                { "D_2", 33 },
                { "D_3", 29 }
            }
        },
        // Seção 4 mm²
        {
            4.0, new Dictionary<string, double>
            {
                { "A1_2", 35 },
                { "A1_3", 30 },
                { "B1_2", 42 },
                { "B1_3", 37 },
                { "C_2", 42 },
                { "C_3", 37 },
                { "D_2", 45 },
                { "D_3", 39 }
            }
        },
        // Seção 6 mm²
        {
            6.0, new Dictionary<string, double>
            {
                { "A1_2", 46 },
                { "A1_3", 40 },
                { "B1_2", 54 },
                { "B1_3", 47 },
                { "C_2", 54 },
                { "C_3", 47 },
                { "D_2", 58 },
                { "D_3", 50 }
            }
        },
        // Seção 10 mm²
        {
            10.0, new Dictionary<string, double>
            {
                { "A1_2", 63 },
                { "A1_3", 54 },
                { "B1_2", 75 },
                { "B1_3", 65 },
                { "C_2", 75 },
                { "C_3", 65 },
                { "D_2", 80 },
                { "D_3", 69 }
            }
        },
        // Seção 16 mm²
        {
            16.0, new Dictionary<string, double>
            {
                { "A1_2", 85 },
                { "A1_3", 73 },
                { "B1_2", 100 },
                { "B1_3", 88 },
                { "C_2", 100 },
                { "C_3", 88 },
                { "D_2", 107 },
                { "D_3", 93 }
            }
        },
        // Seção 25 mm²
        {
            25.0, new Dictionary<string, double>
            {
                { "A1_2", 112 },
                { "A1_3", 96 },
                { "B1_2", 133 },
                { "B1_3", 115 },
                { "C_2", 133 },
                { "C_3", 115 },
                { "D_2", 143 },
                { "D_3", 123 }
            }
        },
        // Seção 35 mm²
        {
            35.0, new Dictionary<string, double>
            {
                { "A1_2", 138 },
                { "A1_3", 119 },
                { "B1_2", 164 },
                { "B1_3", 143 },
                { "C_2", 164 },
                { "C_3", 143 },
                { "D_2", 176 },
                { "D_3", 152 }
            }
        },
        // Seção 50 mm²
        {
            50.0, new Dictionary<string, double>
            {
                { "A1_2", 168 },
                { "A1_3", 144 },
                { "B1_2", 198 },
                { "B1_3", 172 },
                { "C_2", 198 },
                { "C_3", 172 },
                { "D_2", 213 },
                { "D_3", 184 }
            }
        },
        // Seção 70 mm²
        {
            70.0, new Dictionary<string, double>
            {
                { "A1_2", 213 },
                { "A1_3", 184 },
                { "B1_2", 253 },
                { "B1_3", 219 },
                { "C_2", 253 },
                { "C_3", 219 },
                { "D_2", 272 },
                { "D_3", 234 }
            }
        },
        // Seção 95 mm²
        {
            95.0, new Dictionary<string, double>
            {
                { "A1_2", 258 },
                { "A1_3", 223 },
                { "B1_2", 306 },
                { "B1_3", 265 },
                { "C_2", 306 },
                { "C_3", 265 },
                { "D_2", 328 },
                { "D_3", 283 }
            }
        },
        // Seção 120 mm²
        {
            120.0, new Dictionary<string, double>
            {
                { "A1_2", 299 },
                { "A1_3", 257 },
                { "B1_2", 354 },
                { "B1_3", 306 },
                { "C_2", 354 },
                { "C_3", 306 },
                { "D_2", 379 },
                { "D_3", 328 }
            }
        },
        // Seção 150 mm²
        {
            150.0, new Dictionary<string, double>
            {
                { "A1_2", 341 },
                { "A1_3", 293 },
                { "B1_2", 406 },
                { "B1_3", 352 },
                { "C_2", 406 },
                { "C_3", 352 },
                { "D_2", 435 },
                { "D_3", 376 }
            }
        },
        // Seção 185 mm²
        {
            185.0, new Dictionary<string, double>
            {
                { "A1_2", 387 },
                { "A1_3", 334 },
                { "B1_2", 464 },
                { "B1_3", 402 },
                { "C_2", 464 },
                { "C_3", 402 },
                { "D_2", 497 },
                { "D_3", 429 }
            }
        },
        // Seção 240 mm²
        {
            240.0, new Dictionary<string, double>
            {
                { "A1_2", 456 },
                { "A1_3", 394 },
                { "B1_2", 546 },
                { "B1_3", 473 },
                { "C_2", 546 },
                { "C_3", 473 },
                { "D_2", 585 },
                { "D_3", 505 }
            }
        }
    };
    
    /// <summary>
    /// Obtém a chave composta (método + condutores) para busca na tabela
    /// </summary>
    public static string ObterChave(string metodoInstalacao, int numeroCondutoresCarregados)
    {
        return $"{metodoInstalacao}_{numeroCondutoresCarregados}";
    }
}
