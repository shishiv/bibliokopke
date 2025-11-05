using System;
using System.Windows.Forms;
using BibliotecaJK.Model;
using BibliotecaJK.DAL;

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

            // FormTrocaSenha
            this.ClientSize = new System.Drawing.Size(500, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = _obrigatorio ? "Troca de Senha Obrigatória" : "Alterar Senha";
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Se for obrigatório, não permite fechar
            if (_obrigatorio)
            {
                this.FormClosing += FormTrocaSenha_FormClosing;
            }

            // Título
            var lblTitulo = new Label
            {
                Text = _obrigatorio ? "🔒 TROCA DE SENHA OBRIGATÓRIA" : "🔒 ALTERAR SENHA",
                Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold),
                ForeColor = _obrigatorio ? System.Drawing.Color.DarkRed : System.Drawing.Color.DarkSlateBlue,
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(460, 30),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
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
                Size = new System.Drawing.Size(460, 50),
                TextAlign = System.Drawing.ContentAlignment.TopCenter
            };
            this.Controls.Add(lblMensagem);

            // Panel principal
            var pnlMain = new Panel
            {
                Location = new System.Drawing.Point(20, 120),
                Size = new System.Drawing.Size(460, 250),
                BackColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Senha Atual
            pnlMain.Controls.Add(new Label
            {
                Text = "Senha Atual:",
                Location = new System.Drawing.Point(20, 25),
                Size = new System.Drawing.Size(120, 20),
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold)
            });

            txtSenhaAtual = new TextBox
            {
                Location = new System.Drawing.Point(150, 23),
                Size = new System.Drawing.Size(280, 25),
                PasswordChar = '●',
                Font = new System.Drawing.Font("Segoe UI", 10F)
            };
            pnlMain.Controls.Add(txtSenhaAtual);

            // Nova Senha
            pnlMain.Controls.Add(new Label
            {
                Text = "Nova Senha:",
                Location = new System.Drawing.Point(20, 70),
                Size = new System.Drawing.Size(120, 20),
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold)
            });

            txtNovaSenha = new TextBox
            {
                Location = new System.Drawing.Point(150, 68),
                Size = new System.Drawing.Size(280, 25),
                PasswordChar = '●',
                Font = new System.Drawing.Font("Segoe UI", 10F)
            };
            txtNovaSenha.TextChanged += TxtNovaSenha_TextChanged;
            pnlMain.Controls.Add(txtNovaSenha);

            // Força da senha
            lblForcaSenha = new Label
            {
                Text = "Força: ",
                Location = new System.Drawing.Point(150, 98),
                Size = new System.Drawing.Size(280, 20),
                Font = new System.Drawing.Font("Segoe UI", 8F),
                ForeColor = System.Drawing.Color.Gray
            };
            pnlMain.Controls.Add(lblForcaSenha);

            progressForca = new ProgressBar
            {
                Location = new System.Drawing.Point(150, 118),
                Size = new System.Drawing.Size(280, 10),
                Minimum = 0,
                Maximum = 100,
                Value = 0
            };
            pnlMain.Controls.Add(progressForca);

            // Confirmar Senha
            pnlMain.Controls.Add(new Label
            {
                Text = "Confirmar Senha:",
                Location = new System.Drawing.Point(20, 150),
                Size = new System.Drawing.Size(120, 20),
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold)
            });

            txtConfirmaSenha = new TextBox
            {
                Location = new System.Drawing.Point(150, 148),
                Size = new System.Drawing.Size(280, 25),
                PasswordChar = '●',
                Font = new System.Drawing.Font("Segoe UI", 10F)
            };
            pnlMain.Controls.Add(txtConfirmaSenha);

            // Requisitos
            var lblRequisitos = new Label
            {
                Text = "✓ Mínimo 8 caracteres\n" +
                       "✓ Use letras, números e símbolos\n" +
                       "✓ Evite senhas óbvias",
                Location = new System.Drawing.Point(20, 185),
                Size = new System.Drawing.Size(410, 50),
                Font = new System.Drawing.Font("Segoe UI", 8F),
                ForeColor = System.Drawing.Color.Gray
            };
            pnlMain.Controls.Add(lblRequisitos);

            this.Controls.Add(pnlMain);

            // Botões
            btnSalvar = new Button
            {
                Text = "💾 Salvar Nova Senha",
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(120, 385),
                Size = new System.Drawing.Size(180, 40),
                BackColor = System.Drawing.Color.DarkGreen,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSalvar.FlatAppearance.BorderSize = 0;
            btnSalvar.Click += BtnSalvar_Click;
            this.Controls.Add(btnSalvar);

            btnCancelar = new Button
            {
                Text = _obrigatorio ? "❌ Sair do Sistema" : "Cancelar",
                Font = new System.Drawing.Font("Segoe UI", 10F),
                Location = new System.Drawing.Point(310, 385),
                Size = new System.Drawing.Size(170, 40),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Click += BtnCancelar_Click;
            this.Controls.Add(btnCancelar);

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

                // Verificar senha atual
                if (!BCrypt.Net.BCrypt.Verify(txtSenhaAtual.Text, _funcionario.SenhaHash))
                {
                    MessageBox.Show("Senha atual incorreta.", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSenhaAtual.Focus();
                    txtSenhaAtual.SelectAll();
                    return;
                }

                // Verificar se a nova senha é diferente da atual
                if (BCrypt.Net.BCrypt.Verify(txtNovaSenha.Text, _funcionario.SenhaHash))
                {
                    MessageBox.Show("A nova senha deve ser diferente da senha atual.", "Atenção",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNovaSenha.Focus();
                    return;
                }

                // Gerar hash da nova senha
                string novoHash = BCrypt.Net.BCrypt.HashPassword(txtNovaSenha.Text, 11);

                // Atualizar no banco
                _funcionario.SenhaHash = novoHash;
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
    }
}
