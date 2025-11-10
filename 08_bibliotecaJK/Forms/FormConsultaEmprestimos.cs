using System;
using System.Linq;
using System.Windows.Forms;
using BibliotecaJK.Model;
using BibliotecaJK.BLL;
using BibliotecaJK.DAL;

namespace BibliotecaJK.Forms
{
    /// <summary>
    /// Formulário de Consulta de Empréstimos
    /// Relatórios e estatísticas
    /// </summary>
    public partial class FormConsultaEmprestimos : Form
    {
        private readonly Funcionario _funcionarioLogado;
        private readonly EmprestimoService _emprestimoService;
        private readonly EmprestimoDAL _emprestimoDAL;
        private readonly AlunoDAL _alunoDAL;
        private readonly LivroDAL _livroDAL;

        public FormConsultaEmprestimos(Funcionario funcionario)
        {
            _funcionarioLogado = funcionario;
            _emprestimoService = new EmprestimoService();
            _emprestimoDAL = new EmprestimoDAL();
            _alunoDAL = new AlunoDAL();
            _livroDAL = new LivroDAL();

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // FormConsultaEmprestimos
            this.ClientSize = new System.Drawing.Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Consulta de Empréstimos";
            this.BackColor = System.Drawing.Color.WhiteSmoke;

            // Título
            var lblTitulo = new Label
            {
                Text = "CONSULTA DE EMPRÉSTIMOS",
                Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                Location = new System.Drawing.Point(20, 15),
                Size = new System.Drawing.Size(1060, 30)
            };
            this.Controls.Add(lblTitulo);

            // Abas
            var tabControl = new TabControl
            {
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(1060, 590)
            };

            // Aba 1: Todos os Empréstimos
            var tabTodos = new TabPage("Todos os Empréstimos");
            CriarAbaTodos(tabTodos);
            tabControl.TabPages.Add(tabTodos);

            // Aba 2: Empréstimos Ativos
            var tabAtivos = new TabPage("Ativos");
            CriarAbaAtivos(tabAtivos);
            tabControl.TabPages.Add(tabAtivos);

            // Aba 3: Empréstimos Atrasados
            var tabAtrasados = new TabPage("Atrasados");
            CriarAbaAtrasados(tabAtrasados);
            tabControl.TabPages.Add(tabAtrasados);

            // Aba 4: Histórico (Devolvidos)
            var tabHistorico = new TabPage("Histórico");
            CriarAbaHistorico(tabHistorico);
            tabControl.TabPages.Add(tabHistorico);

            // Aba 5: Estatísticas
            var tabEstatisticas = new TabPage("Estatísticas");
            CriarAbaEstatisticas(tabEstatisticas);
            tabControl.TabPages.Add(tabEstatisticas);

            this.Controls.Add(tabControl);

            // Botão Fechar
            var btnFechar = new Button
            {
                Text = "Fechar",
                Location = new System.Drawing.Point(1030, 660),
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

        private DataGridView dgvTodos = new DataGridView();
        private DataGridView dgvAtivos = new DataGridView();
        private DataGridView dgvAtrasados = new DataGridView();
        private DataGridView dgvHistorico = new DataGridView();

        private void CriarAbaTodos(TabPage tab)
        {
            dgvTodos = CriarDataGridView(tab, 15);

            var btnAtualizar = new Button
            {
                Text = "🔄 Atualizar",
                Location = new System.Drawing.Point(930, 520),
                Size = new System.Drawing.Size(100, 30),
                BackColor = System.Drawing.Color.SteelBlue,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAtualizar.FlatAppearance.BorderSize = 0;
            btnAtualizar.Click += (s, e) => CarregarTodos();
            tab.Controls.Add(btnAtualizar);

            CarregarTodos();
        }

        private void CriarAbaAtivos(TabPage tab)
        {
            dgvAtivos = CriarDataGridView(tab, 15);

            var btnAtualizar = new Button
            {
                Text = "🔄 Atualizar",
                Location = new System.Drawing.Point(930, 520),
                Size = new System.Drawing.Size(100, 30),
                BackColor = System.Drawing.Color.SteelBlue,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAtualizar.FlatAppearance.BorderSize = 0;
            btnAtualizar.Click += (s, e) => CarregarAtivos();
            tab.Controls.Add(btnAtualizar);

            CarregarAtivos();
        }

        private void CriarAbaAtrasados(TabPage tab)
        {
            dgvAtrasados = CriarDataGridView(tab, 15);

            var btnAtualizar = new Button
            {
                Text = "🔄 Atualizar",
                Location = new System.Drawing.Point(930, 520),
                Size = new System.Drawing.Size(100, 30),
                BackColor = System.Drawing.Color.SteelBlue,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAtualizar.FlatAppearance.BorderSize = 0;
            btnAtualizar.Click += (s, e) => CarregarAtrasados();
            tab.Controls.Add(btnAtualizar);

            CarregarAtrasados();
        }

        private void CriarAbaHistorico(TabPage tab)
        {
            dgvHistorico = CriarDataGridView(tab, 15);

            var btnAtualizar = new Button
            {
                Text = "🔄 Atualizar",
                Location = new System.Drawing.Point(930, 520),
                Size = new System.Drawing.Size(100, 30),
                BackColor = System.Drawing.Color.SteelBlue,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAtualizar.FlatAppearance.BorderSize = 0;
            btnAtualizar.Click += (s, e) => CarregarHistorico();
            tab.Controls.Add(btnAtualizar);

            CarregarHistorico();
        }

        private void CriarAbaEstatisticas(TabPage tab)
        {
            var lblTitulo = new Label
            {
                Text = "ESTATÍSTICAS GERAIS",
                Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(1000, 30)
            };
            tab.Controls.Add(lblTitulo);

            // Cards de estatísticas
            int y = 70;

            var pnl1 = CriarPanelEstatistica("Total de Empréstimos", "0", y, System.Drawing.Color.SteelBlue);
            tab.Controls.Add(pnl1);
            this.lblStatTotal = pnl1.Controls.OfType<Label>().ElementAt(1);

            var pnl2 = CriarPanelEstatistica("Empréstimos Ativos", "0", y + 90, System.Drawing.Color.MediumSeaGreen);
            tab.Controls.Add(pnl2);
            this.lblStatAtivos = pnl2.Controls.OfType<Label>().ElementAt(1);

            var pnl3 = CriarPanelEstatistica("Empréstimos Atrasados", "0", y + 180, System.Drawing.Color.OrangeRed);
            tab.Controls.Add(pnl3);
            this.lblStatAtrasados = pnl3.Controls.OfType<Label>().ElementAt(1);

            var pnl4 = CriarPanelEstatistica("Multa Total Acumulada", "R$ 0,00", y + 270, System.Drawing.Color.Crimson);
            tab.Controls.Add(pnl4);
            this.lblStatMulta = pnl4.Controls.OfType<Label>().ElementAt(1);

            var btnAtualizar = new Button
            {
                Text = "🔄 Atualizar Estatísticas",
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(850, 510),
                Size = new System.Drawing.Size(180, 35),
                BackColor = System.Drawing.Color.DarkSlateBlue,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAtualizar.FlatAppearance.BorderSize = 0;
            btnAtualizar.Click += (s, e) => CarregarEstatisticas();
            tab.Controls.Add(btnAtualizar);

            CarregarEstatisticas();
        }

        private Label lblStatTotal = new Label();
        private Label lblStatAtivos = new Label();
        private Label lblStatAtrasados = new Label();
        private Label lblStatMulta = new Label();

        private DataGridView CriarDataGridView(TabPage tab, int y)
        {
            var dgv = new DataGridView
            {
                Location = new System.Drawing.Point(15, y),
                Size = new System.Drawing.Size(1020, 490),
                BackgroundColor = System.Drawing.Color.White,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            tab.Controls.Add(dgv);
            return dgv;
        }

        private Panel CriarPanelEstatistica(string titulo, string valor, int y, System.Drawing.Color cor)
        {
            var pnl = new Panel
            {
                Location = new System.Drawing.Point(20, y),
                Size = new System.Drawing.Size(1000, 70),
                BackColor = cor
            };

            var lblTitulo = new Label
            {
                Text = titulo,
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(15, 10),
                Size = new System.Drawing.Size(970, 20)
            };
            pnl.Controls.Add(lblTitulo);

            var lblValor = new Label
            {
                Text = valor,
                Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(15, 35),
                Size = new System.Drawing.Size(970, 30)
            };
            pnl.Controls.Add(lblValor);

            return pnl;
        }

        private void CarregarTodos()
        {
            try
            {
                var emprestimos = _emprestimoDAL.Listar();
                dgvTodos.DataSource = FormatarDados(emprestimos);
                ConfigurarColunas(dgvTodos);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar empréstimos: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarAtivos()
        {
            try
            {
                var emprestimos = _emprestimoDAL.Listar()
                    .Where(e => e.DataDevolucao == null)
                    .ToList();
                dgvAtivos.DataSource = FormatarDados(emprestimos);
                ConfigurarColunas(dgvAtivos);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar empréstimos ativos: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarAtrasados()
        {
            try
            {
                var hoje = DateTime.Now.Date;
                var emprestimos = _emprestimoDAL.Listar()
                    .Where(e => e.DataDevolucao == null && e.DataPrevista.Date < hoje)
                    .ToList();
                dgvAtrasados.DataSource = FormatarDados(emprestimos);
                ConfigurarColunas(dgvAtrasados);

                // Colorir em vermelho
                foreach (DataGridViewRow row in dgvAtrasados.Rows)
                {
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
                    row.DefaultCellStyle.ForeColor = System.Drawing.Color.DarkRed;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar empréstimos atrasados: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarHistorico()
        {
            try
            {
                var emprestimos = _emprestimoDAL.Listar()
                    .Where(e => e.DataDevolucao != null)
                    .OrderByDescending(e => e.DataDevolucao)
                    .ToList();
                dgvHistorico.DataSource = FormatarDados(emprestimos);
                ConfigurarColunas(dgvHistorico);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar histórico: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarEstatisticas()
        {
            try
            {
                var stats = _emprestimoService.ObterEstatisticas();

                lblStatTotal.Text = stats.Total.ToString();
                lblStatAtivos.Text = stats.Ativos.ToString();
                lblStatAtrasados.Text = stats.Atrasados.ToString();
                lblStatMulta.Text = $"R$ {stats.MultaTotal:F2}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar estatísticas: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private object FormatarDados(System.Collections.Generic.List<Emprestimo> emprestimos)
        {
            return emprestimos.Select(e =>
            {
                var aluno = _alunoDAL.ObterPorId(e.IdAluno);
                var livro = _livroDAL.ObterPorId(e.IdLivro);
                var diasAtraso = e.DataDevolucao == null
                    ? (DateTime.Now.Date - e.DataPrevista.Date).Days
                    : (e.DataDevolucao.Value.Date - e.DataPrevista.Date).Days;
                var multa = diasAtraso > 0 ? diasAtraso * 2.00m : 0;

                return new
                {
                    e.Id,
                    Aluno = aluno?.Nome ?? "N/A",
                    Livro = livro?.Titulo ?? "N/A",
                    DataEmprestimo = e.DataEmprestimo.ToString("dd/MM/yyyy"),
                    DataPrevista = e.DataPrevista.ToString("dd/MM/yyyy"),
                    DataDevolucao = e.DataDevolucao?.ToString("dd/MM/yyyy") ?? "Não devolvido",
                    DiasAtraso = diasAtraso > 0 ? diasAtraso : 0,
                    Multa = $"R$ {multa:F2}"
                };
            }).ToList();
        }

        private void ConfigurarColunas(DataGridView dgv)
        {
            if (dgv.Columns.Count > 0)
            {
                dgv.Columns["Id"].Visible = false;
            }
        }
    }
}
