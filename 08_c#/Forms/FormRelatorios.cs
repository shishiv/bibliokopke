using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BibliotecaJK.Model;
using BibliotecaJK.BLL;
using BibliotecaJK.DAL;

namespace BibliotecaJK.Forms
{
    /// <summary>
    /// Formulário de Relatórios Avançados (Pessoa 5)
    /// Gera relatórios gerenciais e permite exportação
    /// </summary>
    public partial class FormRelatorios : Form
    {
        private readonly Funcionario _funcionarioLogado;
        private readonly EmprestimoService _emprestimoService;
        private readonly LivroService _livroService;
        private readonly AlunoService _alunoService;
        private readonly EmprestimoDAL _emprestimoDAL;
        private readonly AlunoDAL _alunoDAL;
        private readonly LivroDAL _livroDAL;

        public FormRelatorios(Funcionario funcionario)
        {
            _funcionarioLogado = funcionario;
            _emprestimoService = new EmprestimoService();
            _livroService = new LivroService();
            _alunoService = new AlunoService();
            _emprestimoDAL = new EmprestimoDAL();
            _alunoDAL = new AlunoDAL();
            _livroDAL = new LivroDAL();

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // FormRelatorios
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Relatórios Gerenciais - BibliotecaJK";
            this.BackColor = System.Drawing.Color.WhiteSmoke;

            // Título
            var lblTitulo = new Label
            {
                Text = "RELATÓRIOS GERENCIAIS",
                Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                Location = new System.Drawing.Point(20, 15),
                Size = new System.Drawing.Size(1160, 30)
            };
            this.Controls.Add(lblTitulo);

            // Panel de Opções
            var pnlOpcoes = new Panel
            {
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(250, 580),
                BackColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            pnlOpcoes.Controls.Add(new Label
            {
                Text = "TIPOS DE RELATÓRIOS",
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(230, 25)
            });

            // Botões de relatórios
            int btnY = 50;
            int btnSpacing = 60;

            var btnEmprestimosPeriodo = CriarBotaoRelatorio("📅 Empréstimos por Período", btnY);
            btnEmprestimosPeriodo.Click += (s, e) => GerarRelatorioEmprestimosPeriodo();
            pnlOpcoes.Controls.Add(btnEmprestimosPeriodo);

            var btnLivrosMaisEmprestados = CriarBotaoRelatorio("📚 Livros Mais Emprestados", btnY + btnSpacing);
            btnLivrosMaisEmprestados.Click += (s, e) => GerarRelatorioLivrosMaisEmprestados();
            pnlOpcoes.Controls.Add(btnLivrosMaisEmprestados);

            var btnAlunosAtivos = CriarBotaoRelatorio("👥 Alunos Mais Ativos", btnY + btnSpacing * 2);
            btnAlunosAtivos.Click += (s, e) => GerarRelatorioAlunosAtivos();
            pnlOpcoes.Controls.Add(btnAlunosAtivos);

            var btnMultas = CriarBotaoRelatorio("💰 Relatório de Multas", btnY + btnSpacing * 3);
            btnMultas.Click += (s, e) => GerarRelatorioMultas();
            pnlOpcoes.Controls.Add(btnMultas);

            var btnAtrasos = CriarBotaoRelatorio("⚠️ Empréstimos Atrasados", btnY + btnSpacing * 4);
            btnAtrasos.Click += (s, e) => GerarRelatorioAtrasos();
            pnlOpcoes.Controls.Add(btnAtrasos);

            var btnReservas = CriarBotaoRelatorio("🔖 Relatório de Reservas", btnY + btnSpacing * 5);
            btnReservas.Click += (s, e) => GerarRelatorioReservas();
            pnlOpcoes.Controls.Add(btnReservas);

            var btnEstatisticas = CriarBotaoRelatorio("📊 Estatísticas Gerais", btnY + btnSpacing * 6);
            btnEstatisticas.Click += (s, e) => GerarRelatorioEstatisticas();
            pnlOpcoes.Controls.Add(btnEstatisticas);

            this.Controls.Add(pnlOpcoes);

            // Panel de Visualização
            var pnlVisualizacao = new Panel
            {
                Location = new System.Drawing.Point(280, 60),
                Size = new System.Drawing.Size(900, 580),
                BackColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            lblTituloRelatorio = new Label
            {
                Text = "Selecione um relatório ao lado",
                Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.Gray,
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(860, 30),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            pnlVisualizacao.Controls.Add(lblTituloRelatorio);

            dgvRelatorio = new DataGridView
            {
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(860, 460),
                BackgroundColor = System.Drawing.Color.White,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            pnlVisualizacao.Controls.Add(dgvRelatorio);

            btnExportar = new Button
            {
                Text = "💾 Exportar para CSV",
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(720, 530),
                Size = new System.Drawing.Size(160, 35),
                BackColor = System.Drawing.Color.MediumSeaGreen,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnExportar.FlatAppearance.BorderSize = 0;
            btnExportar.Click += BtnExportar_Click;
            pnlVisualizacao.Controls.Add(btnExportar);

            this.Controls.Add(pnlVisualizacao);

            // Botão Fechar
            var btnFechar = new Button
            {
                Text = "Fechar",
                Location = new System.Drawing.Point(1130, 655),
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

        private Label lblTituloRelatorio = new Label();
        private DataGridView dgvRelatorio = new DataGridView();
        private Button btnExportar = new Button();
        private string _relatorioAtual = "";

        private Button CriarBotaoRelatorio(string texto, int y)
        {
            var btn = new Button
            {
                Text = texto,
                Location = new System.Drawing.Point(10, y),
                Size = new System.Drawing.Size(230, 45),
                BackColor = System.Drawing.Color.DarkSlateBlue,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                Padding = new System.Windows.Forms.Padding(10, 0, 0, 0)
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private void GerarRelatorioEmprestimosPeriodo()
        {
            try
            {
                _relatorioAtual = "Empréstimos por Período";
                lblTituloRelatorio.Text = "RELATÓRIO: Empréstimos por Período (Últimos 30 dias)";
                lblTituloRelatorio.ForeColor = System.Drawing.Color.DarkSlateBlue;

                var dataInicio = DateTime.Now.AddDays(-30);
                var emprestimos = _emprestimoDAL.Listar()
                    .Where(e => e.DataEmprestimo >= dataInicio)
                    .OrderByDescending(e => e.DataEmprestimo)
                    .ToList();

                var dados = emprestimos.Select(e =>
                {
                    var aluno = _alunoDAL.ObterPorId(e.IdAluno);
                    var livro = _livroDAL.ObterPorId(e.IdLivro);
                    var status = e.DataDevolucao == null ? "Ativo" : "Devolvido";
                    var diasAtraso = e.DataDevolucao == null
                        ? (DateTime.Now.Date - e.DataPrevista.Date).Days
                        : (e.DataDevolucao.Value.Date - e.DataPrevista.Date).Days;
                    var multa = diasAtraso > 0 ? diasAtraso * 2.00m : 0;

                    return new
                    {
                        Data = e.DataEmprestimo.ToString("dd/MM/yyyy"),
                        Aluno = aluno?.Nome ?? "N/A",
                        Livro = livro?.Titulo ?? "N/A",
                        Prevista = e.DataPrevista.ToString("dd/MM/yyyy"),
                        Devolvido = e.DataDevolucao?.ToString("dd/MM/yyyy") ?? "-",
                        Status = status,
                        Multa = $"R$ {multa:F2}"
                    };
                }).ToList();

                dgvRelatorio.DataSource = dados;
                btnExportar.Enabled = dados.Count > 0;

                MessageBox.Show($"Relatório gerado: {dados.Count} empréstimo(s) encontrado(s).", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao gerar relatório: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GerarRelatorioLivrosMaisEmprestados()
        {
            try
            {
                _relatorioAtual = "Livros Mais Emprestados";
                lblTituloRelatorio.Text = "RELATÓRIO: Top 20 Livros Mais Emprestados";
                lblTituloRelatorio.ForeColor = System.Drawing.Color.DarkSlateBlue;

                var emprestimos = _emprestimoDAL.Listar();
                var livrosAgrupados = emprestimos
                    .GroupBy(e => e.IdLivro)
                    .Select(g => new
                    {
                        IdLivro = g.Key,
                        TotalEmprestimos = g.Count()
                    })
                    .OrderByDescending(x => x.TotalEmprestimos)
                    .Take(20)
                    .ToList();

                var dados = livrosAgrupados.Select(lg =>
                {
                    var livro = _livroDAL.ObterPorId(lg.IdLivro);
                    return new
                    {
                        Posição = livrosAgrupados.IndexOf(lg) + 1,
                        Título = livro?.Titulo ?? "N/A",
                        Autor = livro?.Autor ?? "N/A",
                        Categoria = livro?.Categoria ?? "N/A",
                        TotalEmpréstimos = lg.TotalEmprestimos,
                        Disponível = livro?.QuantidadeDisponivel ?? 0
                    };
                }).ToList();

                dgvRelatorio.DataSource = dados;
                btnExportar.Enabled = dados.Count > 0;

                MessageBox.Show($"Relatório gerado: Top {dados.Count} livros mais emprestados.", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao gerar relatório: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GerarRelatorioAlunosAtivos()
        {
            try
            {
                _relatorioAtual = "Alunos Mais Ativos";
                lblTituloRelatorio.Text = "RELATÓRIO: Top 20 Alunos Mais Ativos";
                lblTituloRelatorio.ForeColor = System.Drawing.Color.DarkSlateBlue;

                var emprestimos = _emprestimoDAL.Listar();
                var alunosAgrupados = emprestimos
                    .GroupBy(e => e.IdAluno)
                    .Select(g => new
                    {
                        IdAluno = g.Key,
                        TotalEmprestimos = g.Count(),
                        Ativos = g.Count(e => e.DataDevolucao == null),
                        Atrasados = g.Count(e => e.DataDevolucao == null && e.DataPrevista.Date < DateTime.Now.Date)
                    })
                    .OrderByDescending(x => x.TotalEmprestimos)
                    .Take(20)
                    .ToList();

                var dados = alunosAgrupados.Select(ag =>
                {
                    var aluno = _alunoDAL.ObterPorId(ag.IdAluno);
                    return new
                    {
                        Posição = alunosAgrupados.IndexOf(ag) + 1,
                        Nome = aluno?.Nome ?? "N/A",
                        Matrícula = aluno?.Matricula ?? "N/A",
                        TotalEmpréstimos = ag.TotalEmprestimos,
                        Ativos = ag.Ativos,
                        Atrasados = ag.Atrasados
                    };
                }).ToList();

                dgvRelatorio.DataSource = dados;
                btnExportar.Enabled = dados.Count > 0;

                MessageBox.Show($"Relatório gerado: Top {dados.Count} alunos mais ativos.", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao gerar relatório: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GerarRelatorioMultas()
        {
            try
            {
                _relatorioAtual = "Relatório de Multas";
                lblTituloRelatorio.Text = "RELATÓRIO: Multas por Atraso";
                lblTituloRelatorio.ForeColor = System.Drawing.Color.Crimson;

                var emprestimos = _emprestimoDAL.Listar()
                    .Where(e =>
                    {
                        if (e.DataDevolucao == null)
                            return (DateTime.Now.Date - e.DataPrevista.Date).Days > 0;
                        else
                            return (e.DataDevolucao.Value.Date - e.DataPrevista.Date).Days > 0;
                    })
                    .ToList();

                var dados = emprestimos.Select(e =>
                {
                    var aluno = _alunoDAL.ObterPorId(e.IdAluno);
                    var livro = _livroDAL.ObterPorId(e.IdLivro);
                    var diasAtraso = e.DataDevolucao == null
                        ? (DateTime.Now.Date - e.DataPrevista.Date).Days
                        : (e.DataDevolucao.Value.Date - e.DataPrevista.Date).Days;
                    var multa = diasAtraso * 2.00m;
                    var status = e.DataDevolucao == null ? "Pendente" : "Paga";

                    return new
                    {
                        Aluno = aluno?.Nome ?? "N/A",
                        Livro = livro?.Titulo ?? "N/A",
                        DataEmpréstimo = e.DataEmprestimo.ToString("dd/MM/yyyy"),
                        Prevista = e.DataPrevista.ToString("dd/MM/yyyy"),
                        Devolvido = e.DataDevolucao?.ToString("dd/MM/yyyy") ?? "-",
                        DiasAtraso = diasAtraso,
                        Multa = $"R$ {multa:F2}",
                        Status = status
                    };
                }).OrderByDescending(x => x.DiasAtraso).ToList();

                dgvRelatorio.DataSource = dados;
                btnExportar.Enabled = dados.Count > 0;

                var totalMultas = emprestimos.Sum(e =>
                {
                    var dias = e.DataDevolucao == null
                        ? (DateTime.Now.Date - e.DataPrevista.Date).Days
                        : (e.DataDevolucao.Value.Date - e.DataPrevista.Date).Days;
                    return dias * 2.00m;
                });

                MessageBox.Show(
                    $"Relatório gerado:\n{dados.Count} multa(s) registrada(s)\nTotal: R$ {totalMultas:F2}",
                    "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao gerar relatório: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GerarRelatorioAtrasos()
        {
            try
            {
                _relatorioAtual = "Empréstimos Atrasados";
                lblTituloRelatorio.Text = "RELATÓRIO: Empréstimos Atrasados (Não Devolvidos)";
                lblTituloRelatorio.ForeColor = System.Drawing.Color.OrangeRed;

                var hoje = DateTime.Now.Date;
                var emprestimos = _emprestimoDAL.Listar()
                    .Where(e => e.DataDevolucao == null && e.DataPrevista.Date < hoje)
                    .OrderBy(e => e.DataPrevista)
                    .ToList();

                var dados = emprestimos.Select(e =>
                {
                    var aluno = _alunoDAL.ObterPorId(e.IdAluno);
                    var livro = _livroDAL.ObterPorId(e.IdLivro);
                    var diasAtraso = (hoje - e.DataPrevista.Date).Days;
                    var multa = diasAtraso * 2.00m;

                    return new
                    {
                        Aluno = aluno?.Nome ?? "N/A",
                        Matrícula = aluno?.Matricula ?? "N/A",
                        Telefone = aluno?.Telefone ?? "N/A",
                        Livro = livro?.Titulo ?? "N/A",
                        DataEmpréstimo = e.DataEmprestimo.ToString("dd/MM/yyyy"),
                        Prevista = e.DataPrevista.ToString("dd/MM/yyyy"),
                        DiasAtraso = diasAtraso,
                        MultaAcumulada = $"R$ {multa:F2}"
                    };
                }).ToList();

                dgvRelatorio.DataSource = dados;
                btnExportar.Enabled = dados.Count > 0;

                // Colorir em vermelho
                foreach (DataGridViewRow row in dgvRelatorio.Rows)
                {
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
                    row.DefaultCellStyle.ForeColor = System.Drawing.Color.DarkRed;
                }

                MessageBox.Show($"⚠️ ATENÇÃO: {dados.Count} empréstimo(s) atrasado(s)!", "Alerta",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao gerar relatório: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GerarRelatorioReservas()
        {
            try
            {
                _relatorioAtual = "Relatório de Reservas";
                lblTituloRelatorio.Text = "RELATÓRIO: Reservas Ativas";
                lblTituloRelatorio.ForeColor = System.Drawing.Color.MediumPurple;

                var reservaDAL = new ReservaDAL();
                var reservas = reservaDAL.Listar()
                    .Where(r => r.Status == "Ativa")
                    .OrderBy(r => r.DataReserva)
                    .ToList();

                var dados = reservas.Select(r =>
                {
                    var aluno = _alunoDAL.ObterPorId(r.IdAluno);
                    var livro = _livroDAL.ObterPorId(r.IdLivro);
                    var diasEspera = (DateTime.Now.Date - r.DataReserva.Date).Days;

                    return new
                    {
                        Aluno = aluno?.Nome ?? "N/A",
                        Matrícula = aluno?.Matricula ?? "N/A",
                        Livro = livro?.Titulo ?? "N/A",
                        DataReserva = r.DataReserva.ToString("dd/MM/yyyy HH:mm"),
                        DiasEspera = diasEspera,
                        Status = r.Status
                    };
                }).ToList();

                dgvRelatorio.DataSource = dados;
                btnExportar.Enabled = dados.Count > 0;

                MessageBox.Show($"Relatório gerado: {dados.Count} reserva(s) ativa(s).", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao gerar relatório: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GerarRelatorioEstatisticas()
        {
            try
            {
                _relatorioAtual = "Estatísticas Gerais";
                lblTituloRelatorio.Text = "RELATÓRIO: Estatísticas Gerais do Sistema";
                lblTituloRelatorio.ForeColor = System.Drawing.Color.DarkSlateBlue;

                var statsEmprestimos = _emprestimoService.ObterEstatisticas();
                var statsLivros = _livroService.ObterEstatisticas();
                var statsAlunos = _alunoService.ObterEstatisticas();

                var dados = new[]
                {
                    new { Categoria = "EMPRÉSTIMOS", Descrição = "Total de empréstimos", Valor = statsEmprestimos.Total.ToString() },
                    new { Categoria = "EMPRÉSTIMOS", Descrição = "Empréstimos ativos", Valor = statsEmprestimos.Ativos.ToString() },
                    new { Categoria = "EMPRÉSTIMOS", Descrição = "Empréstimos atrasados", Valor = statsEmprestimos.Atrasados.ToString() },
                    new { Categoria = "EMPRÉSTIMOS", Descrição = "Multa total acumulada", Valor = $"R$ {statsEmprestimos.MultaTotal:F2}" },
                    new { Categoria = "LIVROS", Descrição = "Total de livros no acervo", Valor = statsLivros.TotalLivros.ToString() },
                    new { Categoria = "LIVROS", Descrição = "Exemplares disponíveis", Valor = statsLivros.ExemplaresDisponiveis.ToString() },
                    new { Categoria = "LIVROS", Descrição = "Exemplares emprestados", Valor = statsLivros.ExemplaresEmprestados.ToString() },
                    new { Categoria = "ALUNOS", Descrição = "Total de alunos cadastrados", Valor = statsAlunos.TotalAlunos.ToString() },
                    new { Categoria = "ALUNOS", Descrição = "Alunos com empréstimos", Valor = statsAlunos.ComEmprestimos.ToString() },
                    new { Categoria = "ALUNOS", Descrição = "Alunos com atrasos", Valor = statsAlunos.ComAtrasos.ToString() }
                }.ToList();

                dgvRelatorio.DataSource = dados;
                btnExportar.Enabled = true;

                MessageBox.Show("Estatísticas gerais do sistema geradas com sucesso!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao gerar relatório: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExportar_Click(object? sender, EventArgs e)
        {
            try
            {
                if (dgvRelatorio.Rows.Count == 0)
                {
                    MessageBox.Show("Não há dados para exportar.", "Atenção",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var saveDialog = new SaveFileDialog
                {
                    Filter = "Arquivo CSV (*.csv)|*.csv|Arquivo de Texto (*.txt)|*.txt",
                    FileName = $"{_relatorioAtual.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}",
                    Title = "Exportar Relatório"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportarParaCSV(saveDialog.FileName);
                    MessageBox.Show($"Relatório exportado com sucesso!\n\n{saveDialog.FileName}",
                        "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao exportar relatório: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportarParaCSV(string caminhoArquivo)
        {
            var sb = new StringBuilder();

            // Cabeçalhos
            var colunas = dgvRelatorio.Columns.Cast<DataGridViewColumn>()
                .Where(c => c.Visible)
                .Select(c => c.HeaderText);
            sb.AppendLine(string.Join(";", colunas));

            // Dados
            foreach (DataGridViewRow row in dgvRelatorio.Rows)
            {
                if (!row.IsNewRow)
                {
                    var valores = dgvRelatorio.Columns.Cast<DataGridViewColumn>()
                        .Where(c => c.Visible)
                        .Select(c => row.Cells[c.Index].Value?.ToString() ?? "");
                    sb.AppendLine(string.Join(";", valores));
                }
            }

            // Adicionar rodapé
            sb.AppendLine();
            sb.AppendLine($"Relatório gerado em: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            sb.AppendLine($"Usuário: {_funcionarioLogado.Nome}");
            sb.AppendLine($"Sistema: BibliotecaJK v3.0");

            File.WriteAllText(caminhoArquivo, sb.ToString(), Encoding.UTF8);
        }
    }
}
