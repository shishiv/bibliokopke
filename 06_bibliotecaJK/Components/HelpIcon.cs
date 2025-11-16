using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BibliotecaJK.Components
{
    /// <summary>
    /// HelpIcon - Custom help icon control with rich tooltip support
    /// Displays a "?" icon in a circle with hover effects and detailed tooltips
    /// Provides contextual help for form fields and UI elements
    /// </summary>
    public class HelpIcon : PictureBox
    {
        private bool _isHovered = false;
        private ToolTip _richTooltip;
        private string _helpText = string.Empty;
        private string _helpTitle = "Ajuda";

        /// <summary>
        /// Gets or sets the help text to display in the tooltip
        /// </summary>
        public string HelpText
        {
            get => _helpText;
            set
            {
                _helpText = value;
                UpdateTooltip();
            }
        }

        /// <summary>
        /// Gets or sets the title of the help tooltip. Default is "Ajuda".
        /// </summary>
        public string HelpTitle
        {
            get => _helpTitle;
            set
            {
                _helpTitle = value;
                UpdateTooltip();
            }
        }

        /// <summary>
        /// Initializes a new instance of HelpIcon with default settings
        /// </summary>
        public HelpIcon() : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of HelpIcon with the specified help text
        /// </summary>
        /// <param name="helpText">The text to display in the tooltip</param>
        public HelpIcon(string helpText)
        {
            _helpText = helpText;

            // Set default size to 16x16
            Size = new Size(16, 16);
            MinimumSize = new Size(16, 16);
            MaximumSize = new Size(16, 16);

            // Configure cursor
            Cursor = Cursors.Hand;

            // Enable double buffering for smooth rendering
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);

            // Set transparent background
            BackColor = Color.Transparent;

            // Initialize rich tooltip
            InitializeTooltip();

            // Track hover state for visual feedback
            MouseEnter += (s, e) => { _isHovered = true; Invalidate(); };
            MouseLeave += (s, e) => { _isHovered = false; Invalidate(); };

            // Update tooltip if help text was provided
            if (!string.IsNullOrEmpty(helpText))
            {
                UpdateTooltip();
            }
        }

        /// <summary>
        /// Initializes the rich tooltip with custom settings
        /// </summary>
        private void InitializeTooltip()
        {
            _richTooltip = new ToolTip
            {
                // Enable balloon style for modern appearance
                IsBalloon = true,

                // Set auto-hide delay to 10 seconds
                AutoPopDelay = 10000,

                // Fade in/out animations
                InitialDelay = 300,
                ReshowDelay = 100,

                // Use system font for tooltip (Caption style)
                // Note: ToolTip doesn't expose Font property directly, but uses system font
                ToolTipIcon = ToolTipIcon.Info,

                // Show always, even when form is inactive
                ShowAlways = true,

                // Use relative positioning
                UseAnimation = true,
                UseFading = true
            };
        }

        /// <summary>
        /// Updates the tooltip with current help text and title
        /// </summary>
        private void UpdateTooltip()
        {
            if (_richTooltip != null && !string.IsNullOrEmpty(_helpText))
            {
                // Set the tooltip with title and text
                _richTooltip.ToolTipTitle = _helpTitle;
                _richTooltip.SetToolTip(this, _helpText);
            }
        }

        /// <summary>
        /// Custom paint for rendering the help icon
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;

            // Enable anti-aliasing for smooth rendering
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            // Calculate circle bounds (leave 1px margin for anti-aliasing)
            var circleRect = new Rectangle(1, 1, Width - 3, Height - 3);

            // Determine icon color based on hover state
            Color iconColor = _isHovered
                ? ThemeManager.Semantic.InfoLight  // Lighter color on hover
                : ThemeManager.Semantic.Info;       // Normal info color

            // Draw circle background
            using (var brush = new SolidBrush(iconColor))
            {
                g.FillEllipse(brush, circleRect);
            }

            // Draw "?" text in the center
            string questionMark = "?";

            // Use a slightly larger font for better visibility
            // Scale font size based on icon size
            float fontSize = Height * 0.65f; // ~10.4px for 16px icon
            using (var font = new Font("Segoe UI", fontSize, FontStyle.Bold))
            {
                // Measure text to center it properly
                var textSize = g.MeasureString(questionMark, font);

                // Calculate centered position
                float x = (Width - textSize.Width) / 2;
                float y = (Height - textSize.Height) / 2;

                // Draw white "?" text
                using (var brush = new SolidBrush(Color.White))
                {
                    g.DrawString(questionMark, font, brush, x, y);
                }
            }

            // Optional: Add subtle border for better definition
            if (!_isHovered)
            {
                using (var pen = new Pen(Color.FromArgb(30, 0, 0, 0), 1))
                {
                    g.DrawEllipse(pen, circleRect);
                }
            }
        }

        /// <summary>
        /// Override to prevent default PictureBox painting
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Paint transparent background if parent has one
            if (Parent != null && BackColor == Color.Transparent)
            {
                using (var brush = new SolidBrush(Parent.BackColor))
                {
                    e.Graphics.FillRectangle(brush, ClientRectangle);
                }
            }
        }

        /// <summary>
        /// Clean up resources when control is disposed
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _richTooltip?.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Factory method to quickly create a help icon with text
        /// </summary>
        /// <param name="helpText">The help text to display</param>
        /// <returns>A configured HelpIcon instance</returns>
        public static HelpIcon Create(string helpText)
        {
            return new HelpIcon(helpText);
        }

        /// <summary>
        /// Factory method to create a help icon with custom title and text
        /// </summary>
        /// <param name="helpText">The help text to display</param>
        /// <param name="helpTitle">The title for the tooltip</param>
        /// <returns>A configured HelpIcon instance</returns>
        public static HelpIcon Create(string helpText, string helpTitle)
        {
            return new HelpIcon(helpText)
            {
                HelpTitle = helpTitle
            };
        }
    }
}
