using System;
using System.Drawing;
using System.Windows.Forms;
using BibliotecaJK.Model;
using BibliotecaJK.BLL;
using BibliotecaJK.DAL;
using BibliotecaJK.Components;

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
        private readonly ReservaService _reservaService;
        private readonly NotificacaoDAL _notificacaoDAL;
        private Label lblNotificacaoBadge = new Label();
        private System.Windows.Forms.Timer timerNotificacoes = new System.Windows.Forms.Timer();
        private KeyboardShortcutManager _shortcutManager = null!;
        private ToolTip _tooltip = new ToolTip();
        private StatusStrip _statusStrip = new StatusStrip();
        private ToolStripStatusLabel _lblStatus = null!;
        private ToolStripStatusLabel _lblUsuario = null!;
        private ToolStripStatusLabel _lblHora = null!;
        private Panel cardEmprestimos = null!;
        private Panel cardLivros = null!;
        private Panel cardAlunos = null!;
        private Panel cardMultas = null!;
        private Panel cardEmprestados = null!;
        private Panel cardAtrasos = null!;
        private Panel cardReservas = null!;
        private Panel cardAcoesHoje = null!;
        private Panel cardTaxaUso = null!;

        public FormPrincipal(Funcionario funcionario)
        {
            _funcionarioLogado = funcionario;
            _emprestimoService = new EmprestimoService();
            _livroService = new LivroService();
            _alunoService = new AlunoService();
            _reservaService = new ReservaService();
            _notificacaoDAL = new NotificacaoDAL();

            InitializeComponent();
            ConfigurarAtalhosTeclado();
            ConfigurarTooltips();
            ConfigurarStatusBar();
            AtualizarDashboard();
            AtualizarNotificacoes();

            // Atualizar notificações a cada 1 minuto
            timerNotificacoes.Interval = 60000;
            timerNotificacoes.Tick += (s, e) => AtualizarNotificacoes();
            timerNotificacoes.Start();

            // Toast de boas-vindas
            ToastNotification.Info($"Bem-vindo(a), {_funcionarioLogado.Nome}!");
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // FormPrincipal - Adaptive sizing para diferentes resoluções
            // Detectar área de trabalho disponível
            Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;

            // Calcular tamanho ideal (90% da tela, mas não menor que mínimo)
            int idealWidth = Math.Max(1200, (int)(workingArea.Width * 0.9));
            int idealHeight = Math.Max(650, (int)(workingArea.Height * 0.9));

            // Limitar ao máximo disponível
            int formWidth = Math.Min(idealWidth, workingArea.Width - 50);
            int formHeight = Math.Min(idealHeight, workingArea.Height - 50);

            this.ClientSize = new Size(formWidth, formHeight);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "BibliotecaJK - Sistema de Gerenciamento";
            this.BackColor = Color.FromArgb(245, 245, 250);
            this.MinimumSize = new Size(1200, 650);
            this.AutoScroll = true;

            // ==================== SIDEBAR ====================
            var pnlSidebar = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(250, 800),
                BackColor = Color.FromArgb(45, 52, 71),
                Dock = DockStyle.Left
            };

            // Logo e Título
            var lblLogo = new Label
            {
                Text = "📚 BibliotecaJK",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(10, 20),
                Size = new Size(230, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlSidebar.Controls.Add(lblLogo);

            // Separador
            var sep1 = new Panel
            {
                Location = new Point(20, 70),
                Size = new Size(210, 1),
                BackColor = Color.FromArgb(100, 100, 120)
            };
            pnlSidebar.Controls.Add(sep1);

            int btnY = 90;
            int btnHeight = 50;
            int btnSpacing = 5;

            // Botão Dashboard
            var btnDashboard = CriarBotaoSidebar("🏠  Dashboard", btnY);
            btnDashboard.Click += (s, e) => AtualizarDashboard();
            pnlSidebar.Controls.Add(btnDashboard);
            btnY += btnHeight + btnSpacing;

            // Botão Notificações com Badge
            var btnNotificacoes = CriarBotaoSidebar("🔔  Notificações", btnY);
            btnNotificacoes.Click += (s, e) => AbrirNotificacoes();
            pnlSidebar.Controls.Add(btnNotificacoes);

            // Badge de notificações
            lblNotificacaoBadge = new Label
            {
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(190, btnY + 15),
                Size = new Size(40, 20),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(244, 67, 54),
                Visible = false
            };
            pnlSidebar.Controls.Add(lblNotificacaoBadge);
            btnY += btnHeight + btnSpacing;

            // Separador
            var sep2 = new Panel
            {
                Location = new Point(20, btnY),
                Size = new Size(210, 1),
                BackColor = Color.FromArgb(100, 100, 120)
            };
            pnlSidebar.Controls.Add(sep2);
            btnY += 15;

            // Botão Alunos
            var btnAlunos = CriarBotaoSidebar("👥  Alunos", btnY);
            btnAlunos.Click += (s, e) => AbrirCadastroAlunos();
            pnlSidebar.Controls.Add(btnAlunos);
            btnY += btnHeight + btnSpacing;

            // Botão Livros
            var btnLivros = CriarBotaoSidebar("📖  Livros", btnY);
            btnLivros.Click += (s, e) => AbrirCadastroLivros();
            pnlSidebar.Controls.Add(btnLivros);
            btnY += btnHeight + btnSpacing;

            // Separador
            var sep3 = new Panel
            {
                Location = new Point(20, btnY),
                Size = new Size(210, 1),
                BackColor = Color.FromArgb(100, 100, 120)
            };
            pnlSidebar.Controls.Add(sep3);
            btnY += 15;

            // Botão Novo Empréstimo
            var btnNovoEmprestimo = CriarBotaoSidebar("📤  Novo Empréstimo", btnY);
            btnNovoEmprestimo.Click += (s, e) => AbrirNovoEmprestimo();
            pnlSidebar.Controls.Add(btnNovoEmprestimo);
            btnY += btnHeight + btnSpacing;

            // Botão Devoluções
            var btnDevolucoes = CriarBotaoSidebar("📥  Devoluções", btnY);
            btnDevolucoes.Click += (s, e) => AbrirDevolucoes();
            pnlSidebar.Controls.Add(btnDevolucoes);
            btnY += btnHeight + btnSpacing;

            // Botão Empréstimos
            var btnEmprestimos = CriarBotaoSidebar("📋  Consultar Empréstimos", btnY);
            btnEmprestimos.Click += (s, e) => AbrirConsultaEmprestimos();
            pnlSidebar.Controls.Add(btnEmprestimos);
            btnY += btnHeight + btnSpacing;

            // Botão Reservas
            var btnReservas = CriarBotaoSidebar("⏳  Reservas", btnY);
            btnReservas.Click += (s, e) => AbrirReservas();
            pnlSidebar.Controls.Add(btnReservas);
            btnY += btnHeight + btnSpacing;

            // Separador
            var sep4 = new Panel
            {
                Location = new Point(20, btnY),
                Size = new Size(210, 1),
                BackColor = Color.FromArgb(100, 100, 120)
            };
            pnlSidebar.Controls.Add(sep4);
            btnY += 15;

            // Botão Relatórios
            var btnRelatorios = CriarBotaoSidebar("📊  Relatórios", btnY);
            btnRelatorios.Click += (s, e) => AbrirRelatorios();
            pnlSidebar.Controls.Add(btnRelatorios);
            btnY += btnHeight + btnSpacing;

            // Botão Backup
            var btnBackup = CriarBotaoSidebar("💾  Backup", btnY);
            btnBackup.Click += (s, e) => AbrirBackup();
            pnlSidebar.Controls.Add(btnBackup);
            btnY += btnHeight + btnSpacing;

            // Separador
            var sep5 = new Panel
            {
                Location = new Point(20, btnY),
                Size = new Size(210, 1),
                BackColor = Color.FromArgb(100, 100, 120)
            };
            pnlSidebar.Controls.Add(sep5);
            btnY += 15;

            // Botão Modo Escuro
            var btnModoEscuro = ThemeManager.CreateThemeToggleButton();
            btnModoEscuro.Location = new Point(10, btnY);
            btnModoEscuro.Size = new Size(230, 45);
            btnModoEscuro.BackColor = Color.FromArgb(100, 67, 86);
            pnlSidebar.Controls.Add(btnModoEscuro);
            btnY += 50;

            // Botão Ajuda/Atalhos
            var btnAjuda = CriarBotaoSidebar("❓  Atalhos (F1)", btnY);
            btnAjuda.Click += (s, e) => _shortcutManager?.ShowShortcutsHelp();
            pnlSidebar.Controls.Add(btnAjuda);
            btnY += btnHeight + btnSpacing;

            // Botão Sair (no final)
            var btnSair = CriarBotaoSidebar("🚪  Sair", 720);
            btnSair.BackColor = Color.FromArgb(183, 28, 28);
            btnSair.Click += (s, e) => {
                if (MessageBox.Show("Deseja realmente sair?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    this.Close();
            };
            pnlSidebar.Controls.Add(btnSair);

            this.Controls.Add(pnlSidebar);

            // ==================== HEADER ====================
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(30, 15, 30, 10)
            };

            lblBoasVindas = new Label
            {
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(63, 81, 181),
                Location = new Point(30, 15),
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            pnlHeader.Controls.Add(lblBoasVindas);

            lblPerfil = new Label
            {
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.Gray,
                Location = new Point(30, 45),
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            pnlHeader.Controls.Add(lblPerfil);

            // Botão Atualizar Dashboard (pequeno, ancorado no topo direito)
            var btnAtualizarTopo = new Button
            {
                Text = "🔄 Atualizar",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(63, 81, 181),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            // Posição calculada dinamicamente para ficar à direita
            btnAtualizarTopo.Location = new Point(pnlHeader.ClientSize.Width - 260, 15);
            btnAtualizarTopo.FlatAppearance.BorderSize = 0;
            btnAtualizarTopo.FlatAppearance.MouseOverBackColor = Color.FromArgb(83, 101, 201);
            btnAtualizarTopo.Click += (s, e) => AtualizarDashboard();
            pnlHeader.Controls.Add(btnAtualizarTopo);

            // Botão Criar Usuário (apenas para ADMIN, ancorado no topo direito)
            if (_funcionarioLogado.Perfil == Constants.PerfilFuncionario.ADMIN)
            {
                var btnCriarUsuario = new Button
                {
                    Text = "➕ Novo Usuário",
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    Size = new Size(130, 35),
                    BackColor = Color.FromArgb(76, 175, 80),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand,
                    Anchor = AnchorStyles.Top | AnchorStyles.Right
                };
                btnCriarUsuario.Location = new Point(pnlHeader.ClientSize.Width - 130, 15);
                btnCriarUsuario.FlatAppearance.BorderSize = 0;
                btnCriarUsuario.FlatAppearance.MouseOverBackColor = Color.FromArgb(96, 195, 100);
                btnCriarUsuario.Click += (s, e) => AbrirCadastroUsuario();
                pnlHeader.Controls.Add(btnCriarUsuario);
            }

            this.Controls.Add(pnlHeader);

            // ==================== ÁREA DE CONTEÚDO ====================
            var pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 245, 250),
                AutoScroll = true,
                Padding = new Padding(20)
            };

            // Título Dashboard
            var lblTituloDashboard = new Label
            {
                Text = "ESTATÍSTICAS DO SISTEMA",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(63, 81, 181),
                Dock = DockStyle.Top,
                Height = 40,
                Padding = new Padding(0, 10, 0, 10)
            };
            pnlContent.Controls.Add(lblTituloDashboard);

            // Dashboard Cards Container - TableLayoutPanel responsivo
            var dashboardLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 3,
                AutoScroll = true,
                Padding = new Padding(0, 10, 0, 10)
            };

            // Configurar colunas com largura percentual
            dashboardLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            dashboardLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            dashboardLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));

            // Configurar linhas com altura automática
            dashboardLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            dashboardLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            dashboardLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // Suspender layout do TableLayoutPanel para performance
            dashboardLayout.SuspendLayout();

            // ========== LINHA 1 ==========

            // Card 1: Empréstimos Ativos (coluna 0, linha 0)
            cardEmprestimos = CriarCardResponsivo("EMPRÉSTIMOS ATIVOS", Color.FromArgb(76, 175, 80), "📚");
            lblEmprestimosAtivos = CriarLabelCardModerno("0", 50, cardEmprestimos, 36F);
            lblEmprestimosAtrasados = CriarLabelCardModerno("0 atrasados", 100, cardEmprestimos, 12F, Color.FromArgb(255, 235, 238));
            var lblDetalhesEmprestimos = CriarLabelCardModerno("Ver todos →", 125, cardEmprestimos, 10F, Color.FromArgb(200, 230, 201));
            cardEmprestimos.Cursor = Cursors.Hand;
            cardEmprestimos.Click += (s, e) => AbrirConsultaEmprestimos();
            TornarCardClicavel(cardEmprestimos);
            dashboardLayout.Controls.Add(cardEmprestimos, 0, 0);

            // Card 2: Livros Cadastrados (coluna 1, linha 0)
            cardLivros = CriarCardResponsivo("LIVROS NO ACERVO", Color.FromArgb(33, 150, 243), "📖");
            lblTotalLivros = CriarLabelCardModerno("0", 50, cardLivros, 36F);
            lblLivrosDisponiveis = CriarLabelCardModerno("0 disponíveis", 100, cardLivros, 12F, Color.FromArgb(227, 242, 253));
            var lblDetalhesLivros = CriarLabelCardModerno("Ver catálogo →", 125, cardLivros, 10F, Color.FromArgb(187, 222, 251));
            cardLivros.Cursor = Cursors.Hand;
            cardLivros.Click += (s, e) => AbrirCadastroLivros();
            TornarCardClicavel(cardLivros);
            dashboardLayout.Controls.Add(cardLivros, 1, 0);

            // Card 3: Alunos Cadastrados (coluna 2, linha 0)
            cardAlunos = CriarCardResponsivo("ALUNOS CADASTRADOS", Color.FromArgb(156, 39, 176), "👥");
            lblTotalAlunos = CriarLabelCardModerno("0", 50, cardAlunos, 36F);
            lblAlunosComEmprestimos = CriarLabelCardModerno("0 com empréstimos ativos", 100, cardAlunos, 12F, Color.FromArgb(243, 229, 245));
            var lblDetalhesAlunos = CriarLabelCardModerno("Ver lista →", 125, cardAlunos, 10F, Color.FromArgb(225, 190, 231));
            cardAlunos.Cursor = Cursors.Hand;
            cardAlunos.Click += (s, e) => AbrirCadastroAlunos();
            TornarCardClicavel(cardAlunos);
            dashboardLayout.Controls.Add(cardAlunos, 2, 0);

            // ========== LINHA 2 ==========

            // Card 4: Livros Emprestados (coluna 0, linha 1)
            cardEmprestados = CriarCardResponsivo("EXEMPLARES EMPRESTADOS", Color.FromArgb(255, 152, 0), "📦");
            lblLivrosEmprestados = CriarLabelCardModerno("0", 50, cardEmprestados, 36F);
            var lblExemplaresTotal = CriarLabelCardModerno("de 0 exemplares totais", 100, cardEmprestados, 12F, Color.FromArgb(255, 243, 224));
            var lblDetalhesEmprestados = CriarLabelCardModerno("Ver detalhes →", 125, cardEmprestados, 10F, Color.FromArgb(255, 224, 178));
            cardEmprestados.Cursor = Cursors.Hand;
            cardEmprestados.Click += (s, e) => AbrirConsultaEmprestimos();
            TornarCardClicavel(cardEmprestados);
            dashboardLayout.Controls.Add(cardEmprestados, 0, 1);

            // Card 5: Multas Acumuladas (coluna 1, linha 1)
            cardMultas = CriarCardResponsivo("MULTAS PENDENTES", Color.FromArgb(244, 67, 54), "💰");
            lblMultaTotal = CriarLabelCardModerno("R$ 0,00", 50, cardMultas, 32F);
            var lblMultasEmprestimos = CriarLabelCardModerno("0 empréstimos com multa", 100, cardMultas, 12F, Color.FromArgb(255, 235, 238));
            var lblDetalhesMultas = CriarLabelCardModerno("Ver cobranças →", 125, cardMultas, 10F, Color.FromArgb(255, 205, 210));
            cardMultas.Cursor = Cursors.Hand;
            cardMultas.Click += (s, e) => AbrirConsultaEmprestimos();
            TornarCardClicavel(cardMultas);
            dashboardLayout.Controls.Add(cardMultas, 1, 1);

            // Card 6: Alunos com Atrasos (coluna 2, linha 1)
            cardAtrasos = CriarCardResponsivo("ATRASOS E PENDÊNCIAS", Color.FromArgb(255, 87, 34), "⚠️");
            lblAlunosComAtrasos = CriarLabelCardModerno("0", 50, cardAtrasos, 36F);
            var lblAlunosPendentes = CriarLabelCardModerno("alunos precisam devolver", 100, cardAtrasos, 12F, Color.FromArgb(255, 235, 238));
            var lblDetalhesAtrasos = CriarLabelCardModerno("Ação necessária →", 125, cardAtrasos, 10F, Color.FromArgb(255, 204, 188));
            cardAtrasos.Cursor = Cursors.Hand;
            cardAtrasos.Click += (s, e) => AbrirConsultaEmprestimos();
            TornarCardClicavel(cardAtrasos);
            dashboardLayout.Controls.Add(cardAtrasos, 2, 1);

            // ========== LINHA 3 ==========

            // Card 7: Reservas Ativas (coluna 0, linha 2)
            cardReservas = CriarCardResponsivo("RESERVAS ATIVAS", Color.FromArgb(103, 58, 183), "🔖");
            lblReservasAtivas = CriarLabelCardModerno("0", 50, cardReservas, 36F);
            lblReservasPendentes = CriarLabelCardModerno("0 aguardando disponibilidade", 100, cardReservas, 12F, Color.FromArgb(237, 231, 246));
            var lblDetalhesReservas = CriarLabelCardModerno("Gerenciar reservas →", 125, cardReservas, 10F, Color.FromArgb(209, 196, 233));
            cardReservas.Cursor = Cursors.Hand;
            cardReservas.Click += (s, e) => AbrirReservas();
            TornarCardClicavel(cardReservas);
            dashboardLayout.Controls.Add(cardReservas, 0, 2);

            // Card 8: Ações Hoje (coluna 1, linha 2)
            cardAcoesHoje = CriarCardResponsivo("MOVIMENTAÇÃO HOJE", Color.FromArgb(0, 150, 136), "📊");
            lblAcoesHoje = CriarLabelCardModerno("0", 50, cardAcoesHoje, 36F);
            lblDetalhesAcoes = CriarLabelCardModerno("empréstimos + devoluções", 100, cardAcoesHoje, 12F, Color.FromArgb(224, 242, 241));
            var lblVerLogs = CriarLabelCardModerno("Ver atividades →", 125, cardAcoesHoje, 10F, Color.FromArgb(178, 223, 219));
            cardAcoesHoje.Cursor = Cursors.Hand;
            cardAcoesHoje.Click += (s, e) => AbrirRelatorios();
            TornarCardClicavel(cardAcoesHoje);
            dashboardLayout.Controls.Add(cardAcoesHoje, 1, 2);

            // Card 9: Taxa de Uso (coluna 2, linha 2)
            cardTaxaUso = CriarCardResponsivo("TAXA DE UTILIZAÇÃO", Color.FromArgb(121, 85, 72), "📈");
            lblTaxaUso = CriarLabelCardModerno("0%", 50, cardTaxaUso, 36F);
            lblDetalheTaxaUso = CriarLabelCardModerno("do acervo está em uso", 100, cardTaxaUso, 12F, Color.FromArgb(239, 235, 233));
            var lblDetalhesTaxa = CriarLabelCardModerno("Ver estatísticas →", 125, cardTaxaUso, 10F, Color.FromArgb(215, 204, 200));
            cardTaxaUso.Cursor = Cursors.Hand;
            cardTaxaUso.Click += (s, e) => AbrirRelatorios();
            TornarCardClicavel(cardTaxaUso);
            dashboardLayout.Controls.Add(cardTaxaUso, 2, 2);

            // Retomar layout do TableLayoutPanel
            dashboardLayout.ResumeLayout(false);

            // Adicionar TableLayoutPanel ao content
            pnlContent.Controls.Add(dashboardLayout);
            this.Controls.Add(pnlContent);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Button CriarBotaoSidebar(string texto, int y)
        {
            var btn = new Button
            {
                Text = texto,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.White,
                Location = new Point(10, y),
                Size = new Size(230, 50),
                BackColor = Color.FromArgb(60, 67, 86),
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 77, 96);
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(80, 87, 106);
            return btn;
        }

        private Panel CriarCardModerno(string titulo, int x, int y, int width, int height, Color cor)
        {
            var card = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = cor,
                BorderStyle = BorderStyle.None
            };

            var lblTitulo = new Label
            {
                Text = titulo,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(15, 15),
                Size = new Size(width - 30, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };
            card.Controls.Add(lblTitulo);

            return card;
        }

        private Panel CriarCardDetalhadoModerno(string titulo, int x, int y, int width, int height, Color cor, string icone)
        {
            var card = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = cor,
                BorderStyle = BorderStyle.None
            };

            // Ícone no canto superior esquerdo
            var lblIcone = new Label
            {
                Text = icone,
                Font = new Font("Segoe UI", 24F),
                ForeColor = Color.White,
                Location = new Point(15, 10),
                Size = new Size(40, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };
            card.Controls.Add(lblIcone);

            // Título ao lado do ícone
            var lblTitulo = new Label
            {
                Text = titulo,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(60, 15),
                Size = new Size(width - 75, 25),
                TextAlign = ContentAlignment.MiddleLeft
            };
            card.Controls.Add(lblTitulo);

            return card;
        }

        /// <summary>
        /// Cria card responsivo para TableLayoutPanel - sem posições fixas
        /// </summary>
        private Panel CriarCardResponsivo(string titulo, Color cor, string icone)
        {
            var card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = cor,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(10),
                MinimumSize = new Size(180, 100),
                Padding = new Padding(5)
            };

            // Ícone no canto superior esquerdo
            var lblIcone = new Label
            {
                Text = icone,
                Font = new Font("Segoe UI", 24F),
                ForeColor = Color.White,
                Location = new Point(15, 10),
                Size = new Size(40, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };
            card.Controls.Add(lblIcone);

            // Título ao lado do ícone - usa Anchor para expansão
            var lblTitulo = new Label
            {
                Text = titulo,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(60, 15),
                AutoSize = false,
                Size = new Size(250, 25),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                TextAlign = ContentAlignment.MiddleLeft
            };
            card.Controls.Add(lblTitulo);

            return card;
        }

        private Label CriarLabelCardModerno(string texto, int y, Panel card, float fontSize = 14F, Color? cor = null)
        {
            var lbl = new Label
            {
                Text = texto,
                Font = new Font("Segoe UI", fontSize, FontStyle.Bold),
                ForeColor = cor ?? Color.White,
                Location = new Point(15, y),
                Size = new Size(card.Width - 30, (int)(fontSize * 2)),
                TextAlign = ContentAlignment.MiddleLeft
            };
            card.Controls.Add(lbl);
            return lbl;
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
        private Label lblReservasAtivas = new Label();
        private Label lblReservasPendentes = new Label();
        private Label lblAcoesHoje = new Label();
        private Label lblDetalhesAcoes = new Label();
        private Label lblTaxaUso = new Label();
        private Label lblDetalheTaxaUso = new Label();

        private void AtualizarNotificacoes()
        {
            try
            {
                int naoLidas = _notificacaoDAL.ContarNaoLidas();

                if (naoLidas > 0)
                {
                    lblNotificacaoBadge.Text = naoLidas > 99 ? "99+" : naoLidas.ToString();
                    lblNotificacaoBadge.Visible = true;
                }
                else
                {
                    lblNotificacaoBadge.Visible = false;
                }
            }
            catch
            {
                // Silenciar erros de atualização de notificações
            }
        }

        private void AbrirNotificacoes()
        {
            var form = new FormNotificacoes();
            form.FormClosed += (s, e) => AtualizarNotificacoes();
            form.ShowDialog();
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
                lblLivrosEmprestados.Text = statsLivros.ExemplaresEmprestados.ToString();

                // Estatísticas de Alunos
                var statsAlunos = _alunoService.ObterEstatisticas();
                lblTotalAlunos.Text = statsAlunos.TotalAlunos.ToString();
                lblAlunosComEmprestimos.Text = $"{statsAlunos.ComEmprestimos} com empréstimos ativos";
                lblAlunosComAtrasos.Text = statsAlunos.ComAtrasos.ToString();

                // Estatísticas de Reservas
                var statsReservas = _reservaService.ObterEstatisticas();
                lblReservasAtivas.Text = statsReservas.Ativas.ToString();
                lblReservasPendentes.Text = $"{statsReservas.Ativas} aguardando disponibilidade";

                // Ações Hoje (empréstimos do dia)
                var statsHoje = _emprestimoService.ObterEstatisticas(DateTime.Today, DateTime.Today);
                lblAcoesHoje.Text = statsHoje.Total.ToString();
                lblDetalhesAcoes.Text = $"{statsHoje.Total} operações realizadas hoje";

                // Taxa de Utilização
                if (statsLivros.TotalExemplares > 0)
                {
                    decimal taxaUso = (decimal)statsLivros.ExemplaresEmprestados / statsLivros.TotalExemplares * 100;
                    lblTaxaUso.Text = $"{taxaUso:F1}%";
                    lblDetalheTaxaUso.Text = $"do acervo está em uso";
                }
                else
                {
                    lblTaxaUso.Text = "0%";
                    lblDetalheTaxaUso.Text = "nenhum livro cadastrado";
                }

                ToastNotification.Success("Dashboard atualizado!");
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

        private void AbrirBackup()
        {
            var form = new FormBackup(_funcionarioLogado);
            form.ShowDialog();
        }

        private void AbrirCadastroUsuario()
        {
            // Verificar se é admin
            if (_funcionarioLogado.Perfil != Constants.PerfilFuncionario.ADMIN)
            {
                MessageBox.Show("Apenas administradores podem criar novos usuários.",
                    "Acesso Negado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Criar formulário de cadastro de funcionário
            var form = new FormCadastroFuncionario(_funcionarioLogado);
            form.ShowDialog();
            AtualizarDashboard();
        }

        /// <summary>
        /// Torna um card clicável propagando o click para todos os controles filhos
        /// </summary>
        private void TornarCardClicavel(Panel card)
        {
            foreach (Control control in card.Controls)
            {
                control.Cursor = Cursors.Hand;
                control.Click += (s, e) => {
                    // Disparar o evento Click do card usando reflexão
                    var onClickMethod = typeof(Control).GetMethod("OnClick",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    onClickMethod?.Invoke(card, new object[] { EventArgs.Empty });
                };
            }
        }

        /// <summary>
        /// Configura atalhos de teclado globais
        /// </summary>
        private void ConfigurarAtalhosTeclado()
        {
            _shortcutManager = new KeyboardShortcutManager(this);

            // Atalhos principais
            _shortcutManager.RegisterShortcut(Keys.F5, AtualizarDashboard, "Atualizar Dashboard");
            _shortcutManager.RegisterShortcut(Keys.F1, () => _shortcutManager.ShowShortcutsHelp(), "Mostrar Ajuda de Atalhos");

            // Navegação
            _shortcutManager.RegisterShortcut(Keys.Control | Keys.N, AbrirNovoEmprestimo, "Novo Empréstimo");
            _shortcutManager.RegisterShortcut(Keys.Control | Keys.D, AbrirDevolucoes, "Devoluções");
            _shortcutManager.RegisterShortcut(Keys.Control | Keys.E, AbrirConsultaEmprestimos, "Consultar Empréstimos");
            _shortcutManager.RegisterShortcut(Keys.Control | Keys.R, AbrirReservas, "Reservas");
            _shortcutManager.RegisterShortcut(Keys.Control | Keys.B, AbrirBackup, "Backup");

            // Cadastros
            _shortcutManager.RegisterShortcut(Keys.Alt | Keys.D1, AbrirCadastroAlunos, "Cadastro de Alunos");
            _shortcutManager.RegisterShortcut(Keys.Alt | Keys.D2, AbrirCadastroLivros, "Cadastro de Livros");

            // Notificações
            _shortcutManager.RegisterShortcut(Keys.Control | Keys.Shift | Keys.N, AbrirNotificacoes, "Central de Notificações");
        }

        /// <summary>
        /// Configura tooltips para todos os botões
        /// </summary>
        private void ConfigurarTooltips()
        {
            _tooltip.SetToolTip(lblNotificacaoBadge, "Clique para ver notificações não lidas");

            // Nota: Tooltips nos botões da sidebar podem ser adicionados dinamicamente
            // se necessário, mas como já têm labels descritivos, são opcionais
        }

        /// <summary>
        /// Configura a barra de status no rodapé
        /// </summary>
        private void ConfigurarStatusBar()
        {
            _lblStatus = new ToolStripStatusLabel
            {
                Text = "Pronto",
                Spring = true,
                TextAlign = ContentAlignment.MiddleLeft
            };

            _lblUsuario = new ToolStripStatusLabel
            {
                Text = $"Usuário: {_funcionarioLogado.Login} | Perfil: {_funcionarioLogado.Perfil}",
                BorderSides = ToolStripStatusLabelBorderSides.Left,
                BorderStyle = Border3DStyle.Etched
            };

            _lblHora = new ToolStripStatusLabel
            {
                Text = DateTime.Now.ToString("HH:mm:ss"),
                BorderSides = ToolStripStatusLabelBorderSides.Left,
                BorderStyle = Border3DStyle.Etched
            };

            _statusStrip.Items.AddRange(new ToolStripItem[] { _lblStatus, _lblUsuario, _lblHora });
            this.Controls.Add(_statusStrip);

            // Timer para atualizar hora
            var timerHora = new System.Windows.Forms.Timer { Interval = 1000 };
            timerHora.Tick += (s, e) => _lblHora.Text = DateTime.Now.ToString("HH:mm:ss");
            timerHora.Start();
        }

        /// <summary>
        /// Atualiza mensagem na barra de status
        /// </summary>
        public void AtualizarStatus(string mensagem)
        {
            if (_lblStatus != null)
                _lblStatus.Text = mensagem;
        }
    }
}
