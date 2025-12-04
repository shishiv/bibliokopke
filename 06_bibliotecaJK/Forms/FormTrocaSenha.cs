using System;
using System.Windows.Forms;
using BibliotecaJK.Model;
using BibliotecaJK.DAL;
using Npgsql;

namespace BibliotecaJK.Forms
{
    /// <summary>
    /// Formulário para troca de senha
    /// Usado no primeiro login ou quando o usuário deseja alterar sua senha
    /// </summary>
    public partial class FormTrocaSenha : Form
    {
        private readonly Funcionario _funcionario;
        private readonly bool _obrigatorio;

        private TextBox txtSenhaAtual = new TextBox();
        private TextBox txtNovaSenha = new TextBox();
        private TextBox txtConfirmaSenha = new TextBox();
        private Label lblForcaSenha = new Label();
        private ProgressBar progressForca = new ProgressBar();
        private Button btnSalvar = new Button();
        private Button btnCancelar = new Button();

        public FormTrocaSenha(Funcionario funcionario, bool obrigatorio = false)
        {
            _funcionario = funcionario;
            _obrigatorio = obrigatorio;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // FormTrocaSenha - Configurações base com responsividade
            this.ClientSize = new System.Drawing.Size(600, 500);
            this.MinimumSize = new System.Drawing.Size(500, 350);
            this.AutoScroll = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = _obrigatorio ? "Troca de Senha Obrigatória" : "Alterar Senha";
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Se for obrigatório, não permite fechar
            if (_obrigatorio)
            {
                this.FormClosing += FormTrocaSenha_FormClosing;
            }

            // Título com Anchor responsivo
            var lblTitulo = new Label
            {
                Text = _obrigatorio ? "🔒 TROCA DE SENHA OBRIGATÓRIA" : "🔒 ALTERAR SENHA",
                Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold),
                ForeColor = _obrigatorio ? System.Drawing.Color.DarkRed : System.Drawing.Color.DarkSlateBlue,
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(560, 30),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            this.Controls.Add(lblTitulo);

            // Mensagem explicativa
            var lblMensagem = new Label
            {
                Text = _obrigatorio
                    ? "Por segurança, você deve alterar sua senha padrão antes de continuar.\n" +
                      "Escolha uma senha forte com pelo menos 8 caracteres."
                    : "Digite sua senha atual e escolha uma nova senha.\n" +
                      "A nova senha deve ter pelo menos 8 caracteres.",
                Font = new System.Drawing.Font("Segoe UI", 9F),
                ForeColor = System.Drawing.Color.Gray,
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(560, 50),
                TextAlign = System.Drawing.ContentAlignment.TopCenter,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            this.Controls.Add(lblMensagem);

            // Panel principal com Anchor responsivo
            var pnlMain = new Panel
            {
                Location = new System.Drawing.Point(20, 120),
                Size = new System.Drawing.Size(560, 300),
                BackColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            // Senha Atual
            pnlMain.Controls.Add(new Label
            {
                Text = "Senha Atual:",
                Location = new System.Drawing.Point(20, 25),
                Size = new System.Drawing.Size(100, 20),
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold)
            });

            txtSenhaAtual = new TextBox
            {
                Location = new System.Drawing.Point(130, 23),
                Size = new System.Drawing.Size(400, 25),
                PasswordChar = '●',
                Font = new System.Drawing.Font("Segoe UI", 10F),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            pnlMain.Controls.Add(txtSenhaAtual);

            // Nova Senha
            pnlMain.Controls.Add(new Label
            {
                Text = "Nova Senha:",
                Location = new System.Drawing.Point(20, 70),
                Size = new System.Drawing.Size(100, 20),
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold)
            });

            txtNovaSenha = new TextBox
            {
                Location = new System.Drawing.Point(130, 68),
                Size = new System.Drawing.Size(400, 25),
                PasswordChar = '●',
                Font = new System.Drawing.Font("Segoe UI", 10F),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            txtNovaSenha.TextChanged += TxtNovaSenha_TextChanged;
            pnlMain.Controls.Add(txtNovaSenha);

            // Força da senha
            lblForcaSenha = new Label
            {
                Text = "Força: ",
                Location = new System.Drawing.Point(130, 98),
                Size = new System.Drawing.Size(400, 20),
                Font = new System.Drawing.Font("Segoe UI", 8F),
                ForeColor = System.Drawing.Color.Gray,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            pnlMain.Controls.Add(lblForcaSenha);

            progressForca = new ProgressBar
            {
                Location = new System.Drawing.Point(130, 118),
                Size = new System.Drawing.Size(400, 10),
                Minimum = 0,
                Maximum = 100,
                Value = 0,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            pnlMain.Controls.Add(progressForca);

            // Confirmar Senha
            pnlMain.Controls.Add(new Label
            {
                Text = "Confirmar Senha:",
                Location = new System.Drawing.Point(20, 150),
                Size = new System.Drawing.Size(110, 20),
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold)
            });

            txtConfirmaSenha = new TextBox
            {
                Location = new System.Drawing.Point(130, 148),
                Size = new System.Drawing.Size(400, 25),
                PasswordChar = '●',
                Font = new System.Drawing.Font("Segoe UI", 10F),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            pnlMain.Controls.Add(txtConfirmaSenha);

            // Requisitos
            var lblRequisitos = new Label
            {
                Text = "✓ Mínimo 8 caracteres\n" +
                       "✓ Use letras, números e símbolos\n" +
                       "✓ Evite senhas óbvias",
                Location = new System.Drawing.Point(20, 195),
                Size = new System.Drawing.Size(510, 60),
                Font = new System.Drawing.Font("Segoe UI", 8F),
                ForeColor = System.Drawing.Color.Gray,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            pnlMain.Controls.Add(lblRequisitos);

            this.Controls.Add(pnlMain);

            // FlowLayoutPanel para botões ancorado Bottom | Right
            var flowButtons = new FlowLayoutPanel
            {
                Location = new System.Drawing.Point(20, 430),
                Size = new System.Drawing.Size(560, 50),
                BackColor = System.Drawing.Color.WhiteSmoke,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left
            };

            btnCancelar = new Button
            {
                Text = _obrigatorio ? "❌ Sair do Sistema" : "Cancelar",
                Font = new System.Drawing.Font("Segoe UI", 10F),
                Size = new System.Drawing.Size(160, 40),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding(5)
            };
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Click += BtnCancelar_Click;
            flowButtons.Controls.Add(btnCancelar);

            btnSalvar = new Button
            {
                Text = "💾 Salvar Nova Senha",
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                Size = new System.Drawing.Size(180, 40),
                BackColor = System.Drawing.Color.DarkGreen,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding(5)
            };
            btnSalvar.FlatAppearance.BorderSize = 0;
            btnSalvar.Click += BtnSalvar_Click;
            flowButtons.Controls.Add(btnSalvar);

            this.Controls.Add(flowButtons);

            this.ResumeLayout(false);
        }

        private void FormTrocaSenha_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (_obrigatorio && this.DialogResult != DialogResult.OK)
            {
                var result = MessageBox.Show(
                    "A troca de senha é obrigatória!\n\n" +
                    "Deseja realmente sair do sistema?",
                    "Atenção",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                {
                    e.Cancel = true;
                }
            }
        }

        private void TxtNovaSenha_TextChanged(object? sender, EventArgs e)
        {
            AvaliarForcaSenha(txtNovaSenha.Text);
        }

        private void AvaliarForcaSenha(string senha)
        {
            if (string.IsNullOrEmpty(senha))
            {
                progressForca.Value = 0;
                lblForcaSenha.Text = "Força: ";
                lblForcaSenha.ForeColor = System.Drawing.Color.Gray;
                return;
            }

            int forca = 0;

            // Comprimento
            if (senha.Length >= 8) forca += 20;
            if (senha.Length >= 12) forca += 20;

            // Contém letras minúsculas
            if (System.Text.RegularExpressions.Regex.IsMatch(senha, "[a-z]")) forca += 15;

            // Contém letras maiúsculas
            if (System.Text.RegularExpressions.Regex.IsMatch(senha, "[A-Z]")) forca += 15;

            // Contém números
            if (System.Text.RegularExpressions.Regex.IsMatch(senha, "[0-9]")) forca += 15;

            // Contém símbolos
            if (System.Text.RegularExpressions.Regex.IsMatch(senha, "[^a-zA-Z0-9]")) forca += 15;

            progressForca.Value = Math.Min(forca, 100);

            if (forca < 40)
            {
                lblForcaSenha.Text = "Força: Fraca ⚠️";
                lblForcaSenha.ForeColor = System.Drawing.Color.Red;
            }
            else if (forca < 70)
            {
                lblForcaSenha.Text = "Força: Média ⚡";
                lblForcaSenha.ForeColor = System.Drawing.Color.Orange;
            }
            else
            {
                lblForcaSenha.Text = "Força: Forte ✓";
                lblForcaSenha.ForeColor = System.Drawing.Color.Green;
            }
        }

        private void BtnSalvar_Click(object? sender, EventArgs e)
        {
            try
            {
                // Validações
                if (string.IsNullOrWhiteSpace(txtSenhaAtual.Text))
                {
                    MessageBox.Show("Digite a senha atual.", "Atenção",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSenhaAtual.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtNovaSenha.Text))
                {
                    MessageBox.Show("Digite a nova senha.", "Atenção",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNovaSenha.Focus();
                    return;
                }

                if (txtNovaSenha.Text.Length < 8)
                {
                    MessageBox.Show("A nova senha deve ter pelo menos 8 caracteres.", "Atenção",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNovaSenha.Focus();
                    return;
                }

                if (txtNovaSenha.Text != txtConfirmaSenha.Text)
                {
                    MessageBox.Show("As senhas não coincidem.", "Atenção",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtConfirmaSenha.Focus();
                    return;
                }

                // Verificar senha atual usando função PostgreSQL
                if (!VerificarSenhaPostgreSQL(txtSenhaAtual.Text, _funcionario.SenhaHash))
                {
                    MessageBox.Show("Senha atual incorreta.", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSenhaAtual.Focus();
                    txtSenhaAtual.SelectAll();
                    return;
                }

                // Verificar se a nova senha é diferente da atual
                if (VerificarSenhaPostgreSQL(txtNovaSenha.Text, _funcionario.SenhaHash))
                {
                    MessageBox.Show("A nova senha deve ser diferente da senha atual.", "Atenção",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNovaSenha.Focus();
                    return;
                }

                // Atualizar no banco (o trigger hash_senha_funcionario irá hashear automaticamente)
                _funcionario.SenhaHash = txtNovaSenha.Text; // Enviar texto plano - será hasheado pelo trigger
                _funcionario.PrimeiroLogin = false;

                var dal = new FuncionarioDAL();
                dal.Atualizar(_funcionario);

                MessageBox.Show(
                    "Senha alterada com sucesso!",
                    "Sucesso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao alterar senha: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelar_Click(object? sender, EventArgs e)
        {
            if (_obrigatorio)
            {
                var result = MessageBox.Show(
                    "A troca de senha é obrigatória!\n\n" +
                    "Se você cancelar, o sistema será fechado.\n" +
                    "Deseja realmente sair?",
                    "Atenção",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
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
