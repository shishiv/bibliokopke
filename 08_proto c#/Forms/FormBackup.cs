using System;
using System.Windows.Forms;
using BibliotecaJK.Model;
using BibliotecaJK.BLL;

namespace BibliotecaJK.Forms
{
    /// <summary>
    /// Formulário de Configuração e Execução de Backups
    /// </summary>
    public partial class FormBackup : Form
    {
        private readonly Funcionario _funcionarioLogado;
        private BackupConfig _config;

        public FormBackup(Funcionario funcionario)
        {
            _funcionarioLogado = funcionario;
            _config = BackupConfig.Carregar() ?? new BackupConfig();

            InitializeComponent();
            CarregarConfiguracoes();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // FormBackup
            this.ClientSize = new System.Drawing.Size(700, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Configuração de Backup - BibliotecaJK";
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Título
            var lblTitulo = new Label
            {
                Text = "CONFIGURAÇÃO DE BACKUP AUTOMÁTICO",
                Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                Location = new System.Drawing.Point(20, 15),
                Size = new System.Drawing.Size(660, 30)
            };
            this.Controls.Add(lblTitulo);

            // Panel Conexão MySQL
            var pnlMySQL = new Panel
            {
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(660, 200),
                BackColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            pnlMySQL.Controls.Add(new Label
            {
                Text = "CREDENCIAIS DO MYSQL",
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(640, 20)
            });

            // Host
            pnlMySQL.Controls.Add(new Label { Text = "Host:", Location = new System.Drawing.Point(20, 45), Size = new System.Drawing.Size(100, 20) });
            txtHost = new TextBox { Location = new System.Drawing.Point(130, 43), Size = new System.Drawing.Size(200, 25) };
            pnlMySQL.Controls.Add(txtHost);

            // Porta
            pnlMySQL.Controls.Add(new Label { Text = "Porta:", Location = new System.Drawing.Point(350, 45), Size = new System.Drawing.Size(60, 20) });
            nudPorta = new NumericUpDown
            {
                Location = new System.Drawing.Point(420, 43),
                Size = new System.Drawing.Size(100, 25),
                Minimum = 1,
                Maximum = 65535,
                Value = 3306
            };
            pnlMySQL.Controls.Add(nudPorta);

            // Usuário
            pnlMySQL.Controls.Add(new Label { Text = "Usuário:", Location = new System.Drawing.Point(20, 80), Size = new System.Drawing.Size(100, 20) });
            txtUsuario = new TextBox { Location = new System.Drawing.Point(130, 78), Size = new System.Drawing.Size(200, 25) };
            pnlMySQL.Controls.Add(txtUsuario);

            // Senha
            pnlMySQL.Controls.Add(new Label { Text = "Senha:", Location = new System.Drawing.Point(350, 80), Size = new System.Drawing.Size(60, 20) });
            txtSenha = new TextBox
            {
                Location = new System.Drawing.Point(420, 78),
                Size = new System.Drawing.Size(200, 25),
                PasswordChar = '●'
            };
            pnlMySQL.Controls.Add(txtSenha);

            // Database
            pnlMySQL.Controls.Add(new Label { Text = "Database:", Location = new System.Drawing.Point(20, 115), Size = new System.Drawing.Size(100, 20) });
            txtDatabase = new TextBox { Location = new System.Drawing.Point(130, 113), Size = new System.Drawing.Size(200, 25) };
            pnlMySQL.Controls.Add(txtDatabase);

            // Botão Testar Conexão
            btnTestarConexao = new Button
            {
                Text = "🔌 Testar Conexão",
                Location = new System.Drawing.Point(350, 113),
                Size = new System.Drawing.Size(150, 30),
                BackColor = System.Drawing.Color.SteelBlue,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnTestarConexao.FlatAppearance.BorderSize = 0;
            btnTestarConexao.Click += BtnTestarConexao_Click;
            pnlMySQL.Controls.Add(btnTestarConexao);

            // Status da conexão
            lblStatusConexao = new Label
            {
                Text = "",
                Location = new System.Drawing.Point(20, 155),
                Size = new System.Drawing.Size(620, 30),
                ForeColor = System.Drawing.Color.Gray,
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic)
            };
            pnlMySQL.Controls.Add(lblStatusConexao);

            this.Controls.Add(pnlMySQL);

            // Panel Configuração de Backup
            var pnlBackup = new Panel
            {
                Location = new System.Drawing.Point(20, 275),
                Size = new System.Drawing.Size(660, 220),
                BackColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            pnlBackup.Controls.Add(new Label
            {
                Text = "CONFIGURAÇÕES DE BACKUP",
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(640, 20)
            });

            // Caminho de Backup
            pnlBackup.Controls.Add(new Label { Text = "Pasta destino:", Location = new System.Drawing.Point(20, 45), Size = new System.Drawing.Size(100, 20) });
            txtCaminhoBackup = new TextBox { Location = new System.Drawing.Point(130, 43), Size = new System.Drawing.Size(400, 25) };
            pnlBackup.Controls.Add(txtCaminhoBackup);

            var btnProcurar = new Button
            {
                Text = "...",
                Location = new System.Drawing.Point(540, 43),
                Size = new System.Drawing.Size(40, 25),
                Cursor = Cursors.Hand
            };
            btnProcurar.Click += BtnProcurar_Click;
            pnlBackup.Controls.Add(btnProcurar);

            // Horário do Backup
            pnlBackup.Controls.Add(new Label { Text = "Horário:", Location = new System.Drawing.Point(20, 80), Size = new System.Drawing.Size(100, 20) });
            txtHorario = new MaskedTextBox
            {
                Location = new System.Drawing.Point(130, 78),
                Size = new System.Drawing.Size(60, 25),
                Mask = "00:00",
                Text = "23:00"
            };
            pnlBackup.Controls.Add(txtHorario);

            pnlBackup.Controls.Add(new Label
            {
                Text = "(backup diário automático)",
                Location = new System.Drawing.Point(200, 80),
                Size = new System.Drawing.Size(200, 20),
                ForeColor = System.Drawing.Color.Gray,
                Font = new System.Drawing.Font("Segoe UI", 8F)
            });

            // Dias de Retenção
            pnlBackup.Controls.Add(new Label { Text = "Manter por:", Location = new System.Drawing.Point(20, 115), Size = new System.Drawing.Size(100, 20) });
            nudDiasRetencao = new NumericUpDown
            {
                Location = new System.Drawing.Point(130, 113),
                Size = new System.Drawing.Size(60, 25),
                Minimum = 7,
                Maximum = 365,
                Value = 30
            };
            pnlBackup.Controls.Add(nudDiasRetencao);

            pnlBackup.Controls.Add(new Label
            {
                Text = "dias (backups mais antigos serão excluídos)",
                Location = new System.Drawing.Point(200, 115),
                Size = new System.Drawing.Size(300, 20),
                ForeColor = System.Drawing.Color.Gray,
                Font = new System.Drawing.Font("Segoe UI", 8F)
            });

            // Checkbox Agendar
            chkAgendar = new CheckBox
            {
                Text = "Agendar backup automático diário",
                Location = new System.Drawing.Point(20, 150),
                Size = new System.Drawing.Size(400, 25),
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold)
            };
            pnlBackup.Controls.Add(chkAgendar);

            // Status do agendamento
            lblStatusAgendamento = new Label
            {
                Text = "",
                Location = new System.Drawing.Point(20, 180),
                Size = new System.Drawing.Size(620, 25),
                ForeColor = System.Drawing.Color.Green,
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic)
            };
            pnlBackup.Controls.Add(lblStatusAgendamento);

            this.Controls.Add(pnlBackup);

            // Botões
            btnSalvar = new Button
            {
                Text = "💾 Salvar Configurações",
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(20, 510),
                Size = new System.Drawing.Size(200, 40),
                BackColor = System.Drawing.Color.DarkSlateBlue,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSalvar.FlatAppearance.BorderSize = 0;
            btnSalvar.Click += BtnSalvar_Click;
            this.Controls.Add(btnSalvar);

            btnBackupAgora = new Button
            {
                Text = "⚡ Fazer Backup Agora",
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(230, 510),
                Size = new System.Drawing.Size(200, 40),
                BackColor = System.Drawing.Color.MediumSeaGreen,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnBackupAgora.FlatAppearance.BorderSize = 0;
            btnBackupAgora.Click += BtnBackupAgora_Click;
            this.Controls.Add(btnBackupAgora);

            var btnFechar = new Button
            {
                Text = "Fechar",
                Location = new System.Drawing.Point(620, 510),
                Size = new System.Drawing.Size(60, 40),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnFechar.FlatAppearance.BorderSize = 0;
            btnFechar.Click += (s, e) => this.Close();
            this.Controls.Add(btnFechar);

            this.ResumeLayout(false);
        }

        private TextBox txtHost = new TextBox();
        private NumericUpDown nudPorta = new NumericUpDown();
        private TextBox txtUsuario = new TextBox();
        private TextBox txtSenha = new TextBox();
        private TextBox txtDatabase = new TextBox();
        private Button btnTestarConexao = new Button();
        private Label lblStatusConexao = new Label();
        private TextBox txtCaminhoBackup = new TextBox();
        private MaskedTextBox txtHorario = new MaskedTextBox();
        private NumericUpDown nudDiasRetencao = new NumericUpDown();
        private CheckBox chkAgendar = new CheckBox();
        private Label lblStatusAgendamento = new Label();
        private Button btnSalvar = new Button();
        private Button btnBackupAgora = new Button();

        private void CarregarConfiguracoes()
        {
            txtHost.Text = _config.MySqlHost;
            nudPorta.Value = _config.MySqlPort;
            txtUsuario.Text = _config.MySqlUser;
            txtSenha.Text = _config.MySqlPassword;
            txtDatabase.Text = _config.MySqlDatabase;
            txtCaminhoBackup.Text = _config.BackupPath;
            txtHorario.Text = _config.HorarioBackup;
            nudDiasRetencao.Value = _config.DiasRetencao;
            chkAgendar.Checked = _config.BackupAgendado;

            VerificarStatusAgendamento();
        }

        private void BtnProcurar_Click(object? sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog
            {
                Description = "Selecione a pasta para salvar os backups",
                ShowNewFolderButton = true
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtCaminhoBackup.Text = dialog.SelectedPath;
            }
        }

        private void BtnTestarConexao_Click(object? sender, EventArgs e)
        {
            try
            {
                lblStatusConexao.Text = "Testando conexão...";
                lblStatusConexao.ForeColor = System.Drawing.Color.Gray;
                Application.DoEvents();

                AtualizarConfigDeFormulario();
                var backupService = new BackupService(_config);
                var resultado = backupService.TestarConexao();

                if (resultado.Sucesso)
                {
                    lblStatusConexao.Text = "✓ " + resultado.Mensagem;
                    lblStatusConexao.ForeColor = System.Drawing.Color.Green;
                    MessageBox.Show(resultado.Mensagem, "Sucesso",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    lblStatusConexao.Text = "✗ " + resultado.Mensagem;
                    lblStatusConexao.ForeColor = System.Drawing.Color.Red;
                    MessageBox.Show(resultado.Mensagem, "Erro de Conexão",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                lblStatusConexao.Text = "✗ Erro: " + ex.Message;
                lblStatusConexao.ForeColor = System.Drawing.Color.Red;
                MessageBox.Show($"Erro ao testar conexão: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSalvar_Click(object? sender, EventArgs e)
        {
            try
            {
                // Validações
                if (string.IsNullOrWhiteSpace(txtHost.Text))
                {
                    MessageBox.Show("Informe o host do MySQL.", "Atenção",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtHost.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtCaminhoBackup.Text))
                {
                    MessageBox.Show("Informe a pasta para salvar os backups.", "Atenção",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCaminhoBackup.Focus();
                    return;
                }

                // Atualizar config
                AtualizarConfigDeFormulario();

                // Salvar
                _config.Salvar();

                // Agendar se marcado
                if (chkAgendar.Checked)
                {
                    var backupService = new BackupService(_config);
                    var resultado = backupService.AgendarBackupAutomatico();

                    if (!resultado.Sucesso)
                    {
                        MessageBox.Show(resultado.Mensagem, "Atenção",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    MessageBox.Show(
                        "Configurações salvas com sucesso!\n\n" + resultado.Mensagem,
                        "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Cancelar agendamento se existir
                    var backupService = new BackupService(_config);
                    backupService.CancelarBackupAutomatico();

                    MessageBox.Show("Configurações salvas com sucesso!", "Sucesso",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                VerificarStatusAgendamento();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar configurações: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBackupAgora_Click(object? sender, EventArgs e)
        {
            try
            {
                AtualizarConfigDeFormulario();

                var backupService = new BackupService(_config);
                var resultado = backupService.ExecutarBackup();

                if (resultado.Sucesso)
                {
                    MessageBox.Show(resultado.Mensagem, "Backup Concluído",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(resultado.Mensagem, "Erro no Backup",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao executar backup: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AtualizarConfigDeFormulario()
        {
            _config.MySqlHost = txtHost.Text.Trim();
            _config.MySqlPort = (int)nudPorta.Value;
            _config.MySqlUser = txtUsuario.Text.Trim();
            _config.MySqlPassword = txtSenha.Text;
            _config.MySqlDatabase = txtDatabase.Text.Trim();
            _config.BackupPath = txtCaminhoBackup.Text.Trim();
            _config.HorarioBackup = txtHorario.Text;
            _config.DiasRetencao = (int)nudDiasRetencao.Value;
            _config.BackupAgendado = chkAgendar.Checked;
        }

        private void VerificarStatusAgendamento()
        {
            var backupService = new BackupService(_config);
            if (backupService.VerificarSeEstaAgendado())
            {
                lblStatusAgendamento.Text = $"✓ Backup automático agendado para {_config.HorarioBackup} (diariamente)";
                lblStatusAgendamento.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblStatusAgendamento.Text = "⚠ Backup automático não está agendado";
                lblStatusAgendamento.ForeColor = System.Drawing.Color.OrangeRed;
            }
        }
    }
}
