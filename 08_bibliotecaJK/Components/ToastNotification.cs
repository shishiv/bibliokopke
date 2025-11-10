using System;
using System.Drawing;
using System.Windows.Forms;

namespace BibliotecaJK.Components
{
    /// <summary>
    /// Toast Notification - Notificação não-intrusiva estilo Android/Toast
    /// </summary>
    public class ToastNotification : Form
    {
        private Label lblMensagem = null!;
        private Label lblIcone = null!;
        private System.Windows.Forms.Timer timerFade = null!;
        private System.Windows.Forms.Timer timerClose = null!;
        private double opacity = 1.0;

        public enum ToastType
        {
            Success,
            Error,
            Warning,
            Info
        }

        private ToastNotification()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.Size = new Size(350, 80);
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Opacity = 0;

            // Ícone
            lblIcone = new Label
            {
                Font = new Font("Segoe UI", 24F),
                Location = new Point(15, 15),
                Size = new Size(50, 50),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblIcone);

            // Mensagem
            lblMensagem = new Label
            {
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.White,
                Location = new Point(75, 10),
                Size = new Size(260, 60),
                TextAlign = ContentAlignment.MiddleLeft
            };
            this.Controls.Add(lblMensagem);

            // Timer de fade out
            timerFade = new System.Windows.Forms.Timer { Interval = 50 };
            timerFade.Tick += TimerFade_Tick;

            // Timer de fechamento
            timerClose = new System.Windows.Forms.Timer { Interval = 3000 };
            timerClose.Tick += (s, e) => {
                timerClose.Stop();
                timerFade.Start();
            };

            // Clique para fechar
            this.Click += (s, e) => this.Close();
            lblMensagem.Click += (s, e) => this.Close();
            lblIcone.Click += (s, e) => this.Close();
        }

        private void TimerFade_Tick(object? sender, EventArgs e)
        {
            opacity -= 0.1;
            if (opacity <= 0)
            {
                timerFade.Stop();
                this.Close();
            }
            else
            {
                this.Opacity = opacity;
            }
        }

        public static void Show(string mensagem, ToastType tipo = ToastType.Info, int duracao = 3000)
        {
            var toast = new ToastNotification();
            toast.lblMensagem.Text = mensagem;
            toast.timerClose.Interval = duracao;

            // Configurar cores e ícones por tipo
            switch (tipo)
            {
                case ToastType.Success:
                    toast.BackColor = Color.FromArgb(76, 175, 80);
                    toast.lblIcone.Text = "✓";
                    toast.lblIcone.ForeColor = Color.White;
                    break;

                case ToastType.Error:
                    toast.BackColor = Color.FromArgb(244, 67, 54);
                    toast.lblIcone.Text = "✗";
                    toast.lblIcone.ForeColor = Color.White;
                    break;

                case ToastType.Warning:
                    toast.BackColor = Color.FromArgb(255, 152, 0);
                    toast.lblIcone.Text = "⚠";
                    toast.lblIcone.ForeColor = Color.White;
                    break;

                case ToastType.Info:
                    toast.BackColor = Color.FromArgb(33, 150, 243);
                    toast.lblIcone.Text = "ℹ";
                    toast.lblIcone.ForeColor = Color.White;
                    break;
            }

            // Posicionar no canto superior direito
            var screen = Screen.PrimaryScreen?.WorkingArea ?? Screen.FromHandle(toast.Handle).WorkingArea;
            toast.Location = new Point(screen.Width - toast.Width - 20, 20);

            // Fade in
            toast.Show();
            var fadeInTimer = new System.Windows.Forms.Timer { Interval = 50 };
            fadeInTimer.Tick += (s, e) => {
                if (toast.Opacity < 1.0)
                    toast.Opacity += 0.1;
                else
                {
                    fadeInTimer?.Stop();
                    toast.timerClose?.Start();
                }
            };
            fadeInTimer.Start();
        }

        /// <summary>
        /// Atalhos para tipos específicos
        /// </summary>
        public static void Success(string mensagem, int duracao = 3000)
            => Show(mensagem, ToastType.Success, duracao);

        public static void Error(string mensagem, int duracao = 4000)
            => Show(mensagem, ToastType.Error, duracao);

        public static void Warning(string mensagem, int duracao = 3500)
            => Show(mensagem, ToastType.Warning, duracao);

        public static void Info(string mensagem, int duracao = 3000)
            => Show(mensagem, ToastType.Info, duracao);
    }
}
