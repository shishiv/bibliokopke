using BibliotecaJK.Utilitarios;
using BibliotecaJK.Modelos;
using BibliotecaJK.Servicos;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BibliotecaJK
{
    public partial class CadastrarLivro : Form
    {
        private readonly Funcionario _usuarioLogado;
        private readonly ServicoLivro _servicoLivro = new();
        private readonly BindingSource _bindingSource = new();
        private DataGridView? _grid;
        private Livro? _selecionado;

        public CadastrarLivro(Funcionario usuarioLogado)
        {
            _usuarioLogado = usuarioLogado;
            InitializeComponent();
            InicializarGrid();
            RegistrarEventos();
            CarregarLivros();
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
                Text = "📋 Livros Cadastrados - Clique em uma linha para editar/excluir",
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
                new DataGridViewTextBoxColumn { DataPropertyName = nameof(Livro.Titulo), HeaderText = "Título", Width = 180 },
                new DataGridViewTextBoxColumn { DataPropertyName = nameof(Livro.Autor), HeaderText = "Autor", Width = 120 },
                new DataGridViewTextBoxColumn { DataPropertyName = nameof(Livro.ISBN), HeaderText = "ISBN", Width = 120 },
                new DataGridViewTextBoxColumn { DataPropertyName = nameof(Livro.QuantidadeDisponivel), HeaderText = "Disponível", Width = 80 }
            });

            _grid.DataSource = _bindingSource;
            _grid.SelectionChanged += Grid_SelectionChanged;
            Controls.Add(_grid);
        }

        private void CarregarLivros()
        {
            try
            {
                _bindingSource.DataSource = _servicoLivro.Listar().ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar livros: {ex.Message}", "Livros", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Grid_SelectionChanged(object? sender, EventArgs e)
        {
            if (_grid?.CurrentRow?.DataBoundItem is Livro livro)
            {
                _selecionado = livro;
                PreencherFormulario(livro);
            }
        }

        private void PreencherFormulario(Livro livro)
        {
            txt_titulo.Text = livro.Titulo;
            txt_autor.Text = livro.Autor;
            txt_editora.Text = livro.Editora;
            txt_codigolivro.Text = livro.ISBN;
            txt_ano.Text = livro.AnoPublicacao?.ToString();
            txt_estoque.Text = livro.QuantidadeTotal.ToString();
            txt_localizacao.Text = livro.Localizacao;
        }

        private Livro LerFormulario()
        {
            int.TryParse(txt_ano.Text, out var ano);
            int.TryParse(txt_estoque.Text, out var estoque);

            return new Livro
            {
                Id = _selecionado?.Id ?? 0,
                Titulo = txt_titulo.Text.Trim(),
                Autor = txt_autor.Text?.Trim(),
                Editora = txt_editora.Text?.Trim(),
                ISBN = txt_codigolivro.Text.Trim(),
                AnoPublicacao = ano > 0 ? ano : null,
                QuantidadeTotal = estoque > 0 ? estoque : 0,
                QuantidadeDisponivel = _selecionado?.QuantidadeDisponivel ?? estoque,
                Localizacao = txt_localizacao.Text?.Trim()
            };
        }

        private void btn_salvar_Click(object? sender, EventArgs e)
        {
            try
            {
                var livro = LerFormulario();
                _servicoLivro.Criar(livro, _usuarioLogado.Id);
                MessageBox.Show("Livro cadastrado com sucesso!", "Livros", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimparCampos();
                CarregarLivros();
                txt_titulo.Focus();
            }
            catch (ExcecaoValidacao ex)
            {
                MessageBox.Show(ex.Message, "Livros", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar livro: {ex.Message}", "Livros", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_editar_Click(object? sender, EventArgs e)
        {
            if (_selecionado == null)
            {
                MessageBox.Show("Selecione um livro.", "Livros", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var livro = LerFormulario();
                livro.Id = _selecionado.Id;
                livro.QuantidadeDisponivel = _selecionado.QuantidadeDisponivel;
                _servicoLivro.Atualizar(livro, _usuarioLogado.Id);
                MessageBox.Show("Livro atualizado com sucesso!", "Livros", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimparCampos();
                CarregarLivros();
            }
            catch (ExcecaoValidacao ex)
            {
                MessageBox.Show(ex.Message, "Livros", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar livro: {ex.Message}", "Livros", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_excluir_Click(object? sender, EventArgs e)
        {
            if (_selecionado == null)
            {
                MessageBox.Show("Selecione um livro.", "Livros", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmacao = MessageBox.Show($"Confirma a exclusão de {_selecionado.Titulo}?", "Livros", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmacao != DialogResult.Yes)
            {
                return;
            }

            try
            {
                _servicoLivro.Remover(_selecionado.Id, _usuarioLogado.Id);
                MessageBox.Show("Livro removido com sucesso!", "Livros", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimparCampos();
                CarregarLivros();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao remover livro: {ex.Message}", "Livros", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimparCampos()
        {
            txt_titulo.Clear();
            txt_autor.Clear();
            txt_editora.Clear();
            txt_codigolivro.Clear();
            txt_ano.Clear();
            txt_estoque.Clear();
            txt_localizacao.Clear();
            _selecionado = null;
            _grid?.ClearSelection();
        }
    }
}
