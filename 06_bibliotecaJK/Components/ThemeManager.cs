using System;
using System.Drawing;
using System.Windows.Forms;

namespace BibliotecaJK.Components
{
    /// <summary>
    /// Theme Manager - Gerenciador de temas claro/escuro
    /// </summary>
    public static class ThemeManager
    {
        public static bool IsDarkMode { get; private set; } = false;

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
