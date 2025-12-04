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

            // FormConsultaEmprestimos - Adaptive sizing para diferentes resoluções
            System.Drawing.Rectangle workingArea = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;

            // Calcular tamanho ideal (90% da tela, mas não menor que mínimo)
            int idealWidth = Math.Max(1000, (int)(workingArea.Width * 0.9));
            int idealHeight = Math.Max(600, (int)(workingArea.Height * 0.9));

            // Limitar ao máximo disponível
            int formWidth = Math.Min(idealWidth, workingArea.Width - 50);
            int formHeight = Math.Min(idealHeight, workingArea.Height - 50);

            this.ClientSize = new System.Drawing.Size(formWidth, formHeight);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Consulta de Empréstimos";
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.AutoScroll = true;

            // Título
            var lblTitulo = new Label
            {
                Text = "CONSULTA DE EMPRÉSTIMOS",
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
                Size = new System.Drawing.Size(formWidth - 40, formHeight - 110),
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom |
                         System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right
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

            // Botão Fechar - ancorado no canto inferior direito
            var btnFechar = new Button
            {
                Text = "Fechar",
                Size = new System.Drawing.Size(80, 35),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right
            };
            btnFechar.Location = new System.Drawing.Point(formWidth - 100, formHeight - 45);
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
                Size = new System.Drawing.Size(120, 35),
                BackColor = System.Drawing.Color.SteelBlue,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right
            };
            btnAtualizar.Location = new System.Drawing.Point(tab.Width - 140, tab.Height - 45);
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
                Size = new System.Drawing.Size(120, 35),
                BackColor = System.Drawing.Color.SteelBlue,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right
            };
            btnAtualizar.Location = new System.Drawing.Point(tab.Width - 140, tab.Height - 45);
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
                Size = new System.Drawing.Size(120, 35),
                BackColor = System.Drawing.Color.SteelBlue,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right
            };
            btnAtualizar.Location = new System.Drawing.Point(tab.Width - 140, tab.Height - 45);
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
                Size = new System.Drawing.Size(120, 35),
                BackColor = System.Drawing.Color.SteelBlue,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right
            };
            btnAtualizar.Location = new System.Drawing.Point(tab.Width - 140, tab.Height - 45);
            btnAtualizar.FlatAppearance.BorderSize = 0;
            btnAtualizar.Click += (s, e) => CarregarHistorico();
            tab.Controls.Add(btnAtualizar);

            CarregarHistorico();
        }

        private void CriarAbaEstatisticas(TabPage tab)
        {
            tab.AutoScroll = true;

            var lblTitulo = new Label
            {
                Text = "ESTATÍSTICAS GERAIS",
                Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                Dock = System.Windows.Forms.DockStyle.Top,
                Height = 50,
                Padding = new System.Windows.Forms.Padding(20, 15, 0, 0)
            };
            tab.Controls.Add(lblTitulo);

            // TableLayoutPanel para cards de estatísticas
            var layoutStats = new TableLayoutPanel
            {
                Dock = System.Windows.Forms.DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                Padding = new System.Windows.Forms.Padding(20, 10, 20, 60),
                AutoScroll = true
            };

            // Configurar linhas com altura automática
            layoutStats.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layoutStats.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layoutStats.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layoutStats.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // Cards de estatísticas usando Dock.Fill
            var pnl1 = CriarPanelEstatisticaResponsivo("Total de Empréstimos", "0", System.Drawing.Color.SteelBlue);
            this.lblStatTotal = pnl1.Controls.OfType<Label>().ElementAt(1);
            layoutStats.Controls.Add(pnl1, 0, 0);

            var pnl2 = CriarPanelEstatisticaResponsivo("Empréstimos Ativos", "0", System.Drawing.Color.MediumSeaGreen);
            this.lblStatAtivos = pnl2.Controls.OfType<Label>().ElementAt(1);
            layoutStats.Controls.Add(pnl2, 0, 1);

            var pnl3 = CriarPanelEstatisticaResponsivo("Empréstimos Atrasados", "0", System.Drawing.Color.OrangeRed);
            this.lblStatAtrasados = pnl3.Controls.OfType<Label>().ElementAt(1);
            layoutStats.Controls.Add(pnl3, 0, 2);

            var pnl4 = CriarPanelEstatisticaResponsivo("Multa Total Acumulada", "R$ 0,00", System.Drawing.Color.Crimson);
            this.lblStatMulta = pnl4.Controls.OfType<Label>().ElementAt(1);
            layoutStats.Controls.Add(pnl4, 0, 3);

            tab.Controls.Add(layoutStats);

            // Botão atualizar - ancorado no canto inferior direito
            var btnAtualizar = new Button
            {
                Text = "🔄 Atualizar Estatísticas",
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                Size = new System.Drawing.Size(200, 40),
                BackColor = System.Drawing.Color.DarkSlateBlue,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right
            };
            btnAtualizar.Location = new System.Drawing.Point(tab.Width - 220, tab.Height - 50);
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
            // Criar container panel para o DataGridView com margem
            var pnlContainer = new Panel
            {
                Dock = System.Windows.Forms.DockStyle.Fill,
                Padding = new System.Windows.Forms.Padding(15, y, 15, 50)
            };

            var dgv = new DataGridView
            {
                Dock = System.Windows.Forms.DockStyle.Fill,
                BackgroundColor = System.Drawing.Color.White,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            pnlContainer.Controls.Add(dgv);
            tab.Controls.Add(pnlContainer);
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

        /// <summary>
        /// Versão responsiva do panel de estatística para TableLayoutPanel
        /// </summary>
        private Panel CriarPanelEstatisticaResponsivo(string titulo, string valor, System.Drawing.Color cor)
        {
            var pnl = new Panel
            {
                Dock = System.Windows.Forms.DockStyle.Fill,
                BackColor = cor,
                Margin = new System.Windows.Forms.Padding(0, 5, 0, 5),
                MinimumSize = new System.Drawing.Size(200, 70),
                Padding = new System.Windows.Forms.Padding(15)
            };

            var lblTitulo = new Label
            {
                Text = titulo,
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(15, 10),
                AutoSize = true,
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left
            };
            pnl.Controls.Add(lblTitulo);

            var lblValor = new Label
            {
                Text = valor,
                Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(15, 35),
                AutoSize = true,
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left
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
