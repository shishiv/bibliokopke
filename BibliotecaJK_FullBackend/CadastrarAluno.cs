using BibliotecaJK.Utilitarios;
using BibliotecaJK.Modelos;
using BibliotecaJK.Servicos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BibliotecaJK
{
    public partial class CadastrarAluno : Form
    {
        private readonly Funcionario _usuarioLogado;
        private readonly ServicoAluno _servicoAluno = new();
        private readonly BindingSource _bindingSource = new();
        private DataGridView? _grid;
        private Aluno? _selecionado;

        public CadastrarAluno(Funcionario usuarioLogado)
        {
            _usuarioLogado = usuarioLogado;
            InitializeComponent();
            InicializarGrid();
            RegistrarEventos();
            CarregarAlunos();
            LimparCampos();
        }

        private void RegistrarEventos()
        {
            btn_salvar.Click += btn_salvar_Click;
            btn_editar.Click += btn_editar_Click;
            btn_excluir.Click += btn_excluir_Click;
            btn_limpar.Click += btn_limpar_Click;
            btn_voltar.Click += (_, _) => Close();
        }

        private void InicializarGrid()
        {
            // Label explicativo acima do grid
            var lblGridHint = new Label
            {
                Text = "📋 Alunos Cadastrados - Clique em uma linha para editar/excluir",
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
                new DataGridViewTextBoxColumn { DataPropertyName = nameof(Aluno.Matricula), HeaderText = "Matrícula", Width = 120 },
                new DataGridViewTextBoxColumn { DataPropertyName = nameof(Aluno.Nome), HeaderText = "Nome", Width = 150 },
                new DataGridViewTextBoxColumn { DataPropertyName = nameof(Aluno.CPF), HeaderText = "CPF", Width = 120 },
                new DataGridViewTextBoxColumn { DataPropertyName = nameof(Aluno.Turma), HeaderText = "Turma", Width = 80 }
            });

            _grid.DataSource = _bindingSource;
            _grid.SelectionChanged += Grid_SelectionChanged;
            Controls.Add(_grid);
        }

        private void CarregarAlunos()
        {
            try
            {
                var alunos = _servicoAluno.Listar().ToList();
                _bindingSource.DataSource = alunos;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar alunos: {ex.Message}", "Alunos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Grid_SelectionChanged(object? sender, EventArgs e)
        {
            if (_grid?.CurrentRow?.DataBoundItem is Aluno aluno)
            {
                _selecionado = aluno;
                PreencherFormulario(aluno);
            }
        }

        private void PreencherFormulario(Aluno aluno)
        {
            txt_matricula.Text = aluno.Matricula;
            txt_nomeAluno.Text = aluno.Nome;
            txt_CursoTurma.Text = aluno.Turma;
            txt_telefone.Text = aluno.Telefone;
            txt_email.Text = aluno.Email;
            txt_cpf.Text = aluno.CPF;
        }

        private Aluno LerFormulario()
        {
            return new Aluno
            {
                Id = _selecionado?.Id ?? 0,
                Matricula = txt_matricula.Text.Trim(),
                Nome = txt_nomeAluno.Text.Trim(),
                Turma = txt_CursoTurma.Text?.Trim(),
                Telefone = txt_telefone.Text?.Trim(),
                Email = txt_email.Text?.Trim(),
                CPF = txt_cpf.Text.Trim()
            };
        }

        private void btn_salvar_Click(object? sender, EventArgs e)
        {
            try
            {
                var aluno = LerFormulario();
                _servicoAluno.Criar(aluno, _usuarioLogado.Id);
                MessageBox.Show("Aluno cadastrado com sucesso!", "Alunos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimparCampos();
                CarregarAlunos();
                txt_matricula.Focus();
            }
            catch (ExcecaoValidacao ex)
            {
                MessageBox.Show(ex.Message, "Alunos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar aluno: {ex.Message}", "Alunos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_editar_Click(object? sender, EventArgs e)
        {
            if (_selecionado == null)
            {
                MessageBox.Show("Selecione um aluno na lista.", "Alunos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var aluno = LerFormulario();
                aluno.Id = _selecionado.Id;
                _servicoAluno.Atualizar(aluno, _usuarioLogado.Id);
                MessageBox.Show("Aluno atualizado com sucesso!", "Alunos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimparCampos();
                CarregarAlunos();
            }
            catch (ExcecaoValidacao ex)
            {
                MessageBox.Show(ex.Message, "Alunos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar aluno: {ex.Message}", "Alunos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_excluir_Click(object? sender, EventArgs e)
        {
            if (_selecionado == null)
            {
                MessageBox.Show("Selecione um aluno na lista.", "Alunos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmacao = MessageBox.Show($"Confirma a exclusão do aluno {_selecionado.Nome}?", "Alunos", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmacao != DialogResult.Yes)
            {
                return;
            }

            try
            {
                _servicoAluno.Remover(_selecionado.Id, _usuarioLogado.Id);
                MessageBox.Show("Aluno removido com sucesso!", "Alunos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimparCampos();
                CarregarAlunos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao remover aluno: {ex.Message}", "Alunos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_limpar_Click(object? sender, EventArgs e)
        {
            LimparCampos();
        }

        private void LimparCampos()
        {
            txt_matricula.Clear();
            txt_nomeAluno.Clear();
            txt_CursoTurma.Clear();
            txt_telefone.Clear();
            txt_email.Clear();
            txt_cpf.Clear();
            _selecionado = null;
            _grid?.ClearSelection();
        }
    }
}
