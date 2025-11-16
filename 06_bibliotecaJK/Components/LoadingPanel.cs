using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BibliotecaJK.Components
{
    /// <summary>
    /// Estilos de loading disponíveis
    /// </summary>
    public enum LoadingStyle
    {
        Spinner,    // Spinner animado (padrão)
        Skeleton,   // Skeleton screen com shimmer
        Progress,   // Barra de progresso
        Dots        // Dots animados (...)
    }

    /// <summary>
    /// Loading Panel - Overlay com múltiplos estilos de loading
    /// </summary>
    public class LoadingPanel : Panel
    {
        private Label lblSpinner = null!;
        private Label lblMensagem = null!;
        private Panel pnlCenter = null!;
        private Panel pnlSkeletonContainer = null!;
        private System.Windows.Forms.Timer timerAnimation = null!;

        private string[] spinnerFrames = { "⠋", "⠙", "⠹", "⠸", "⠼", "⠴", "⠦", "⠧", "⠇", "⠏" };
        private int currentFrame = 0;

        // Skeleton shimmer animation
        private int shimmerPosition = 0;
        private const int SHIMMER_WIDTH = 100;

        // Dots animation
        private int dotsCount = 0;

        // Progress properties
        private int _progress = 0;
        private int _skeletonCardCount = 3;

        private LoadingStyle _currentStyle = LoadingStyle.Spinner;

        public LoadingStyle CurrentStyle
        {
            get => _currentStyle;
            set
            {
                _currentStyle = value;
                UpdateStyleVisibility();
            }
        }

        public string Mensagem
        {
            get => lblMensagem.Text;
            set => lblMensagem.Text = value;
        }

        /// <summary>
        /// Progresso atual (0-100) para LoadingStyle.Progress
        /// </summary>
        public int Progress
        {
            get => _progress;
            set
            {
                _progress = Math.Max(0, Math.Min(100, value));
                if (CurrentStyle == LoadingStyle.Progress)
                {
                    pnlCenter.Invalidate(); // Force repaint
                }
            }
        }

        public LoadingPanel()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(200, 0, 0, 0); // Semi-transparente
            this.Dock = DockStyle.Fill;
            this.Visible = false;
            this.DoubleBuffered = true;

            // Painel central branco
            pnlCenter = new Panel
            {
                Size = new Size(250, 150),
                BackColor = Color.White,
                Location = new Point((this.Width - 250) / 2, (this.Height - 150) / 2)
            };
            SetDoubleBuffered(pnlCenter);
            pnlCenter.Paint += PnlCenter_Paint;

            // Spinner
            lblSpinner = new Label
            {
                Text = spinnerFrames[0],
                Font = new Font("Segoe UI", 48F),
                ForeColor = ThemeManager.Colors.Primary500,
                Location = new Point(85, 20),
                Size = new Size(80, 80),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlCenter.Controls.Add(lblSpinner);

            // Mensagem
            lblMensagem = new Label
            {
                Text = "Carregando...",
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.Gray,
                Location = new Point(10, 105),
                Size = new Size(230, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlCenter.Controls.Add(lblMensagem);

            // Container para skeleton screens
            pnlSkeletonContainer = new Panel
            {
                AutoScroll = true,
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Visible = false
            };
            SetDoubleBuffered(pnlSkeletonContainer);
            this.Controls.Add(pnlSkeletonContainer);

            this.Controls.Add(pnlCenter);

            // Ajustar posição do painel quando o tamanho mudar
            this.Resize += (s, e) => {
                pnlCenter.Location = new Point((this.Width - 250) / 2, (this.Height - 150) / 2);
            };

            // Timer de animação
            timerAnimation = new System.Windows.Forms.Timer { Interval = 80 };
            timerAnimation.Tick += TimerAnimation_Tick;
        }

        private void TimerAnimation_Tick(object? sender, EventArgs e)
        {
            switch (CurrentStyle)
            {
                case LoadingStyle.Spinner:
                    currentFrame = (currentFrame + 1) % spinnerFrames.Length;
                    lblSpinner.Text = spinnerFrames[currentFrame];
                    break;

                case LoadingStyle.Skeleton:
                    shimmerPosition += 10;
                    if (shimmerPosition > pnlSkeletonContainer.Width + SHIMMER_WIDTH)
                        shimmerPosition = -SHIMMER_WIDTH;
                    pnlSkeletonContainer.Invalidate();
                    break;

                case LoadingStyle.Dots:
                    dotsCount = (dotsCount + 1) % 4;
                    lblMensagem.Text = Mensagem.TrimEnd('.') + new string('.', dotsCount);
                    break;

                case LoadingStyle.Progress:
                    pnlCenter.Invalidate();
                    break;
            }
        }

        private void PnlCenter_Paint(object? sender, PaintEventArgs e)
        {
            if (CurrentStyle == LoadingStyle.Progress)
            {
                DrawProgressBar(e.Graphics);
            }
        }

        private void DrawProgressBar(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Barra de fundo
            int barWidth = 210;
            int barHeight = 8;
            int barX = (pnlCenter.Width - barWidth) / 2;
            int barY = 60;

            using (var bgBrush = new SolidBrush(ThemeManager.Colors.Neutral200))
            {
                g.FillRectangle(bgBrush, barX, barY, barWidth, barHeight);
            }

            // Barra de progresso
            int progressWidth = (int)(barWidth * (_progress / 100.0));
            if (progressWidth > 0)
            {
                using (var progressBrush = new LinearGradientBrush(
                    new Rectangle(barX, barY, progressWidth, barHeight),
                    ThemeManager.Colors.Primary400,
                    ThemeManager.Colors.Primary600,
                    LinearGradientMode.Horizontal))
                {
                    g.FillRectangle(progressBrush, barX, barY, progressWidth, barHeight);
                }
            }

            // Texto de porcentagem
            string percentText = $"{_progress}%";
            using (var font = new Font("Segoe UI", 24F, FontStyle.Bold))
            using (var brush = new SolidBrush(ThemeManager.Colors.Primary500))
            {
                var textSize = g.MeasureString(percentText, font);
                g.DrawString(percentText, font, brush,
                    (pnlCenter.Width - textSize.Width) / 2, 15);
            }
        }

        /// <summary>
        /// Mostra skeleton screen com número especificado de cards
        /// </summary>
        public void ShowSkeleton(int cardCount = 3)
        {
            _skeletonCardCount = cardCount;
            CurrentStyle = LoadingStyle.Skeleton;

            pnlSkeletonContainer.Controls.Clear();

            int cardWidth = 350;
            int cardHeight = 120;
            int spacing = 16;
            int totalWidth = this.Width;
            int cardsPerRow = Math.Max(1, totalWidth / (cardWidth + spacing));

            for (int i = 0; i < cardCount; i++)
            {
                int row = i / cardsPerRow;
                int col = i % cardsPerRow;

                var skeletonCard = new SkeletonCard
                {
                    Width = cardWidth,
                    Height = cardHeight,
                    Location = new Point(
                        spacing + col * (cardWidth + spacing),
                        spacing + row * (cardHeight + spacing)
                    )
                };

                pnlSkeletonContainer.Controls.Add(skeletonCard);
            }

            Show();
        }

        /// <summary>
        /// Mostra loading de progresso com valores current/total
        /// </summary>
        public void ShowProgress(int current, int total, string message = "Processando...")
        {
            if (total > 0)
            {
                Progress = (int)((current / (double)total) * 100);
            }
            Mensagem = $"{message} ({current}/{total})";
            CurrentStyle = LoadingStyle.Progress;
            Show();
        }

        private void UpdateStyleVisibility()
        {
            // Esconder todos primeiro
            lblSpinner.Visible = false;
            pnlSkeletonContainer.Visible = false;
            pnlCenter.Visible = false;

            switch (CurrentStyle)
            {
                case LoadingStyle.Spinner:
                    lblSpinner.Visible = true;
                    pnlCenter.Visible = true;
                    break;

                case LoadingStyle.Skeleton:
                    pnlSkeletonContainer.Visible = true;
                    break;

                case LoadingStyle.Dots:
                case LoadingStyle.Progress:
                    pnlCenter.Visible = true;
                    break;
            }

            lblMensagem.Visible = (CurrentStyle != LoadingStyle.Skeleton);
        }

        public new void Show()
        {
            UpdateStyleVisibility();
            this.Visible = true;
            this.BringToFront();
            timerAnimation.Start();
        }

        public new void Hide()
        {
            timerAnimation.Stop();
            this.Visible = false;
        }

        /// <summary>
        /// Mostrar loading e executar ação assíncrona
        /// </summary>
        public void ShowWhile(Action action, string mensagem = "Carregando...", LoadingStyle style = LoadingStyle.Spinner)
        {
            this.Mensagem = mensagem;
            this.CurrentStyle = style;
            this.Show();

            var backgroundWorker = new System.ComponentModel.BackgroundWorker();
            backgroundWorker.DoWork += (s, e) => action();
            backgroundWorker.RunWorkerCompleted += (s, e) => {
                this.Hide();
                if (e.Error != null)
                {
                    ToastNotification.Error($"Erro: {e.Error.Message}");
                }
            };
            backgroundWorker.RunWorkerAsync();
        }

        private static void SetDoubleBuffered(Control control)
        {
            typeof(Control).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null, control, new object[] { true });
        }
    }

    /// <summary>
    /// Skeleton Card - Card com shimmer animation
    /// </summary>
    internal class SkeletonCard : Panel
    {
        private System.Windows.Forms.Timer shimmerTimer;
        private int shimmerOffset = 0;

        public SkeletonCard()
        {
            this.BackColor = Color.White;
            this.DoubleBuffered = true;

            shimmerTimer = new System.Windows.Forms.Timer { Interval = 30 };
            shimmerTimer.Tick += (s, e) =>
            {
                shimmerOffset += 5;
                if (shimmerOffset > this.Width + 100)
                    shimmerOffset = -100;
                this.Invalidate();
            };
            shimmerTimer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Background
            using (var bgBrush = new SolidBrush(Color.White))
            {
                g.FillRectangle(bgBrush, 0, 0, Width, Height);
            }

            // Draw skeleton elements
            DrawSkeletonElements(g);

            // Draw shimmer effect
            DrawShimmer(g);
        }

        private void DrawSkeletonElements(Graphics g)
        {
            using (var brush = new SolidBrush(ThemeManager.Colors.Neutral200))
            {
                // Avatar/Image placeholder (left side)
                g.FillRectangle(brush, 15, 15, 60, 60);

                // Title line
                g.FillRectangle(brush, 85, 15, 180, 16);

                // Subtitle line
                g.FillRectangle(brush, 85, 40, 140, 12);

                // Description lines
                g.FillRectangle(brush, 85, 60, 200, 10);
                g.FillRectangle(brush, 85, 75, 160, 10);

                // Bottom tag/badge placeholders
                g.FillRectangle(brush, 15, 90, 60, 20);
                g.FillRectangle(brush, 85, 90, 80, 20);
            }
        }

        private void DrawShimmer(Graphics g)
        {
            // Create shimmer gradient
            var shimmerRect = new Rectangle(shimmerOffset, 0, 100, this.Height);

            // Clipping to stay within bounds
            if (shimmerRect.X + shimmerRect.Width > 0 && shimmerRect.X < this.Width)
            {
                using (var shimmerBrush = new LinearGradientBrush(
                    new Rectangle(shimmerOffset - 50, 0, 200, this.Height),
                    Color.FromArgb(0, 255, 255, 255),      // Transparent
                    Color.FromArgb(100, 255, 255, 255),    // Semi-transparent white
                    LinearGradientMode.Horizontal))
                {
                    // Create color blend for smooth shimmer effect
                    var colorBlend = new ColorBlend(3);
                    colorBlend.Colors = new Color[] {
                        Color.FromArgb(0, 255, 255, 255),
                        Color.FromArgb(120, 255, 255, 255),
                        Color.FromArgb(0, 255, 255, 255)
                    };
                    colorBlend.Positions = new float[] { 0f, 0.5f, 1f };
                    shimmerBrush.InterpolationColors = colorBlend;

                    g.FillRectangle(shimmerBrush, shimmerOffset, 0, 100, this.Height);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                shimmerTimer?.Stop();
                shimmerTimer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
