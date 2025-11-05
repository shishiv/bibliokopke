using System;
using System.IO;
using System.Windows.Forms;
using System.Text;
using Npgsql;

namespace BibliotecaJK.Forms
{
    /// <summary>
    /// Formulário wizard para setup inicial do banco de dados
    /// Verifica se as tabelas existem e oferece executar o schema
    /// </summary>
    public partial class FormSetupInicial : Form
    {
        private readonly string _connectionString;
        private bool _tabelasExistem = false;

        private TextBox txtLog = new TextBox();
        private Button btnVerificar = new Button();
        private Button btnExecutarSchema = new Button();
        private Button btnContinuar = new Button();
        private ProgressBar progressBar = new ProgressBar();
        private Label lblStatus = new Label();

        public FormSetupInicial(string connectionString)
        {
            _connectionString = connectionString;
            InitializeComponent();
            VerificarTabelas();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // FormSetupInicial
            this.ClientSize = new System.Drawing.Size(700, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Setup Inicial - BibliotecaJK";
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Título
            var lblTitulo = new Label
            {
                Text = "🛠️ SETUP INICIAL DO BANCO DE DADOS",
                Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(660, 30),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblTitulo);

            // Mensagem
            var lblMensagem = new Label
            {
                Text = "Este assistente irá verificar se o banco de dados está configurado corretamente\n" +
                       "e criará as tabelas necessárias caso não existam.",
                Font = new System.Drawing.Font("Segoe UI", 9F),
                ForeColor = System.Drawing.Color.Gray,
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(660, 40),
                TextAlign = System.Drawing.ContentAlignment.TopCenter
            };
            this.Controls.Add(lblMensagem);

            // Status
            lblStatus = new Label
            {
                Text = "Verificando banco de dados...",
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkOrange,
                Location = new System.Drawing.Point(20, 110),
                Size = new System.Drawing.Size(660, 25)
            };
            this.Controls.Add(lblStatus);

            // Progress Bar
            progressBar = new ProgressBar
            {
                Location = new System.Drawing.Point(20, 140),
                Size = new System.Drawing.Size(660, 15),
                Style = ProgressBarStyle.Marquee
            };
            this.Controls.Add(progressBar);

            // Log de execução
            txtLog = new TextBox
            {
                Location = new System.Drawing.Point(20, 170),
                Size = new System.Drawing.Size(660, 350),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true,
                Font = new System.Drawing.Font("Consolas", 9F),
                BackColor = System.Drawing.Color.White,
                ForeColor = System.Drawing.Color.Black
            };
            this.Controls.Add(txtLog);

            // Botões
            btnVerificar = new Button
            {
                Text = "🔍 Verificar Novamente",
                Font = new System.Drawing.Font("Segoe UI", 9F),
                Location = new System.Drawing.Point(20, 535),
                Size = new System.Drawing.Size(180, 40),
                BackColor = System.Drawing.Color.SteelBlue,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnVerificar.FlatAppearance.BorderSize = 0;
            btnVerificar.Click += BtnVerificar_Click;
            this.Controls.Add(btnVerificar);

            btnExecutarSchema = new Button
            {
                Text = "⚡ Executar Schema SQL",
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(210, 535),
                Size = new System.Drawing.Size(180, 40),
                BackColor = System.Drawing.Color.DarkOrange,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnExecutarSchema.FlatAppearance.BorderSize = 0;
            btnExecutarSchema.Click += BtnExecutarSchema_Click;
            this.Controls.Add(btnExecutarSchema);

            btnContinuar = new Button
            {
                Text = "✓ Continuar",
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(520, 535),
                Size = new System.Drawing.Size(160, 40),
                BackColor = System.Drawing.Color.DarkGreen,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnContinuar.FlatAppearance.BorderSize = 0;
            btnContinuar.Click += BtnContinuar_Click;
            this.Controls.Add(btnContinuar);

            this.ResumeLayout(false);
        }

        private void AdicionarLog(string mensagem, bool erro = false)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            string prefix = erro ? "[ERRO]" : "[INFO]";
            txtLog.AppendText($"{timestamp} {prefix} {mensagem}\r\n");
        }

        private async void VerificarTabelas()
        {
            AdicionarLog("Iniciando verificação do banco de dados...");

            try
            {
                using var conn = new NpgsqlConnection(_connectionString);
                await conn.OpenAsync();

                AdicionarLog("✓ Conexão estabelecida com sucesso");

                // Verificar se as tabelas principais existem
                string[] tabelasNecessarias = {
                    "aluno", "funcionario", "livro",
                    "emprestimo", "reserva", "log_acao"
                };

                int tabelasEncontradas = 0;

                foreach (var tabela in tabelasNecessarias)
                {
                    string sql = @"
                        SELECT EXISTS (
                            SELECT FROM information_schema.tables
                            WHERE table_schema = 'public'
                            AND table_name = @tabela
                        );";

                    using var cmd = new NpgsqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@tabela", tabela.ToLower());

                    bool existe = (bool)(await cmd.ExecuteScalarAsync() ?? false);

                    if (existe)
                    {
                        AdicionarLog($"✓ Tabela '{tabela}' encontrada");
                        tabelasEncontradas++;
                    }
                    else
                    {
                        AdicionarLog($"✗ Tabela '{tabela}' NÃO encontrada", true);
                    }
                }

                conn.Close();

                _tabelasExistem = (tabelasEncontradas == tabelasNecessarias.Length);

                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Value = 100;

                if (_tabelasExistem)
                {
                    lblStatus.Text = "✓ Banco de dados configurado corretamente!";
                    lblStatus.ForeColor = System.Drawing.Color.DarkGreen;
                    AdicionarLog("==============================================");
                    AdicionarLog("✓ SUCESSO: Todas as tabelas estão criadas!");
                    AdicionarLog("==============================================");
                    btnContinuar.Enabled = true;
                }
                else
                {
                    lblStatus.Text = $"⚠ Faltam {tabelasNecessarias.Length - tabelasEncontradas} tabela(s)";
                    lblStatus.ForeColor = System.Drawing.Color.DarkOrange;
                    AdicionarLog("==============================================");
                    AdicionarLog($"⚠ ATENÇÃO: {tabelasNecessarias.Length - tabelasEncontradas} tabela(s) faltando!");
                    AdicionarLog("Clique em 'Executar Schema SQL' para criar as tabelas.");
                    AdicionarLog("==============================================");
                    btnExecutarSchema.Enabled = true;
                }

                btnVerificar.Enabled = true;
            }
            catch (Exception ex)
            {
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Value = 0;
                lblStatus.Text = "✗ Erro ao verificar banco de dados";
                lblStatus.ForeColor = System.Drawing.Color.DarkRed;
                AdicionarLog($"ERRO: {ex.Message}", true);
                btnVerificar.Enabled = true;
            }
        }

        private void BtnVerificar_Click(object? sender, EventArgs e)
        {
            txtLog.Clear();
            btnVerificar.Enabled = false;
            btnExecutarSchema.Enabled = false;
            btnContinuar.Enabled = false;
            progressBar.Style = ProgressBarStyle.Marquee;
            lblStatus.Text = "Verificando banco de dados...";
            lblStatus.ForeColor = System.Drawing.Color.DarkOrange;
            VerificarTabelas();
        }

        private async void BtnExecutarSchema_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Este processo irá criar todas as tabelas necessárias no banco de dados.\n\n" +
                "ATENÇÃO: Se as tabelas já existirem, elas NÃO serão recriadas (CREATE TABLE IF NOT EXISTS).\n\n" +
                "Deseja continuar?",
                "Confirmar Execução do Schema",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            btnExecutarSchema.Enabled = false;
            btnVerificar.Enabled = false;
            progressBar.Style = ProgressBarStyle.Marquee;
            lblStatus.Text = "Executando schema SQL...";
            lblStatus.ForeColor = System.Drawing.Color.DarkOrange;

            AdicionarLog("==============================================");
            AdicionarLog("Iniciando execução do schema SQL...");

            try
            {
                // Localizar o arquivo schema-postgresql.sql
                string schemaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "schema-postgresql.sql");

                if (!File.Exists(schemaPath))
                {
                    // Tentar uma pasta acima
                    schemaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "schema-postgresql.sql");
                }

                if (!File.Exists(schemaPath))
                {
                    throw new FileNotFoundException("Arquivo 'schema-postgresql.sql' não encontrado.");
                }

                AdicionarLog($"✓ Arquivo schema encontrado: {schemaPath}");

                // Ler o conteúdo do arquivo
                string schemaContent = await File.ReadAllTextAsync(schemaPath);
                AdicionarLog($"✓ Schema carregado ({schemaContent.Length} caracteres)");

                using var conn = new NpgsqlConnection(_connectionString);
                await conn.OpenAsync();

                AdicionarLog("✓ Conexão aberta");
                AdicionarLog("Executando comandos SQL...");

                // Executar o schema completo
                using var cmd = new NpgsqlCommand(schemaContent, conn);
                cmd.CommandTimeout = 120; // 2 minutos

                await cmd.ExecuteNonQueryAsync();

                conn.Close();

                AdicionarLog("==============================================");
                AdicionarLog("✓ SUCESSO: Schema executado com sucesso!");
                AdicionarLog("==============================================");

                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Value = 100;
                lblStatus.Text = "✓ Schema executado com sucesso!";
                lblStatus.ForeColor = System.Drawing.Color.DarkGreen;

                MessageBox.Show(
                    "Schema executado com sucesso!\n\n" +
                    "Todas as tabelas foram criadas.\n" +
                    "Clique em 'Verificar Novamente' para confirmar.",
                    "Sucesso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                btnVerificar.Enabled = true;
            }
            catch (Exception ex)
            {
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Value = 0;
                lblStatus.Text = "✗ Erro ao executar schema";
                lblStatus.ForeColor = System.Drawing.Color.DarkRed;
                AdicionarLog("==============================================");
                AdicionarLog($"✗ ERRO: {ex.Message}", true);
                AdicionarLog("==============================================");

                MessageBox.Show(
                    $"Erro ao executar schema:\n\n{ex.Message}\n\n" +
                    "Verifique se o arquivo 'schema-postgresql.sql' está na pasta do executável.",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                btnExecutarSchema.Enabled = true;
                btnVerificar.Enabled = true;
            }
        }

        private void BtnContinuar_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
