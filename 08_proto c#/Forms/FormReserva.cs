using System;
using System.Linq;
using System.Windows.Forms;
using BibliotecaJK.Model;
using BibliotecaJK.BLL;
using BibliotecaJK.DAL;

namespace BibliotecaJK.Forms
{
    /// <summary>
    /// Formulário de Gerenciamento de Reservas
    /// Sistema de fila FIFO
    /// </summary>
    public partial class FormReserva : Form
    {
        private readonly Funcionario _funcionarioLogado;
        private readonly ReservaService _reservaService;
        private readonly ReservaDAL _reservaDAL;
        private readonly AlunoDAL _alunoDAL;
        private readonly LivroDAL _livroDAL;

        public FormReserva(Funcionario funcionario)
        {
            _funcionarioLogado = funcionario;
            _reservaService = new ReservaService();
            _reservaDAL = new ReservaDAL();
            _alunoDAL = new AlunoDAL();
            _livroDAL = new LivroDAL();

            InitializeComponent();
            CarregarReservas();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // FormReserva
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Gerenciamento de Reservas";
            this.BackColor = System.Drawing.Color.WhiteSmoke;

            // Título
            var lblTitulo = new Label
            {
                Text = "GERENCIAMENTO DE RESERVAS",
                Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                Location = new System.Drawing.Point(20, 15),
                Size = new System.Drawing.Size(960, 30)
            };
            this.Controls.Add(lblTitulo);

            // Abas
            var tabControl = new TabControl
            {
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(960, 490)
            };

            // Aba 1: Nova Reserva
            var tabNovaReserva = new TabPage("Nova Reserva");
            CriarAbaNovaReserva(tabNovaReserva);
            tabControl.TabPages.Add(tabNovaReserva);

            // Aba 2: Reservas Ativas
            var tabReservasAtivas = new TabPage("Reservas Ativas");
            CriarAbaReservasAtivas(tabReservasAtivas);
            tabControl.TabPages.Add(tabReservasAtivas);

            this.Controls.Add(tabControl);

            // Botão Fechar
            var btnFechar = new Button
            {
                Text = "Fechar",
                Location = new System.Drawing.Point(910, 560),
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

        private TextBox txtBuscarAlunoReserva = new TextBox();
        private TextBox txtBuscarLivroReserva = new TextBox();
        private DataGridView dgvAlunosReserva = new DataGridView();
        private DataGridView dgvLivrosReserva = new DataGridView();
        private DataGridView dgvReservasAtivas = new DataGridView();

        private int? _alunoReservaSelecionadoId;
        private int? _livroReservaSelecionadoId;

        private void CriarAbaNovaReserva(TabPage tab)
        {
            // Seção Aluno
            tab.Controls.Add(new Label
            {
                Text = "SELECIONAR ALUNO",
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                Location = new System.Drawing.Point(15, 15),
                Size = new System.Drawing.Size(900, 20)
            });

            tab.Controls.Add(new Label
            {
                Text = "Buscar:",
                Location = new System.Drawing.Point(15, 45),
                Size = new System.Drawing.Size(60, 20)
            });

            txtBuscarAlunoReserva = new TextBox
            {
                Location = new System.Drawing.Point(85, 43),
                Size = new System.Drawing.Size(300, 25)
            };
            txtBuscarAlunoReserva.TextChanged += (s, e) => CarregarAlunosParaReserva();
            tab.Controls.Add(txtBuscarAlunoReserva);

            dgvAlunosReserva = new DataGridView
            {
                Location = new System.Drawing.Point(15, 75),
                Size = new System.Drawing.Size(920, 120),
                BackgroundColor = System.Drawing.Color.White,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            dgvAlunosReserva.SelectionChanged += (s, e) =>
            {
                if (dgvAlunosReserva.SelectedRows.Count > 0)
                    _alunoReservaSelecionadoId = Convert.ToInt32(dgvAlunosReserva.SelectedRows[0].Cells["Id"].Value);
                else
                    _alunoReservaSelecionadoId = null;
            };
            tab.Controls.Add(dgvAlunosReserva);

            // Seção Livro
            tab.Controls.Add(new Label
            {
                Text = "SELECIONAR LIVRO (apenas livros indisponíveis podem ser reservados)",
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                Location = new System.Drawing.Point(15, 210),
                Size = new System.Drawing.Size(900, 20)
            });

            tab.Controls.Add(new Label
            {
                Text = "Buscar:",
                Location = new System.Drawing.Point(15, 240),
                Size = new System.Drawing.Size(60, 20)
            });

            txtBuscarLivroReserva = new TextBox
            {
                Location = new System.Drawing.Point(85, 238),
                Size = new System.Drawing.Size(300, 25)
            };
            txtBuscarLivroReserva.TextChanged += (s, e) => CarregarLivrosParaReserva();
            tab.Controls.Add(txtBuscarLivroReserva);

            dgvLivrosReserva = new DataGridView
            {
                Location = new System.Drawing.Point(15, 270),
                Size = new System.Drawing.Size(920, 120),
                BackgroundColor = System.Drawing.Color.White,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            dgvLivrosReserva.SelectionChanged += (s, e) =>
            {
                if (dgvLivrosReserva.SelectedRows.Count > 0)
                    _livroReservaSelecionadoId = Convert.ToInt32(dgvLivrosReserva.SelectedRows[0].Cells["Id"].Value);
                else
                    _livroReservaSelecionadoId = null;
            };
            tab.Controls.Add(dgvLivrosReserva);

            // Botão Criar Reserva
            var btnCriarReserva = new Button
            {
                Text = "Criar Reserva",
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(800, 405),
                Size = new System.Drawing.Size(135, 35),
                BackColor = System.Drawing.Color.MediumPurple,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCriarReserva.FlatAppearance.BorderSize = 0;
            btnCriarReserva.Click += BtnCriarReserva_Click;
            tab.Controls.Add(btnCriarReserva);

            CarregarAlunosParaReserva();
            CarregarLivrosParaReserva();
        }

        private void CriarAbaReservasAtivas(TabPage tab)
        {
            tab.Controls.Add(new Label
            {
                Text = "RESERVAS ATIVAS (Fila FIFO)",
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                Location = new System.Drawing.Point(15, 15),
                Size = new System.Drawing.Size(900, 20)
            });

            dgvReservasAtivas = new DataGridView
            {
                Location = new System.Drawing.Point(15, 50),
                Size = new System.Drawing.Size(920, 340),
                BackgroundColor = System.Drawing.Color.White,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            tab.Controls.Add(dgvReservasAtivas);

            var btnAtualizar = new Button
            {
                Text = "🔄 Atualizar",
                Location = new System.Drawing.Point(705, 405),
                Size = new System.Drawing.Size(100, 30),
                BackColor = System.Drawing.Color.SteelBlue,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAtualizar.FlatAppearance.BorderSize = 0;
            btnAtualizar.Click += (s, e) => CarregarReservas();
            tab.Controls.Add(btnAtualizar);

            var btnCancelar = new Button
            {
                Text = "Cancelar Reserva",
                Location = new System.Drawing.Point(815, 405),
                Size = new System.Drawing.Size(120, 30),
                BackColor = System.Drawing.Color.Crimson,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Click += BtnCancelarReserva_Click;
            tab.Controls.Add(btnCancelar);
        }

        private void CarregarAlunosParaReserva()
        {
            try
            {
                var alunos = _alunoDAL.Listar();

                if (!string.IsNullOrWhiteSpace(txtBuscarAlunoReserva.Text))
                {
                    alunos = alunos.Where(a =>
                        a.Nome.Contains(txtBuscarAlunoReserva.Text, StringComparison.OrdinalIgnoreCase) ||
                        a.Matricula.Contains(txtBuscarAlunoReserva.Text, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }

                dgvAlunosReserva.DataSource = alunos.Select(a => new
                {
                    a.Id,
                    a.Nome,
                    a.Matricula
                }).ToList();

                if (dgvAlunosReserva.Columns.Count > 0)
                    dgvAlunosReserva.Columns["Id"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar alunos: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarLivrosParaReserva()
        {
            try
            {
                // Apenas livros indisponíveis
                var livros = _livroDAL.Listar()
                    .Where(l => l.QuantidadeDisponivel == 0)
                    .ToList();

                if (!string.IsNullOrWhiteSpace(txtBuscarLivroReserva.Text))
                {
                    livros = livros.Where(l =>
                        l.Titulo.Contains(txtBuscarLivroReserva.Text, StringComparison.OrdinalIgnoreCase) ||
                        (l.Autor != null && l.Autor.Contains(txtBuscarLivroReserva.Text, StringComparison.OrdinalIgnoreCase))
                    ).ToList();
                }

                dgvLivrosReserva.DataSource = livros.Select(l => new
                {
                    l.Id,
                    l.Titulo,
                    l.Autor,
                    Status = "Indisponível"
                }).ToList();

                if (dgvLivrosReserva.Columns.Count > 0)
                    dgvLivrosReserva.Columns["Id"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar livros: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarReservas()
        {
            try
            {
                var reservas = _reservaDAL.Listar()
                    .Where(r => r.Status == "Ativa")
                    .OrderBy(r => r.DataReserva)
                    .ToList();

                var dados = reservas.Select(r =>
                {
                    var aluno = _alunoDAL.ObterPorId(r.IdAluno);
                    var livro = _livroDAL.ObterPorId(r.IdLivro);
                    var posicao = _reservaService.ObterPosicaoNaFila(r.IdLivro, r.IdAluno);

                    return new
                    {
                        r.Id,
                        Aluno = aluno?.Nome ?? "N/A",
                        Livro = livro?.Titulo ?? "N/A",
                        DataReserva = r.DataReserva.ToString("dd/MM/yyyy HH:mm"),
                        Posicao = posicao > 0 ? $"{posicao}º na fila" : "N/A"
                    };
                }).ToList();

                dgvReservasAtivas.DataSource = dados;

                if (dgvReservasAtivas.Columns.Count > 0)
                    dgvReservasAtivas.Columns["Id"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar reservas: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCriarReserva_Click(object? sender, EventArgs e)
        {
            if (!_alunoReservaSelecionadoId.HasValue)
            {
                MessageBox.Show("Selecione um aluno.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!_livroReservaSelecionadoId.HasValue)
            {
                MessageBox.Show("Selecione um livro.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var resultado = _reservaService.CriarReserva(
                    _alunoReservaSelecionadoId.Value,
                    _livroReservaSelecionadoId.Value
                );

                if (resultado.Sucesso)
                {
                    MessageBox.Show(resultado.Mensagem, "Sucesso",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    _alunoReservaSelecionadoId = null;
                    _livroReservaSelecionadoId = null;
                    txtBuscarAlunoReserva.Clear();
                    txtBuscarLivroReserva.Clear();
                    CarregarAlunosParaReserva();
                    CarregarLivrosParaReserva();
                    CarregarReservas();
                }
                else
                {
                    MessageBox.Show(resultado.Mensagem, "Atenção",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao criar reserva: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelarReserva_Click(object? sender, EventArgs e)
        {
            if (dgvReservasAtivas.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione uma reserva para cancelar.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int reservaId = Convert.ToInt32(dgvReservasAtivas.SelectedRows[0].Cells["Id"].Value);
            string aluno = dgvReservasAtivas.SelectedRows[0].Cells["Aluno"].Value.ToString() ?? "";
            string livro = dgvReservasAtivas.SelectedRows[0].Cells["Livro"].Value.ToString() ?? "";

            var confirmacao = MessageBox.Show(
                $"Deseja realmente cancelar a reserva?\n\nAluno: {aluno}\nLivro: {livro}",
                "Confirmação",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacao == DialogResult.Yes)
            {
                try
                {
                    var resultado = _reservaService.CancelarReserva(reservaId, _funcionarioLogado.Id);

                    if (resultado.Sucesso)
                    {
                        MessageBox.Show(resultado.Mensagem, "Sucesso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CarregarReservas();
                    }
                    else
                    {
                        MessageBox.Show(resultado.Mensagem, "Erro",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao cancelar reserva: {ex.Message}", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
