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

            // FormReserva - Adaptive sizing para diferentes resoluções
            System.Drawing.Rectangle workingArea = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;

            // Calcular tamanho ideal (90% da tela, mas não menor que mínimo)
            int idealWidth = Math.Max(900, (int)(workingArea.Width * 0.9));
            int idealHeight = Math.Max(600, (int)(workingArea.Height * 0.9));

            // Limitar ao máximo disponível
            int formWidth = Math.Min(idealWidth, workingArea.Width - 50);
            int formHeight = Math.Min(idealHeight, workingArea.Height - 50);

            this.ClientSize = new System.Drawing.Size(formWidth, formHeight);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Gerenciamento de Reservas";
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.AutoScroll = true;

            // Título
            var lblTitulo = new Label
            {
                Text = "GERENCIAMENTO DE RESERVAS",
                Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                Location = new System.Drawing.Point(20, 15),
                Size = new System.Drawing.Size(formWidth - 40, 30),
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right
            };
            this.Controls.Add(lblTitulo);

            // Abas - responsivo com Anchor
            var tabControl = new TabControl
            {
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(formWidth - 40, formHeight - 120),
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom |
                         System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right
            };

            // Aba 1: Nova Reserva
            var tabNovaReserva = new TabPage("Nova Reserva");
            CriarAbaNovaReserva(tabNovaReserva, formWidth);
            tabControl.TabPages.Add(tabNovaReserva);

            // Aba 2: Reservas Ativas
            var tabReservasAtivas = new TabPage("Reservas Ativas");
            CriarAbaReservasAtivas(tabReservasAtivas, formWidth);
            tabControl.TabPages.Add(tabReservasAtivas);

            this.Controls.Add(tabControl);

            // FlowLayoutPanel para botões - ancorado no canto inferior direito
            var flowButtons = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right,
                WrapContents = false,
                Padding = new System.Windows.Forms.Padding(0)
            };
            flowButtons.Location = new System.Drawing.Point(formWidth - 100, formHeight - 45);

            var btnFechar = new Button
            {
                Text = "Fechar",
                Size = new System.Drawing.Size(80, 35),
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

        private TextBox txtBuscarAlunoReserva = new TextBox();
        private TextBox txtBuscarLivroReserva = new TextBox();
        private DataGridView dgvAlunosReserva = new DataGridView();
        private DataGridView dgvLivrosReserva = new DataGridView();
        private DataGridView dgvReservasAtivas = new DataGridView();

        private int? _alunoReservaSelecionadoId;
        private int? _livroReservaSelecionadoId;

        private void CriarAbaNovaReserva(TabPage tab, int formWidth)
        {
            // Panel principal para a aba - responsivo
            var pnlAba = new Panel
            {
                Location = new System.Drawing.Point(0, 0),
                Size = new System.Drawing.Size(formWidth - 40, tab.Height - 20),
                AutoScroll = true,
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom |
                         System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right
            };

            // Seção Aluno
            pnlAba.Controls.Add(new Label
            {
                Text = "SELECIONAR ALUNO",
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                Location = new System.Drawing.Point(15, 15),
                Size = new System.Drawing.Size(formWidth - 60, 20)
            });

            pnlAba.Controls.Add(new Label
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
            pnlAba.Controls.Add(txtBuscarAlunoReserva);

            dgvAlunosReserva = new DataGridView
            {
                Location = new System.Drawing.Point(15, 75),
                Size = new System.Drawing.Size(formWidth - 60, 120),
                BackgroundColor = System.Drawing.Color.White,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvAlunosReserva.SelectionChanged += (s, e) =>
            {
                if (dgvAlunosReserva.SelectedRows.Count > 0)
                    _alunoReservaSelecionadoId = Convert.ToInt32(dgvAlunosReserva.SelectedRows[0].Cells["Id"].Value);
                else
                    _alunoReservaSelecionadoId = null;
            };
            pnlAba.Controls.Add(dgvAlunosReserva);

            // Seção Livro
            pnlAba.Controls.Add(new Label
            {
                Text = "SELECIONAR LIVRO (apenas livros indisponíveis podem ser reservados)",
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                Location = new System.Drawing.Point(15, 210),
                Size = new System.Drawing.Size(formWidth - 60, 20)
            });

            pnlAba.Controls.Add(new Label
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
            pnlAba.Controls.Add(txtBuscarLivroReserva);

            dgvLivrosReserva = new DataGridView
            {
                Location = new System.Drawing.Point(15, 270),
                Size = new System.Drawing.Size(formWidth - 60, 120),
                BackgroundColor = System.Drawing.Color.White,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvLivrosReserva.SelectionChanged += (s, e) =>
            {
                if (dgvLivrosReserva.SelectedRows.Count > 0)
                    _livroReservaSelecionadoId = Convert.ToInt32(dgvLivrosReserva.SelectedRows[0].Cells["Id"].Value);
                else
                    _livroReservaSelecionadoId = null;
            };
            pnlAba.Controls.Add(dgvLivrosReserva);

            // Botão Criar Reserva - ancorado no canto inferior direito da aba
            var btnCriarReserva = new Button
            {
                Text = "Criar Reserva",
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(formWidth - 160, 405),
                Size = new System.Drawing.Size(135, 35),
                BackColor = System.Drawing.Color.MediumPurple,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right
            };
            btnCriarReserva.FlatAppearance.BorderSize = 0;
            btnCriarReserva.Click += BtnCriarReserva_Click;
            pnlAba.Controls.Add(btnCriarReserva);

            tab.Controls.Add(pnlAba);

            CarregarAlunosParaReserva();
            CarregarLivrosParaReserva();
        }

        private void CriarAbaReservasAtivas(TabPage tab, int formWidth)
        {
            // Panel principal para a aba - responsivo
            var pnlAba = new Panel
            {
                Location = new System.Drawing.Point(0, 0),
                Size = new System.Drawing.Size(formWidth - 40, tab.Height - 20),
                AutoScroll = true,
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom |
                         System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right
            };

            pnlAba.Controls.Add(new Label
            {
                Text = "RESERVAS ATIVAS (Fila FIFO)",
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                Location = new System.Drawing.Point(15, 15),
                Size = new System.Drawing.Size(formWidth - 60, 20),
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right
            });

            dgvReservasAtivas = new DataGridView
            {
                Location = new System.Drawing.Point(15, 50),
                Size = new System.Drawing.Size(formWidth - 60, 340),
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
            pnlAba.Controls.Add(dgvReservasAtivas);

            // FlowLayoutPanel para botões - ancorado no canto inferior direito
            var flowButtons = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right,
                WrapContents = false,
                Padding = new System.Windows.Forms.Padding(0)
            };
            flowButtons.Location = new System.Drawing.Point(formWidth - 265, 405);

            var btnAtualizar = new Button
            {
                Text = "Atualizar",
                Size = new System.Drawing.Size(100, 30),
                BackColor = System.Drawing.Color.SteelBlue,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new System.Windows.Forms.Padding(0, 0, 5, 0)
            };
            btnAtualizar.FlatAppearance.BorderSize = 0;
            btnAtualizar.Click += (s, e) => CarregarReservas();
            flowButtons.Controls.Add(btnAtualizar);

            var btnCancelar = new Button
            {
                Text = "Cancelar Reserva",
                Size = new System.Drawing.Size(120, 30),
                BackColor = System.Drawing.Color.Crimson,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new System.Windows.Forms.Padding(0)
            };
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Click += BtnCancelarReserva_Click;
            flowButtons.Controls.Add(btnCancelar);

            pnlAba.Controls.Add(flowButtons);
            tab.Controls.Add(pnlAba);
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
