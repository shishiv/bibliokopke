using System;
using System.Windows.Forms;
using BibliotecaJK.Model;
using BibliotecaJK.BLL;

namespace BibliotecaJK.Forms
{
    /// <summary>
    /// Formulário Principal do Sistema BibliotecaJK
    /// Menu principal e dashboard com estatísticas
    /// </summary>
    public partial class FormPrincipal : Form
    {
        private readonly Funcionario _funcionarioLogado;
        private readonly EmprestimoService _emprestimoService;
        private readonly LivroService _livroService;
        private readonly AlunoService _alunoService;

        public FormPrincipal(Funcionario funcionario)
        {
            _funcionarioLogado = funcionario;
            _emprestimoService = new EmprestimoService();
            _livroService = new LivroService();
            _alunoService = new AlunoService();

            InitializeComponent();
            AtualizarDashboard();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // FormPrincipal
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "BibliotecaJK - Sistema de Gerenciamento";
            this.BackColor = System.Drawing.Color.WhiteSmoke;

            // Menu Superior
            var menuStrip = new MenuStrip
            {
                BackColor = System.Drawing.Color.DarkSlateBlue,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 10F)
            };

            // Menu Cadastros
            var menuCadastros = new ToolStripMenuItem("Cadastros");
            menuCadastros.DropDownItems.Add("Alunos", null, (s, e) => AbrirCadastroAlunos());
            menuCadastros.DropDownItems.Add("Livros", null, (s, e) => AbrirCadastroLivros());
            menuStrip.Items.Add(menuCadastros);

            // Menu Empréstimos
            var menuEmprestimos = new ToolStripMenuItem("Empréstimos");
            menuEmprestimos.DropDownItems.Add("Novo Empréstimo", null, (s, e) => AbrirNovoEmprestimo());
            menuEmprestimos.DropDownItems.Add("Devoluções", null, (s, e) => AbrirDevolucoes());
            menuEmprestimos.DropDownItems.Add("Consultar Empréstimos", null, (s, e) => AbrirConsultaEmprestimos());
            menuStrip.Items.Add(menuEmprestimos);

            // Menu Reservas
            var menuReservas = new ToolStripMenuItem("Reservas");
            menuReservas.DropDownItems.Add("Gerenciar Reservas", null, (s, e) => AbrirReservas());
            menuStrip.Items.Add(menuReservas);

            // Menu Relatórios
            var menuRelatorios = new ToolStripMenuItem("Relatórios");
            menuRelatorios.DropDownItems.Add("Relatórios Gerenciais", null, (s, e) => AbrirRelatorios());
            menuStrip.Items.Add(menuRelatorios);

            // Menu Sair
            var menuSair = new ToolStripMenuItem("Sair");
            menuSair.Click += (s, e) => this.Close();
            menuStrip.Items.Add(menuSair);

            this.Controls.Add(menuStrip);
            this.MainMenuStrip = menuStrip;

            // Panel de Boas-vindas
            var pnlHeader = new Panel
            {
                Location = new System.Drawing.Point(0, 28),
                Size = new System.Drawing.Size(1000, 80),
                BackColor = System.Drawing.Color.White
            };

            lblBoasVindas = new Label
            {
                Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                Location = new System.Drawing.Point(20, 15),
                Size = new System.Drawing.Size(600, 30)
            };
            pnlHeader.Controls.Add(lblBoasVindas);

            lblPerfil = new Label
            {
                Font = new System.Drawing.Font("Segoe UI", 10F),
                ForeColor = System.Drawing.Color.Gray,
                Location = new System.Drawing.Point(20, 45),
                Size = new System.Drawing.Size(600, 25)
            };
            pnlHeader.Controls.Add(lblPerfil);

            this.Controls.Add(pnlHeader);

            // Panel de Dashboard
            var pnlDashboard = new Panel
            {
                Location = new System.Drawing.Point(20, 130),
                Size = new System.Drawing.Size(960, 540),
                BackColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Título Dashboard
            var lblTituloDashboard = new Label
            {
                Text = "DASHBOARD - ESTATÍSTICAS DO SISTEMA",
                Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                Location = new System.Drawing.Point(20, 15),
                Size = new System.Drawing.Size(920, 30)
            };
            pnlDashboard.Controls.Add(lblTituloDashboard);

            // Cards de Estatísticas
            int cardY = 60;
            int cardSpacing = 140;

            // Card 1: Empréstimos
            var cardEmprestimos = CriarCard("EMPRÉSTIMOS", cardY, System.Drawing.Color.MediumSeaGreen);
            lblEmprestimosAtivos = CriarLabelCard("0", 35, cardEmprestimos);
            CriarLabelCard("Ativos", 65, cardEmprestimos, 9F);
            lblEmprestimosAtrasados = CriarLabelCard("0 atrasados", 85, cardEmprestimos, 8F, System.Drawing.Color.OrangeRed);
            pnlDashboard.Controls.Add(cardEmprestimos);

            // Card 2: Livros
            var cardLivros = CriarCard("LIVROS", cardY + cardSpacing, System.Drawing.Color.SteelBlue);
            lblTotalLivros = CriarLabelCard("0", 35, cardLivros);
            lblLivrosDisponiveis = CriarLabelCard("0 disponíveis", 65, cardLivros, 9F);
            lblLivrosEmprestados = CriarLabelCard("0 emprestados", 85, cardLivros, 8F, System.Drawing.Color.Gray);
            pnlDashboard.Controls.Add(cardLivros);

            // Card 3: Alunos
            var cardAlunos = CriarCard("ALUNOS", cardY + (cardSpacing * 2), System.Drawing.Color.MediumPurple);
            lblTotalAlunos = CriarLabelCard("0", 35, cardAlunos);
            lblAlunosComEmprestimos = CriarLabelCard("0 c/ empréstimos", 65, cardAlunos, 9F);
            lblAlunosComAtrasos = CriarLabelCard("0 c/ atrasos", 85, cardAlunos, 8F, System.Drawing.Color.OrangeRed);
            pnlDashboard.Controls.Add(cardAlunos);

            // Card 4: Multas
            var cardMultas = CriarCard("MULTAS", cardY + (cardSpacing * 3), System.Drawing.Color.Crimson);
            lblMultaTotal = CriarLabelCard("R$ 0,00", 35, cardMultas);
            CriarLabelCard("Total Acumulado", 65, cardMultas, 9F);
            pnlDashboard.Controls.Add(cardMultas);

            // Botão Atualizar
            var btnAtualizar = new Button
            {
                Text = "🔄 Atualizar Dashboard",
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(740, 490),
                Size = new System.Drawing.Size(200, 35),
                BackColor = System.Drawing.Color.DarkSlateBlue,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAtualizar.FlatAppearance.BorderSize = 0;
            btnAtualizar.Click += (s, e) => AtualizarDashboard();
            pnlDashboard.Controls.Add(btnAtualizar);

            this.Controls.Add(pnlDashboard);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Label lblBoasVindas = new Label();
        private Label lblPerfil = new Label();
        private Label lblEmprestimosAtivos = new Label();
        private Label lblEmprestimosAtrasados = new Label();
        private Label lblTotalLivros = new Label();
        private Label lblLivrosDisponiveis = new Label();
        private Label lblLivrosEmprestados = new Label();
        private Label lblTotalAlunos = new Label();
        private Label lblAlunosComEmprestimos = new Label();
        private Label lblAlunosComAtrasos = new Label();
        private Label lblMultaTotal = new Label();

        private Panel CriarCard(string titulo, int y, System.Drawing.Color cor)
        {
            var card = new Panel
            {
                Location = new System.Drawing.Point(20, y),
                Size = new System.Drawing.Size(900, 120),
                BackColor = cor,
                BorderStyle = BorderStyle.None
            };

            var lblTitulo = new Label
            {
                Text = titulo,
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(15, 10),
                Size = new System.Drawing.Size(870, 20),
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            };
            card.Controls.Add(lblTitulo);

            return card;
        }

        private Label CriarLabelCard(string texto, int y, Panel card,
            float fontSize = 18F, System.Drawing.Color? cor = null)
        {
            var lbl = new Label
            {
                Text = texto,
                Font = new System.Drawing.Font("Segoe UI", fontSize, System.Drawing.FontStyle.Bold),
                ForeColor = cor ?? System.Drawing.Color.White,
                Location = new System.Drawing.Point(15, y),
                Size = new System.Drawing.Size(870, 20),
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            };
            card.Controls.Add(lbl);
            return lbl;
        }

        private void AtualizarDashboard()
        {
            try
            {
                // Atualizar informações do usuário
                lblBoasVindas.Text = $"Bem-vindo(a), {_funcionarioLogado.Nome}!";
                lblPerfil.Text = $"Perfil: {_funcionarioLogado.Perfil} | Login: {_funcionarioLogado.Login}";

                // Estatísticas de Empréstimos
                var statsEmprestimos = _emprestimoService.ObterEstatisticas();
                lblEmprestimosAtivos.Text = statsEmprestimos.Ativos.ToString();
                lblEmprestimosAtrasados.Text = $"{statsEmprestimos.Atrasados} atrasados";
                lblMultaTotal.Text = $"R$ {statsEmprestimos.MultaTotal:F2}";

                // Estatísticas de Livros
                var statsLivros = _livroService.ObterEstatisticas();
                lblTotalLivros.Text = statsLivros.TotalLivros.ToString();
                lblLivrosDisponiveis.Text = $"{statsLivros.ExemplaresDisponiveis} disponíveis";
                lblLivrosEmprestados.Text = $"{statsLivros.ExemplaresEmprestados} emprestados";

                // Estatísticas de Alunos
                var statsAlunos = _alunoService.ObterEstatisticas();
                lblTotalAlunos.Text = statsAlunos.TotalAlunos.ToString();
                lblAlunosComEmprestimos.Text = $"{statsAlunos.ComEmprestimos} c/ empréstimos";
                lblAlunosComAtrasos.Text = $"{statsAlunos.ComAtrasos} c/ atrasos";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar dashboard: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AbrirCadastroAlunos()
        {
            var form = new FormCadastroAluno(_funcionarioLogado);
            form.ShowDialog();
            AtualizarDashboard();
        }

        private void AbrirCadastroLivros()
        {
            var form = new FormCadastroLivro(_funcionarioLogado);
            form.ShowDialog();
            AtualizarDashboard();
        }

        private void AbrirNovoEmprestimo()
        {
            var form = new FormEmprestimo(_funcionarioLogado);
            form.ShowDialog();
            AtualizarDashboard();
        }

        private void AbrirDevolucoes()
        {
            var form = new FormDevolucao(_funcionarioLogado);
            form.ShowDialog();
            AtualizarDashboard();
        }

        private void AbrirConsultaEmprestimos()
        {
            var form = new FormConsultaEmprestimos(_funcionarioLogado);
            form.ShowDialog();
        }

        private void AbrirReservas()
        {
            var form = new FormReserva(_funcionarioLogado);
            form.ShowDialog();
            AtualizarDashboard();
        }

        private void AbrirRelatorios()
        {
            var form = new FormRelatorios(_funcionarioLogado);
            form.ShowDialog();
            AtualizarDashboard();
        }
    }
}
