using System;
using System.Drawing;
using System.Windows.Forms;

namespace BibliotecaJK.Components
{
    /// <summary>
    /// Progress Dialog - Shows progress for long-running operations
    /// </summary>
    public class ProgressDialog : Form
    {
        private Label lblTitle = null!;
        private Label lblStatus = null!;
        private ProgressBar progressBar = null!;
        private Label lblPercentage = null!;
        private ModernButton btnCancel = null!;

        public string Title
        {
            get => lblTitle.Text;
            set => lblTitle.Text = value;
        }

        public string Status
        {
            get => lblStatus.Text;
            set
            {
                if (lblStatus.InvokeRequired)
                {
                    lblStatus.Invoke(new Action(() => lblStatus.Text = value));
                }
                else
                {
                    lblStatus.Text = value;
                }
            }
        }

        public int Progress
        {
            get => progressBar.Value;
            set
            {
                if (progressBar.InvokeRequired)
                {
                    progressBar.Invoke(new Action(() =>
                    {
                        progressBar.Value = Math.Max(0, Math.Min(100, value));
                        lblPercentage.Text = $"{progressBar.Value}%";
                    }));
                }
                else
                {
                    progressBar.Value = Math.Max(0, Math.Min(100, value));
                    lblPercentage.Text = $"{progressBar.Value}%";
                }
            }
        }

        public bool IsCancellable { get; set; } = true;
        public bool IsCancelled { get; private set; } = false;

        public ProgressDialog(string title = "Processando...")
        {
            Title = title;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.Size = new Size(450, 220);
            this.BackColor = ThemeManager.Colors.BackgroundPrimary;
            this.ControlBox = false;

            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 5,
                Padding = new Padding(ThemeManager.Spacing.LG),
                AutoSize = true
            };

            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // Title
            lblTitle = new Label
            {
                Text = Title,
                Font = ThemeManager.Typography.H4,
                ForeColor = ThemeManager.Colors.Gray800,
                AutoSize = true,
                Padding = new Padding(0, 0, 0, ThemeManager.Spacing.MD),
                Dock = DockStyle.Top
            };
            mainLayout.Controls.Add(lblTitle, 0, 0);

            // Status message
            lblStatus = new Label
            {
                Text = "Iniciando...",
                Font = ThemeManager.Typography.Body2,
                ForeColor = ThemeManager.Colors.Gray600,
                AutoSize = true,
                Padding = new Padding(0, 0, 0, ThemeManager.Spacing.SM),
                Dock = DockStyle.Top
            };
            mainLayout.Controls.Add(lblStatus, 0, 1);

            // Progress bar
            progressBar = new ProgressBar
            {
                Style = ProgressBarStyle.Continuous,
                Minimum = 0,
                Maximum = 100,
                Value = 0,
                Height = 24,
                Dock = DockStyle.Top,
                MarqueeAnimationSpeed = 30
            };
            mainLayout.Controls.Add(progressBar, 0, 2);

            // Percentage label
            lblPercentage = new Label
            {
                Text = "0%",
                Font = ThemeManager.Typography.Body2,
                ForeColor = ThemeManager.Colors.Primary,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleRight,
                Padding = new Padding(0, ThemeManager.Spacing.SM, 0, ThemeManager.Spacing.MD),
                Dock = DockStyle.Top
            };
            mainLayout.Controls.Add(lblPercentage, 0, 3);

            // Cancel button panel
            var pnlButtons = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Top,
                Height = 40,
                Padding = new Padding(0, ThemeManager.Spacing.SM, 0, 0)
            };

            btnCancel = new ModernButton
            {
                Text = "Cancelar",
                Variant = ModernButton.ButtonVariant.Outlined,
                ForeColor = ThemeManager.Colors.Gray700,
                Size = new Size(100, 36),
                Visible = IsCancellable
            };
            btnCancel.Click += (s, e) =>
            {
                IsCancelled = true;
                btnCancel.Enabled = false;
                btnCancel.Text = "Cancelando...";
                lblStatus.Text = "Operação cancelada pelo usuário";
            };
            pnlButtons.Controls.Add(btnCancel);

            mainLayout.Controls.Add(pnlButtons, 0, 4);

            this.Controls.Add(mainLayout);
        }

        /// <summary>
        /// Set indeterminate mode (marquee progress bar)
        /// </summary>
        public void SetIndeterminate(bool indeterminate)
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new Action(() =>
                {
                    progressBar.Style = indeterminate
                        ? ProgressBarStyle.Marquee
                        : ProgressBarStyle.Continuous;
                    lblPercentage.Visible = !indeterminate;
                }));
            }
            else
            {
                progressBar.Style = indeterminate
                    ? ProgressBarStyle.Marquee
                    : ProgressBarStyle.Continuous;
                lblPercentage.Visible = !indeterminate;
            }
        }

        /// <summary>
        /// Update progress and status in one call
        /// </summary>
        public void UpdateProgress(int progress, string status)
        {
            Progress = progress;
            Status = status;
        }

        /// <summary>
        /// Complete the operation and close
        /// </summary>
        public void Complete(string finalMessage = "Concluído!")
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => Complete(finalMessage)));
                return;
            }

            Progress = 100;
            Status = finalMessage;
            btnCancel.Visible = false;

            // Auto-close after brief delay
            var timer = new System.Windows.Forms.Timer { Interval = 500 };
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                this.DialogResult = DialogResult.OK;
                this.Close();
            };
            timer.Start();
        }

        /// <summary>
        /// Create Progress<int> reporter for async operations
        /// </summary>
        public IProgress<int> CreateProgressReporter()
        {
            return new Progress<int>(value => Progress = value);
        }

        /// <summary>
        /// Create Progress<(int, string)> reporter with status updates
        /// </summary>
        public IProgress<(int progress, string status)> CreateDetailedProgressReporter()
        {
            return new Progress<(int progress, string status)>(update =>
            {
                Progress = update.progress;
                Status = update.status;
            });
        }
    }
}
