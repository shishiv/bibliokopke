using System;
using System.Linq;
using System.Windows.Forms;
using BibliotecaJK.Model;
using BibliotecaJK.BLL;
using BibliotecaJK.DAL;

namespace BibliotecaJK.Forms
{
    /// <summary>
    /// Formulário de Devolução de Livros
    /// Calcula multas automaticamente e processa fila de reservas
    /// </summary>
    public partial class FormDevolucao : Form
    {
        private readonly Funcionario _funcionarioLogado;
        private readonly EmprestimoService _emprestimoService;
        private readonly EmprestimoDAL _emprestimoDAL;

        public FormDevolucao(Funcionario funcionario)
        {
            _funcionarioLogado = funcionario;
            _emprestimoService = new EmprestimoService();
            _emprestimoDAL = new EmprestimoDAL();

            InitializeComponent();
            CarregarEmprestimosAtivos();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // FormDevolucao
            this.ClientSize = new System.Drawing.Size(1100, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Devolução de Livros";
            this.BackColor = System.Drawing.Color.WhiteSmoke;

            // Título
            var lblTitulo = new Label
            {
                Text = "DEVOLUÇÃO DE LIVROS",
                Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                Location = new System.Drawing.Point(20, 15),
                Size = new System.Drawing.Size(1060, 30)
            };
            this.Controls.Add(lblTitulo);

            // Panel de Busca
            var pnlBusca = new Panel
            {
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(1060, 60),
                BackColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            pnlBusca.Controls.Add(new Label
            {
                Text = "Buscar por Aluno:",
                Location = new System.Drawing.Point(15, 18),
                Size = new System.Drawing.Size(120, 20)
            });

            txtBuscarAluno = new TextBox
            {
                Location = new System.Drawing.Point(145, 16),
                Size = new System.Drawing.Size(300, 25)
            };
            txtBuscarAluno.TextChanged += (s, e) => CarregarEmprestimosAtivos();
            pnlBusca.Controls.Add(txtBuscarAluno);

            var chkApenasAtrasados = new CheckBox
            {
                Text = "Apenas empréstimos atrasados",
                Location = new System.Drawing.Point(470, 18),
                Size = new System.Drawing.Size(250, 20),
                Checked = false
            };
            chkApenasAtrasados.CheckedChanged += (s, e) => CarregarEmprestimosAtivos();
            pnlBusca.Controls.Add(chkApenasAtrasados);
            this.chkApenasAtrasados = chkApenasAtrasados;

            var btnAtualizar = new Button
            {
                Text = "🔄 Atualizar",
                Location = new System.Drawing.Point(750, 13),
                Size = new System.Drawing.Size(100, 30),
                BackColor = System.Drawing.Color.SteelBlue,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAtualizar.FlatAppearance.BorderSize = 0;
            btnAtualizar.Click += (s, e) => CarregarEmprestimosAtivos();
            pnlBusca.Controls.Add(btnAtualizar);

            this.Controls.Add(pnlBusca);

            // Grid de Empréstimos
            dgvEmprestimos = new DataGridView
            {
                Location = new System.Drawing.Point(20, 140),
                Size = new System.Drawing.Size(1060, 340),
                BackgroundColor = System.Drawing.Color.White,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvEmprestimos.SelectionChanged += DgvEmprestimos_SelectionChanged;

            this.Controls.Add(dgvEmprestimos);

            // Panel de Detalhes
            var pnlDetalhes = new Panel
            {
                Location = new System.Drawing.Point(20, 500),
                Size = new System.Drawing.Size(1060, 100),
                BackColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            pnlDetalhes.Controls.Add(new Label
            {
                Text = "DETALHES DA DEVOLUÇÃO",
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                Location = new System.Drawing.Point(15, 10),
                Size = new System.Drawing.Size(1030, 20)
            });

            lblDataEmprestimo = new Label
            {
                Text = "Data do Empréstimo: -",
                Location = new System.Drawing.Point(15, 40),
                Size = new System.Drawing.Size(250, 20)
            };
            pnlDetalhes.Controls.Add(lblDataEmprestimo);

            lblDataPrevista = new Label
            {
                Text = "Data Prevista: -",
                Location = new System.Drawing.Point(280, 40),
                Size = new System.Drawing.Size(250, 20)
            };
            pnlDetalhes.Controls.Add(lblDataPrevista);

            lblDiasAtraso = new Label
            {
                Text = "Dias de Atraso: -",
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(545, 40),
                Size = new System.Drawing.Size(200, 20)
            };
            pnlDetalhes.Controls.Add(lblDiasAtraso);

            lblMulta = new Label
            {
                Text = "Multa: R$ 0,00",
                Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.Crimson,
                Location = new System.Drawing.Point(15, 65),
                Size = new System.Drawing.Size(500, 25)
            };
            pnlDetalhes.Controls.Add(lblMulta);

            this.Controls.Add(pnlDetalhes);

            // Botões
            btnRegistrarDevolucao = new Button
            {
                Text = "Registrar Devolução",
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(850, 610),
                Size = new System.Drawing.Size(170, 30),
                BackColor = System.Drawing.Color.MediumSeaGreen,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnRegistrarDevolucao.FlatAppearance.BorderSize = 0;
            btnRegistrarDevolucao.Click += BtnRegistrarDevolucao_Click;
            this.Controls.Add(btnRegistrarDevolucao);

            var btnFechar = new Button
            {
                Text = "Fechar",
                Location = new System.Drawing.Point(1030, 610),
                Size = new System.Drawing.Size(50, 30),
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
        private CheckBox chkApenasAtrasados = new CheckBox();
        private DataGridView dgvEmprestimos = new DataGridView();
        private Label lblDataEmprestimo = new Label();
        private Label lblDataPrevista = new Label();
        private Label lblDiasAtraso = new Label();
        private Label lblMulta = new Label();
        private Button btnRegistrarDevolucao = new Button();

        private int? _emprestimoSelecionadoId;

        private void CarregarEmprestimosAtivos()
        {
            try
            {
                var emprestimos = _emprestimoDAL.Listar()
                    .Where(e => e.DataDevolucao == null)
                    .ToList();

                // Filtro por nome do aluno
                if (!string.IsNullOrWhiteSpace(txtBuscarAluno.Text))
                {
                    var alunoDAL = new AlunoDAL();
                    var alunos = alunoDAL.Listar()
                        .Where(a => a.Nome.Contains(txtBuscarAluno.Text, StringComparison.OrdinalIgnoreCase))
                        .Select(a => a.Id)
                        .ToList();

                    emprestimos = emprestimos.Where(e => alunos.Contains(e.IdAluno)).ToList();
                }

                // Filtro de atrasados
                if (chkApenasAtrasados.Checked)
                {
                    var hoje = DateTime.Now.Date;
                    emprestimos = emprestimos.Where(e => e.DataPrevista.Date < hoje).ToList();
                }

                // Carregar dados para exibição
                var alunoDAL2 = new AlunoDAL();
                var livroDAL = new LivroDAL();

                var dados = emprestimos.Select(e =>
                {
                    var aluno = alunoDAL2.ObterPorId(e.IdAluno);
                    var livro = livroDAL.ObterPorId(e.IdLivro);
                    var diasAtraso = (DateTime.Now.Date - e.DataPrevista.Date).Days;
                    var multa = diasAtraso > 0 ? diasAtraso * 2.00m : 0;

                    return new
                    {
                        e.Id,
                        Aluno = aluno?.Nome ?? "N/A",
                        Livro = livro?.Titulo ?? "N/A",
                        DataEmprestimo = e.DataEmprestimo.ToString("dd/MM/yyyy"),
                        DataPrevista = e.DataPrevista.ToString("dd/MM/yyyy"),
                        DiasAtraso = diasAtraso > 0 ? diasAtraso : 0,
                        Multa = $"R$ {multa:F2}",
                        Status = diasAtraso > 0 ? "ATRASADO" : "No prazo"
                    };
                }).ToList();

                dgvEmprestimos.DataSource = dados;

                if (dgvEmprestimos.Columns.Count > 0)
                {
                    dgvEmprestimos.Columns["Id"].Visible = false;

                    // Colorir linha de atrasados
                    foreach (DataGridViewRow row in dgvEmprestimos.Rows)
                    {
                        if (row.Cells["Status"].Value?.ToString() == "ATRASADO")
                        {
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
                            row.DefaultCellStyle.ForeColor = System.Drawing.Color.DarkRed;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar empréstimos: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvEmprestimos_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvEmprestimos.SelectedRows.Count > 0)
            {
                _emprestimoSelecionadoId = Convert.ToInt32(dgvEmprestimos.SelectedRows[0].Cells["Id"].Value);
                var emprestimo = _emprestimoDAL.ObterPorId(_emprestimoSelecionadoId.Value);

                if (emprestimo != null)
                {
                    var diasAtraso = (DateTime.Now.Date - emprestimo.DataPrevista.Date).Days;
                    var multa = diasAtraso > 0 ? diasAtraso * 2.00m : 0;

                    lblDataEmprestimo.Text = $"Data do Empréstimo: {emprestimo.DataEmprestimo:dd/MM/yyyy}";
                    lblDataPrevista.Text = $"Data Prevista: {emprestimo.DataPrevista:dd/MM/yyyy}";

                    if (diasAtraso > 0)
                    {
                        lblDiasAtraso.Text = $"Dias de Atraso: {diasAtraso} dia(s)";
                        lblDiasAtraso.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        lblDiasAtraso.Text = "Dias de Atraso: 0 (no prazo)";
                        lblDiasAtraso.ForeColor = System.Drawing.Color.Green;
                    }

                    lblMulta.Text = $"Multa: R$ {multa:F2}";
                    btnRegistrarDevolucao.Enabled = true;
                }
            }
            else
            {
                _emprestimoSelecionadoId = null;
                LimparDetalhes();
                btnRegistrarDevolucao.Enabled = false;
            }
        }

        private void BtnRegistrarDevolucao_Click(object? sender, EventArgs e)
        {
            if (!_emprestimoSelecionadoId.HasValue)
            {
                MessageBox.Show("Selecione um empréstimo.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var emprestimo = _emprestimoDAL.ObterPorId(_emprestimoSelecionadoId.Value);
                if (emprestimo == null) return;

                var diasAtraso = (DateTime.Now.Date - emprestimo.DataPrevista.Date).Days;
                var multa = diasAtraso > 0 ? diasAtraso * 2.00m : 0;

                var confirmacao = MessageBox.Show(
                    $"Confirmar devolução?\n\n" +
                    $"Data do empréstimo: {emprestimo.DataEmprestimo:dd/MM/yyyy}\n" +
                    $"Data prevista: {emprestimo.DataPrevista:dd/MM/yyyy}\n" +
                    $"Dias de atraso: {(diasAtraso > 0 ? diasAtraso : 0)}\n" +
                    $"Multa: R$ {multa:F2}",
                    "Confirmar Devolução",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmacao == DialogResult.Yes)
                {
                    var resultado = _emprestimoService.RegistrarDevolucao(
                        _emprestimoSelecionadoId.Value,
                        _funcionarioLogado.Id);

                    if (resultado.Sucesso)
                    {
                        string mensagem = resultado.Mensagem;
                        if (resultado.ValorMulta > 0)
                        {
                            mensagem += $"\n\n💰 Multa gerada: R$ {resultado.ValorMulta:F2}";
                        }

                        MessageBox.Show(mensagem, "Devolução Registrada",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        _emprestimoSelecionadoId = null;
                        LimparDetalhes();
                        CarregarEmprestimosAtivos();
                    }
                    else
                    {
                        MessageBox.Show(resultado.Mensagem, "Erro",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao registrar devolução: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimparDetalhes()
        {
            lblDataEmprestimo.Text = "Data do Empréstimo: -";
            lblDataPrevista.Text = "Data Prevista: -";
            lblDiasAtraso.Text = "Dias de Atraso: -";
            lblDiasAtraso.ForeColor = System.Drawing.Color.Black;
            lblMulta.Text = "Multa: R$ 0,00";
        }
    }
}
