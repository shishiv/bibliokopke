using System.Linq;
using System.Text.RegularExpressions;

namespace BibliotecaJK.Utilitarios;

public static partial class Validador
{
    public static void GarantirNaoVazio(string? valor, string nomeCampo)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new ExcecaoValidacao($"O campo '{nomeCampo}' é obrigatório.");
        }
    }

    public static void GarantirCpfValido(string? cpf)
    {
        GarantirNaoVazio(cpf, "CPF");
        var digitos = ExtrairDigitos(cpf!);
        if (digitos.Length != 11 || !CpfValido(digitos))
        {
            throw new ExcecaoValidacao("CPF inválido.");
        }
    }

    public static string ExtrairDigitos(string valor)
    {
        return RegexDigitos().Replace(valor, string.Empty);
    }

    public static int GarantirNumeroPositivo(int numero, string nomeCampo)
    {
        if (numero <= 0)
        {
            throw new ExcecaoValidacao($"O campo '{nomeCampo}' precisa ser maior que zero.");
        }
        return numero;
    }

    private static bool CpfValido(string digitos)
    {
        if (digitos.Distinct().Count() == 1)
        {
            return false;
        }

        var numeros = digitos.Select(c => c - '0').ToArray();
        for (var j = 9; j < 11; j++)
        {
            var soma = 0;
            for (var i = 0; i < j; i++)
            {
                soma += numeros[i] * (j + 1 - i);
            }
            var resultado = soma % 11;
            var esperado = resultado < 2 ? 0 : 11 - resultado;
            if (numeros[j] != esperado)
            {
                return false;
            }
        }
        return true;
    }

    [GeneratedRegex(@"\D")]
    private static partial Regex RegexDigitos();
}
