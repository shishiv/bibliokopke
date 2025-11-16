using System;
using System.Windows.Forms;
using BibliotecaJK.DAL;
using BibliotecaJK.Model;
using BibliotecaJK.BLL;
using Npgsql;
using BibliotecaJK;

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

            // Configurar eventos
            txtSenha.KeyPress += TxtSenha_KeyPress;
            btnEntrar.Click += BtnEntrar_Click;
            btnCancelar.Click += BtnCancelar_Click;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // FormLogin
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "BibliotecaJK - Login";
            this.BackColor = System.Drawing.Color.WhiteSmoke;

            // lblTitulo
            var lblTitulo = new Label
            {
                Text = "SISTEMA BIBLIOTECAJK",
                Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                Location = new System.Drawing.Point(50, 30),
                Size = new System.Drawing.Size(300, 40),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblTitulo);

            // lblSubtitulo
            var lblSubtitulo = new Label
            {
                Text = "Autenticação de Funcionários",
                Font = new System.Drawing.Font("Segoe UI", 10F),
                ForeColor = System.Drawing.Color.Gray,
                Location = new System.Drawing.Point(50, 70),
                Size = new System.Drawing.Size(300, 25),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblSubtitulo);

            // lblLogin
            var lblLogin = new Label
            {
                Text = "Login:",
                Font = new System.Drawing.Font("Segoe UI", 10F),
                Location = new System.Drawing.Point(50, 120),
                Size = new System.Drawing.Size(80, 25)
            };
            this.Controls.Add(lblLogin);

            // txtLogin
            txtLogin = new TextBox
            {
                Font = new System.Drawing.Font("Segoe UI", 10F),
                Location = new System.Drawing.Point(140, 118),
                Size = new System.Drawing.Size(210, 25),
                MaxLength = 50
            };
            this.Controls.Add(txtLogin);

            // lblSenha
            var lblSenha = new Label
            {
                Text = "Senha:",
                Font = new System.Drawing.Font("Segoe UI", 10F),
                Location = new System.Drawing.Point(50, 160),
                Size = new System.Drawing.Size(80, 25)
            };
            this.Controls.Add(lblSenha);

            // txtSenha
            txtSenha = new TextBox
            {
                Font = new System.Drawing.Font("Segoe UI", 10F),
                Location = new System.Drawing.Point(140, 158),
                Size = new System.Drawing.Size(210, 25),
                MaxLength = 50,
                PasswordChar = '●'
            };
            this.Controls.Add(txtSenha);

            // btnEntrar
            btnEntrar = new Button
            {
                Text = "Entrar",
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(140, 210),
                Size = new System.Drawing.Size(100, 35),
                BackColor = System.Drawing.Color.DarkSlateBlue,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnEntrar.FlatAppearance.BorderSize = 0;
            this.Controls.Add(btnEntrar);

            // btnCancelar
            btnCancelar = new Button
            {
                Text = "Cancelar",
                Font = new System.Drawing.Font("Segoe UI", 10F),
                Location = new System.Drawing.Point(250, 210),
                Size = new System.Drawing.Size(100, 35),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancelar.FlatAppearance.BorderSize = 0;
            this.Controls.Add(btnCancelar);

            this.ResumeLayout(false);
        }

        private TextBox txtLogin = new TextBox();
        private TextBox txtSenha = new TextBox();
        private Button btnEntrar = new Button();
        private Button btnCancelar = new Button();

        private void TxtSenha_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnEntrar_Click(sender, e);
            }
        }

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
