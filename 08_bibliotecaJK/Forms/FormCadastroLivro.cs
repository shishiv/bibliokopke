using System;
using System.Linq;
using System.Windows.Forms;
using BibliotecaJK.Model;
using BibliotecaJK.BLL;
using BibliotecaJK.DAL;

namespace BibliotecaJK.Forms
{
    /// <summary>
    /// Formulário de Cadastro de Livros (CRUD Completo)
    /// </summary>
    public partial class FormCadastroLivro : Form
    {
        private readonly Funcionario _funcionarioLogado;
        private readonly LivroService _livroService;
        private readonly LivroDAL _livroDAL;
        private int? _livroEmEdicaoId;

        public FormCadastroLivro(Funcionario funcionario)
        {
            _funcionarioLogado = funcionario;
            _livroService = new LivroService();
            _livroDAL = new LivroDAL();

            InitializeComponent();
            CarregarGrid();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // FormCadastroLivro
            this.ClientSize = new System.Drawing.Size(1100, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Cadastro de Livros";
            this.BackColor = System.Drawing.Color.WhiteSmoke;

            // Título
            var lblTitulo = new Label
            {
                Text = "CADASTRO DE LIVROS",
                Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                Location = new System.Drawing.Point(20, 15),
                Size = new System.Drawing.Size(1060, 30)
            };
            this.Controls.Add(lblTitulo);

            // Panel de Formulário
            var pnlForm = new Panel
            {
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(1060, 200),
                BackColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Título
            pnlForm.Controls.Add(new Label
            {
                Text = "Título *",
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(80, 20)
            });
            txtTitulo = new TextBox
            {
                Location = new System.Drawing.Point(110, 18),
                Size = new System.Drawing.Size(400, 25),
                MaxLength = 200
            };
            pnlForm.Controls.Add(txtTitulo);

            // Autor
            pnlForm.Controls.Add(new Label
            {
                Text = "Autor",
                Location = new System.Drawing.Point(530, 20),
                Size = new System.Drawing.Size(80, 20)
            });
            txtAutor = new TextBox
            {
                Location = new System.Drawing.Point(620, 18),
                Size = new System.Drawing.Size(400, 25),
                MaxLength = 100
            };
            pnlForm.Controls.Add(txtAutor);

            // ISBN
            pnlForm.Controls.Add(new Label
            {
                Text = "ISBN",
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(80, 20)
            });
            txtISBN = new TextBox
            {
                Location = new System.Drawing.Point(110, 58),
                Size = new System.Drawing.Size(180, 25),
                MaxLength = 20
            };
            pnlForm.Controls.Add(txtISBN);

            // Editora
            pnlForm.Controls.Add(new Label
            {
                Text = "Editora",
                Location = new System.Drawing.Point(310, 60),
                Size = new System.Drawing.Size(80, 20)
            });
            txtEditora = new TextBox
            {
                Location = new System.Drawing.Point(400, 58),
                Size = new System.Drawing.Size(250, 25),
                MaxLength = 100
            };
            pnlForm.Controls.Add(txtEditora);

            // Ano
            pnlForm.Controls.Add(new Label
            {
                Text = "Ano",
                Location = new System.Drawing.Point(670, 60),
                Size = new System.Drawing.Size(50, 20)
            });
            nudAno = new NumericUpDown
            {
                Location = new System.Drawing.Point(730, 58),
                Size = new System.Drawing.Size(80, 25),
                Minimum = 1000,
                Maximum = 9999,
                Value = DateTime.Now.Year
            };
            pnlForm.Controls.Add(nudAno);

            // Categoria
            pnlForm.Controls.Add(new Label
            {
                Text = "Categoria",
                Location = new System.Drawing.Point(20, 100),
                Size = new System.Drawing.Size(80, 20)
            });
            txtCategoria = new TextBox
            {
                Location = new System.Drawing.Point(110, 98),
                Size = new System.Drawing.Size(200, 25),
                MaxLength = 50
            };
            pnlForm.Controls.Add(txtCategoria);

            // Quantidade Total
            pnlForm.Controls.Add(new Label
            {
                Text = "Qtd. Total *",
                Location = new System.Drawing.Point(330, 100),
                Size = new System.Drawing.Size(80, 20)
            });
            nudQuantidadeTotal = new NumericUpDown
            {
                Location = new System.Drawing.Point(420, 98),
                Size = new System.Drawing.Size(80, 25),
                Minimum = 1,
                Maximum = 999,
                Value = 1
            };
            pnlForm.Controls.Add(nudQuantidadeTotal);

            // Quantidade Disponível
            pnlForm.Controls.Add(new Label
            {
                Text = "Qtd. Disponível *",
                Location = new System.Drawing.Point(520, 100),
                Size = new System.Drawing.Size(100, 20)
            });
            nudQuantidadeDisponivel = new NumericUpDown
            {
                Location = new System.Drawing.Point(630, 98),
                Size = new System.Drawing.Size(80, 25),
                Minimum = 0,
                Maximum = 999,
                Value = 1
            };
            pnlForm.Controls.Add(nudQuantidadeDisponivel);

            // Botões
            btnNovo = new Button
            {
                Text = "Novo",
                Location = new System.Drawing.Point(110, 145),
                Size = new System.Drawing.Size(100, 35),
                BackColor = System.Drawing.Color.MediumSeaGreen,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnNovo.FlatAppearance.BorderSize = 0;
            btnNovo.Click += BtnNovo_Click;
            pnlForm.Controls.Add(btnNovo);

            btnSalvar = new Button
            {
                Text = "Salvar",
                Location = new System.Drawing.Point(220, 145),
                Size = new System.Drawing.Size(100, 35),
                BackColor = System.Drawing.Color.DarkSlateBlue,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSalvar.FlatAppearance.BorderSize = 0;
            btnSalvar.Click += BtnSalvar_Click;
            pnlForm.Controls.Add(btnSalvar);

            btnCancelar = new Button
            {
                Text = "Cancelar",
                Location = new System.Drawing.Point(330, 145),
                Size = new System.Drawing.Size(100, 35),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Click += BtnCancelar_Click;
            pnlForm.Controls.Add(btnCancelar);

            this.Controls.Add(pnlForm);

            // Grid de Livros
            dgvLivros = new DataGridView
            {
                Location = new System.Drawing.Point(20, 280),
                Size = new System.Drawing.Size(1060, 320),
                BackgroundColor = System.Drawing.Color.White,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvLivros.CellDoubleClick += DgvLivros_CellDoubleClick;

            this.Controls.Add(dgvLivros);

            // Botões de Ação
            var btnEditar = new Button
            {
                Text = "Editar",
                Location = new System.Drawing.Point(810, 610),
                Size = new System.Drawing.Size(90, 30),
                BackColor = System.Drawing.Color.SteelBlue,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnEditar.FlatAppearance.BorderSize = 0;
            btnEditar.Click += BtnEditar_Click;
            this.Controls.Add(btnEditar);

            var btnFechar = new Button
            {
                Text = "Fechar",
                Location = new System.Drawing.Point(1010, 610),
                Size = new System.Drawing.Size(70, 30),
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

        private TextBox txtTitulo = new TextBox();
        private TextBox txtAutor = new TextBox();
        private TextBox txtISBN = new TextBox();
        private TextBox txtEditora = new TextBox();
        private NumericUpDown nudAno = new NumericUpDown();
        private TextBox txtCategoria = new TextBox();
        private NumericUpDown nudQuantidadeTotal = new NumericUpDown();
        private NumericUpDown nudQuantidadeDisponivel = new NumericUpDown();
        private Button btnNovo = new Button();
        private Button btnSalvar = new Button();
        private Button btnCancelar = new Button();
        private DataGridView dgvLivros = new DataGridView();

        private void CarregarGrid()
        {
            try
            {
                var livros = _livroDAL.Listar();

                dgvLivros.DataSource = livros.Select(l => new
                {
                    l.Id,
                    l.Titulo,
                    l.Autor,
                    l.ISBN,
                    l.Editora,
                    l.AnoPublicacao,
                    l.Categoria,
                    QtdTotal = l.QuantidadeTotal,
                    QtdDisp = l.QuantidadeDisponivel
                }).ToList();

                if (dgvLivros.Columns.Count > 0)
                {
                    dgvLivros.Columns["Id"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar livros: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnNovo_Click(object? sender, EventArgs e)
        {
            LimparCampos();
            _livroEmEdicaoId = null;
            btnCancelar.Enabled = true;
            txtTitulo.Focus();
        }

        private void BtnSalvar_Click(object? sender, EventArgs e)
        {
            try
            {
                var livro = new Livro
                {
                    Titulo = txtTitulo.Text.Trim(),
                    Autor = string.IsNullOrWhiteSpace(txtAutor.Text) ? null : txtAutor.Text.Trim(),
                    ISBN = string.IsNullOrWhiteSpace(txtISBN.Text) ? null : txtISBN.Text.Trim(),
                    Editora = string.IsNullOrWhiteSpace(txtEditora.Text) ? null : txtEditora.Text.Trim(),
                    AnoPublicacao = (int?)nudAno.Value,
                    Categoria = string.IsNullOrWhiteSpace(txtCategoria.Text) ? null : txtCategoria.Text.Trim(),
                    QuantidadeTotal = (int)nudQuantidadeTotal.Value,
                    QuantidadeDisponivel = (int)nudQuantidadeDisponivel.Value
                };

                ResultadoOperacao resultado;

                if (_livroEmEdicaoId.HasValue)
                {
                    // Atualização
                    livro.Id = _livroEmEdicaoId.Value;
                    resultado = _livroService.AtualizarLivro(livro, _funcionarioLogado.Id);
                }
                else
                {
                    // Novo cadastro
                    resultado = _livroService.CadastrarLivro(livro, _funcionarioLogado.Id);
                }

                if (resultado.Sucesso)
                {
                    MessageBox.Show(resultado.Mensagem, "Sucesso",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimparCampos();
                    CarregarGrid();
                    _livroEmEdicaoId = null;
                    btnCancelar.Enabled = false;
                }
                else
                {
                    MessageBox.Show(resultado.Mensagem, "Atenção",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar livro: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelar_Click(object? sender, EventArgs e)
        {
            LimparCampos();
            _livroEmEdicaoId = null;
            btnCancelar.Enabled = false;
        }

        private void BtnEditar_Click(object? sender, EventArgs e)
        {
            if (dgvLivros.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um livro para editar.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int livroId = Convert.ToInt32(dgvLivros.SelectedRows[0].Cells["Id"].Value);
            var livro = _livroDAL.ObterPorId(livroId);

            if (livro != null)
            {
                txtTitulo.Text = livro.Titulo;
                txtAutor.Text = livro.Autor ?? "";
                txtISBN.Text = livro.ISBN ?? "";
                txtEditora.Text = livro.Editora ?? "";
                nudAno.Value = livro.AnoPublicacao ?? DateTime.Now.Year;
                txtCategoria.Text = livro.Categoria ?? "";
                nudQuantidadeTotal.Value = livro.QuantidadeTotal;
                nudQuantidadeDisponivel.Value = livro.QuantidadeDisponivel;

                _livroEmEdicaoId = livro.Id;
                btnCancelar.Enabled = true;
                txtTitulo.Focus();
            }
        }

        private void DgvLivros_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                BtnEditar_Click(sender, EventArgs.Empty);
            }
        }

        private void LimparCampos()
        {
            txtTitulo.Clear();
            txtAutor.Clear();
            txtISBN.Clear();
            txtEditora.Clear();
            nudAno.Value = DateTime.Now.Year;
            txtCategoria.Clear();
            nudQuantidadeTotal.Value = 1;
            nudQuantidadeDisponivel.Value = 1;
        }
    }
}
