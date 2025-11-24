using BibliotecaJK.Utilitarios;
using BibliotecaJK.Modelos;
using BibliotecaJK.Servicos;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BibliotecaJK
{
    public partial class CadastrarFuncionario : Form
    {
        private readonly Funcionario _usuarioLogado;
        private readonly ServicoFuncionario _servicoFuncionario = new();
        private readonly BindingSource _bindingSource = new();
        private DataGridView? _grid;
        private Funcionario? _selecionado;

        public CadastrarFuncionario(Funcionario usuarioLogado)
        {
            _usuarioLogado = usuarioLogado;
            InitializeComponent();
            txt_senha.UseSystemPasswordChar = true;
            InicializarGrid();
            RegistrarEventos();
            CarregarFuncionarios();
            LimparCampos();
        }

        private void RegistrarEventos()
        {
            btn_salvar.Click += btn_salvar_Click;
            btn_editar.Click += btn_editar_Click;
            btn_excluir.Click += btn_excluir_Click;
            btn_limpar.Click += (_, _) => LimparCampos();
            btn_voltar.Click += (_, _) => Close();
        }

        private void InicializarGrid()
        {
            // Label explicativo acima do grid
            var lblGridHint = new Label
            {
                Text = "📋 Funcionários Cadastrados - Clique em uma linha para editar/excluir",
                Location = new Point(136, 175),
                Size = new Size(500, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.DarkSlateGray
            };
            Controls.Add(lblGridHint);

            _grid = new DataGridView
            {
                Location = new Point(136, 200),
                Size = new Size(695, 250),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = false,
                BackgroundColor = SystemColors.ControlLight,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            _grid.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { DataPropertyName = nameof(Funcionario.Nome), HeaderText = "Nome", Width = 180 },
                new DataGridViewTextBoxColumn { DataPropertyName = nameof(Funcionario.Login), HeaderText = "Login", Width = 120 },
                new DataGridViewTextBoxColumn { DataPropertyName = nameof(Funcionario.Perfil), HeaderText = "Perfil", Width = 100 }
            });

            _grid.DataSource = _bindingSource;
            _grid.SelectionChanged += Grid_SelectionChanged;
            Controls.Add(_grid);
        }

        private void CarregarFuncionarios()
        {
            try
            {
                _bindingSource.DataSource = _servicoFuncionario.Listar().ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar funcionários: {ex.Message}", "Funcionários", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Grid_SelectionChanged(object? sender, EventArgs e)
        {
            if (_grid?.CurrentRow?.DataBoundItem is Funcionario funcionario)
            {
                _selecionado = funcionario;
                PreencherFormulario(funcionario);
            }
        }

        private void PreencherFormulario(Funcionario funcionario)
        {
            txt_nomeFuncionario.Text = funcionario.Nome;
            txt_cpfFuncionario.Text = funcionario.CPF;
            txt_cargo.Text = funcionario.Cargo;
            txt_login.Text = funcionario.Login;
            txt_senha.Clear();
            cmb_perfil.SelectedItem = funcionario.Perfil;
        }

        private Funcionario LerFormulario()
        {
            return new Funcionario
            {
                Id = _selecionado?.Id ?? 0,
                Nome = txt_nomeFuncionario.Text.Trim(),
                CPF = txt_cpfFuncionario.Text.Trim(),
                Cargo = txt_cargo.Text?.Trim(),
                Login = txt_login.Text.Trim(),
                SenhaHash = txt_senha.Text.Trim(),
                Perfil = cmb_perfil.SelectedItem?.ToString() ?? "OPERADOR"
            };
        }

        private void btn_salvar_Click(object? sender, EventArgs e)
        {
            try
            {
                var funcionario = LerFormulario();
                _servicoFuncionario.Criar(funcionario, _usuarioLogado.Id);
                MessageBox.Show("Funcionário cadastrado com sucesso!", "Funcionários", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimparCampos();
                CarregarFuncionarios();
                txt_cpfFuncionario.Focus();
            }
            catch (ExcecaoValidacao ex)
            {
                MessageBox.Show(ex.Message, "Funcionários", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar funcionário: {ex.Message}", "Funcionários", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_editar_Click(object? sender, EventArgs e)
        {
            if (_selecionado == null)
            {
                MessageBox.Show("Selecione um funcionário.", "Funcionários", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var funcionario = LerFormulario();
                if (string.IsNullOrWhiteSpace(txt_senha.Text))
                {
                    funcionario.SenhaHash = _selecionado.SenhaHash;
                    _servicoFuncionario.Atualizar(funcionario, false, _usuarioLogado.Id);
                }
                else
                {
                    _servicoFuncionario.Atualizar(funcionario, true, _usuarioLogado.Id);
                }

                MessageBox.Show("Funcionário atualizado com sucesso!", "Funcionários", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimparCampos();
                CarregarFuncionarios();
            }
            catch (ExcecaoValidacao ex)
            {
                MessageBox.Show(ex.Message, "Funcionários", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar funcionário: {ex.Message}", "Funcionários", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_excluir_Click(object? sender, EventArgs e)
        {
            if (_selecionado == null)
            {
                MessageBox.Show("Selecione um funcionário.", "Funcionários", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmacao = MessageBox.Show($"Confirma a exclusão de {_selecionado.Nome}?", "Funcionários", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmacao != DialogResult.Yes)
            {
                return;
            }

            try
            {
                _servicoFuncionario.Remover(_selecionado.Id, _usuarioLogado.Id);
                MessageBox.Show("Funcionário removido com sucesso!", "Funcionários", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimparCampos();
                CarregarFuncionarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao remover funcionário: {ex.Message}", "Funcionários", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimparCampos()
        {
            txt_nomeFuncionario.Clear();
            txt_cpfFuncionario.Clear();
            txt_cargo.Clear();
            txt_login.Clear();
            txt_senha.Clear();
            cmb_perfil.SelectedItem = "OPERADOR";
            _selecionado = null;
            _grid?.ClearSelection();
        }
    }
}
