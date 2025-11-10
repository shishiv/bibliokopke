using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BibliotecaJK.Components
{
    /// <summary>
    /// Keyboard Shortcut Manager - Gerenciador de atalhos de teclado
    /// </summary>
    public class KeyboardShortcutManager
    {
        private Form _form;
        private Dictionary<Keys, Action> _shortcuts = new Dictionary<Keys, Action>();
        private Dictionary<Keys, string> _shortcutDescriptions = new Dictionary<Keys, string>();

        public KeyboardShortcutManager(Form form)
        {
            _form = form;
            _form.KeyPreview = true;
            _form.KeyDown += Form_KeyDown;
        }

        /// <summary>
        /// Registra um atalho de teclado
        /// </summary>
        public void RegisterShortcut(Keys key, Action action, string description = "")
        {
            _shortcuts[key] = action;
            if (!string.IsNullOrEmpty(description))
                _shortcutDescriptions[key] = description;
        }

        /// <summary>
        /// Remove um atalho de teclado
        /// </summary>
        public void UnregisterShortcut(Keys key)
        {
            _shortcuts.Remove(key);
            _shortcutDescriptions.Remove(key);
        }

        private void Form_KeyDown(object? sender, KeyEventArgs e)
        {
            if (_shortcuts.ContainsKey(e.KeyData))
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                _shortcuts[e.KeyData]?.Invoke();
            }
        }

        /// <summary>
        /// Mostra uma janela com todos os atalhos disponíveis
        /// </summary>
        public void ShowShortcutsHelp()
        {
            var helpForm = new Form
            {
                Text = "Atalhos de Teclado",
                Size = new Size(500, 600),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White
            };

            var lblTitulo = new Label
            {
                Text = "⌨️ ATALHOS DE TECLADO DISPONÍVEIS",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(63, 81, 181),
                Location = new Point(20, 20),
                Size = new Size(460, 30)
            };
            helpForm.Controls.Add(lblTitulo);

            var pnlLista = new Panel
            {
                Location = new Point(20, 60),
                Size = new Size(460, 480),
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle
            };

            int y = 10;
            foreach (var shortcut in _shortcutDescriptions)
            {
                var pnlItem = new Panel
                {
                    Location = new Point(10, y),
                    Size = new Size(420, 50),
                    BackColor = Color.FromArgb(245, 245, 250)
                };

                var lblKey = new Label
                {
                    Text = FormatKeyName(shortcut.Key),
                    Font = new Font("Consolas", 10F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(63, 81, 181),
                    Location = new Point(10, 10),
                    Size = new Size(150, 30),
                    TextAlign = ContentAlignment.MiddleLeft
                };
                pnlItem.Controls.Add(lblKey);

                var lblDesc = new Label
                {
                    Text = shortcut.Value,
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = Color.Gray,
                    Location = new Point(170, 10),
                    Size = new Size(240, 30),
                    TextAlign = ContentAlignment.MiddleLeft
                };
                pnlItem.Controls.Add(lblDesc);

                pnlLista.Controls.Add(pnlItem);
                y += 60;
            }

            helpForm.Controls.Add(pnlLista);

            var btnFechar = new Button
            {
                Text = "Fechar",
                Location = new Point(390, 550),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(158, 158, 158),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnFechar.FlatAppearance.BorderSize = 0;
            btnFechar.Click += (s, e) => helpForm.Close();
            helpForm.Controls.Add(btnFechar);

            helpForm.ShowDialog(_form);
        }

        /// <summary>
        /// Formata o nome da tecla de forma legível
        /// </summary>
        private string FormatKeyName(Keys key)
        {
            string keyName = key.ToString();

            // Separar modificadores
            var parts = new List<string>();

            if (key.HasFlag(Keys.Control))
                parts.Add("Ctrl");
            if (key.HasFlag(Keys.Alt))
                parts.Add("Alt");
            if (key.HasFlag(Keys.Shift))
                parts.Add("Shift");

            // Remover modificadores da tecla
            Keys baseKey = key & ~Keys.Control & ~Keys.Alt & ~Keys.Shift;

            // Traduzir nomes de teclas comuns
            string baseName = baseKey switch
            {
                Keys.D1 => "1",
                Keys.D2 => "2",
                Keys.D3 => "3",
                Keys.D4 => "4",
                Keys.D5 => "5",
                Keys.D6 => "6",
                Keys.D7 => "7",
                Keys.D8 => "8",
                Keys.D9 => "9",
                Keys.D0 => "0",
                Keys.Return => "Enter",
                Keys.Escape => "Esc",
                Keys.Back => "Backspace",
                Keys.Delete => "Del",
                Keys.Space => "Espaço",
                _ => baseKey.ToString()
            };

            parts.Add(baseName);

            return string.Join(" + ", parts);
        }

        /// <summary>
        /// Atalhos padrão comuns em formulários
        /// </summary>
        public static class CommonShortcuts
        {
            public static void SetupFormShortcuts(Form form, Button? btnSalvar = null, Button? btnCancelar = null)
            {
                form.KeyPreview = true;

                form.KeyDown += (s, e) =>
                {
                    // Enter para salvar
                    if (e.KeyCode == Keys.Enter && e.Modifiers == Keys.Control && btnSalvar != null && btnSalvar.Enabled)
                    {
                        e.Handled = true;
                        btnSalvar.PerformClick();
                    }
                    // Esc para cancelar
                    else if (e.KeyCode == Keys.Escape && btnCancelar != null)
                    {
                        e.Handled = true;
                        btnCancelar.PerformClick();
                    }
                    // F1 para ajuda
                    else if (e.KeyCode == Keys.F1)
                    {
                        e.Handled = true;
                        MessageBox.Show(
                            "ATALHOS DISPONÍVEIS:\n\n" +
                            "Ctrl + Enter - Salvar\n" +
                            "Esc - Cancelar\n" +
                            "F1 - Ajuda",
                            "Atalhos de Teclado",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                };
            }

            /// <summary>
            /// Adiciona foco automático ao pressionar Enter em TextBox
            /// </summary>
            public static void SetupTabOrder(params Control[] controls)
            {
                for (int i = 0; i < controls.Length; i++)
                {
                    int nextIndex = i + 1;
                    if (controls[i] is TextBox txt)
                    {
                        txt.KeyDown += (s, e) =>
                        {
                            if (e.KeyCode == Keys.Enter)
                            {
                                e.Handled = true;
                                e.SuppressKeyPress = true;
                                if (nextIndex < controls.Length)
                                    controls[nextIndex].Focus();
                            }
                        };
                    }
                }
            }
        }
    }
}
