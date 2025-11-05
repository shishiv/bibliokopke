using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace BibliotecaJK.BLL
{
    /// <summary>
    /// Classe com métodos de validação de dados
    /// </summary>
    public static class Validadores
    {
        /// <summary>
        /// Valida CPF usando algoritmo de dígitos verificadores
        /// </summary>
        public static bool ValidarCPF(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            // Remove caracteres não numéricos
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            // CPF deve ter 11 dígitos
            if (cpf.Length != 11)
                return false;

            // Verifica sequências inválidas (111.111.111-11, etc)
            if (cpf.Distinct().Count() == 1)
                return false;

            // Calcula primeiro dígito verificador
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpf[i].ToString()) * (10 - i);

            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            if (int.Parse(cpf[9].ToString()) != digito1)
                return false;

            // Calcula segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpf[i].ToString()) * (11 - i);

            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            return int.Parse(cpf[10].ToString()) == digito2;
        }

        /// <summary>
        /// Valida ISBN-10 ou ISBN-13
        /// </summary>
        public static bool ValidarISBN(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                return false;

            // Remove hífens e espaços
            isbn = isbn.Replace("-", "").Replace(" ", "");

            // ISBN-10
            if (isbn.Length == 10)
            {
                return ValidarISBN10(isbn);
            }
            // ISBN-13
            else if (isbn.Length == 13)
            {
                return ValidarISBN13(isbn);
            }

            return false;
        }

        private static bool ValidarISBN10(string isbn)
        {
            int soma = 0;
            for (int i = 0; i < 9; i++)
            {
                if (!char.IsDigit(isbn[i]))
                    return false;
                soma += int.Parse(isbn[i].ToString()) * (10 - i);
            }

            // Último caractere pode ser X (representa 10)
            char ultimoChar = isbn[9];
            int ultimoDigito = ultimoChar == 'X' || ultimoChar == 'x' ? 10 :
                               char.IsDigit(ultimoChar) ? int.Parse(ultimoChar.ToString()) : -1;

            if (ultimoDigito == -1)
                return false;

            soma += ultimoDigito;
            return soma % 11 == 0;
        }

        private static bool ValidarISBN13(string isbn)
        {
            if (!isbn.All(char.IsDigit))
                return false;

            int soma = 0;
            for (int i = 0; i < 12; i++)
            {
                int digito = int.Parse(isbn[i].ToString());
                soma += (i % 2 == 0) ? digito : digito * 3;
            }

            int digitoVerificador = (10 - (soma % 10)) % 10;
            return digitoVerificador == int.Parse(isbn[12].ToString());
        }

        /// <summary>
        /// Valida formato de e-mail
        /// </summary>
        public static bool ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Regex simplificado para validação de e-mail
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Valida formato de matrícula (letras e números, 3-20 caracteres)
        /// </summary>
        public static bool ValidarMatricula(string matricula)
        {
            if (string.IsNullOrWhiteSpace(matricula))
                return false;

            // Matrícula deve ter entre 3 e 20 caracteres alfanuméricos
            return matricula.Length >= 3 &&
                   matricula.Length <= 20 &&
                   matricula.All(c => char.IsLetterOrDigit(c));
        }

        /// <summary>
        /// Valida se string não está vazia
        /// </summary>
        public static bool CampoObrigatorio(string valor, string nomeCampo, out string mensagemErro)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                mensagemErro = $"O campo '{nomeCampo}' é obrigatório.";
                return false;
            }

            mensagemErro = string.Empty;
            return true;
        }

        /// <summary>
        /// Valida se número é positivo
        /// </summary>
        public static bool NumeroPositivo(int valor, string nomeCampo, out string mensagemErro)
        {
            if (valor <= 0)
            {
                mensagemErro = $"O campo '{nomeCampo}' deve ser um número positivo.";
                return false;
            }

            mensagemErro = string.Empty;
            return true;
        }
    }
}
