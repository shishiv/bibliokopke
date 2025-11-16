using System;
using System.Windows.Forms;
using BibliotecaJK.DAL;
using BibliotecaJK.Model;
using BibliotecaJK.BLL;
using Npgsql;
using BibliotecaJK;
using BibliotecaJK.Components;

namespace BibliotecaJK.Forms
{
    /// <summary>
    /// Formulário de Login do Sistema BibliotecaJK
    /// </summary>
    public partial class FormLogin : Form
    {
        private readonly FuncionarioDAL _funcionarioDAL;
        private readonly LogService _logService;

        public Funcionario? FuncionarioLogado { get; private set; }
        public bool PrecisaTrocarSenha { get; private set; } = false;

        public FormLogin()
        {
            InitializeComponent();
            _funcionarioDAL = new FuncionarioDAL();
            _logService = new LogService();

            // Make form responsive
            LayoutManager.MakeFormResponsive(this);

            // Configurar eventos
            txtSenha.TextChanged += (s, e) => { }; // ModernTextBox compatibility
            btnEntrar.Click += BtnEntrar_Click;
            btnCancelar.Click += BtnCancelar_Click;

            // Add keyboard shortcuts
            this.KeyPreview = true;
            this.KeyDown += FormLogin_KeyDown;
        }

        private void FormLogin_KeyDown(object? sender, KeyEventArgs e)
        {
            // Enter to submit
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                BtnEntrar_Click(sender, e);
            }
            // Escape to cancel
            else if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                BtnCancelar_Click(sender, e);
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // FormLogin
            this.ClientSize = new System.Drawing.Size(480, 420);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "BibliotecaJK - Login";
            this.BackColor = ThemeManager.Light.Background;

            // lblTitulo
            var lblTitulo = new Label
            {
                Text = "SISTEMA BIBLIOTECAJK",
                Font = ThemeManager.Typography.H3,
                ForeColor = ThemeManager.Light.Primary,
                Location = new System.Drawing.Point(ThemeManager.Spacing.LG, ThemeManager.Spacing.LG),
                Size = new System.Drawing.Size(432, 50),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblTitulo);

            // lblSubtitulo
            var lblSubtitulo = new Label
            {
                Text = "Autenticação de Funcionários",
                Font = ThemeManager.Typography.Body1,
                ForeColor = ThemeManager.Light.TextSecondary,
                Location = new System.Drawing.Point(ThemeManager.Spacing.LG, 74),
                Size = new System.Drawing.Size(432, 30),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblSubtitulo);

            // txtLogin (ModernTextBox)
            txtLogin = new ModernTextBox
            {
                FloatingLabel = "Login",
                Location = new System.Drawing.Point(ThemeManager.Spacing.XXL, 140),
                Size = new System.Drawing.Size(340, 56),
                MaxLength = 50,
                FocusedLineColor = ThemeManager.Light.Primary,
                UnfocusedLineColor = ThemeManager.Colors.Neutral300
            };
            this.Controls.Add(txtLogin);

            // HelpIcon for Login
            var helpIconLogin = new HelpIcon("Use seu login de funcionário cadastrado no sistema para acessar.")
            {
                Location = new System.Drawing.Point(ThemeManager.Spacing.XXL + 345, 150),
                HelpTitle = "Login do Sistema"
            };
            this.Controls.Add(helpIconLogin);

            // txtSenha (ModernTextBox)
            txtSenha = new ModernTextBox
            {
                FloatingLabel = "Senha",
                Location = new System.Drawing.Point(ThemeManager.Spacing.XXL, 220),
                Size = new System.Drawing.Size(340, 56),
                MaxLength = 50,
                PasswordChar = '●',
                FocusedLineColor = ThemeManager.Light.Primary,
                UnfocusedLineColor = ThemeManager.Colors.Neutral300
            };
            this.Controls.Add(txtSenha);

            // btnEntrar (ModernButton)
            btnEntrar = new ModernButton
            {
                Text = "Entrar",
                Location = new System.Drawing.Point(ThemeManager.Spacing.XXL, 320),
                Size = new System.Drawing.Size(160, 44),
                BackColor = ThemeManager.Light.Primary,
                ForeColor = System.Drawing.Color.White,
                Variant = ModernButton.ButtonVariant.Contained,
                BorderRadiusValue = ThemeManager.BorderRadius.MD
            };
            this.Controls.Add(btnEntrar);

            // btnCancelar (ModernButton)
            btnCancelar = new ModernButton
            {
                Text = "Cancelar",
                Location = new System.Drawing.Point(ThemeManager.Spacing.XXL + 175, 320),
                Size = new System.Drawing.Size(160, 44),
                BackColor = ThemeManager.Colors.Neutral500,
                ForeColor = ThemeManager.Light.Text,
                Variant = ModernButton.ButtonVariant.Outlined,
                BorderRadiusValue = ThemeManager.BorderRadius.MD
            };
            this.Controls.Add(btnCancelar);

            // Footer info
            var lblFooter = new Label
            {
                Text = "Pressione Enter para entrar ou Esc para cancelar",
                Font = ThemeManager.Typography.Caption,
                ForeColor = ThemeManager.Light.TextSecondary,
                Location = new System.Drawing.Point(ThemeManager.Spacing.LG, 380),
                Size = new System.Drawing.Size(432, 20),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblFooter);

            this.ResumeLayout(false);
        }

        private ModernTextBox txtLogin = new ModernTextBox();
        private ModernTextBox txtSenha = new ModernTextBox();
        private ModernButton btnEntrar = new ModernButton();
        private ModernButton btnCancelar = new ModernButton();

        private void BtnEntrar_Click(object? sender, EventArgs e)
        {
            try
            {
                // Validar campos
                if (string.IsNullOrWhiteSpace(txtLogin.Text))
                {
                    MessageBox.Show("Por favor, informe o login.", "Atenção",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtLogin.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtSenha.Text))
                {
                    MessageBox.Show("Por favor, informe a senha.", "Atenção",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSenha.Focus();
                    return;
                }

                // Buscar funcionário pelo login
                var funcionarios = _funcionarioDAL.Listar();
                var funcionario = funcionarios.Find(f =>
                    f.Login?.Equals(txtLogin.Text, StringComparison.OrdinalIgnoreCase) == true);

                if (funcionario == null)
                {
                    MessageBox.Show("Login ou senha incorretos.", "Erro de Autenticação",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _logService.Registrar(null, "LOGIN_FALHA",
                        $"Tentativa de login com usuário inexistente: {txtLogin.Text}");
                    txtSenha.Clear();
                    txtLogin.Focus();
                    return;
                }

                // Validar senha usando função PostgreSQL verificar_senha()
                bool senhaValida = VerificarSenhaPostgreSQL(txtSenha.Text, funcionario.SenhaHash);

                if (!senhaValida)
                {
                    MessageBox.Show("Login ou senha incorretos.", "Erro de Autenticação",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _logService.Registrar(funcionario.Id, "LOGIN_FALHA",
                        $"Senha incorreta para o usuário: {txtLogin.Text}");
                    txtSenha.Clear();
                    txtSenha.Focus();
                    return;
                }

                // Login bem-sucedido
                FuncionarioLogado = funcionario;
                _logService.Registrar(funcionario.Id, "LOGIN_SUCESSO",
                    $"Funcionário {funcionario.Nome} autenticado com sucesso");

                // Verificar se é primeiro login
                if (funcionario.PrimeiroLogin)
                {
                    MessageBox.Show(
                        $"Bem-vindo, {funcionario.Nome}!\n\n" +
                        "Este é seu primeiro acesso ao sistema.\n" +
                        "Por segurança, você deve alterar sua senha antes de continuar.",
                        "Primeiro Acesso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    PrecisaTrocarSenha = true;
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao realizar login: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelar_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// Verifica senha usando a função PostgreSQL verificar_senha()
        /// </summary>
        private bool VerificarSenhaPostgreSQL(string senhaTexto, string senhaHash)
        {
            try
            {
                using var conn = Conexao.GetConnection();
                conn.Open();

                string sql = "SELECT verificar_senha(@senha, @hash)";
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@senha", senhaTexto);
                cmd.Parameters.AddWithValue("@hash", senhaHash);

                var result = cmd.ExecuteScalar();
                return result != null && (bool)result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao verificar senha: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
