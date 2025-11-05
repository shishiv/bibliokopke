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
        private readonly NotificacaoDAL _notificacaoDAL;
        private Label lblNotificacaoBadge = new Label();
        private System.Windows.Forms.Timer timerNotificacoes = new System.Windows.Forms.Timer();
        private KeyboardShortcutManager _shortcutManager;
        private ToolTip _tooltip = new ToolTip();
        private StatusStrip _statusStrip = new StatusStrip();
        private ToolStripStatusLabel _lblStatus;
        private ToolStripStatusLabel _lblUsuario;
        private ToolStripStatusLabel _lblHora;
        private Panel cardEmprestimos;
        private Panel cardLivros;
        private Panel cardAlunos;
        private Panel cardMultas;
        private Panel cardEmprestados;
        private Panel cardAtrasos;

        public FormPrincipal(Funcionario funcionario)
        {
            _funcionarioLogado = funcionario;
            _emprestimoService = new EmprestimoService();
            _livroService = new LivroService();
            _alunoService = new AlunoService();
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

            // FormPrincipal - Tamanho maior e moderno
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

            this.Controls.Add(pnlHeader);

            // ==================== ÁREA DE CONTEÚDO ====================
            var pnlContent = new Panel
            {
                Location = new Point(250, 80),
                Size = new Size(1150, 720),
                BackColor = Color.FromArgb(245, 245, 250),
                AutoScroll = true
            };

            // Dashboard Cards Container
            var pnlDashboard = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(1110, 680),
                BackColor = Color.Transparent
            };

            // Título Dashboard
            var lblTituloDashboard = new Label
            {
                Text = "ESTATÍSTICAS DO SISTEMA",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(63, 81, 181),
                Location = new Point(0, 0),
                Size = new Size(1110, 30)
            };
            pnlDashboard.Controls.Add(lblTituloDashboard);

            // Cards Grid - 4 colunas
            int cardWidth = 260;
            int cardHeight = 140;
            int cardMargin = 20;

            // Card 1: Empréstimos (clicável)
            cardEmprestimos = CriarCardModerno("EMPRÉSTIMOS ATIVOS", 0, 50, cardWidth, cardHeight, Color.FromArgb(76, 175, 80));
            lblEmprestimosAtivos = CriarLabelCardModerno("0", 40, cardEmprestimos, 32F);
            lblEmprestimosAtrasados = CriarLabelCardModerno("0 atrasados", 90, cardEmprestimos, 11F, Color.FromArgb(255, 87, 34));
            cardEmprestimos.Cursor = Cursors.Hand;
            cardEmprestimos.Click += (s, e) => AbrirConsultaEmprestimos();
            TornarCardClicavel(cardEmprestimos);
            pnlDashboard.Controls.Add(cardEmprestimos);

            // Card 2: Livros (clicável)
            cardLivros = CriarCardModerno("LIVROS CADASTRADOS", cardWidth + cardMargin, 50, cardWidth, cardHeight, Color.FromArgb(33, 150, 243));
            lblTotalLivros = CriarLabelCardModerno("0", 40, cardLivros, 32F);
            lblLivrosDisponiveis = CriarLabelCardModerno("0 disponíveis", 90, cardLivros, 11F, Color.FromArgb(100, 181, 246));
            cardLivros.Cursor = Cursors.Hand;
            cardLivros.Click += (s, e) => AbrirCadastroLivros();
            TornarCardClicavel(cardLivros);
            pnlDashboard.Controls.Add(cardLivros);

            // Card 3: Alunos (clicável)
            cardAlunos = CriarCardModerno("ALUNOS CADASTRADOS", (cardWidth + cardMargin) * 2, 50, cardWidth, cardHeight, Color.FromArgb(156, 39, 176));
            lblTotalAlunos = CriarLabelCardModerno("0", 40, cardAlunos, 32F);
            lblAlunosComEmprestimos = CriarLabelCardModerno("0 c/ empréstimos", 90, cardAlunos, 11F, Color.FromArgb(186, 104, 200));
            cardAlunos.Cursor = Cursors.Hand;
            cardAlunos.Click += (s, e) => AbrirCadastroAlunos();
            TornarCardClicavel(cardAlunos);
            pnlDashboard.Controls.Add(cardAlunos);

            // Card 4: Multas (clicável)
            cardMultas = CriarCardModerno("MULTAS ACUMULADAS", (cardWidth + cardMargin) * 3, 50, cardWidth, cardHeight, Color.FromArgb(244, 67, 54));
            lblMultaTotal = CriarLabelCardModerno("R$ 0,00", 40, cardMultas, 28F);
            CriarLabelCardModerno("Total pendente", 90, cardMultas, 11F, Color.FromArgb(255, 138, 128));
            cardMultas.Cursor = Cursors.Hand;
            cardMultas.Click += (s, e) => AbrirConsultaEmprestimos();
            TornarCardClicavel(cardMultas);
            pnlDashboard.Controls.Add(cardMultas);

            // Segunda linha de cards
            int row2Y = 50 + cardHeight + cardMargin;

            // Card 5: Livros Emprestados (clicável)
            cardEmprestados = CriarCardModerno("LIVROS EMPRESTADOS", 0, row2Y, cardWidth, cardHeight, Color.FromArgb(255, 152, 0));
            lblLivrosEmprestados = CriarLabelCardModerno("0", 40, cardEmprestados, 32F);
            CriarLabelCardModerno("Exemplares em uso", 90, cardEmprestados, 11F, Color.FromArgb(255, 183, 77));
            cardEmprestados.Cursor = Cursors.Hand;
            cardEmprestados.Click += (s, e) => AbrirConsultaEmprestimos();
            TornarCardClicavel(cardEmprestados);
            pnlDashboard.Controls.Add(cardEmprestados);

            // Card 6: Alunos com Atrasos (clicável)
            cardAtrasos = CriarCardModerno("ALUNOS C/ ATRASOS", cardWidth + cardMargin, row2Y, cardWidth, cardHeight, Color.FromArgb(255, 87, 34));
            lblAlunosComAtrasos = CriarLabelCardModerno("0", 40, cardAtrasos, 32F);
            CriarLabelCardModerno("Necessitam atenção", 90, cardAtrasos, 11F, Color.FromArgb(255, 138, 101));
            cardAtrasos.Cursor = Cursors.Hand;
            cardAtrasos.Click += (s, e) => AbrirCadastroAlunos();
            TornarCardClicavel(cardAtrasos);
            pnlDashboard.Controls.Add(cardAtrasos);

            // Botão Atualizar Dashboard
            var btnAtualizar = new Button
            {
                Text = "🔄 Atualizar Dashboard",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Location = new Point((cardWidth + cardMargin) * 2, row2Y + 50),
                Size = new Size(280, 45),
                BackColor = Color.FromArgb(63, 81, 181),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAtualizar.FlatAppearance.BorderSize = 0;
            btnAtualizar.FlatAppearance.MouseOverBackColor = Color.FromArgb(83, 101, 201);
            btnAtualizar.Click += (s, e) => AtualizarDashboard();
            pnlDashboard.Controls.Add(btnAtualizar);

            pnlContent.Controls.Add(pnlDashboard);
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

        private void AbrirBackup()
        {
            var form = new FormBackup(_funcionarioLogado);
            form.ShowDialog();
        }

        /// <summary>
        /// Torna um card clicável propagando o click para todos os controles filhos
        /// </summary>
        private void TornarCardClicavel(Panel card)
        {
            foreach (Control control in card.Controls)
            {
                control.Cursor = Cursors.Hand;
                control.Click += (s, e) => card.PerformClick();
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
