using System;
using System.Windows.Forms;

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
            this.Size = new System.Drawing.Size(700, 550);

            // Label de titulo
            var lblTitulo = new Label
            {
                Text = "Bem-vindo ao BibliotecaJK!",
                Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(640, 30),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblTitulo);

            // Label de descricao
            var lblDescricao = new Label
            {
                Text = "Configure a conexao com o banco de dados Supabase ou PostgreSQL.\n" +
                       "Voce pode obter a connection string no painel do Supabase em:\n" +
                       "Settings > Database > Connection String (URI)",
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(640, 60),
                TextAlign = System.Drawing.ContentAlignment.TopLeft
            };
            this.Controls.Add(lblDescricao);

            // Label Connection String
            var lblConnString = new Label
            {
                Text = "Connection String (PostgreSQL/Supabase):",
                Location = new System.Drawing.Point(20, 130),
                Size = new System.Drawing.Size(300, 20)
            };
            this.Controls.Add(lblConnString);

            // TextBox Connection String (multiline para conexoes longas)
            var txtConnectionString = new TextBox
            {
                Name = "txtConnectionString",
                Location = new System.Drawing.Point(20, 155),
                Size = new System.Drawing.Size(640, 80),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new System.Drawing.Font("Consolas", 9F)
            };
            this.Controls.Add(txtConnectionString);

            // Label de exemplo
            var lblExemplo = new Label
            {
                Text = "Exemplo Supabase URI (recomendado):\n" +
                       "postgresql://postgres.xxxxx:[SUA-SENHA]@aws-1-sa-east-1.pooler.supabase.com:5432/postgres\n\n" +
                       "Exemplo Supabase Tradicional:\n" +
                       "Host=db.xxxxx.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=sua_senha\n\n" +
                       "Exemplo Local:\n" +
                       "Host=localhost;Port=5432;Database=bibliokopke;Username=postgres;Password=sua_senha",
                Location = new System.Drawing.Point(20, 245),
                Size = new System.Drawing.Size(640, 110),
                ForeColor = System.Drawing.Color.Gray,
                Font = new System.Drawing.Font("Consolas", 7.5F)
            };
            this.Controls.Add(lblExemplo);

            // Botao Testar Conexao
            var btnTestar = new Button
            {
                Text = "Testar Conexao",
                Location = new System.Drawing.Point(20, 370),
                Size = new System.Drawing.Size(150, 35),
                BackColor = System.Drawing.Color.FromArgb(0, 120, 215),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnTestar.Click += (s, e) => TestarConexao(txtConnectionString.Text);
            this.Controls.Add(btnTestar);

            // Label de status
            var lblStatus = new Label
            {
                Name = "lblStatus",
                Text = "",
                Location = new System.Drawing.Point(180, 370),
                Size = new System.Drawing.Size(480, 35),
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold)
            };
            this.Controls.Add(lblStatus);

            // Botao Salvar
            var btnSalvar = new Button
            {
                Text = "Salvar e Continuar",
                Location = new System.Drawing.Point(450, 460),
                Size = new System.Drawing.Size(200, 40),
                BackColor = System.Drawing.Color.FromArgb(0, 150, 0),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold)
            };
            btnSalvar.Click += (s, e) => SalvarConfiguracao(txtConnectionString.Text);
            this.Controls.Add(btnSalvar);

            // Botao Cancelar
            var btnCancelar = new Button
            {
                Text = "Cancelar",
                Location = new System.Drawing.Point(330, 460),
                Size = new System.Drawing.Size(100, 40),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancelar.Click += (s, e) =>
            {
                Configurado = false;
                this.Close();
            };
            this.Controls.Add(btnCancelar);

            // Label de ajuda
            var lblAjuda = new Label
            {
                Text = "Precisa de ajuda? Consulte o manual de instalacao ou visite:\n" +
                       "https://supabase.com/docs",
                Location = new System.Drawing.Point(20, 390),
                Size = new System.Drawing.Size(640, 40),
                ForeColor = System.Drawing.Color.Blue,
                Font = new System.Drawing.Font("Segoe UI", 8F),
                Cursor = Cursors.Hand
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
