using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BibliotecaJK.Components
{
    /// <summary>
    /// ModernButton - Custom button control with Material Design styling
    /// Supports three variants: Contained, Outlined, and Text
    /// Features smooth rounded corners, hover effects, and anti-aliased rendering
    /// </summary>
    public class ModernButton : Button
    {
        /// <summary>
        /// Button variant styles following Material Design principles
        /// </summary>
        public enum ButtonVariant
        {
            /// <summary>Filled button with background color (high emphasis)</summary>
            Contained,
            /// <summary>Button with border and no background (medium emphasis)</summary>
            Outlined,
            /// <summary>Button with no border or background (low emphasis)</summary>
            Text
        }

        private bool _isHovered = false;
        private bool _isPressed = false;

        /// <summary>
        /// Gets or sets the button variant style
        /// </summary>
        public ButtonVariant Variant { get; set; } = ButtonVariant.Contained;

        /// <summary>
        /// Gets or sets the custom hover color. If not set, a lighter shade will be calculated.
        /// </summary>
        public Color HoverColor { get; set; } = Color.Empty;

        /// <summary>
        /// Gets or sets the border radius for rounded corners. Default is 8px (ThemeManager.BorderRadius.MD).
        /// </summary>
        public int BorderRadiusValue { get; set; } = 8;

        /// <summary>
        /// Initializes a new instance of ModernButton with default Material Design styling
        /// </summary>
        public ModernButton()
        {
            // Remove default Windows Forms button styling
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;

            // Set default sizes and padding
            MinimumSize = new Size(64, 36);
            Padding = new Padding(16, 8, 16, 8);
            Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            Cursor = Cursors.Hand;

            // Enable double buffering for smooth rendering
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            // Default colors if not set
            if (BackColor == SystemColors.Control)
                BackColor = Color.FromArgb(63, 81, 181); // ThemeManager.Colors.Primary

            if (ForeColor == SystemColors.ControlText)
                ForeColor = Color.White;

            // Track hover and press states
            MouseEnter += (s, e) => { _isHovered = true; Invalidate(); };
            MouseLeave += (s, e) => { _isHovered = false; _isPressed = false; Invalidate(); };
            MouseDown += (s, e) => { _isPressed = true; Invalidate(); };
            MouseUp += (s, e) => { _isPressed = false; Invalidate(); };
        }

        /// <summary>
        /// Custom paint override for Material Design rendering
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;

            // Enable anti-aliasing for smooth edges
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            var rect = new Rectangle(0, 0, Width - 1, Height - 1);

            // Draw based on variant
            switch (Variant)
            {
                case ButtonVariant.Contained:
                    DrawContainedButton(g, rect);
                    break;
                case ButtonVariant.Outlined:
                    DrawOutlinedButton(g, rect);
                    break;
                case ButtonVariant.Text:
                    DrawTextButton(g, rect);
                    break;
            }

            // Draw text centered
            var textRect = new Rectangle(0, 0, Width, Height);
            TextRenderer.DrawText(g, Text, Font, textRect, ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        /// <summary>
        /// Draws a contained (filled) button with background color
        /// </summary>
        private void DrawContainedButton(Graphics g, Rectangle rect)
        {
            Color bgColor = BackColor;

            // Apply state-based color adjustments
            if (!Enabled)
            {
                bgColor = Color.FromArgb(189, 189, 189); // Gray400
            }
            else if (_isPressed)
            {
                bgColor = ControlPaint.Dark(bgColor, 0.2f);
            }
            else if (_isHovered)
            {
                bgColor = HoverColor != Color.Empty
                    ? HoverColor
                    : ControlPaint.Light(bgColor, 0.1f);
            }

            var path = GetRoundedRectangle(rect, BorderRadiusValue);

            // Fill button background
            using (var brush = new SolidBrush(bgColor))
            {
                g.FillPath(brush, path);
            }

            // Add subtle shadow for depth (elevation)
            if (Enabled && !_isPressed)
            {
                var shadowRect = new Rectangle(rect.X + 2, rect.Y + 2, rect.Width, rect.Height);
                var shadowPath = GetRoundedRectangle(shadowRect, BorderRadiusValue);
                using (var shadowBrush = new SolidBrush(Color.FromArgb(40, 0, 0, 0)))
                {
                    g.FillPath(shadowBrush, shadowPath);
                }
            }

            path.Dispose();
        }

        /// <summary>
        /// Draws an outlined button with border and no background
        /// </summary>
        private void DrawOutlinedButton(Graphics g, Rectangle rect)
        {
            Color borderColor = BackColor;
            Color bgColor = Color.Transparent;

            // Apply state-based color adjustments
            if (!Enabled)
            {
                borderColor = Color.FromArgb(189, 189, 189); // Gray400
            }
            else if (_isPressed)
            {
                bgColor = Color.FromArgb(30, BackColor.R, BackColor.G, BackColor.B);
                borderColor = ControlPaint.Dark(BackColor, 0.2f);
            }
            else if (_isHovered)
            {
                bgColor = HoverColor != Color.Empty
                    ? Color.FromArgb(20, HoverColor.R, HoverColor.G, HoverColor.B)
                    : Color.FromArgb(20, BackColor.R, BackColor.G, BackColor.B);
            }

            var path = GetRoundedRectangle(rect, BorderRadiusValue);

            // Fill background (transparent or hover state)
            if (bgColor != Color.Transparent)
            {
                using (var brush = new SolidBrush(bgColor))
                {
                    g.FillPath(brush, path);
                }
            }

            // Draw border
            using (var pen = new Pen(borderColor, 2))
            {
                g.DrawPath(pen, path);
            }

            path.Dispose();
        }

        /// <summary>
        /// Draws a text-only button with no border or background (minimal styling)
        /// </summary>
        private void DrawTextButton(Graphics g, Rectangle rect)
        {
            Color bgColor = Color.Transparent;

            // Apply state-based hover/press effects
            if (!Enabled)
            {
                // Text color will be dimmed automatically
            }
            else if (_isPressed)
            {
                bgColor = Color.FromArgb(40, BackColor.R, BackColor.G, BackColor.B);
            }
            else if (_isHovered)
            {
                bgColor = HoverColor != Color.Empty
                    ? Color.FromArgb(15, HoverColor.R, HoverColor.G, HoverColor.B)
                    : Color.FromArgb(15, BackColor.R, BackColor.G, BackColor.B);
            }

            // Fill with subtle background on hover/press
            if (bgColor != Color.Transparent)
            {
                var path = GetRoundedRectangle(rect, BorderRadiusValue);
                using (var brush = new SolidBrush(bgColor))
                {
                    g.FillPath(brush, path);
                }
                path.Dispose();
            }
        }

        /// <summary>
        /// Creates a rounded rectangle path for smooth corners
        /// </summary>
        /// <param name="rect">The rectangle bounds</param>
        /// <param name="radius">The corner radius</param>
        /// <returns>A GraphicsPath with rounded corners</returns>
        private GraphicsPath GetRoundedRectangle(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int diameter = radius * 2;

            // Ensure radius doesn't exceed rectangle dimensions
            if (diameter > rect.Width) diameter = rect.Width;
            if (diameter > rect.Height) diameter = rect.Height;

            // Create rounded rectangle path
            // Top-left corner
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);

            // Top-right corner
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);

            // Bottom-right corner
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);

            // Bottom-left corner
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);

            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// Override to prevent default button painting
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // Don't paint background - we handle it in OnPaint
        }
    }
}
