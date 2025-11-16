using System;
using System.Drawing;
using System.Windows.Forms;
using BibliotecaJK.Model;
using BibliotecaJK.BLL;
using BibliotecaJK.DAL;
using BibliotecaJK.Components;
using BibliotecaJK.Helpers;

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

        // ModernCard components for dashboard
        private ModernCard cardEmprestimos = null!;
        private ModernCard cardLivros = null!;
        private ModernCard cardAlunos = null!;
        private ModernCard cardMultas = null!;
        private ModernCard cardEmprestados = null!;
        private ModernCard cardAtrasos = null!;
        private ModernCard cardReservas = null!;
        private ModernCard cardAcoesHoje = null!;
        private ModernCard cardTaxaUso = null!;

        // FlowLayoutPanel for responsive dashboard
        private FlowLayoutPanel pnlDashboardFlow = null!;
        private Panel pnlContent = null!;

        // Current screen size for responsive layout
        private LayoutManager.ScreenSize _currentScreenSize;

        public FormPrincipal(Funcionario funcionario)
        {
            _funcionarioLogado = funcionario;
            _emprestimoService = new EmprestimoService();
            _livroService = new LivroService();
            _alunoService = new AlunoService();
            _reservaService = new ReservaService();
            _notificacaoDAL = new NotificacaoDAL();

            InitializeComponent();

            // Make form responsive
            LayoutManager.MakeFormResponsive(this);

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

            // FormPrincipal - Tamanho maior e moderno
            this.Name = "FormPrincipal";
            this.ClientSize = new Size(1400, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "BibliotecaJK - Sistema de Gerenciamento";
            this.BackColor = Color.FromArgb(245, 245, 250);
            this.MinimumSize = new Size(1200, 700);

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
                Location = new Point(250, 0),
                Size = new Size(1150, 80),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            lblBoasVindas = new Label
            {
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(63, 81, 181),
                Location = new Point(30, 15),
                Size = new Size(800, 30),
                AutoSize = true
            };
            pnlHeader.Controls.Add(lblBoasVindas);

            lblPerfil = new Label
            {
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.Gray,
                Location = new Point(30, 45),
                Size = new Size(800, 25),
                AutoSize = true
            };
            pnlHeader.Controls.Add(lblPerfil);

            // Botão Atualizar Dashboard (pequeno, no topo direito)
            var btnAtualizarTopo = new Button
            {
                Text = "🔄 Atualizar",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Location = new Point(900, 15),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(63, 81, 181),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAtualizarTopo.FlatAppearance.BorderSize = 0;
            btnAtualizarTopo.FlatAppearance.MouseOverBackColor = Color.FromArgb(83, 101, 201);
            btnAtualizarTopo.Click += (s, e) => AtualizarDashboard();
            pnlHeader.Controls.Add(btnAtualizarTopo);

            // Botão Criar Usuário (apenas para ADMIN)
            if (_funcionarioLogado.Perfil == Constants.PerfilFuncionario.ADMIN)
            {
                var btnCriarUsuario = new Button
                {
                    Text = "➕ Novo Usuário",
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    Location = new Point(1030, 15),
                    Size = new Size(130, 35),
                    BackColor = Color.FromArgb(76, 175, 80),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btnCriarUsuario.FlatAppearance.BorderSize = 0;
                btnCriarUsuario.FlatAppearance.MouseOverBackColor = Color.FromArgb(96, 195, 100);
                btnCriarUsuario.Click += (s, e) => AbrirCadastroUsuario();
                pnlHeader.Controls.Add(btnCriarUsuario);
            }

            this.Controls.Add(pnlHeader);

            // ==================== ÁREA DE CONTEÚDO ====================
            pnlContent = new Panel
            {
                Location = new Point(250, 80),
                Size = new Size(1150, 720),
                BackColor = Color.FromArgb(245, 245, 250),
                AutoScroll = true
            };

            // Título Dashboard
            var lblTituloDashboard = new Label
            {
                Text = "ESTATÍSTICAS DO SISTEMA",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(63, 81, 181),
                Location = new Point(20, 20),
                Size = new Size(1110, 30)
            };
            pnlContent.Controls.Add(lblTituloDashboard);

            // FlowLayoutPanel for responsive dashboard
            pnlDashboardFlow = new FlowLayoutPanel
            {
                Location = new Point(20, 60),
                Size = new Size(1110, 640),
                BackColor = Color.Transparent,
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Padding = new Padding(0)
            };

            // Create dashboard cards
            CriarCardsDashboard();

            pnlContent.Controls.Add(pnlDashboardFlow);
            this.Controls.Add(pnlContent);

            // Add resize handler for responsive layout
            this.Resize += FormPrincipal_Resize;

            // Initial screen size calculation
            _currentScreenSize = LayoutManager.GetScreenSize(this.Width);
            AjustarLayoutResponsivo();

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void CriarCardsDashboard()
        {
            // Card 1: Empréstimos Ativos
            cardEmprestimos = CriarModernCard(
                "EMPRÉSTIMOS ATIVOS",
                "📚",
                ThemeManager.Colors.Success,
                Color.FromArgb(76, 175, 80)
            );
            lblEmprestimosAtivos = CriarLabelValor(cardEmprestimos, "0");
            lblEmprestimosAtrasados = CriarLabelDetalhe(cardEmprestimos, "0 atrasados", 95);
            cardEmprestimos.Cursor = Cursors.Hand;
            cardEmprestimos.Click += (s, e) => AbrirConsultaEmprestimos();
            TornarCardClicavel(cardEmprestimos);
            pnlDashboardFlow.Controls.Add(cardEmprestimos);

            // Card 2: Livros Cadastrados
            cardLivros = CriarModernCard(
                "LIVROS NO ACERVO",
                "📖",
                ThemeManager.Colors.Primary,
                Color.FromArgb(33, 150, 243)
            );
            lblTotalLivros = CriarLabelValor(cardLivros, "0");
            lblLivrosDisponiveis = CriarLabelDetalhe(cardLivros, "0 disponíveis", 95);
            cardLivros.Cursor = Cursors.Hand;
            cardLivros.Click += (s, e) => AbrirCadastroLivros();
            TornarCardClicavel(cardLivros);
            pnlDashboardFlow.Controls.Add(cardLivros);

            // Card 3: Alunos Cadastrados
            cardAlunos = CriarModernCard(
                "ALUNOS CADASTRADOS",
                "👥",
                ThemeManager.Colors.Secondary,
                Color.FromArgb(156, 39, 176)
            );
            lblTotalAlunos = CriarLabelValor(cardAlunos, "0");
            lblAlunosComEmprestimos = CriarLabelDetalhe(cardAlunos, "0 com empréstimos ativos", 95);
            cardAlunos.Cursor = Cursors.Hand;
            cardAlunos.Click += (s, e) => AbrirCadastroAlunos();
            TornarCardClicavel(cardAlunos);
            pnlDashboardFlow.Controls.Add(cardAlunos);

            // Card 4: Livros Emprestados
            cardEmprestados = CriarModernCard(
                "EXEMPLARES EMPRESTADOS",
                "📦",
                ThemeManager.Colors.Warning,
                Color.FromArgb(255, 152, 0)
            );
            lblLivrosEmprestados = CriarLabelValor(cardEmprestados, "0");
            lblExemplaresTotal = CriarLabelDetalhe(cardEmprestados, "de 0 exemplares totais", 95);
            cardEmprestados.Cursor = Cursors.Hand;
            cardEmprestados.Click += (s, e) => AbrirConsultaEmprestimos();
            TornarCardClicavel(cardEmprestados);
            pnlDashboardFlow.Controls.Add(cardEmprestados);

            // Card 5: Multas Acumuladas
            cardMultas = CriarModernCard(
                "MULTAS PENDENTES",
                "💰",
                ThemeManager.Colors.Error,
                Color.FromArgb(244, 67, 54)
            );
            lblMultaTotal = CriarLabelValor(cardMultas, "R$ 0,00");
            lblMultasEmprestimos = CriarLabelDetalhe(cardMultas, "0 empréstimos com multa", 95);
            cardMultas.Cursor = Cursors.Hand;
            cardMultas.Click += (s, e) => AbrirConsultaEmprestimos();
            TornarCardClicavel(cardMultas);
            pnlDashboardFlow.Controls.Add(cardMultas);

            // Card 6: Alunos com Atrasos
            cardAtrasos = CriarModernCard(
                "ATRASOS E PENDÊNCIAS",
                "⚠️",
                Color.FromArgb(255, 87, 34),
                Color.FromArgb(255, 87, 34)
            );
            lblAlunosComAtrasos = CriarLabelValor(cardAtrasos, "0");
            lblAlunosPendentes = CriarLabelDetalhe(cardAtrasos, "alunos precisam devolver", 95);
            cardAtrasos.Cursor = Cursors.Hand;
            cardAtrasos.Click += (s, e) => AbrirConsultaEmprestimos();
            TornarCardClicavel(cardAtrasos);
            pnlDashboardFlow.Controls.Add(cardAtrasos);

            // Card 7: Reservas Ativas
            cardReservas = CriarModernCard(
                "RESERVAS ATIVAS",
                "🔖",
                Color.FromArgb(103, 58, 183),
                Color.FromArgb(103, 58, 183)
            );
            lblReservasAtivas = CriarLabelValor(cardReservas, "0");
            lblReservasPendentes = CriarLabelDetalhe(cardReservas, "0 aguardando disponibilidade", 95);
            cardReservas.Cursor = Cursors.Hand;
            cardReservas.Click += (s, e) => AbrirReservas();
            TornarCardClicavel(cardReservas);
            pnlDashboardFlow.Controls.Add(cardReservas);

            // Card 8: Ações Hoje
            cardAcoesHoje = CriarModernCard(
                "MOVIMENTAÇÃO HOJE",
                "📊",
                ThemeManager.Colors.Info,
                Color.FromArgb(0, 150, 136)
            );
            lblAcoesHoje = CriarLabelValor(cardAcoesHoje, "0");
            lblDetalhesAcoes = CriarLabelDetalhe(cardAcoesHoje, "empréstimos + devoluções", 95);
            cardAcoesHoje.Cursor = Cursors.Hand;
            cardAcoesHoje.Click += (s, e) => AbrirRelatorios();
            TornarCardClicavel(cardAcoesHoje);
            pnlDashboardFlow.Controls.Add(cardAcoesHoje);

            // Card 9: Taxa de Uso
            cardTaxaUso = CriarModernCard(
                "TAXA DE UTILIZAÇÃO",
                "📈",
                Color.FromArgb(121, 85, 72),
                Color.FromArgb(121, 85, 72)
            );
            lblTaxaUso = CriarLabelValor(cardTaxaUso, "0%");
            lblDetalheTaxaUso = CriarLabelDetalhe(cardTaxaUso, "do acervo está em uso", 95);
            cardTaxaUso.Cursor = Cursors.Hand;
            cardTaxaUso.Click += (s, e) => AbrirRelatorios();
            TornarCardClicavel(cardTaxaUso);
            pnlDashboardFlow.Controls.Add(cardTaxaUso);
        }

        private ModernCard CriarModernCard(string titulo, string icone, Color accentColor, Color backgroundColor)
        {
            var card = new ModernCard
            {
                Elevation = 2,
                BackColor = Color.White,
                Size = new Size(350, 150),
                Margin = new Padding(8)
            };

            // Accent bar on left edge
            var accentBar = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(4, 150),
                BackColor = accentColor,
                Dock = DockStyle.Left
            };
            card.Controls.Add(accentBar);

            // Icon
            var lblIcone = new Label
            {
                Text = icone,
                Font = new Font("Segoe UI", 32F),
                ForeColor = backgroundColor,
                Location = new Point(20, 15),
                Size = new Size(50, 50),
                TextAlign = ContentAlignment.MiddleCenter
            };
            card.Controls.Add(lblIcone);

            // Title
            var lblTitulo = new Label
            {
                Text = titulo,
                Font = ThemeManager.Typography.Body2,
                ForeColor = Color.FromArgb(100, 100, 100),
                Location = new Point(80, 20),
                Size = new Size(260, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };
            card.Controls.Add(lblTitulo);

            return card;
        }

        private Label CriarLabelValor(ModernCard card, string texto)
        {
            var lbl = new Label
            {
                Text = texto,
                Font = ThemeManager.Typography.H2,
                ForeColor = Color.FromArgb(33, 33, 33),
                Location = new Point(80, 45),
                Size = new Size(260, 40),
                TextAlign = ContentAlignment.MiddleLeft
            };
            card.Controls.Add(lbl);
            return lbl;
        }

        private Label CriarLabelDetalhe(ModernCard card, string texto, int y)
        {
            var lbl = new Label
            {
                Text = texto,
                Font = ThemeManager.Typography.Body2,
                ForeColor = Color.FromArgb(120, 120, 120),
                Location = new Point(80, y),
                Size = new Size(260, 40),
                TextAlign = ContentAlignment.MiddleLeft
            };
            card.Controls.Add(lbl);
            return lbl;
        }

        private void FormPrincipal_Resize(object sender, EventArgs e)
        {
            var newScreenSize = LayoutManager.GetScreenSize(this.Width);
            if (newScreenSize != _currentScreenSize)
            {
                _currentScreenSize = newScreenSize;
                AjustarLayoutResponsivo();
            }
        }

        private void AjustarLayoutResponsivo()
        {
            if (pnlDashboardFlow == null) return;

            // Calculate card width based on screen size
            int containerWidth = pnlContent.Width - 40; // Account for padding
            int cardWidth;

            switch (_currentScreenSize)
            {
                case LayoutManager.ScreenSize.Small:
                    // 1 column - full width
                    cardWidth = containerWidth - 16;
                    break;
                case LayoutManager.ScreenSize.Medium:
                    // 2 columns - 50% width
                    cardWidth = (containerWidth / 2) - 16;
                    break;
                case LayoutManager.ScreenSize.Large:
                    // 3 columns - 33% width
                    cardWidth = (containerWidth / 3) - 16;
                    break;
                case LayoutManager.ScreenSize.ExtraLarge:
                    // 4 columns - 25% width
                    cardWidth = (containerWidth / 4) - 16;
                    break;
                default:
                    cardWidth = 350;
                    break;
            }

            // Update all cards with new width
            foreach (Control control in pnlDashboardFlow.Controls)
            {
                if (control is ModernCard card)
                {
                    card.Width = cardWidth;

                    // Update accent bar height to match card height
                    foreach (Control child in card.Controls)
                    {
                        if (child is Panel panel && panel.Dock == DockStyle.Left)
                        {
                            panel.Height = card.Height;
                        }
                    }
                }
            }
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

        private Label lblBoasVindas = new Label();
        private Label lblPerfil = new Label();
        private Label lblEmprestimosAtivos = new Label();
        private Label lblEmprestimosAtrasados = new Label();
        private Label lblTotalLivros = new Label();
        private Label lblLivrosDisponiveis = new Label();
        private Label lblLivrosEmprestados = new Label();
        private Label lblExemplaresTotal = new Label();
        private Label lblTotalAlunos = new Label();
        private Label lblAlunosComEmprestimos = new Label();
        private Label lblAlunosComAtrasos = new Label();
        private Label lblAlunosPendentes = new Label();
        private Label lblMultaTotal = new Label();
        private Label lblMultasEmprestimos = new Label();
        private Label lblReservasAtivas = new Label();
        private Label lblReservasPendentes = new Label();
        private Label lblAcoesHoje = new Label();
        private Label lblDetalhesAcoes = new Label();
        private Label lblTaxaUso = new Label();
        private Label lblDetalheTaxaUso = new Label();

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Restore form state
            FormStateManager.RestoreFormState(this);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            // Save form state
            FormStateManager.SaveFormState(this);
        }

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
                lblMultasEmprestimos.Text = $"{statsEmprestimos.ComMulta} empréstimos com multa";

                // Estatísticas de Livros
                var statsLivros = _livroService.ObterEstatisticas();
                lblTotalLivros.Text = statsLivros.TotalLivros.ToString();
                lblLivrosDisponiveis.Text = $"{statsLivros.ExemplaresDisponiveis} disponíveis";
                lblLivrosEmprestados.Text = statsLivros.ExemplaresEmprestados.ToString();
                lblExemplaresTotal.Text = $"de {statsLivros.TotalExemplares} exemplares totais";

                // Estatísticas de Alunos
                var statsAlunos = _alunoService.ObterEstatisticas();
                lblTotalAlunos.Text = statsAlunos.TotalAlunos.ToString();
                lblAlunosComEmprestimos.Text = $"{statsAlunos.ComEmprestimos} com empréstimos ativos";
                lblAlunosComAtrasos.Text = statsAlunos.ComAtrasos.ToString();
                lblAlunosPendentes.Text = "alunos precisam devolver";

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
        private void TornarCardClicavel(ModernCard card)
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
