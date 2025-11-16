using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BibliotecaJK.Components
{
    /// <summary>
    /// ModernCard - Material Design inspired card component with elevation shadows and rounded corners
    /// </summary>
    public class ModernCard : Panel
    {
        private int _elevation = 2;
        private int _borderRadiusValue = 8;

        /// <summary>
        /// Shadow depth (0-5). Higher values create deeper shadows.
        /// </summary>
        public int Elevation
        {
            get => _elevation;
            set
            {
                if (value < 0) value = 0;
                if (value > 5) value = 5;
                _elevation = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Border radius in pixels for rounded corners (default: 8)
        /// </summary>
        public int BorderRadiusValue
        {
            get => _borderRadiusValue;
            set
            {
                _borderRadiusValue = value;
                Invalidate();
            }
        }

        public ModernCard()
        {
            // Set default appearance
            BackColor = ThemeManager.IsDarkMode
                ? ThemeManager.Dark.Surface
                : ThemeManager.Light.Surface;

            // Default padding (16px - will use ThemeManager.Spacing.MD once available)
            Padding = new Padding(16);

            // Enable double buffering for smooth rendering
            DoubleBuffered = true;

            // Set default size
            MinimumSize = new Size(100, 100);
        }

        /// <summary>
        /// Custom paint method to render card with shadow and rounded corners
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;

            // Enable anti-aliasing for smooth edges
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // Draw shadow if elevation is greater than 0
            if (Elevation > 0)
            {
                DrawShadow(g);
            }

            // Draw main card background
            DrawCard(g);

            // Call base to paint child controls
            base.OnPaint(e);
        }

        /// <summary>
        /// Draws the shadow based on elevation level
        /// </summary>
        private void DrawShadow(Graphics g)
        {
            // Calculate shadow properties based on elevation
            int shadowOffset = Elevation;
            int shadowBlur = Elevation * 2;

            // Create shadow color with opacity based on elevation
            // Elevation 1: 10% opacity, Elevation 5: 30% opacity
            int shadowAlpha = 10 + (Elevation * 4);
            Color shadowColor = Color.FromArgb(shadowAlpha, 0, 0, 0);

            // Draw multiple shadow layers for blur effect
            for (int i = 0; i < shadowBlur; i++)
            {
                int currentAlpha = shadowAlpha - (i * shadowAlpha / shadowBlur);
                if (currentAlpha <= 0) continue;

                var shadowRect = new Rectangle(
                    shadowOffset + i / 2,
                    shadowOffset + i / 2,
                    Width - shadowOffset * 2 - i,
                    Height - shadowOffset * 2 - i
                );

                var shadowPath = GetRoundedRectangle(shadowRect, BorderRadiusValue);
                using (var brush = new SolidBrush(Color.FromArgb(currentAlpha, 0, 0, 0)))
                {
                    g.FillPath(brush, shadowPath);
                }
            }
        }

        /// <summary>
        /// Draws the main card surface
        /// </summary>
        private void DrawCard(Graphics g)
        {
            // Account for shadow offset
            int offset = Elevation > 0 ? Elevation : 0;

            var cardRect = new Rectangle(
                0,
                0,
                Width - offset - 1,
                Height - offset - 1
            );

            var cardPath = GetRoundedRectangle(cardRect, BorderRadiusValue);

            // Fill card background
            using (var brush = new SolidBrush(BackColor))
            {
                g.FillPath(brush, cardPath);
            }

            // Draw subtle border for depth (optional)
            if (!ThemeManager.IsDarkMode)
            {
                using (var pen = new Pen(Color.FromArgb(20, 0, 0, 0), 1))
                {
                    g.DrawPath(pen, cardPath);
                }
            }
        }

        /// <summary>
        /// Creates a rounded rectangle GraphicsPath
        /// </summary>
        /// <param name="rect">Rectangle bounds</param>
        /// <param name="radius">Corner radius</param>
        /// <returns>GraphicsPath with rounded corners</returns>
        private GraphicsPath GetRoundedRectangle(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();

            // Ensure radius doesn't exceed rectangle dimensions
            int diameter = radius * 2;
            if (diameter > rect.Width) diameter = rect.Width;
            if (diameter > rect.Height) diameter = rect.Height;
            radius = diameter / 2;

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
        /// Override background painting to prevent default panel rendering
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Don't call base to prevent default panel background
            // We handle all painting in OnPaint
        }

        /// <summary>
        /// Update card appearance when theme changes
        /// </summary>
        public void UpdateTheme()
        {
            BackColor = ThemeManager.IsDarkMode
                ? ThemeManager.Dark.Surface
                : ThemeManager.Light.Surface;

            Invalidate();
        }
    }
}
