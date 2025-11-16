using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Npgsql;

namespace BibliotecaJK.Components
{
    /// <summary>
    /// Error Dialog - Enhanced error display with actionable suggestions
    /// </summary>
    public class ErrorDialog : Form
    {
        public string ErrorTitle { get; set; } = "Erro";
        public string ErrorMessage { get; set; } = "";
        public string TechnicalDetails { get; set; } = "";
        public List<string> Suggestions { get; set; } = new List<string>();

        private ErrorDialog()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.Size = new Size(500, 400);
            this.BackColor = ThemeManager.Colors.BackgroundPrimary;
            this.Font = ThemeManager.Typography.Body2;

            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                Padding = new Padding(ThemeManager.Spacing.LG),
                AutoSize = true
            };

            // Header panel with icon and title
            var pnlHeader = new Panel
            {
                Height = 60,
                Dock = DockStyle.Top
            };

            var iconError = new PictureBox
            {
                Image = SystemIcons.Error.ToBitmap(),
                SizeMode = PictureBoxSizeMode.CenterImage,
                Size = new Size(48, 48),
                Location = new Point(10, 6)
            };
            pnlHeader.Controls.Add(iconError);

            var lblTitle = new Label
            {
                Text = ErrorTitle,
                Font = ThemeManager.Typography.H4,
                ForeColor = ThemeManager.Colors.Error,
                Location = new Point(68, 15),
                AutoSize = true
            };
            pnlHeader.Controls.Add(lblTitle);

            mainLayout.Controls.Add(pnlHeader, 0, 0);

            // Message panel
            var pnlMessage = new Panel
            {
                AutoSize = true,
                Dock = DockStyle.Top,
                Padding = new Padding(0, 0, 0, ThemeManager.Spacing.MD)
            };

            var lblMessage = new Label
            {
                Text = ErrorMessage,
                Font = ThemeManager.Typography.Body1,
                AutoSize = true,
                MaximumSize = new Size(430, 0),
                Dock = DockStyle.Top
            };
            pnlMessage.Controls.Add(lblMessage);

            mainLayout.Controls.Add(pnlMessage, 0, 1);

            // Suggestions panel
            if (Suggestions.Count > 0)
            {
                var pnlSuggestions = new Panel
                {
                    AutoSize = true,
                    Dock = DockStyle.Top,
                    Padding = new Padding(0, ThemeManager.Spacing.MD, 0, ThemeManager.Spacing.MD)
                };

                var lblSuggestionsTitle = new Label
                {
                    Text = "Possíveis soluções:",
                    Font = ThemeManager.Typography.H6,
                    ForeColor = ThemeManager.Colors.Gray700,
                    AutoSize = true,
                    Dock = DockStyle.Top
                };
                pnlSuggestions.Controls.Add(lblSuggestionsTitle);

                var flowSuggestions = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.TopDown,
                    AutoSize = true,
                    Dock = DockStyle.Top,
                    Padding = new Padding(ThemeManager.Spacing.SM, ThemeManager.Spacing.SM, 0, 0),
                    WrapContents = false
                };

                foreach (var suggestion in Suggestions)
                {
                    var lblSuggestion = new Label
                    {
                        Text = $"• {suggestion}",
                        Font = ThemeManager.Typography.Body2,
                        ForeColor = ThemeManager.Colors.Gray600,
                        AutoSize = true,
                        MaximumSize = new Size(410, 0),
                        Padding = new Padding(0, 2, 0, 2)
                    };
                    flowSuggestions.Controls.Add(lblSuggestion);
                }

                pnlSuggestions.Controls.Add(flowSuggestions);
                mainLayout.Controls.Add(pnlSuggestions, 0, 2);
            }

            // Technical details expander
            if (!string.IsNullOrEmpty(TechnicalDetails))
            {
                var pnlTechnical = new Panel
                {
                    AutoSize = true,
                    Dock = DockStyle.Top,
                    Padding = new Padding(0, ThemeManager.Spacing.MD, 0, ThemeManager.Spacing.MD)
                };

                var btnExpandTechnical = new LinkLabel
                {
                    Text = "▶ Mostrar detalhes técnicos",
                    Font = ThemeManager.Typography.Body2,
                    AutoSize = true,
                    LinkColor = ThemeManager.Colors.Primary,
                    Dock = DockStyle.Top
                };

                var txtTechnical = new TextBox
                {
                    Text = TechnicalDetails,
                    Multiline = true,
                    ReadOnly = true,
                    ScrollBars = ScrollBars.Vertical,
                    Height = 120,
                    Dock = DockStyle.Top,
                    Visible = false,
                    Font = new Font("Consolas", 9F),
                    BackColor = ThemeManager.Colors.Gray100,
                    BorderStyle = BorderStyle.FixedSingle
                };

                bool isExpanded = false;
                btnExpandTechnical.LinkClicked += (s, e) =>
                {
                    isExpanded = !isExpanded;
                    txtTechnical.Visible = isExpanded;
                    btnExpandTechnical.Text = isExpanded
                        ? "▼ Ocultar detalhes técnicos"
                        : "▶ Mostrar detalhes técnicos";
                    this.Height = isExpanded ? 550 : 400;
                };

                pnlTechnical.Controls.Add(txtTechnical);
                pnlTechnical.Controls.Add(btnExpandTechnical);
                mainLayout.Controls.Add(pnlTechnical, 0, 3);
            }

            // Button panel
            var pnlButtons = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Bottom,
                Height = 50,
                Padding = new Padding(0, ThemeManager.Spacing.MD, 0, 0)
            };

            var btnOK = new ModernButton
            {
                Text = "OK",
                Variant = ModernButton.ButtonVariant.Contained,
                BackColor = ThemeManager.Colors.Primary,
                ForeColor = Color.White,
                Size = new Size(100, 36),
                DialogResult = DialogResult.OK
            };
            pnlButtons.Controls.Add(btnOK);

            if (!string.IsNullOrEmpty(TechnicalDetails))
            {
                var btnCopy = new ModernButton
                {
                    Text = "Copiar Erro",
                    Variant = ModernButton.ButtonVariant.Outlined,
                    ForeColor = ThemeManager.Colors.Primary,
                    Size = new Size(120, 36)
                };
                btnCopy.Click += (s, e) =>
                {
                    try
                    {
                        Clipboard.SetText($"{ErrorTitle}\n\n{ErrorMessage}\n\nDetalhes Técnicos:\n{TechnicalDetails}");
                        ToastNotification.Success("Erro copiado para a área de transferência");
                    }
                    catch
                    {
                        ToastNotification.Error("Não foi possível copiar o erro");
                    }
                };
                pnlButtons.Controls.Add(btnCopy);
            }

            this.Controls.Add(pnlButtons);
            this.Controls.Add(mainLayout);
            this.AcceptButton = btnOK;
        }

        /// <summary>
        /// Show error dialog with smart suggestions
        /// </summary>
        public static void Show(string title, string message, Exception? ex = null)
        {
            var dialog = new ErrorDialog
            {
                ErrorTitle = title,
                ErrorMessage = message,
                TechnicalDetails = ex?.ToString() ?? "",
                Suggestions = GenerateSuggestions(message, ex)
            };

            // Update title in UI
            dialog.InitializeComponent();
            dialog.Text = title;

            dialog.ShowDialog();
        }

        /// <summary>
        /// Show error dialog with parent form
        /// </summary>
        public static void Show(IWin32Window owner, string title, string message, Exception? ex = null)
        {
            var dialog = new ErrorDialog
            {
                ErrorTitle = title,
                ErrorMessage = message,
                TechnicalDetails = ex?.ToString() ?? "",
                Suggestions = GenerateSuggestions(message, ex)
            };

            dialog.InitializeComponent();
            dialog.Text = title;

            dialog.ShowDialog(owner);
        }

        /// <summary>
        /// Generate smart suggestions based on exception type and message
        /// </summary>
        private static List<string> GenerateSuggestions(string message, Exception? ex)
        {
            var suggestions = new List<string>();

            if (ex is NpgsqlException)
            {
                suggestions.Add("Verifique se o banco de dados está acessível");
                suggestions.Add("Confirme as credenciais de conexão nas configurações");
                suggestions.Add("Verifique se o serviço PostgreSQL/Supabase está em execução");
            }
            else if (ex is UnauthorizedAccessException)
            {
                suggestions.Add("Execute o aplicativo como administrador");
                suggestions.Add("Verifique as permissões de arquivo/pasta");
                suggestions.Add("Certifique-se de ter acesso ao recurso solicitado");
            }
            else if (ex is System.IO.IOException)
            {
                suggestions.Add("Verifique se o arquivo não está sendo usado por outro programa");
                suggestions.Add("Confirme se há espaço em disco suficiente");
                suggestions.Add("Verifique as permissões de leitura/gravação");
            }
            else if (ex is System.Net.WebException || ex is System.Net.Http.HttpRequestException)
            {
                suggestions.Add("Verifique sua conexão com a internet");
                suggestions.Add("Confirme se o servidor está acessível");
                suggestions.Add("Verifique configurações de firewall ou proxy");
            }
            else if (message.Contains("CPF", StringComparison.OrdinalIgnoreCase))
            {
                suggestions.Add("Verifique se o CPF foi digitado corretamente");
                suggestions.Add("O CPF deve conter 11 dígitos numéricos");
                suggestions.Add("Certifique-se de que o CPF é válido");
            }
            else if (message.Contains("ISBN", StringComparison.OrdinalIgnoreCase))
            {
                suggestions.Add("Verifique se o ISBN foi digitado corretamente");
                suggestions.Add("O ISBN-10 deve ter 10 dígitos ou ISBN-13 deve ter 13 dígitos");
            }
            else if (message.Contains("email", StringComparison.OrdinalIgnoreCase) ||
                     message.Contains("e-mail", StringComparison.OrdinalIgnoreCase))
            {
                suggestions.Add("Verifique se o e-mail está no formato correto (usuario@dominio.com)");
                suggestions.Add("Certifique-se de não ter espaços ou caracteres inválidos");
            }
            else if (message.Contains("obrigatório", StringComparison.OrdinalIgnoreCase) ||
                     message.Contains("campo vazio", StringComparison.OrdinalIgnoreCase))
            {
                suggestions.Add("Preencha todos os campos obrigatórios");
                suggestions.Add("Verifique se nenhum campo necessário está em branco");
            }

            // Generic fallback suggestions
            if (suggestions.Count == 0)
            {
                suggestions.Add("Tente realizar a operação novamente");
                suggestions.Add("Se o problema persistir, contate o suporte técnico");
            }

            return suggestions;
        }
    }
}
