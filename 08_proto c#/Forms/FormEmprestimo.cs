using System;
using System.Linq;
using System.Windows.Forms;
using BibliotecaJK.Model;
using BibliotecaJK.BLL;
using BibliotecaJK.DAL;

namespace BibliotecaJK.Forms
{
    /// <summary>
    /// Formulário de Registro de Empréstimos
    /// Integra com EmprestimoService para validar regras de negócio
    /// </summary>
    public partial class FormEmprestimo : Form
    {
        private readonly Funcionario _funcionarioLogado;
        private readonly EmprestimoService _emprestimoService;
        private readonly AlunoDAL _alunoDAL;
        private readonly LivroDAL _livroDAL;

        public FormEmprestimo(Funcionario funcionario)
        {
            _funcionarioLogado = funcionario;
            _emprestimoService = new EmprestimoService();
            _alunoDAL = new AlunoDAL();
            _livroDAL = new LivroDAL();

            InitializeComponent();
            CarregarAlunos();
            CarregarLivros();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // FormEmprestimo
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Novo Empréstimo";
            this.BackColor = System.Drawing.Color.WhiteSmoke;

            // Título
            var lblTitulo = new Label
            {
                Text = "REGISTRO DE EMPRÉSTIMO",
                Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                Location = new System.Drawing.Point(20, 15),
                Size = new System.Drawing.Size(860, 30)
            };
            this.Controls.Add(lblTitulo);

            // Panel de Formulário
            var pnlForm = new Panel
            {
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(860, 480),
                BackColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Seção Aluno
            pnlForm.Controls.Add(new Label
            {
                Text = "SELECIONAR ALUNO",
                Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                Location = new System.Drawing.Point(20, 15),
                Size = new System.Drawing.Size(820, 25)
            });

            pnlForm.Controls.Add(new Label
            {
                Text = "Buscar Aluno:",
                Location = new System.Drawing.Point(20, 50),
                Size = new System.Drawing.Size(100, 20)
            });

            txtBuscarAluno = new TextBox
            {
                Location = new System.Drawing.Point(130, 48),
                Size = new System.Drawing.Size(300, 25)
            };
            txtBuscarAluno.TextChanged += TxtBuscarAluno_TextChanged;
            pnlForm.Controls.Add(txtBuscarAluno);

            dgvAlunos = new DataGridView
            {
                Location = new System.Drawing.Point(20, 80),
                Size = new System.Drawing.Size(820, 120),
                BackgroundColor = System.Drawing.Color.White,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            dgvAlunos.SelectionChanged += DgvAlunos_SelectionChanged;
            pnlForm.Controls.Add(dgvAlunos);

            lblInfoAluno = new Label
            {
                Text = "Nenhum aluno selecionado",
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic),
                ForeColor = System.Drawing.Color.Gray,
                Location = new System.Drawing.Point(20, 205),
                Size = new System.Drawing.Size(820, 20)
            };
            pnlForm.Controls.Add(lblInfoAluno);

            // Seção Livro
            pnlForm.Controls.Add(new Label
            {
                Text = "SELECIONAR LIVRO",
                Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                Location = new System.Drawing.Point(20, 235),
                Size = new System.Drawing.Size(820, 25)
            });

            pnlForm.Controls.Add(new Label
            {
                Text = "Buscar Livro:",
                Location = new System.Drawing.Point(20, 270),
                Size = new System.Drawing.Size(100, 20)
            });

            txtBuscarLivro = new TextBox
            {
                Location = new System.Drawing.Point(130, 268),
                Size = new System.Drawing.Size(300, 25)
            };
            txtBuscarLivro.TextChanged += TxtBuscarLivro_TextChanged;
            pnlForm.Controls.Add(txtBuscarLivro);

            dgvLivros = new DataGridView
            {
                Location = new System.Drawing.Point(20, 300),
                Size = new System.Drawing.Size(820, 120),
                BackgroundColor = System.Drawing.Color.White,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            dgvLivros.SelectionChanged += DgvLivros_SelectionChanged;
            pnlForm.Controls.Add(dgvLivros);

            lblInfoLivro = new Label
            {
                Text = "Nenhum livro selecionado",
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic),
                ForeColor = System.Drawing.Color.Gray,
                Location = new System.Drawing.Point(20, 425),
                Size = new System.Drawing.Size(820, 20)
            };
            pnlForm.Controls.Add(lblInfoLivro);

            this.Controls.Add(pnlForm);

            // Botões
            btnRegistrar = new Button
            {
                Text = "Registrar Empréstimo",
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(630, 555),
                Size = new System.Drawing.Size(170, 35),
                BackColor = System.Drawing.Color.DarkSlateBlue,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnRegistrar.FlatAppearance.BorderSize = 0;
            btnRegistrar.Click += BtnRegistrar_Click;
            this.Controls.Add(btnRegistrar);

            var btnFechar = new Button
            {
                Text = "Fechar",
                Location = new System.Drawing.Point(810, 555),
                Size = new System.Drawing.Size(70, 35),
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

        private TextBox txtBuscarAluno = new TextBox();
        private TextBox txtBuscarLivro = new TextBox();
        private DataGridView dgvAlunos = new DataGridView();
        private DataGridView dgvLivros = new DataGridView();
        private Label lblInfoAluno = new Label();
        private Label lblInfoLivro = new Label();
        private Button btnRegistrar = new Button();

        private int? _alunoSelecionadoId;
        private int? _livroSelecionadoId;

        private void CarregarAlunos(string filtro = "")
        {
            try
            {
                var alunos = _alunoDAL.Listar();

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    alunos = alunos.Where(a =>
                        a.Nome.Contains(filtro, StringComparison.OrdinalIgnoreCase) ||
                        a.Matricula.Contains(filtro, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }

                dgvAlunos.DataSource = alunos.Select(a => new
                {
                    a.Id,
                    a.Nome,
                    a.Matricula,
                    a.CPF
                }).ToList();

                if (dgvAlunos.Columns.Count > 0)
                {
                    dgvAlunos.Columns["Id"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar alunos: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarLivros(string filtro = "")
        {
            try
            {
                var livros = _livroDAL.Listar();

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    livros = livros.Where(l =>
                        l.Titulo.Contains(filtro, StringComparison.OrdinalIgnoreCase) ||
                        (l.Autor != null && l.Autor.Contains(filtro, StringComparison.OrdinalIgnoreCase))
                    ).ToList();
                }

                dgvLivros.DataSource = livros.Select(l => new
                {
                    l.Id,
                    l.Titulo,
                    l.Autor,
                    Disponivel = l.QuantidadeDisponivel
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

        private void TxtBuscarAluno_TextChanged(object? sender, EventArgs e)
        {
            CarregarAlunos(txtBuscarAluno.Text);
        }

        private void TxtBuscarLivro_TextChanged(object? sender, EventArgs e)
        {
            CarregarLivros(txtBuscarLivro.Text);
        }

        private void DgvAlunos_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvAlunos.SelectedRows.Count > 0)
            {
                _alunoSelecionadoId = Convert.ToInt32(dgvAlunos.SelectedRows[0].Cells["Id"].Value);
                var nome = dgvAlunos.SelectedRows[0].Cells["Nome"].Value.ToString();
                var matricula = dgvAlunos.SelectedRows[0].Cells["Matricula"].Value.ToString();

                // Verificar empréstimos ativos
                var emprestimosAtivos = _emprestimoService.ObterEmprestimosAtivos(_alunoSelecionadoId.Value);
                lblInfoAluno.Text = $"✓ Aluno: {nome} ({matricula}) - {emprestimosAtivos.Count} empréstimo(s) ativo(s)";
                lblInfoAluno.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                _alunoSelecionadoId = null;
                lblInfoAluno.Text = "Nenhum aluno selecionado";
                lblInfoAluno.ForeColor = System.Drawing.Color.Gray;
            }

            VerificarBotaoRegistrar();
        }

        private void DgvLivros_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvLivros.SelectedRows.Count > 0)
            {
                _livroSelecionadoId = Convert.ToInt32(dgvLivros.SelectedRows[0].Cells["Id"].Value);
                var titulo = dgvLivros.SelectedRows[0].Cells["Titulo"].Value.ToString();
                var disponivel = Convert.ToInt32(dgvLivros.SelectedRows[0].Cells["Disponivel"].Value);

                lblInfoLivro.Text = $"✓ Livro: {titulo} - {disponivel} exemplar(es) disponível(is)";
                lblInfoLivro.ForeColor = disponivel > 0 ? System.Drawing.Color.Green : System.Drawing.Color.Red;
            }
            else
            {
                _livroSelecionadoId = null;
                lblInfoLivro.Text = "Nenhum livro selecionado";
                lblInfoLivro.ForeColor = System.Drawing.Color.Gray;
            }

            VerificarBotaoRegistrar();
        }

        private void VerificarBotaoRegistrar()
        {
            btnRegistrar.Enabled = _alunoSelecionadoId.HasValue && _livroSelecionadoId.HasValue;
        }

        private void BtnRegistrar_Click(object? sender, EventArgs e)
        {
            if (!_alunoSelecionadoId.HasValue || !_livroSelecionadoId.HasValue)
            {
                MessageBox.Show("Selecione um aluno e um livro.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Chamar o serviço que faz todas as validações
                var resultado = _emprestimoService.RegistrarEmprestimo(
                    _alunoSelecionadoId.Value,
                    _livroSelecionadoId.Value,
                    _funcionarioLogado.Id
                );

                if (resultado.Sucesso)
                {
                    MessageBox.Show(
                        resultado.Mensagem + "\n\n" +
                        "Prazo de devolução: 7 dias\n" +
                        "Multa por atraso: R$ 2,00/dia",
                        "Empréstimo Registrado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    // Limpar seleções e recarregar
                    _alunoSelecionadoId = null;
                    _livroSelecionadoId = null;
                    txtBuscarAluno.Clear();
                    txtBuscarLivro.Clear();
                    CarregarAlunos();
                    CarregarLivros();
                    lblInfoAluno.Text = "Nenhum aluno selecionado";
                    lblInfoLivro.Text = "Nenhum livro selecionado";
                }
                else
                {
                    MessageBox.Show(resultado.Mensagem, "Não foi possível registrar",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao registrar empréstimo: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
