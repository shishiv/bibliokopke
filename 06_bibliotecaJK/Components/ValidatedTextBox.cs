using System;
using System.Drawing;
using System.Windows.Forms;

namespace BibliotecaJK.Components
{
    /// <summary>
    /// ValidatedTextBox - Material Design text input with built-in validation and visual feedback
    /// Extends ModernTextBox to add real-time validation capabilities
    /// </summary>
    public class ValidatedTextBox : ModernTextBox
    {
        private Label _lblErrorMessage = null!;
        private Panel _pnlValidationIcon = null!;
        private Label _lblValidationIcon = null!;
        private bool _isValid = true;
        private bool _validationEnabled = false;

        // Validation properties
        /// <summary>
        /// Função de validação que retorna se o valor é válido e a mensagem de erro (se houver)
        /// </summary>
        public Func<string, (bool IsValid, string ErrorMessage)>? Validator { get; set; }

        /// <summary>
        /// Indica se o valor atual é válido
        /// </summary>
        public bool IsValid
        {
            get => _isValid;
            private set
            {
                if (_isValid != value)
                {
                    _isValid = value;
                    ValidationStateChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Habilita/desabilita a validação automática
        /// </summary>
        public bool ValidationEnabled
        {
            get => _validationEnabled;
            set
            {
                _validationEnabled = value;
                if (value)
                {
                    ValidateInput();
                }
                else
                {
                    // Limpar estado de validação
                    IsValid = true;
                    UpdateValidationUI(true, string.Empty);
                }
            }
        }

        /// <summary>
        /// Define se deve validar apenas quando o usuário sair do campo (blur)
        /// Por padrão valida em tempo real (TextChanged)
        /// </summary>
        public bool ValidateOnBlurOnly { get; set; } = false;

        /// <summary>
        /// Mensagem de erro personalizada (sobrescreve a mensagem do Validator)
        /// </summary>
        public string CustomErrorMessage { get; set; } = string.Empty;

        // Events
        public event EventHandler? ValidationStateChanged;

        public ValidatedTextBox() : base()
        {
            InitializeValidationComponents();
        }

        private void InitializeValidationComponents()
        {
            // Aumentar a altura do controle para acomodar a mensagem de erro
            this.Height = 80; // 56 (ModernTextBox) + 24 (error message)

            // Painel do ícone de validação (à direita do textbox)
            _pnlValidationIcon = new Panel
            {
                Size = new Size(24, 24),
                Location = new Point(this.Width - 28, 24),
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Visible = false
            };

            // Label do ícone de validação (checkmark ou X)
            _lblValidationIcon = new Label
            {
                Size = new Size(24, 24),
                Location = new Point(0, 0),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                BackColor = Color.Transparent,
                Dock = DockStyle.Fill
            };

            _pnlValidationIcon.Controls.Add(_lblValidationIcon);

            // Label da mensagem de erro
            _lblErrorMessage = new Label
            {
                Font = new Font("Segoe UI", 8F, FontStyle.Regular),
                ForeColor = ThemeManager.Semantic.Error,
                AutoSize = false,
                Size = new Size(this.Width - 28, 20),
                Location = new Point(0, 58),
                Visible = false,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right
            };

            // Adicionar controles
            this.Controls.Add(_pnlValidationIcon);
            this.Controls.Add(_lblErrorMessage);

            // Trazer painel do ícone para frente
            _pnlValidationIcon.BringToFront();

            // Wire up validation events
            this.TextChanged += ValidatedTextBox_TextChanged;
            this.LostFocus += ValidatedTextBox_LostFocus;
            this.Resize += ValidatedTextBox_Resize;
        }

        private void ValidatedTextBox_Resize(object? sender, EventArgs e)
        {
            // Reposicionar ícone de validação
            if (_pnlValidationIcon != null)
            {
                _pnlValidationIcon.Location = new Point(this.Width - 28, 24);
            }

            // Redimensionar mensagem de erro
            if (_lblErrorMessage != null)
            {
                _lblErrorMessage.Width = this.Width - 28;
            }
        }

        private void ValidatedTextBox_TextChanged(object? sender, EventArgs e)
        {
            // Validar em tempo real apenas se não estiver configurado para validar apenas no blur
            if (_validationEnabled && !ValidateOnBlurOnly)
            {
                ValidateInput();
            }
        }

        private void ValidatedTextBox_LostFocus(object? sender, EventArgs e)
        {
            // Validar no blur se configurado, ou sempre validar se a validação está habilitada
            if (_validationEnabled)
            {
                ValidateInput();
            }
        }

        /// <summary>
        /// Executa a validação do input atual
        /// </summary>
        public void ValidateInput()
        {
            if (Validator == null)
            {
                // Sem validador configurado, considerar válido
                IsValid = true;
                UpdateValidationUI(true, string.Empty);
                return;
            }

            try
            {
                var (isValid, errorMessage) = Validator(this.Text);
                IsValid = isValid;

                // Usar mensagem customizada se fornecida
                string messageToDisplay = !string.IsNullOrEmpty(CustomErrorMessage) && !isValid
                    ? CustomErrorMessage
                    : errorMessage;

                UpdateValidationUI(isValid, messageToDisplay);
            }
            catch (Exception ex)
            {
                // Em caso de erro no validador, considerar inválido
                IsValid = false;
                UpdateValidationUI(false, $"Erro na validação: {ex.Message}");
            }
        }

        /// <summary>
        /// Atualiza a interface visual baseada no estado de validação
        /// </summary>
        private void UpdateValidationUI(bool isValid, string errorMessage)
        {
            if (_lblValidationIcon == null || _pnlValidationIcon == null || _lblErrorMessage == null)
                return;

            if (isValid)
            {
                // Estado válido - ícone verde de check
                _lblValidationIcon.Text = "✓";
                _lblValidationIcon.ForeColor = ThemeManager.Semantic.Success;
                _pnlValidationIcon.Visible = !string.IsNullOrEmpty(this.Text) && _validationEnabled;

                // Esconder mensagem de erro
                _lblErrorMessage.Visible = false;

                // Linha verde quando válido e tem texto
                if (!string.IsNullOrEmpty(this.Text) && _validationEnabled)
                {
                    this.FocusedLineColor = ThemeManager.Semantic.Success;
                    this.UnfocusedLineColor = ThemeManager.Semantic.Success;
                }
                else
                {
                    // Restaurar cores padrão
                    this.FocusedLineColor = ThemeManager.IsDarkMode
                        ? ThemeManager.Dark.Primary
                        : ThemeManager.Light.Primary;
                    this.UnfocusedLineColor = Color.FromArgb(224, 224, 224);
                }
            }
            else
            {
                // Estado inválido - ícone vermelho de erro
                _lblValidationIcon.Text = "✕";
                _lblValidationIcon.ForeColor = ThemeManager.Semantic.Error;
                _pnlValidationIcon.Visible = _validationEnabled;

                // Mostrar mensagem de erro
                _lblErrorMessage.Text = errorMessage;
                _lblErrorMessage.Visible = _validationEnabled && !string.IsNullOrEmpty(errorMessage);

                // Linha vermelha quando inválido
                if (_validationEnabled)
                {
                    this.FocusedLineColor = ThemeManager.Semantic.Error;
                    this.UnfocusedLineColor = ThemeManager.Semantic.Error;
                }
            }
        }

        /// <summary>
        /// Limpa o texto e reseta o estado de validação
        /// </summary>
        public new void Clear()
        {
            base.Clear();
            IsValid = true;
            UpdateValidationUI(true, string.Empty);
        }

        /// <summary>
        /// Define um validador a partir de uma expressão regular
        /// </summary>
        public void SetRegexValidator(string pattern, string errorMessage)
        {
            Validator = (text) =>
            {
                if (string.IsNullOrEmpty(text))
                    return (true, string.Empty); // Campo vazio é válido por padrão

                var regex = new System.Text.RegularExpressions.Regex(pattern);
                bool isValid = regex.IsMatch(text);
                return (isValid, isValid ? string.Empty : errorMessage);
            };
        }

        /// <summary>
        /// Define um validador de campo obrigatório
        /// </summary>
        public void SetRequiredValidator(string errorMessage = "Este campo é obrigatório")
        {
            Validator = (text) =>
            {
                bool isValid = !string.IsNullOrWhiteSpace(text);
                return (isValid, isValid ? string.Empty : errorMessage);
            };
        }

        /// <summary>
        /// Define um validador de comprimento mínimo
        /// </summary>
        public void SetMinLengthValidator(int minLength, string? errorMessage = null)
        {
            Validator = (text) =>
            {
                if (string.IsNullOrEmpty(text))
                    return (true, string.Empty); // Campo vazio não valida comprimento

                bool isValid = text.Length >= minLength;
                string message = errorMessage ?? $"Mínimo de {minLength} caracteres";
                return (isValid, isValid ? string.Empty : message);
            };
        }

        /// <summary>
        /// Define um validador de comprimento máximo
        /// </summary>
        public void SetMaxLengthValidator(int maxLength, string? errorMessage = null)
        {
            Validator = (text) =>
            {
                if (string.IsNullOrEmpty(text))
                    return (true, string.Empty); // Campo vazio não valida comprimento

                bool isValid = text.Length <= maxLength;
                string message = errorMessage ?? $"Máximo de {maxLength} caracteres";
                return (isValid, isValid ? string.Empty : message);
            };
        }

        /// <summary>
        /// Define um validador de faixa de comprimento
        /// </summary>
        public void SetLengthRangeValidator(int minLength, int maxLength, string? errorMessage = null)
        {
            Validator = (text) =>
            {
                if (string.IsNullOrEmpty(text))
                    return (true, string.Empty); // Campo vazio não valida comprimento

                bool isValid = text.Length >= minLength && text.Length <= maxLength;
                string message = errorMessage ?? $"Entre {minLength} e {maxLength} caracteres";
                return (isValid, isValid ? string.Empty : message);
            };
        }

        /// <summary>
        /// Combina múltiplos validadores em sequência
        /// Retorna o primeiro erro encontrado
        /// </summary>
        public void SetCompositeValidator(params Func<string, (bool IsValid, string ErrorMessage)>[] validators)
        {
            Validator = (text) =>
            {
                foreach (var validator in validators)
                {
                    var (isValid, errorMessage) = validator(text);
                    if (!isValid)
                    {
                        return (false, errorMessage);
                    }
                }
                return (true, string.Empty);
            };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _lblErrorMessage?.Dispose();
                _lblValidationIcon?.Dispose();
                _pnlValidationIcon?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
