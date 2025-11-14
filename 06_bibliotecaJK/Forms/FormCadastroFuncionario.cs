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

            // Form
            this.ClientSize = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Gerenciamento de Usuários - BibliotecaJK";
            this.BackColor = Color.FromArgb(245, 245, 250);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Header
            var pnlHeader = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(1000, 80),
                BackColor = Color.FromArgb(63, 81, 181)
            };

            var lblTitulo = new Label
            {
                Text = "👤 GERENCIAMENTO DE USUÁRIOS",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 20),
                Size = new Size(960, 40),
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnlHeader.Controls.Add(lblTitulo);
            this.Controls.Add(pnlHeader);

            // Panel de Cadastro
            var pnlCadastro = new Panel
            {
                Location = new Point(20, 100),
                Size = new Size(960, 220),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblCadastroTitulo = new Label
            {
                Text = "Dados do Usuário",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Location = new Point(15, 10),
                Size = new Size(930, 25),
                ForeColor = Color.FromArgb(63, 81, 181)
            };
            pnlCadastro.Controls.Add(lblCadastroTitulo);

            // Nome
            var lblNome = new Label
            {
                Text = "Nome Completo *",
                Font = new Font("Segoe UI", 9F),
                Location = new Point(15, 45),
                Size = new Size(200, 20)
            };
            pnlCadastro.Controls.Add(lblNome);

            txtNome = new TextBox
            {
                Font = new Font("Segoe UI", 10F),
                Location = new Point(15, 65),
                Size = new Size(300, 25)
            };
            pnlCadastro.Controls.Add(txtNome);

            // CPF
            var lblCPF = new Label
            {
                Text = "CPF *",
                Font = new Font("Segoe UI", 9F),
                Location = new Point(330, 45),
                Size = new Size(150, 20)
            };
            pnlCadastro.Controls.Add(lblCPF);

            txtCPF = new TextBox
            {
                Font = new Font("Segoe UI", 10F),
                Location = new Point(330, 65),
                Size = new Size(180, 25),
                MaxLength = 14
            };
            txtCPF.AllowOnlyNumbers();
            pnlCadastro.Controls.Add(txtCPF);

            // Cargo
            var lblCargo = new Label
            {
                Text = "Cargo",
                Font = new Font("Segoe UI", 9F),
                Location = new Point(525, 45),
                Size = new Size(200, 20)
            };
            pnlCadastro.Controls.Add(lblCargo);

            txtCargo = new TextBox
            {
                Font = new Font("Segoe UI", 10F),
                Location = new Point(525, 65),
                Size = new Size(250, 25)
            };
            pnlCadastro.Controls.Add(txtCargo);

            // Login
            var lblLogin = new Label
            {
                Text = "Login *",
                Font = new Font("Segoe UI", 9F),
                Location = new Point(15, 100),
                Size = new Size(200, 20)
            };
            pnlCadastro.Controls.Add(lblLogin);

            txtLogin = new TextBox
            {
                Font = new Font("Segoe UI", 10F),
                Location = new Point(15, 120),
                Size = new Size(200, 25)
            };
            pnlCadastro.Controls.Add(txtLogin);

            // Senha
            var lblSenha = new Label
            {
                Text = "Senha *",
                Font = new Font("Segoe UI", 9F),
                Location = new Point(230, 100),
                Size = new Size(200, 20)
            };
            pnlCadastro.Controls.Add(lblSenha);

            txtSenha = new TextBox
            {
                Font = new Font("Segoe UI", 10F),
                Location = new Point(230, 120),
                Size = new Size(200, 25),
                UseSystemPasswordChar = true
            };
            pnlCadastro.Controls.Add(txtSenha);

            // Perfil
            var lblPerfil = new Label
            {
                Text = "Perfil *",
                Font = new Font("Segoe UI", 9F),
                Location = new Point(445, 100),
                Size = new Size(150, 20)
            };
            pnlCadastro.Controls.Add(lblPerfil);

            cboPerfil = new ComboBox
            {
                Font = new Font("Segoe UI", 10F),
                Location = new Point(445, 120),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboPerfil.Items.AddRange(new object[] {
                Constants.PerfilFuncionario.ADMIN,
                Constants.PerfilFuncionario.BIBLIOTECARIO,
                Constants.PerfilFuncionario.OPERADOR
            });
            cboPerfil.SelectedIndex = 2; // Default: OPERADOR
            pnlCadastro.Controls.Add(cboPerfil);

            // Botões
            btnSalvar = new Button
            {
                Text = "💾 Salvar",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(15, 165),
                Size = new Size(130, 40),
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSalvar.FlatAppearance.BorderSize = 0;
            btnSalvar.Click += BtnSalvar_Click;
            pnlCadastro.Controls.Add(btnSalvar);

            btnNovo = new Button
            {
                Text = "📄 Novo",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(160, 165),
                Size = new Size(130, 40),
                BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnNovo.FlatAppearance.BorderSize = 0;
            btnNovo.Click += (s, e) => LimparCampos();
            pnlCadastro.Controls.Add(btnNovo);

            btnExcluir = new Button
            {
                Text = "🗑️ Excluir",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(305, 165),
                Size = new Size(130, 40),
                BackColor = Color.FromArgb(244, 67, 54),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnExcluir.FlatAppearance.BorderSize = 0;
            btnExcluir.Click += BtnExcluir_Click;
            pnlCadastro.Controls.Add(btnExcluir);

            this.Controls.Add(pnlCadastro);

            // Grid de Funcionários
            var lblGrid = new Label
            {
                Text = "Usuários Cadastrados",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Location = new Point(20, 335),
                Size = new Size(960, 25),
                ForeColor = Color.FromArgb(63, 81, 181)
            };
            this.Controls.Add(lblGrid);

            dgvFuncionarios = new DataGridView
            {
                Location = new Point(20, 365),
                Size = new Size(960, 315),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvFuncionarios.SelectionChanged += DgvFuncionarios_SelectionChanged;
            dgvFuncionarios.DoubleClick += (s, e) => CarregarFuncionarioSelecionado();
            this.Controls.Add(dgvFuncionarios);

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
                return;

            var row = dgvFuncionarios.SelectedRows[0];
            _idFuncionarioSelecionado = (int)row.Cells["Id"].Value;

            txtNome.Text = row.Cells["Nome"].Value?.ToString() ?? "";
            txtCPF.Text = row.Cells["CPF"].Value?.ToString() ?? "";
            txtCargo.Text = row.Cells["Cargo"].Value?.ToString() ?? "";
            txtLogin.Text = row.Cells["Login"].Value?.ToString() ?? "";
            cboPerfil.SelectedItem = row.Cells["Perfil"].Value?.ToString() ?? Constants.PerfilFuncionario.OPERADOR;
            txtSenha.Clear();
            txtSenha.PlaceholderText = "Deixe em branco para manter a senha atual";

            btnSalvar.Text = "💾 Atualizar";
            btnExcluir.Enabled = true;
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
