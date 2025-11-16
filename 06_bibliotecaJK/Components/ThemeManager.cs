using System;
using System.Drawing;
using System.Windows.Forms;

namespace BibliotecaJK.Components
{
    /// <summary>
    /// Theme Manager - Gerenciador de temas claro/escuro com sistema de design tokens
    /// </summary>
    public static class ThemeManager
    {
        public static bool IsDarkMode { get; private set; } = false;

        // === DESIGN TOKENS ===

        /// <summary>
        /// Cores primárias (Material Design inspired)
        /// </summary>
        public static class Colors
        {
            // Primary Palette (Indigo)
            public static Color Primary50 = Color.FromArgb(232, 234, 246);
            public static Color Primary100 = Color.FromArgb(197, 202, 233);
            public static Color Primary200 = Color.FromArgb(159, 168, 218);
            public static Color Primary300 = Color.FromArgb(121, 134, 203);
            public static Color Primary400 = Color.FromArgb(92, 107, 192);
            public static Color Primary500 = Color.FromArgb(63, 81, 181);  // Main Primary
            public static Color Primary600 = Color.FromArgb(57, 73, 171);
            public static Color Primary700 = Color.FromArgb(48, 63, 159);
            public static Color Primary800 = Color.FromArgb(40, 53, 147);
            public static Color Primary900 = Color.FromArgb(26, 35, 126);

            // Secondary Palette (Blue)
            public static Color Secondary50 = Color.FromArgb(225, 245, 254);
            public static Color Secondary100 = Color.FromArgb(179, 229, 252);
            public static Color Secondary200 = Color.FromArgb(129, 212, 250);
            public static Color Secondary300 = Color.FromArgb(79, 195, 247);
            public static Color Secondary400 = Color.FromArgb(41, 182, 246);
            public static Color Secondary500 = Color.FromArgb(3, 169, 244);  // Main Secondary
            public static Color Secondary600 = Color.FromArgb(3, 155, 229);
            public static Color Secondary700 = Color.FromArgb(2, 136, 209);
            public static Color Secondary800 = Color.FromArgb(2, 119, 189);
            public static Color Secondary900 = Color.FromArgb(1, 87, 155);

            // Neutral/Gray Palette
            public static Color Neutral50 = Color.FromArgb(250, 250, 250);
            public static Color Neutral100 = Color.FromArgb(245, 245, 245);
            public static Color Neutral200 = Color.FromArgb(238, 238, 238);
            public static Color Neutral300 = Color.FromArgb(224, 224, 224);
            public static Color Neutral400 = Color.FromArgb(189, 189, 189);
            public static Color Neutral500 = Color.FromArgb(158, 158, 158);
            public static Color Neutral600 = Color.FromArgb(117, 117, 117);
            public static Color Neutral700 = Color.FromArgb(97, 97, 97);
            public static Color Neutral800 = Color.FromArgb(66, 66, 66);
            public static Color Neutral900 = Color.FromArgb(33, 33, 33);
        }

        /// <summary>
        /// Cores semânticas
        /// </summary>
        public static class Semantic
        {
            // Success (Green)
            public static Color Success = Color.FromArgb(76, 175, 80);
            public static Color SuccessLight = Color.FromArgb(129, 199, 132);
            public static Color SuccessDark = Color.FromArgb(56, 142, 60);
            public static Color SuccessBackground = Color.FromArgb(232, 245, 233);

            // Warning (Orange)
            public static Color Warning = Color.FromArgb(255, 152, 0);
            public static Color WarningLight = Color.FromArgb(255, 183, 77);
            public static Color WarningDark = Color.FromArgb(245, 124, 0);
            public static Color WarningBackground = Color.FromArgb(255, 243, 224);

            // Error (Red)
            public static Color Error = Color.FromArgb(244, 67, 54);
            public static Color ErrorLight = Color.FromArgb(239, 83, 80);
            public static Color ErrorDark = Color.FromArgb(211, 47, 47);
            public static Color ErrorBackground = Color.FromArgb(255, 235, 238);

            // Info (Cyan)
            public static Color Info = Color.FromArgb(0, 188, 212);
            public static Color InfoLight = Color.FromArgb(77, 208, 225);
            public static Color InfoDark = Color.FromArgb(0, 151, 167);
            public static Color InfoBackground = Color.FromArgb(224, 247, 250);
        }

        /// <summary>
        /// Cores de background e superfície
        /// </summary>
        public static class Background
        {
            // Light Mode
            public static Color LightDefault = Color.FromArgb(245, 245, 250);
            public static Color LightPaper = Color.White;
            public static Color LightCard = Color.White;
            public static Color LightElevated = Color.FromArgb(250, 250, 250);

            // Dark Mode
            public static Color DarkDefault = Color.FromArgb(18, 18, 18);
            public static Color DarkPaper = Color.FromArgb(30, 30, 30);
            public static Color DarkCard = Color.FromArgb(30, 30, 30);
            public static Color DarkElevated = Color.FromArgb(42, 42, 42);
        }

        /// <summary>
        /// Escala tipográfica
        /// </summary>
        public static class Typography
        {
            // Headings
            public static Font H1 = new Font("Segoe UI", 32F, FontStyle.Bold);
            public static Font H2 = new Font("Segoe UI", 28F, FontStyle.Bold);
            public static Font H3 = new Font("Segoe UI", 24F, FontStyle.Bold);
            public static Font H4 = new Font("Segoe UI", 20F, FontStyle.Bold);
            public static Font H5 = new Font("Segoe UI", 16F, FontStyle.Bold);
            public static Font H6 = new Font("Segoe UI", 14F, FontStyle.Bold);

            // Body
            public static Font Body1 = new Font("Segoe UI", 10F, FontStyle.Regular);
            public static Font Body2 = new Font("Segoe UI", 9F, FontStyle.Regular);

            // Utilities
            public static Font Caption = new Font("Segoe UI", 8F, FontStyle.Regular);
            public static Font Overline = new Font("Segoe UI", 7F, FontStyle.Regular);
            public static Font Button = new Font("Segoe UI", 9F, FontStyle.Bold);

            // Sizes (for custom font creation)
            public const float H1_SIZE = 32F;
            public const float H2_SIZE = 28F;
            public const float H3_SIZE = 24F;
            public const float H4_SIZE = 20F;
            public const float H5_SIZE = 16F;
            public const float H6_SIZE = 14F;
            public const float BODY1_SIZE = 10F;
            public const float BODY2_SIZE = 9F;
            public const float CAPTION_SIZE = 8F;
            public const float OVERLINE_SIZE = 7F;
            public const float BUTTON_SIZE = 9F;
        }

        /// <summary>
        /// Escala de espaçamento
        /// </summary>
        public static class Spacing
        {
            public const int XS = 4;    // Extra Small
            public const int SM = 8;    // Small
            public const int MD = 16;   // Medium
            public const int LG = 24;   // Large
            public const int XL = 32;   // Extra Large
            public const int XXL = 48;  // Extra Extra Large

            /// <summary>
            /// Retorna um Padding baseado na escala de espaçamento
            /// </summary>
            public static Padding GetPadding(int all) => new Padding(all);
            public static Padding GetPadding(int horizontal, int vertical) => new Padding(horizontal, vertical, horizontal, vertical);
            public static Padding GetPadding(int left, int top, int right, int bottom) => new Padding(left, top, right, bottom);
        }

        /// <summary>
        /// Raios de borda
        /// </summary>
        public static class BorderRadius
        {
            public const int None = 0;
            public const int SM = 4;
            public const int MD = 8;
            public const int LG = 12;
            public const int XL = 16;
            public const int Round = 9999;  // Fully rounded
        }

        /// <summary>
        /// Sombras (box-shadow)
        /// Nota: Windows Forms não suporta sombras nativas, mas pode ser usado para customizações
        /// </summary>
        public static class Shadows
        {
            // Shadow1: Sombra leve (elevation 2dp)
            public static readonly ShadowDefinition Shadow1 = new ShadowDefinition
            {
                OffsetX = 0,
                OffsetY = 1,
                BlurRadius = 3,
                SpreadRadius = 0,
                Color = Color.FromArgb(40, 0, 0, 0)  // 40/255 ≈ 0.16 alpha
            };

            // Shadow2: Sombra média (elevation 4dp)
            public static readonly ShadowDefinition Shadow2 = new ShadowDefinition
            {
                OffsetX = 0,
                OffsetY = 2,
                BlurRadius = 8,
                SpreadRadius = 0,
                Color = Color.FromArgb(50, 0, 0, 0)  // 50/255 ≈ 0.20 alpha
            };

            // Shadow3: Sombra forte (elevation 8dp)
            public static readonly ShadowDefinition Shadow3 = new ShadowDefinition
            {
                OffsetX = 0,
                OffsetY = 4,
                BlurRadius = 16,
                SpreadRadius = 0,
                Color = Color.FromArgb(60, 0, 0, 0)  // 60/255 ≈ 0.24 alpha
            };
        }

        /// <summary>
        /// Definição de sombra
        /// </summary>
        public class ShadowDefinition
        {
            public int OffsetX { get; set; }
            public int OffsetY { get; set; }
            public int BlurRadius { get; set; }
            public int SpreadRadius { get; set; }
            public Color Color { get; set; }

            public override string ToString()
            {
                return $"{OffsetX}px {OffsetY}px {BlurRadius}px {SpreadRadius}px rgba({Color.R},{Color.G},{Color.B},{Color.A / 255.0:F2})";
            }
        }

        // === LEGACY THEME SUPPORT (mantido para compatibilidade) ===

        public static class Light
        {
            public static Color Background = Color.FromArgb(245, 245, 250);
            public static Color Surface = Color.White;
            public static Color Primary = Color.FromArgb(63, 81, 181);
            public static Color Secondary = Color.FromArgb(33, 150, 243);
            public static Color Text = Color.FromArgb(33, 33, 33);
            public static Color TextSecondary = Color.Gray;
            public static Color Border = Color.FromArgb(224, 224, 224);
            public static Color Sidebar = Color.FromArgb(45, 52, 71);
            public static Color SidebarButton = Color.FromArgb(60, 67, 86);
        }

        public static class Dark
        {
            public static Color Background = Color.FromArgb(18, 18, 18);
            public static Color Surface = Color.FromArgb(30, 30, 30);
            public static Color Primary = Color.FromArgb(100, 120, 220);
            public static Color Secondary = Color.FromArgb(80, 180, 250);
            public static Color Text = Color.FromArgb(230, 230, 230);
            public static Color TextSecondary = Color.FromArgb(160, 160, 160);
            public static Color Border = Color.FromArgb(60, 60, 60);
            public static Color Sidebar = Color.FromArgb(25, 25, 25);
            public static Color SidebarButton = Color.FromArgb(35, 35, 35);
        }

        /// <summary>
        /// Retorna a cor atual baseada no tema ativo
        /// </summary>
        public static Color GetColor(Func<Color> lightColor, Func<Color> darkColor)
        {
            return IsDarkMode ? darkColor() : lightColor();
        }

        /// <summary>
        /// Aplica tema em um formulário e todos os seus controles
        /// </summary>
        public static void ApplyTheme(Form form, bool darkMode)
        {
            IsDarkMode = darkMode;

            if (darkMode)
            {
                form.BackColor = Dark.Background;
                form.ForeColor = Dark.Text;
                ApplyThemeToControls(form.Controls, true);
            }
            else
            {
                form.BackColor = Light.Background;
                form.ForeColor = Light.Text;
                ApplyThemeToControls(form.Controls, false);
            }
        }

        private static void ApplyThemeToControls(Control.ControlCollection controls, bool darkMode)
        {
            foreach (Control control in controls)
            {
                // Ignorar controles específicos que têm cores próprias
                if (control is Button btn)
                {
                    // Botões mantêm suas cores específicas
                    continue;
                }
                else if (control is Panel pnl)
                {
                    // Sidebar e cards específicos mantêm cores
                    if (pnl.BackColor == Light.Sidebar || pnl.BackColor == Dark.Sidebar)
                    {
                        pnl.BackColor = darkMode ? Dark.Sidebar : Light.Sidebar;
                    }
                    else if (pnl.BackColor == Color.White || pnl.BackColor == Dark.Surface)
                    {
                        pnl.BackColor = darkMode ? Dark.Surface : Color.White;
                    }
                }
                else if (control is TextBox txt)
                {
                    txt.BackColor = darkMode ? Dark.Surface : Color.White;
                    txt.ForeColor = darkMode ? Dark.Text : Light.Text;
                }
                else if (control is ComboBox cmb)
                {
                    cmb.BackColor = darkMode ? Dark.Surface : Color.White;
                    cmb.ForeColor = darkMode ? Dark.Text : Light.Text;
                }
                else if (control is DataGridView dgv)
                {
                    dgv.BackgroundColor = darkMode ? Dark.Surface : Color.White;
                    dgv.ForeColor = darkMode ? Dark.Text : Light.Text;
                    dgv.DefaultCellStyle.BackColor = darkMode ? Dark.Surface : Color.White;
                    dgv.DefaultCellStyle.ForeColor = darkMode ? Dark.Text : Light.Text;
                    dgv.AlternatingRowsDefaultCellStyle.BackColor = darkMode ? Dark.Background : Color.FromArgb(250, 250, 250);
                }
                else if (control is Label lbl)
                {
                    // Labels secundárias
                    if (lbl.ForeColor == Color.Gray || lbl.ForeColor == Dark.TextSecondary)
                    {
                        lbl.ForeColor = darkMode ? Dark.TextSecondary : Color.Gray;
                    }
                }

                // Aplicar recursivamente aos controles filhos
                if (control.Controls.Count > 0)
                {
                    ApplyThemeToControls(control.Controls, darkMode);
                }
            }
        }

        /// <summary>
        /// Alterna entre modo claro e escuro
        /// </summary>
        public static void ToggleTheme(Form form)
        {
            ApplyTheme(form, !IsDarkMode);
        }

        /// <summary>
        /// Cria um botão de toggle para modo escuro
        /// </summary>
        public static Button CreateThemeToggleButton()
        {
            var btn = new Button
            {
                Text = "🌙 Modo Escuro",
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(158, 158, 158),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9F)
            };
            btn.FlatAppearance.BorderSize = 0;

            btn.Click += (s, e) => {
                IsDarkMode = !IsDarkMode;
                btn.Text = IsDarkMode ? "☀️ Modo Claro" : "🌙 Modo Escuro";

                // Encontrar o form pai e aplicar tema
                var form = btn.FindForm();
                if (form != null)
                {
                    ApplyTheme(form, IsDarkMode);
                }
            };

            return btn;
        }
    }
}
