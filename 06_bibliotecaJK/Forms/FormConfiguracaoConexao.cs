using System;
using System.Windows.Forms;
using BibliotecaJK;

namespace BibliotecaJK.Forms
{
    public partial class FormConfiguracaoConexao : Form
    {
        public bool Configurado { get; private set; } = false;

        public FormConfiguracaoConexao()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void InitializeComponent()
        {
            this.Text = "Configuracao Inicial - Conexao com Banco de Dados";
            this.Size = new System.Drawing.Size(800, 600);
            this.MinimumSize = new System.Drawing.Size(600, 450);
            this.AutoScroll = true;
            this.Padding = new Padding(15);

            int currentY = 15;
            const int PADDING = 15;
            const int FIELD_HEIGHT = 30;
            const int FIELD_SPACING = 10;
            const int BUTTON_HEIGHT = 40;

            // ========== TITULO ==========
            var lblTitulo = new Label
            {
                Text = "Bem-vindo ao BibliotecaJK!",
                Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold),
                AutoSize = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Location = new System.Drawing.Point(PADDING, currentY),
                Size = new System.Drawing.Size(this.ClientSize.Width - (PADDING * 2), 40),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblTitulo);
            currentY += 50;

            // ========== DESCRICAO ==========
            var lblDescricao = new Label
            {
                Text = "Configure a conexao com o banco de dados Supabase ou PostgreSQL.\n" +
                       "Voce pode obter a connection string no painel do Supabase em:\n" +
                       "Settings > Database > Connection String (URI)",
                AutoSize = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Location = new System.Drawing.Point(PADDING, currentY),
                Size = new System.Drawing.Size(this.ClientSize.Width - (PADDING * 2), 60),
                TextAlign = System.Drawing.ContentAlignment.TopLeft
            };
            this.Controls.Add(lblDescricao);
            currentY += 75;

            // ========== PANEL DE FORMULARIO ==========
            var pnlFormulario = new Panel
            {
                AutoSize = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Location = new System.Drawing.Point(PADDING, currentY),
                Size = new System.Drawing.Size(this.ClientSize.Width - (PADDING * 2), 300),
                BackColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            pnlFormulario.Padding = new Padding(10);

            int formY = 10;

            // Label Connection String
            var lblConnString = new Label
            {
                Text = "Connection String (PostgreSQL/Supabase):",
                AutoSize = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Location = new System.Drawing.Point(10, formY),
                Size = new System.Drawing.Size(pnlFormulario.ClientSize.Width - 20, FIELD_HEIGHT)
            };
            pnlFormulario.Controls.Add(lblConnString);
            formY += FIELD_HEIGHT + FIELD_SPACING;

            // TextBox Connection String (multiline para conexoes longas)
            var txtConnectionString = new TextBox
            {
                Name = "txtConnectionString",
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new System.Drawing.Font("Consolas", 9F),
                AutoSize = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Location = new System.Drawing.Point(10, formY),
                Size = new System.Drawing.Size(pnlFormulario.ClientSize.Width - 20, 90)
            };
            pnlFormulario.Controls.Add(txtConnectionString);
            formY += 100;

            // Label de exemplo
            var lblExemplo = new Label
            {
                Text = "SUPABASE - Session Pooler (RECOMENDADO para apps desktop):\n" +
                       "postgresql://postgres.xxxxx:[SUA-SENHA]@aws-0-sa-east-1.pooler.supabase.com:5432/postgres\n" +
                       "SUPABASE - Conexão Direta:\n" +
                       "postgresql://postgres:[SUA-SENHA]@db.xxxxx.supabase.co:5432/postgres\n" +
                       "PostgreSQL Local:\n" +
                       "Host=localhost;Port=5432;Database=bibliokopke;Username=postgres;Password=sua_senha",
                AutoSize = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                ForeColor = System.Drawing.Color.Gray,
                Font = new System.Drawing.Font("Consolas", 7.5F),
                Location = new System.Drawing.Point(10, formY),
                Size = new System.Drawing.Size(pnlFormulario.ClientSize.Width - 20, 100)
            };
            pnlFormulario.Controls.Add(lblExemplo);

            pnlFormulario.Size = new System.Drawing.Size(pnlFormulario.Width, formY + 100);
            this.Controls.Add(pnlFormulario);
            currentY += pnlFormulario.Height + 15;

            // ========== NOTA SOBRE SSL ==========
            var lblSSLNota = new Label
            {
                Text = "Nota: Conexões Supabase usam SSL automaticamente. O sistema detecta e adiciona SSL Mode=Require.",
                AutoSize = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                ForeColor = System.Drawing.Color.Blue,
                Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Italic),
                Location = new System.Drawing.Point(PADDING, currentY),
                Size = new System.Drawing.Size(this.ClientSize.Width - (PADDING * 2), 30)
            };
            this.Controls.Add(lblSSLNota);
            currentY += 40;

            // ========== BOTAO TESTAR CONEXAO ==========
            var btnTestar = new Button
            {
                Text = "Testar Conexao",
                AutoSize = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                BackColor = System.Drawing.Color.FromArgb(0, 120, 215),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new System.Drawing.Point(PADDING, currentY),
                Size = new System.Drawing.Size(120, BUTTON_HEIGHT)
            };
            btnTestar.Click += (s, e) => TestarConexao(txtConnectionString.Text);
            this.Controls.Add(btnTestar);

            // ========== LABEL DE STATUS ==========
            var lblStatus = new Label
            {
                Name = "lblStatus",
                Text = "",
                AutoSize = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(PADDING + 130, currentY),
                Size = new System.Drawing.Size(this.ClientSize.Width - (PADDING * 2) - 130, BUTTON_HEIGHT)
            };
            this.Controls.Add(lblStatus);
            currentY += BUTTON_HEIGHT + 20;

            // ========== LABEL DE AJUDA ==========
            var lblAjuda = new Label
            {
                Text = "Precisa de ajuda? Consulte o manual de instalacao ou visite: https://supabase.com/docs",
                AutoSize = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                ForeColor = System.Drawing.Color.Blue,
                Font = new System.Drawing.Font("Segoe UI", 8F),
                Cursor = Cursors.Hand,
                Location = new System.Drawing.Point(PADDING, currentY),
                Size = new System.Drawing.Size(this.ClientSize.Width - (PADDING * 2), 40)
            };
            lblAjuda.Click += (s, e) =>
            {
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "https://supabase.com/docs",
                        UseShellExecute = true
                    });
                }
                catch { }
            };
            this.Controls.Add(lblAjuda);

            // ========== FLOW LAYOUT PANEL PARA BOTOES (Bottom | Right) ==========
            var flpBotoes = new FlowLayoutPanel
            {
                AutoSize = false,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                Location = new System.Drawing.Point(this.ClientSize.Width - 340, this.ClientSize.Height - 60),
                Size = new System.Drawing.Size(320, BUTTON_HEIGHT + 10),
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                Padding = new Padding(5)
            };

            // Botao Salvar
            var btnSalvar = new Button
            {
                Text = "Salvar e Continuar",
                AutoSize = false,
                BackColor = System.Drawing.Color.FromArgb(0, 150, 0),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                Size = new System.Drawing.Size(140, BUTTON_HEIGHT),
                Margin = new Padding(5, 0, 0, 0)
            };
            btnSalvar.Click += (s, e) => SalvarConfiguracao(txtConnectionString.Text);
            flpBotoes.Controls.Add(btnSalvar);

            // Botao Cancelar
            var btnCancelar = new Button
            {
                Text = "Cancelar",
                AutoSize = false,
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new System.Drawing.Size(100, BUTTON_HEIGHT),
                Margin = new Padding(5, 0, 0, 0)
            };
            btnCancelar.Click += (s, e) =>
            {
                Configurado = false;
                this.Close();
            };
            flpBotoes.Controls.Add(btnCancelar);

            this.Controls.Add(flpBotoes);
        }

        private void TestarConexao(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                MostrarStatus("Por favor, insira uma connection string.", false);
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;

                bool sucesso = Conexao.TestarConexao(connectionString, out string mensagemErro);

                if (sucesso)
                {
                    MostrarStatus("Conexao testada com sucesso!", true);
                }
                else
                {
                    MostrarStatus($"Erro: {mensagemErro}", false);
                }
            }
            catch (Exception ex)
            {
                MostrarStatus($"Erro ao testar: {ex.Message}", false);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void SalvarConfiguracao(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                MessageBox.Show("Por favor, insira uma connection string valida.",
                    "Campo Obrigatorio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;

                // Testar conexao antes de salvar
                bool sucesso = Conexao.TestarConexao(connectionString, out string mensagemErro);

                if (!sucesso)
                {
                    var result = MessageBox.Show(
                        $"Nao foi possivel conectar ao banco de dados:\n\n{mensagemErro}\n\n" +
                        "Deseja salvar mesmo assim?",
                        "Erro de Conexao",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (result == DialogResult.No)
                    {
                        Cursor = Cursors.Default;
                        return;
                    }
                }

                // Salvar connection string
                Conexao.SalvarConnectionString(connectionString);

                MessageBox.Show("Configuracao salva com sucesso!\n\n" +
                    "Voce pode alterar a connection string a qualquer momento pelo menu Ferramentas.",
                    "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Configurado = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar configuracao:\n{ex.Message}",
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void MostrarStatus(string mensagem, bool sucesso)
        {
            var lblStatus = this.Controls.Find("lblStatus", false)[0] as Label;
            if (lblStatus != null)
            {
                lblStatus.Text = mensagem;
                lblStatus.ForeColor = sucesso ? System.Drawing.Color.Green : System.Drawing.Color.Red;
            }
        }
    }
}
