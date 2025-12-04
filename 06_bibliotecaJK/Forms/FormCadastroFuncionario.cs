using System;
using System.Drawing;
using System.Windows.Forms;
using BibliotecaJK.Model;
using BibliotecaJK.DAL;
using BibliotecaJK.BLL;
using BibliotecaJK.Components;

namespace BibliotecaJK.Forms
{
    /// <summary>
    /// Formulário para cadastro e gerenciamento de funcionários/usuários
    /// Apenas administradores podem acessar
    /// </summary>
    public partial class FormCadastroFuncionario : Form
    {
        private readonly Funcionario _funcionarioLogado;
        private readonly FuncionarioDAL _funcionarioDAL;
        private DataGridView dgvFuncionarios;
        private TextBox txtNome;
        private TextBox txtCPF;
        private TextBox txtCargo;
        private TextBox txtLogin;
        private TextBox txtSenha;
        private ComboBox cboPerfil;
        private Button btnSalvar;
        private Button btnNovo;
        private Button btnExcluir;
        private int? _idFuncionarioSelecionado = null;

        public FormCadastroFuncionario(Funcionario funcionarioLogado)
        {
            _funcionarioLogado = funcionarioLogado;
            _funcionarioDAL = new FuncionarioDAL();
            InitializeComponent();
            CarregarFuncionarios();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // FormCadastroFuncionario - Adaptive sizing para diferentes resoluções
            System.Drawing.Rectangle workingArea = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;

            // Calcular tamanho ideal (90% da tela, mas não menor que mínimo)
            int idealWidth = Math.Max(900, (int)(workingArea.Width * 0.9));
            int idealHeight = Math.Max(550, (int)(workingArea.Height * 0.9));

            // Limitar ao máximo disponível
            int formWidth = Math.Min(idealWidth, workingArea.Width - 50);
            int formHeight = Math.Min(idealHeight, workingArea.Height - 50);

            this.ClientSize = new System.Drawing.Size(formWidth, formHeight);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Gerenciamento de Usuários - BibliotecaJK";
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.MinimumSize = new System.Drawing.Size(900, 550);
            this.AutoScroll = true;

            // Título
            var lblTitulo = new Label
            {
                Text = "GERENCIAMENTO DE USUÁRIOS",
                Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                Location = new System.Drawing.Point(20, 15),
                Size = new System.Drawing.Size(formWidth - 40, 30),
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right
            };
            this.Controls.Add(lblTitulo);

            // Panel de Formulário - responsivo com Anchor
            var pnlForm = new Panel
            {
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(formWidth - 40, 200),
                BackColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right
            };

            // Nome
            pnlForm.Controls.Add(new Label
            {
                Text = "Nome Completo *",
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(120, 20)
            });
            txtNome = new TextBox
            {
                Location = new System.Drawing.Point(150, 18),
                Size = new System.Drawing.Size(350, 25),
                MaxLength = 100
            };
            pnlForm.Controls.Add(txtNome);

            // CPF
            pnlForm.Controls.Add(new Label
            {
                Text = "CPF *",
                Location = new System.Drawing.Point(520, 20),
                Size = new System.Drawing.Size(80, 20)
            });
            txtCPF = new TextBox
            {
                Location = new System.Drawing.Point(610, 18),
                Size = new System.Drawing.Size(150, 25),
                MaxLength = 14
            };
            txtCPF.AllowOnlyNumbers();
            pnlForm.Controls.Add(txtCPF);

            // Cargo
            pnlForm.Controls.Add(new Label
            {
                Text = "Cargo",
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(120, 20)
            });
            txtCargo = new TextBox
            {
                Location = new System.Drawing.Point(150, 58),
                Size = new System.Drawing.Size(350, 25),
                MaxLength = 100
            };
            pnlForm.Controls.Add(txtCargo);

            // Login
            pnlForm.Controls.Add(new Label
            {
                Text = "Login *",
                Location = new System.Drawing.Point(520, 60),
                Size = new System.Drawing.Size(80, 20)
            });
            txtLogin = new TextBox
            {
                Location = new System.Drawing.Point(610, 58),
                Size = new System.Drawing.Size(150, 25),
                MaxLength = 50
            };
            pnlForm.Controls.Add(txtLogin);

            // Senha
            pnlForm.Controls.Add(new Label
            {
                Text = "Senha *",
                Location = new System.Drawing.Point(20, 100),
                Size = new System.Drawing.Size(120, 20)
            });
            txtSenha = new TextBox
            {
                Location = new System.Drawing.Point(150, 98),
                Size = new System.Drawing.Size(350, 25),
                UseSystemPasswordChar = true
            };
            pnlForm.Controls.Add(txtSenha);

            // Perfil
            pnlForm.Controls.Add(new Label
            {
                Text = "Perfil *",
                Location = new System.Drawing.Point(520, 100),
                Size = new System.Drawing.Size(80, 20)
            });
            cboPerfil = new ComboBox
            {
                Location = new System.Drawing.Point(610, 98),
                Size = new System.Drawing.Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboPerfil.Items.AddRange(new object[] {
                Constants.PerfilFuncionario.ADMIN,
                Constants.PerfilFuncionario.BIBLIOTECARIO,
                Constants.PerfilFuncionario.OPERADOR
            });
            cboPerfil.SelectedIndex = 2; // Default: OPERADOR
            pnlForm.Controls.Add(cboPerfil);

            // Botões
            btnNovo = new Button
            {
                Text = "Novo",
                Location = new System.Drawing.Point(150, 145),
                Size = new System.Drawing.Size(100, 35),
                BackColor = System.Drawing.Color.MediumSeaGreen,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnNovo.FlatAppearance.BorderSize = 0;
            btnNovo.Click += (s, e) => LimparCampos();
            pnlForm.Controls.Add(btnNovo);

            btnSalvar = new Button
            {
                Text = "Salvar",
                Location = new System.Drawing.Point(260, 145),
                Size = new System.Drawing.Size(100, 35),
                BackColor = System.Drawing.Color.DarkSlateBlue,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSalvar.FlatAppearance.BorderSize = 0;
            btnSalvar.Click += BtnSalvar_Click;
            pnlForm.Controls.Add(btnSalvar);

            btnExcluir = new Button
            {
                Text = "Excluir",
                Location = new System.Drawing.Point(370, 145),
                Size = new System.Drawing.Size(100, 35),
                BackColor = System.Drawing.Color.Crimson,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnExcluir.FlatAppearance.BorderSize = 0;
            btnExcluir.Click += BtnExcluir_Click;
            pnlForm.Controls.Add(btnExcluir);

            this.Controls.Add(pnlForm);

            // Grid de Funcionários - responsivo com Anchor
            dgvFuncionarios = new DataGridView
            {
                Location = new System.Drawing.Point(20, 280),
                Size = new System.Drawing.Size(formWidth - 40, formHeight - 335),
                BackgroundColor = System.Drawing.Color.White,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom |
                         System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right
            };
            dgvFuncionarios.SelectionChanged += DgvFuncionarios_SelectionChanged;
            dgvFuncionarios.DoubleClick += (s, e) => CarregarFuncionarioSelecionado();

            this.Controls.Add(dgvFuncionarios);

            // FlowLayoutPanel para botões de ação - ancorado no canto inferior direito
            var flowButtons = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right,
                WrapContents = false,
                Padding = new System.Windows.Forms.Padding(0)
            };
            flowButtons.Location = new System.Drawing.Point(formWidth - 180, formHeight - 45);

            var btnEditar = new Button
            {
                Text = "Editar",
                Size = new System.Drawing.Size(90, 30),
                BackColor = System.Drawing.Color.SteelBlue,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new System.Windows.Forms.Padding(0, 0, 5, 0)
            };
            btnEditar.FlatAppearance.BorderSize = 0;
            btnEditar.Click += (s, e) => CarregarFuncionarioSelecionado();
            flowButtons.Controls.Add(btnEditar);

            var btnFechar = new Button
            {
                Text = "Fechar",
                Size = new System.Drawing.Size(70, 30),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new System.Windows.Forms.Padding(0)
            };
            btnFechar.FlatAppearance.BorderSize = 0;
            btnFechar.Click += (s, e) => this.Close();
            flowButtons.Controls.Add(btnFechar);

            this.Controls.Add(flowButtons);

            this.ResumeLayout(false);
        }

        private void CarregarFuncionarios()
        {
            try
            {
                var funcionarios = _funcionarioDAL.Listar();
                dgvFuncionarios.DataSource = null;
                dgvFuncionarios.DataSource = funcionarios;

                // Configurar colunas
                if (dgvFuncionarios.Columns.Count > 0)
                {
                    dgvFuncionarios.Columns["Id"].HeaderText = "ID";
                    dgvFuncionarios.Columns["Id"].Width = 60;
                    dgvFuncionarios.Columns["Nome"].HeaderText = "Nome";
                    dgvFuncionarios.Columns["CPF"].HeaderText = "CPF";
                    dgvFuncionarios.Columns["CPF"].Width = 120;
                    dgvFuncionarios.Columns["Cargo"].HeaderText = "Cargo";
                    dgvFuncionarios.Columns["Login"].HeaderText = "Login";
                    dgvFuncionarios.Columns["Login"].Width = 120;
                    dgvFuncionarios.Columns["Perfil"].HeaderText = "Perfil";
                    dgvFuncionarios.Columns["Perfil"].Width = 120;
                    dgvFuncionarios.Columns["PrimeiroLogin"].HeaderText = "1º Login";
                    dgvFuncionarios.Columns["PrimeiroLogin"].Width = 80;
                    dgvFuncionarios.Columns["SenhaHash"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar funcionários: {ex.Message}",
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvFuncionarios_SelectionChanged(object? sender, EventArgs e)
        {
            btnExcluir.Enabled = dgvFuncionarios.SelectedRows.Count > 0;
        }

        private void CarregarFuncionarioSelecionado()
        {
            if (dgvFuncionarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um funcionário para editar.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dgvFuncionarios.SelectedRows[0];
            _idFuncionarioSelecionado = (int)row.Cells["Id"].Value;

            txtNome.Text = row.Cells["Nome"].Value?.ToString() ?? "";
            txtCPF.Text = row.Cells["CPF"].Value?.ToString() ?? "";
            txtCargo.Text = row.Cells["Cargo"].Value?.ToString() ?? "";
            txtLogin.Text = row.Cells["Login"].Value?.ToString() ?? "";
            cboPerfil.SelectedItem = row.Cells["Perfil"].Value?.ToString() ?? Constants.PerfilFuncionario.OPERADOR;
            txtSenha.Clear();
            txtSenha.PlaceholderText = "Deixe em branco para manter a senha atual";

            btnExcluir.Enabled = true;
            txtNome.Focus();
        }

        private void BtnSalvar_Click(object? sender, EventArgs e)
        {
            try
            {
                // Validações
                if (string.IsNullOrWhiteSpace(txtNome.Text))
                {
                    MessageBox.Show("Nome é obrigatório.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNome.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtCPF.Text))
                {
                    MessageBox.Show("CPF é obrigatório.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCPF.Focus();
                    return;
                }

                if (!Validadores.ValidarCPF(txtCPF.Text))
                {
                    MessageBox.Show("CPF inválido.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCPF.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtLogin.Text))
                {
                    MessageBox.Show("Login é obrigatório.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtLogin.Focus();
                    return;
                }

                if (_idFuncionarioSelecionado == null && string.IsNullOrWhiteSpace(txtSenha.Text))
                {
                    MessageBox.Show("Senha é obrigatória para novos usuários.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSenha.Focus();
                    return;
                }

                if (!string.IsNullOrWhiteSpace(txtSenha.Text) && txtSenha.Text.Length < Constants.SENHA_MIN_LENGTH)
                {
                    MessageBox.Show($"Senha deve ter no mínimo {Constants.SENHA_MIN_LENGTH} caracteres.",
                        "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSenha.Focus();
                    return;
                }

                var funcionario = new Funcionario
                {
                    Nome = txtNome.Text.Trim(),
                    CPF = txtCPF.Text.Trim(),
                    Cargo = txtCargo.Text.Trim(),
                    Login = txtLogin.Text.Trim(),
                    Perfil = cboPerfil.SelectedItem?.ToString() ?? Constants.PerfilFuncionario.OPERADOR,
                    PrimeiroLogin = true
                };

                if (_idFuncionarioSelecionado == null)
                {
                    // Novo funcionário
                    funcionario.SenhaHash = txtSenha.Text; // Será hashado pelo trigger do banco
                    _funcionarioDAL.Inserir(funcionario);
                    ToastNotification.Success("Usuário cadastrado com sucesso!");
                }
                else
                {
                    // Atualizar existente
                    funcionario.Id = _idFuncionarioSelecionado.Value;

                    // Se senha foi informada, atualizar
                    if (!string.IsNullOrWhiteSpace(txtSenha.Text))
                    {
                        funcionario.SenhaHash = txtSenha.Text; // Será hashado pelo trigger
                    }
                    else
                    {
                        // Manter senha atual
                        var funcAtual = _funcionarioDAL.ObterPorId(_idFuncionarioSelecionado.Value);
                        funcionario.SenhaHash = funcAtual?.SenhaHash ?? "";
                    }

                    _funcionarioDAL.Atualizar(funcionario);
                    ToastNotification.Success("Usuário atualizado com sucesso!");
                }

                LimparCampos();
                CarregarFuncionarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExcluir_Click(object? sender, EventArgs e)
        {
            if (_idFuncionarioSelecionado == null)
                return;

            // Não permitir excluir a si mesmo
            if (_idFuncionarioSelecionado == _funcionarioLogado.Id)
            {
                MessageBox.Show("Você não pode excluir seu próprio usuário.",
                    "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Deseja realmente excluir o usuário '{txtNome.Text}'?\n\nEsta ação não pode ser desfeita.",
                "Confirmar Exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    _funcionarioDAL.Excluir(_idFuncionarioSelecionado.Value);
                    ToastNotification.Success("Usuário excluído com sucesso!");
                    LimparCampos();
                    CarregarFuncionarios();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao excluir: {ex.Message}", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LimparCampos()
        {
            _idFuncionarioSelecionado = null;
            txtNome.Clear();
            txtCPF.Clear();
            txtCargo.Clear();
            txtLogin.Clear();
            txtSenha.Clear();
            txtSenha.PlaceholderText = "";
            cboPerfil.SelectedIndex = 2; // OPERADOR
            btnSalvar.Text = "💾 Salvar";
            btnExcluir.Enabled = false;
            txtNome.Focus();
        }
    }
}
