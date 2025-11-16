using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using BibliotecaJK.Components;

namespace BibliotecaJK.Helpers
{
    /// <summary>
    /// FocusManager - Gerenciador de indicadores visuais de foco para acessibilidade
    ///
    /// Fornece recursos avançados de gerenciamento de foco:
    /// - Indicadores visuais de foco com anéis customizáveis
    /// - Histórico de navegação de foco
    /// - Distinção entre foco por teclado e mouse
    /// - Armadilhas de foco para diálogos modais
    /// - Destacamento temporário de controles
    /// </summary>
    public static class FocusManager
    {
        #region Private Fields

        // Dicionário para rastrear forms que têm focus tracking habilitado
        private static readonly Dictionary<Form, FocusTrackingData> _trackedForms = new Dictionary<Form, FocusTrackingData>();

        // Cor global para indicadores de foco
        private static Color _globalFocusColor = ThemeManager.Colors.Primary500;

        // Flag para rastrear se o último foco foi via teclado
        private static bool _lastFocusWasKeyboard = false;

        // Histórico de foco global
        private static readonly Stack<Control> _focusHistory = new Stack<Control>();

        // Limite do histórico
        private const int MAX_FOCUS_HISTORY = 50;

        #endregion

        #region Helper Classes

        /// <summary>
        /// Dados de rastreamento de foco para cada formulário
        /// </summary>
        private class FocusTrackingData
        {
            public Color FocusColor { get; set; }
            public Dictionary<Control, Color> OriginalColors { get; set; }
            public Dictionary<Control, EventHandler> PaintHandlers { get; set; }
            public List<Control> FocusableControls { get; set; }
            public Timer HighlightTimer { get; set; }
            public Control HighlightedControl { get; set; }
            public int HighlightAlpha { get; set; }
            public bool IsFocusTrapped { get; set; }
            public List<Control> TrapControls { get; set; }

            public FocusTrackingData()
            {
                OriginalColors = new Dictionary<Control, Color>();
                PaintHandlers = new Dictionary<Control, EventHandler>();
                FocusableControls = new List<Control>();
                TrapControls = new List<Control>();
                HighlightAlpha = 255;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Habilita indicadores visuais de foco para todos os controles em um formulário
        /// </summary>
        /// <param name="form">Formulário alvo</param>
        /// <param name="focusColor">Cor do indicador de foco (usa Primary500 se não especificado)</param>
        public static void EnableFocusIndicators(Form form, Color focusColor = default)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));

            // Se já está rastreado, remover rastreamento anterior
            if (_trackedForms.ContainsKey(form))
            {
                DisableFocusIndicators(form);
            }

            // Usar cor padrão se não especificada
            Color effectiveFocusColor = focusColor == default ? _globalFocusColor : focusColor;

            // Criar dados de rastreamento
            var trackingData = new FocusTrackingData
            {
                FocusColor = effectiveFocusColor
            };

            _trackedForms[form] = trackingData;

            // Encontrar e configurar todos os controles focáveis
            ConfigureFocusableControls(form, trackingData);

            // Configurar event handlers globais do formulário
            form.KeyDown += Form_KeyDown;
            form.MouseDown += Form_MouseDown;
            form.FormClosed += Form_FormClosed;
        }

        /// <summary>
        /// Desabilita indicadores de foco para um formulário
        /// </summary>
        /// <param name="form">Formulário alvo</param>
        public static void DisableFocusIndicators(Form form)
        {
            if (form == null || !_trackedForms.ContainsKey(form))
                return;

            var trackingData = _trackedForms[form];

            // Remover event handlers de todos os controles
            foreach (var kvp in trackingData.PaintHandlers)
            {
                kvp.Key.Paint -= kvp.Value;
            }

            // Limpar timer se existir
            if (trackingData.HighlightTimer != null)
            {
                trackingData.HighlightTimer.Stop();
                trackingData.HighlightTimer.Dispose();
            }

            // Remover event handlers do formulário
            form.KeyDown -= Form_KeyDown;
            form.MouseDown -= Form_MouseDown;
            form.FormClosed -= Form_FormClosed;

            // Invalidar controles para remover focus rings
            foreach (var control in trackingData.FocusableControls)
            {
                control.Invalidate();
            }

            _trackedForms.Remove(form);
        }

        /// <summary>
        /// Desenha um anel de foco ao redor de um controle
        /// </summary>
        /// <param name="control">Controle alvo</param>
        /// <param name="g">Objeto Graphics para desenho</param>
        /// <param name="color">Cor do anel</param>
        public static void DrawFocusRing(Control control, Graphics g, Color color)
        {
            if (control == null || g == null)
                return;

            // Configurar anti-aliasing para desenho suave
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // Criar retângulo com offset de 2px
            Rectangle rect = new Rectangle(
                2,
                2,
                control.Width - 5,
                control.Height - 5
            );

            // Desenhar anel de foco com 2px de largura
            using (Pen focusPen = new Pen(color, 2f))
            {
                // Aplicar pequeno arredondamento nas bordas
                int radius = 4;
                using (GraphicsPath path = CreateRoundedRectanglePath(rect, radius))
                {
                    g.DrawPath(focusPen, path);
                }
            }

            // Restaurar configurações
            g.SmoothingMode = SmoothingMode.Default;
            g.PixelOffsetMode = PixelOffsetMode.Default;
        }

        /// <summary>
        /// Define a cor global dos indicadores de foco
        /// </summary>
        /// <param name="color">Nova cor</param>
        public static void SetFocusColor(Color color)
        {
            _globalFocusColor = color;

            // Atualizar todos os forms rastreados que usam a cor padrão
            foreach (var kvp in _trackedForms)
            {
                if (kvp.Value.FocusColor == ThemeManager.Colors.Primary500)
                {
                    kvp.Value.FocusColor = color;
                }
            }
        }

        /// <summary>
        /// Retorna o histórico de foco
        /// </summary>
        /// <returns>Lista de controles em ordem de foco (mais recente primeiro)</returns>
        public static List<Control> GetFocusHistory()
        {
            return _focusHistory.ToList();
        }

        /// <summary>
        /// Limpa o histórico de foco
        /// </summary>
        public static void ClearFocusHistory()
        {
            _focusHistory.Clear();
        }

        /// <summary>
        /// Cria uma armadilha de foco para manter o foco dentro de um diálogo modal
        /// </summary>
        /// <param name="dialog">Formulário de diálogo</param>
        public static void CreateFocusTrap(Form dialog)
        {
            if (dialog == null)
                throw new ArgumentNullException(nameof(dialog));

            // Garantir que o formulário tem focus tracking
            if (!_trackedForms.ContainsKey(dialog))
            {
                EnableFocusIndicators(dialog);
            }

            var trackingData = _trackedForms[dialog];
            trackingData.IsFocusTrapped = true;

            // Coletar todos os controles focáveis no diálogo
            trackingData.TrapControls = GetFocusableControls(dialog)
                .Where(c => c.Visible && c.Enabled)
                .ToList();

            // Configurar evento de navegação
            dialog.KeyDown -= Dialog_KeyDown;
            dialog.KeyDown += Dialog_KeyDown;

            // Focar no primeiro controle focável
            if (trackingData.TrapControls.Any())
            {
                trackingData.TrapControls.First().Focus();
            }
        }

        /// <summary>
        /// Remove a armadilha de foco de um diálogo
        /// </summary>
        /// <param name="dialog">Formulário de diálogo</param>
        public static void RemoveFocusTrap(Form dialog)
        {
            if (dialog == null || !_trackedForms.ContainsKey(dialog))
                return;

            var trackingData = _trackedForms[dialog];
            trackingData.IsFocusTrapped = false;
            trackingData.TrapControls.Clear();

            dialog.KeyDown -= Dialog_KeyDown;
        }

        /// <summary>
        /// Destaca temporariamente um controle com animação de fade
        /// </summary>
        /// <param name="control">Controle a ser destacado</param>
        /// <param name="durationMs">Duração do destaque em milissegundos</param>
        public static void HighlightControl(Control control, int durationMs = 2000)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            var form = control.FindForm();
            if (form == null || !_trackedForms.ContainsKey(form))
                return;

            var trackingData = _trackedForms[form];

            // Parar highlight anterior se existir
            if (trackingData.HighlightTimer != null)
            {
                trackingData.HighlightTimer.Stop();
                trackingData.HighlightTimer.Dispose();
            }

            // Configurar novo highlight
            trackingData.HighlightedControl = control;
            trackingData.HighlightAlpha = 255;

            // Rolar para tornar o controle visível se possível
            ScrollControlIntoView(control);

            // Criar timer para animação de fade
            trackingData.HighlightTimer = new Timer
            {
                Interval = 50 // 50ms = ~20fps
            };

            int elapsed = 0;
            trackingData.HighlightTimer.Tick += (s, e) =>
            {
                elapsed += 50;

                if (elapsed >= durationMs)
                {
                    // Fade out nos últimos 500ms
                    int fadeTime = Math.Min(500, durationMs / 4);
                    int fadeElapsed = elapsed - (durationMs - fadeTime);

                    if (fadeElapsed >= fadeTime)
                    {
                        // Terminar animação
                        trackingData.HighlightTimer.Stop();
                        trackingData.HighlightedControl = null;
                        control.Invalidate();
                        return;
                    }

                    // Calcular alpha para fade out
                    trackingData.HighlightAlpha = 255 - (int)(255.0 * fadeElapsed / fadeTime);
                }

                control.Invalidate();
            };

            trackingData.HighlightTimer.Start();

            // Invalidar controle para forçar redesenho
            control.Invalidate();
        }

        /// <summary>
        /// Verifica se o último foco foi via teclado
        /// </summary>
        /// <returns>True se o foco foi via teclado (Tab, setas, etc.)</returns>
        public static bool IsKeyboardFocus()
        {
            return _lastFocusWasKeyboard;
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Configura todos os controles focáveis em um formulário
        /// </summary>
        private static void ConfigureFocusableControls(Control parent, FocusTrackingData trackingData)
        {
            var focusableControls = GetFocusableControls(parent);

            foreach (var control in focusableControls)
            {
                trackingData.FocusableControls.Add(control);

                // Armazenar cor original
                trackingData.OriginalColors[control] = control.BackColor;

                // Criar e armazenar event handler
                EventHandler paintHandler = (s, e) => Control_Paint(s, e, trackingData);
                trackingData.PaintHandlers[control] = paintHandler;

                // Registrar event handlers
                control.Paint += paintHandler;
                control.GotFocus += Control_GotFocus;
                control.LostFocus += Control_LostFocus;
            }
        }

        /// <summary>
        /// Obtém todos os controles focáveis recursivamente
        /// </summary>
        private static List<Control> GetFocusableControls(Control parent)
        {
            var focusableControls = new List<Control>();

            foreach (Control control in parent.Controls)
            {
                // Verificar se o controle pode receber foco
                if (control.CanSelect && control.TabStop)
                {
                    focusableControls.Add(control);
                }

                // Recursão para controles filhos
                if (control.HasChildren)
                {
                    focusableControls.AddRange(GetFocusableControls(control));
                }
            }

            return focusableControls;
        }

        /// <summary>
        /// Cria um caminho de retângulo arredondado
        /// </summary>
        private static GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            // Canto superior esquerdo
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);

            // Lado superior
            path.AddLine(rect.X + radius, rect.Y, rect.Right - radius, rect.Y);

            // Canto superior direito
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);

            // Lado direito
            path.AddLine(rect.Right, rect.Y + radius, rect.Right, rect.Bottom - radius);

            // Canto inferior direito
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);

            // Lado inferior
            path.AddLine(rect.Right - radius, rect.Bottom, rect.X + radius, rect.Bottom);

            // Canto inferior esquerdo
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);

            // Lado esquerdo
            path.AddLine(rect.X, rect.Bottom - radius, rect.X, rect.Y + radius);

            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// Rola um controle para torná-lo visível
        /// </summary>
        private static void ScrollControlIntoView(Control control)
        {
            // Verificar se o controle está dentro de um painel com scroll
            Control parent = control.Parent;
            while (parent != null)
            {
                if (parent is Panel panel && panel.AutoScroll)
                {
                    panel.ScrollControlIntoView(control);
                    return;
                }
                parent = parent.Parent;
            }

            // Tentar focar o controle (pode causar scroll automático)
            if (control.CanFocus)
            {
                control.Focus();
            }
        }

        /// <summary>
        /// Adiciona controle ao histórico de foco
        /// </summary>
        private static void AddToFocusHistory(Control control)
        {
            // Remover duplicatas recentes
            if (_focusHistory.Count > 0 && _focusHistory.Peek() == control)
                return;

            _focusHistory.Push(control);

            // Limitar tamanho do histórico
            if (_focusHistory.Count > MAX_FOCUS_HISTORY)
            {
                var items = _focusHistory.ToList();
                _focusHistory.Clear();
                foreach (var item in items.Take(MAX_FOCUS_HISTORY))
                {
                    _focusHistory.Push(item);
                }
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handler de Paint para desenhar indicadores de foco
        /// </summary>
        private static void Control_Paint(object sender, EventArgs e, FocusTrackingData trackingData)
        {
            if (!(sender is Control control))
                return;

            using (Graphics g = control.CreateGraphics())
            {
                // Desenhar anel de foco se o controle tem foco E o último foco foi via teclado
                if (control.Focused && _lastFocusWasKeyboard)
                {
                    DrawFocusRing(control, g, trackingData.FocusColor);
                }

                // Desenhar highlight se este é o controle destacado
                if (trackingData.HighlightedControl == control && trackingData.HighlightAlpha > 0)
                {
                    Color highlightColor = Color.FromArgb(
                        trackingData.HighlightAlpha,
                        ThemeManager.Colors.Secondary300
                    );

                    using (SolidBrush brush = new SolidBrush(highlightColor))
                    {
                        g.FillRectangle(brush, 0, 0, control.Width, control.Height);
                    }

                    // Desenhar borda de destaque
                    using (Pen pen = new Pen(ThemeManager.Colors.Secondary500, 3f))
                    {
                        g.DrawRectangle(pen, 1, 1, control.Width - 3, control.Height - 3);
                    }
                }
            }
        }

        /// <summary>
        /// Handler quando controle recebe foco
        /// </summary>
        private static void Control_GotFocus(object sender, EventArgs e)
        {
            if (sender is Control control)
            {
                AddToFocusHistory(control);
                control.Invalidate(); // Redesenhar para mostrar focus ring
            }
        }

        /// <summary>
        /// Handler quando controle perde foco
        /// </summary>
        private static void Control_LostFocus(object sender, EventArgs e)
        {
            if (sender is Control control)
            {
                control.Invalidate(); // Redesenhar para remover focus ring
            }
        }

        /// <summary>
        /// Handler de teclas pressionadas no formulário
        /// </summary>
        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            // Teclas de navegação indicam foco por teclado
            if (e.KeyCode == Keys.Tab ||
                e.KeyCode == Keys.Up ||
                e.KeyCode == Keys.Down ||
                e.KeyCode == Keys.Left ||
                e.KeyCode == Keys.Right ||
                e.KeyCode == Keys.Enter)
            {
                _lastFocusWasKeyboard = true;
            }
        }

        /// <summary>
        /// Handler de mouse pressionado no formulário
        /// </summary>
        private static void Form_MouseDown(object sender, MouseEventArgs e)
        {
            // Mouse indica foco por mouse (não mostrar focus ring)
            _lastFocusWasKeyboard = false;
        }

        /// <summary>
        /// Handler quando formulário é fechado
        /// </summary>
        private static void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (sender is Form form)
            {
                DisableFocusIndicators(form);
            }
        }

        /// <summary>
        /// Handler de teclas para diálogo com focus trap
        /// </summary>
        private static void Dialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(sender is Form dialog) || !_trackedForms.ContainsKey(dialog))
                return;

            var trackingData = _trackedForms[dialog];
            if (!trackingData.IsFocusTrapped || !trackingData.TrapControls.Any())
                return;

            // Processar apenas Tab e Shift+Tab
            if (e.KeyCode != Keys.Tab)
                return;

            e.Handled = true;
            e.SuppressKeyPress = true;

            // Encontrar controle atualmente focado
            var currentControl = dialog.ActiveControl;
            int currentIndex = trackingData.TrapControls.IndexOf(currentControl);

            // Calcular próximo índice
            int nextIndex;
            if (e.Shift)
            {
                // Shift+Tab: voltar
                nextIndex = currentIndex <= 0
                    ? trackingData.TrapControls.Count - 1
                    : currentIndex - 1;
            }
            else
            {
                // Tab: avançar
                nextIndex = currentIndex >= trackingData.TrapControls.Count - 1
                    ? 0
                    : currentIndex + 1;
            }

            // Focar próximo controle
            trackingData.TrapControls[nextIndex].Focus();
        }

        #endregion
    }
}
