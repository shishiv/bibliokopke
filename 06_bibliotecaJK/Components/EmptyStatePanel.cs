using System;
using System.Drawing;
using System.Windows.Forms;

namespace BibliotecaJK.Components
{
    /// <summary>
    /// EmptyStatePanel - Component to display a beautiful empty state message
    /// Shows an icon, title, description, and optional action button
    /// Follows Material Design principles for empty states
    /// </summary>
    public class EmptyStatePanel : Panel
    {
        private Label lblIcon = null!;
        private Label lblTitle = null!;
        private Label lblDescription = null!;
        private ModernButton? btnAction = null;
        private Panel pnlContainer = null!;

        private string _icon = "📭";
        private string _title = "Nenhum item encontrado";
        private string _description = "";
        private string? _actionText = null;

        /// <summary>
        /// Gets or sets the icon displayed in the empty state (emoji or symbol)
        /// </summary>
        public string Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                if (lblIcon != null)
                {
                    lblIcon.Text = value;
                    RenderEmptyState();
                }
            }
        }

        /// <summary>
        /// Gets or sets the title text
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                if (lblTitle != null)
                {
                    lblTitle.Text = value;
                    RenderEmptyState();
                }
            }
        }

        /// <summary>
        /// Gets or sets the description text
        /// </summary>
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                if (lblDescription != null)
                {
                    lblDescription.Text = value;
                    lblDescription.Visible = !string.IsNullOrWhiteSpace(value);
                    RenderEmptyState();
                }
            }
        }

        /// <summary>
        /// Gets or sets the action button text. If null or empty, button won't be shown.
        /// </summary>
        public string? ActionText
        {
            get => _actionText;
            set
            {
                _actionText = value;
                RenderEmptyState();
            }
        }

        /// <summary>
        /// Event raised when the action button is clicked
        /// </summary>
        public event EventHandler? ActionClick;

        /// <summary>
        /// Initializes a new instance of EmptyStatePanel with default values
        /// </summary>
        public EmptyStatePanel()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Configure panel
            this.Dock = DockStyle.Fill;
            this.BackColor = ThemeManager.Background.LightPaper;
            this.Padding = new Padding(ThemeManager.Spacing.LG);

            // Create container panel for centering content
            pnlContainer = new Panel
            {
                Size = new Size(500, 400),
                BackColor = Color.Transparent
            };

            // Icon label (large emoji/symbol)
            lblIcon = new Label
            {
                Text = _icon,
                Font = new Font("Segoe UI", 48F, FontStyle.Regular),
                ForeColor = ThemeManager.Colors.Neutral400,
                AutoSize = false,
                Size = new Size(500, 80),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Title label
            lblTitle = new Label
            {
                Text = _title,
                Font = ThemeManager.Typography.H4,
                ForeColor = ThemeManager.Colors.Neutral800,
                AutoSize = false,
                Size = new Size(500, 35),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Description label
            lblDescription = new Label
            {
                Text = _description,
                Font = ThemeManager.Typography.Body2,
                ForeColor = ThemeManager.Colors.Neutral600,
                AutoSize = false,
                Size = new Size(500, 60),
                TextAlign = ContentAlignment.TopCenter,
                Visible = !string.IsNullOrWhiteSpace(_description)
            };

            // Add controls to container
            pnlContainer.Controls.Add(lblIcon);
            pnlContainer.Controls.Add(lblTitle);
            pnlContainer.Controls.Add(lblDescription);

            this.Controls.Add(pnlContainer);

            // Handle resize to recenter content
            this.Resize += (s, e) => CenterContainer();

            // Initial render
            RenderEmptyState();
        }

        /// <summary>
        /// Renders the empty state layout with proper positioning
        /// </summary>
        public void RenderEmptyState()
        {
            if (pnlContainer == null || lblIcon == null || lblTitle == null || lblDescription == null)
                return;

            // Remove existing action button if present
            if (btnAction != null)
            {
                pnlContainer.Controls.Remove(btnAction);
                btnAction.Dispose();
                btnAction = null;
            }

            int yOffset = 0;
            const int containerWidth = 500;

            // Position icon at top
            lblIcon.Location = new Point(0, yOffset);
            lblIcon.Size = new Size(containerWidth, 80);
            yOffset += lblIcon.Height + ThemeManager.Spacing.MD;

            // Position title
            lblTitle.Location = new Point(0, yOffset);
            lblTitle.Size = new Size(containerWidth, 35);
            yOffset += lblTitle.Height + ThemeManager.Spacing.SM;

            // Position description (if visible)
            if (!string.IsNullOrWhiteSpace(_description))
            {
                lblDescription.Visible = true;
                lblDescription.Location = new Point(0, yOffset);
                lblDescription.Size = new Size(containerWidth, 60);
                yOffset += lblDescription.Height + ThemeManager.Spacing.MD;
            }
            else
            {
                lblDescription.Visible = false;
            }

            // Add action button if ActionText is provided
            if (!string.IsNullOrWhiteSpace(_actionText))
            {
                btnAction = new ModernButton
                {
                    Text = _actionText,
                    Variant = ModernButton.ButtonVariant.Contained,
                    BackColor = ThemeManager.Colors.Primary500,
                    ForeColor = Color.White,
                    Size = new Size(180, 40),
                    BorderRadiusValue = ThemeManager.BorderRadius.MD,
                    Font = ThemeManager.Typography.Button
                };

                btnAction.Click += (s, e) => ActionClick?.Invoke(this, EventArgs.Empty);

                // Center button horizontally
                int buttonX = (containerWidth - btnAction.Width) / 2;
                btnAction.Location = new Point(buttonX, yOffset);

                pnlContainer.Controls.Add(btnAction);
                yOffset += btnAction.Height + ThemeManager.Spacing.MD;
            }

            // Update container size based on content
            pnlContainer.Size = new Size(containerWidth, yOffset);

            // Center the container in the panel
            CenterContainer();
        }

        /// <summary>
        /// Centers the container panel within the parent panel
        /// </summary>
        private void CenterContainer()
        {
            if (pnlContainer != null && this.Width > 0 && this.Height > 0)
            {
                int x = (this.Width - pnlContainer.Width) / 2;
                int y = (this.Height - pnlContainer.Height) / 2;

                // Ensure we don't go negative
                x = Math.Max(0, x);
                y = Math.Max(0, y);

                pnlContainer.Location = new Point(x, y);
            }
        }

        /// <summary>
        /// Override to ensure initial render happens when control becomes visible
        /// </summary>
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (this.Visible)
            {
                RenderEmptyState();
            }
        }

        /// <summary>
        /// Override to ensure proper rendering on parent resize
        /// </summary>
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            RenderEmptyState();
        }
    }
}
