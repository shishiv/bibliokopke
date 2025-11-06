using System;
using System.Drawing;
using System.Windows.Forms;

namespace BibliotecaJK.Components
{
    /// <summary>
    /// Loading Panel - Overlay com spinner animado
    /// </summary>
    public class LoadingPanel : Panel
    {
        private Label lblSpinner = null!;
        private Label lblMensagem = null!;
        private System.Windows.Forms.Timer timerAnimation = null!;
        private string[] spinnerFrames = { "⠋", "⠙", "⠹", "⠸", "⠼", "⠴", "⠦", "⠧", "⠇", "⠏" };
        private int currentFrame = 0;

        public string Mensagem
        {
            get => lblMensagem.Text;
            set => lblMensagem.Text = value;
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

            // Painel central branco
            var pnlCenter = new Panel
            {
                Size = new Size(250, 150),
                BackColor = Color.White,
                Location = new Point((this.Width - 250) / 2, (this.Height - 150) / 2)
            };

            // Spinner
            lblSpinner = new Label
            {
                Text = spinnerFrames[0],
                Font = new Font("Segoe UI", 48F),
                ForeColor = Color.FromArgb(63, 81, 181),
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

            this.Controls.Add(pnlCenter);

            // Ajustar posição do painel quando o tamanho mudar
            this.Resize += (s, e) => {
                pnlCenter.Location = new Point((this.Width - 250) / 2, (this.Height - 150) / 2);
            };

            // Timer de animação
            timerAnimation = new System.Windows.Forms.Timer { Interval = 80 };
            timerAnimation.Tick += (s, e) => {
                currentFrame = (currentFrame + 1) % spinnerFrames.Length;
                lblSpinner.Text = spinnerFrames[currentFrame];
            };
        }

        public new void Show()
        {
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
        public void ShowWhile(Action action, string mensagem = "Carregando...")
        {
            this.Mensagem = mensagem;
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
    }
}
