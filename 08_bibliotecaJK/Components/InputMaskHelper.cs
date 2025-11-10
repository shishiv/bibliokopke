using System;
using System.Drawing;
using System.Windows.Forms;

namespace BibliotecaJK.Components
{
    /// <summary>
    /// Input Mask Helper - Máscaras e validações para inputs
    /// </summary>
    public static class InputMaskHelper
    {
        /// <summary>
        /// Cria TextBox com máscara de CPF
        /// </summary>
        public static MaskedTextBox CreateCPFTextBox()
        {
            var txt = new MaskedTextBox
            {
                Mask = "000.000.000-00",
                Font = new Font("Segoe UI", 10F)
            };
            txt.Enter += (s, e) => txt.SelectAll();
            return txt;
        }

        /// <summary>
        /// Cria TextBox com máscara de Telefone
        /// </summary>
        public static MaskedTextBox CreateTelefoneTextBox()
        {
            var txt = new MaskedTextBox
            {
                Mask = "(00) 00000-0000",
                Font = new Font("Segoe UI", 10F)
            };
            txt.Enter += (s, e) => txt.SelectAll();
            return txt;
        }

        /// <summary>
        /// Cria TextBox com máscara de CEP
        /// </summary>
        public static MaskedTextBox CreateCEPTextBox()
        {
            var txt = new MaskedTextBox
            {
                Mask = "00000-000",
                Font = new Font("Segoe UI", 10F)
            };
            txt.Enter += (s, e) => txt.SelectAll();
            return txt;
        }

        /// <summary>
        /// Cria TextBox com máscara de ISBN-13
        /// </summary>
        public static MaskedTextBox CreateISBNTextBox()
        {
            var txt = new MaskedTextBox
            {
                Mask = "000-0-00-000000-0",
                Font = new Font("Segoe UI", 10F)
            };
            txt.Enter += (s, e) => txt.SelectAll();
            return txt;
        }

        /// <summary>
        /// Cria TextBox com máscara de Data
        /// </summary>
        public static MaskedTextBox CreateDataTextBox()
        {
            var txt = new MaskedTextBox
            {
                Mask = "00/00/0000",
                Font = new Font("Segoe UI", 10F)
            };
            txt.Enter += (s, e) => txt.SelectAll();
            return txt;
        }

        /// <summary>
        /// Valida CPF
        /// </summary>
        public static bool ValidarCPF(string cpf)
        {
            cpf = cpf.Replace(".", "").Replace("-", "").Trim();

            if (cpf.Length != 11)
                return false;

            // Verificar se todos os dígitos são iguais
            bool todosIguais = true;
            for (int i = 1; i < 11 && todosIguais; i++)
                if (cpf[i] != cpf[0])
                    todosIguais = false;

            if (todosIguais)
                return false;

            // Calcular primeiro dígito verificador
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpf[i].ToString()) * (10 - i);

            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            if (int.Parse(cpf[9].ToString()) != digito1)
                return false;

            // Calcular segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpf[i].ToString()) * (11 - i);

            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            return int.Parse(cpf[10].ToString()) == digito2;
        }

        /// <summary>
        /// Valida ISBN-13
        /// </summary>
        public static bool ValidarISBN13(string isbn)
        {
            isbn = isbn.Replace("-", "").Trim();

            if (isbn.Length != 13)
                return false;

            int soma = 0;
            for (int i = 0; i < 12; i++)
            {
                if (!char.IsDigit(isbn[i]))
                    return false;

                int digito = int.Parse(isbn[i].ToString());
                soma += (i % 2 == 0) ? digito : digito * 3;
            }

            int checksum = (10 - (soma % 10)) % 10;
            return int.Parse(isbn[12].ToString()) == checksum;
        }

        /// <summary>
        /// Cria TextBox de busca com ícone
        /// </summary>
        public static Panel CreateSearchBox(EventHandler textChangedHandler)
        {
            var pnl = new Panel
            {
                Size = new Size(300, 35),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            var lblIcone = new Label
            {
                Text = "🔍",
                Font = new Font("Segoe UI", 12F),
                Location = new Point(5, 5),
                Size = new Size(25, 25),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnl.Controls.Add(lblIcone);

            var txt = new TextBox
            {
                Location = new Point(35, 7),
                Size = new Size(255, 21),
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 10F),
                PlaceholderText = "Buscar..."
            };
            txt.TextChanged += textChangedHandler;
            pnl.Controls.Add(txt);

            pnl.Tag = txt; // Guardar referência ao TextBox

            return pnl;
        }

        /// <summary>
        /// Adiciona validação visual em tempo real
        /// </summary>
        public static void AddValidationFeedback(MaskedTextBox textBox, Func<string, bool> validador)
        {
            var lblFeedback = new Label
            {
                Size = new Size(20, 20),
                Location = new Point(textBox.Right + 5, textBox.Top),
                Font = new Font("Segoe UI", 12F),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Adicionar ao mesmo parent
            if (textBox.Parent != null)
                textBox.Parent.Controls.Add(lblFeedback);

            textBox.TextChanged += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(".", "").Replace("/", "").Replace(" ", "")))
                {
                    lblFeedback.Text = "";
                    textBox.BackColor = Color.White;
                }
                else if (validador(textBox.Text))
                {
                    lblFeedback.Text = "✓";
                    lblFeedback.ForeColor = Color.Green;
                    textBox.BackColor = Color.FromArgb(240, 255, 240);
                }
                else
                {
                    lblFeedback.Text = "✗";
                    lblFeedback.ForeColor = Color.Red;
                    textBox.BackColor = Color.FromArgb(255, 240, 240);
                }
            };
        }

        /// <summary>
        /// Permite apenas números em TextBox
        /// </summary>
        public static void AllowOnlyNumbers(this TextBox textBox)
        {
            textBox.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };
        }

        /// <summary>
        /// Permite apenas letras em TextBox
        /// </summary>
        public static void AllowOnlyLetters(this TextBox textBox)
        {
            textBox.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
                {
                    e.Handled = true;
                }
            };
        }
    }
}
