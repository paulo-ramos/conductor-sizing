namespace ConductorSizing.Application.Data;

/// <summary>
/// Tabela 36 da NBR 5410:2004 - Capacidades de condução de corrente em ampères
/// para condutores isolados em PVC, temperatura no condutor 70°C
/// Temperatura ambiente: 30°C (ar), 20°C (solo)
/// CONDUTOR: COBRE
/// </summary>
public static class Tabela36PVC
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
                { "A1_2", 7 },    // A1 - 2 condutores carregados
                { "A1_3", 7 },    // A1 - 3 condutores carregados
                { "B1_2", 9 },    // B1 - 2 condutores carregados
                { "B1_3", 8 }     // B1 - 3 condutores carregados
            }
        },
        // Seção 0.75 mm²
        {
            0.75, new Dictionary<string, double>
            {
                { "A1_2", 9 },
                { "A1_3", 9 },
                { "B1_2", 11 },
                { "B1_3", 10 }
            }
        },
        // Seção 1.0 mm²
        {
            1.0, new Dictionary<string, double>
            {
                { "A1_2", 11 },
                { "A1_3", 10 },
                { "B1_2", 14 },
                { "B1_3", 12 }
            }
        },
        // Seção 1.5 mm²
        {
            1.5, new Dictionary<string, double>
            {
                { "A1_2", 14 },
                { "A1_3", 13 },
                { "B1_2", 17.5 },
                { "B1_3", 15.5 },
                { "C_2", 17.5 },
                { "C_3", 15.5 },
                { "D_2", 18 },
                { "D_3", 16 }
            }
        },
        // Seção 2.5 mm² - BITOLA MÍNIMA PARA CIRCUITOS DE FORÇA
        {
            2.5, new Dictionary<string, double>
            {
                { "A1_2", 19 },
                { "A1_3", 17 },
                { "B1_2", 24 },
                { "B1_3", 21 },
                { "C_2", 24 },
                { "C_3", 21 },
                { "D_2", 25 },
                { "D_3", 23 }
            }
        },
        // Seção 4 mm²
        {
            4.0, new Dictionary<string, double>
            {
                { "A1_2", 26 },
                { "A1_3", 23 },
                { "B1_2", 32 },
                { "B1_3", 28 },
                { "C_2", 32 },
                { "C_3", 28 },
                { "D_2", 34 },
                { "D_3", 30 }
            }
        },
        // Seção 6 mm²
        {
            6.0, new Dictionary<string, double>
            {
                { "A1_2", 34 },
                { "A1_3", 30 },
                { "B1_2", 41 },
                { "B1_3", 36 },
                { "C_2", 41 },
                { "C_3", 36 },
                { "D_2", 44 },
                { "D_3", 38 }
            }
        },
        // Seção 10 mm²
        {
            10.0, new Dictionary<string, double>
            {
                { "A1_2", 46 },
                { "A1_3", 40 },
                { "B1_2", 57 },
                { "B1_3", 50 },
                { "C_2", 57 },
                { "C_3", 50 },
                { "D_2", 61 },
                { "D_3", 54 }
            }
        },
        // Seção 16 mm²
        {
            16.0, new Dictionary<string, double>
            {
                { "A1_2", 61 },
                { "A1_3", 54 },
                { "B1_2", 76 },
                { "B1_3", 68 },
                { "C_2", 76 },
                { "C_3", 68 },
                { "D_2", 83 },
                { "D_3", 73 }
            }
        },
        // Seção 25 mm²
        {
            25.0, new Dictionary<string, double>
            {
                { "A1_2", 80 },
                { "A1_3", 70 },
                { "B1_2", 101 },
                { "B1_3", 89 },
                { "C_2", 101 },
                { "C_3", 89 },
                { "D_2", 110 },
                { "D_3", 96 }
            }
        },
        // Seção 35 mm²
        {
            35.0, new Dictionary<string, double>
            {
                { "A1_2", 99 },
                { "A1_3", 86 },
                { "B1_2", 125 },
                { "B1_3", 110 },
                { "C_2", 125 },
                { "C_3", 110 },
                { "D_2", 137 },
                { "D_3", 119 }
            }
        },
        // Seção 50 mm²
        {
            50.0, new Dictionary<string, double>
            {
                { "A1_2", 119 },
                { "A1_3", 104 },
                { "B1_2", 151 },
                { "B1_3", 134 },
                { "C_2", 151 },
                { "C_3", 134 },
                { "D_2", 167 },
                { "D_3", 144 }
            }
        },
        // Seção 70 mm²
        {
            70.0, new Dictionary<string, double>
            {
                { "A1_2", 151 },
                { "A1_3", 133 },
                { "B1_2", 192 },
                { "B1_3", 171 },
                { "C_2", 192 },
                { "C_3", 171 },
                { "D_2", 213 },
                { "D_3", 184 }
            }
        },
        // Seção 95 mm²
        {
            95.0, new Dictionary<string, double>
            {
                { "A1_2", 182 },
                { "A1_3", 161 },
                { "B1_2", 232 },
                { "B1_3", 207 },
                { "C_2", 232 },
                { "C_3", 207 },
                { "D_2", 258 },
                { "D_3", 223 }
            }
        },
        // Seção 120 mm²
        {
            120.0, new Dictionary<string, double>
            {
                { "A1_2", 210 },
                { "A1_3", 185 },
                { "B1_2", 269 },
                { "B1_3", 239 },
                { "C_2", 269 },
                { "C_3", 239 },
                { "D_2", 299 },
                { "D_3", 259 }
            }
        },
        // Seção 150 mm²
        {
            150.0, new Dictionary<string, double>
            {
                { "A1_2", 240 },
                { "A1_3", 210 },
                { "B1_2", 309 },
                { "B1_3", 275 },
                { "C_2", 309 },
                { "C_3", 275 },
                { "D_2", 344 },
                { "D_3", 297 }
            }
        },
        // Seção 185 mm²
        {
            185.0, new Dictionary<string, double>
            {
                { "A1_2", 273 },
                { "A1_3", 238 },
                { "B1_2", 353 },
                { "B1_3", 314 },
                { "C_2", 353 },
                { "C_3", 314 },
                { "D_2", 392 },
                { "D_3", 339 }
            }
        },
        // Seção 240 mm²
        {
            240.0, new Dictionary<string, double>
            {
                { "A1_2", 321 },
                { "A1_3", 281 },
                { "B1_2", 415 },
                { "B1_3", 370 },
                { "C_2", 415 },
                { "C_3", 370 },
                { "D_2", 461 },
                { "D_3", 399 }
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
