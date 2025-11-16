using System;
using System.Drawing;
using System.Windows.Forms;

namespace BibliotecaJK.Components
{
    /// <summary>
    /// Layout Manager - Gerenciador de layouts responsivos e adaptativos
    /// Fornece sistema de breakpoints e helpers para criar interfaces que se adaptam a diferentes tamanhos de tela
    /// </summary>
    public static class LayoutManager
    {
        /// <summary>
        /// Tamanhos de tela baseados em breakpoints
        /// </summary>
        public enum ScreenSize
        {
            Small,       // < 1366px (1280x720, 1366x768)
            Medium,      // 1366-1919px (1600x900, 1680x1050)
            Large,       // 1920-2559px (1920x1080, 2048x1152)
            ExtraLarge   // >= 2560px (4K e superior)
        }

        /// <summary>
        /// Modos de layout que afetam densidade de espaçamento
        /// </summary>
        public enum LayoutMode
        {
            Compact,    // Espaçamento reduzido para maximizar conteúdo
            Standard,   // Espaçamento padrão balanceado
            Spacious    // Espaçamento amplo para conforto visual
        }

        // Breakpoints em pixels
        private const int BREAKPOINT_SMALL = 1366;
        private const int BREAKPOINT_MEDIUM = 1920;
        private const int BREAKPOINT_LARGE = 2560;

        /// <summary>
        /// Determina o tamanho de tela baseado na largura em pixels
        /// </summary>
        /// <param name="width">Largura da tela ou janela em pixels</param>
        /// <returns>Categoria de tamanho de tela</returns>
        public static ScreenSize GetScreenSize(int width)
        {
            if (width < BREAKPOINT_SMALL)
                return ScreenSize.Small;
            if (width < BREAKPOINT_MEDIUM)
                return ScreenSize.Medium;
            if (width < BREAKPOINT_LARGE)
                return ScreenSize.Large;
            return ScreenSize.ExtraLarge;
        }

        /// <summary>
        /// Retorna espaçamento apropriado baseado no tamanho da tela e modo de layout
        /// </summary>
        /// <param name="size">Tamanho da tela</param>
        /// <param name="mode">Modo de layout</param>
        /// <returns>Espaçamento em pixels</returns>
        public static int GetSpacing(ScreenSize size, LayoutMode mode = LayoutMode.Standard)
        {
            // Base de espaçamento para cada tamanho de tela
            int baseSpacing = size switch
            {
                ScreenSize.Small => 8,
                ScreenSize.Medium => 12,
                ScreenSize.Large => 16,
                ScreenSize.ExtraLarge => 20,
                _ => 12
            };

            // Multiplicador baseado no modo
            float multiplier = mode switch
            {
                LayoutMode.Compact => 0.75f,
                LayoutMode.Standard => 1.0f,
                LayoutMode.Spacious => 1.5f,
                _ => 1.0f
            };

            return (int)(baseSpacing * multiplier);
        }

        /// <summary>
        /// Retorna altura apropriada para controles baseado no tamanho da tela
        /// </summary>
        /// <param name="size">Tamanho da tela</param>
        /// <returns>Altura em pixels</returns>
        public static int GetControlHeight(ScreenSize size)
        {
            return size switch
            {
                ScreenSize.Small => 32,        // Compacto para telas pequenas
                ScreenSize.Medium => 36,       // Altura padrão confortável
                ScreenSize.Large => 40,        // Maior para facilitar cliques
                ScreenSize.ExtraLarge => 44,   // Ainda maior para 4K
                _ => 36
            };
        }

        /// <summary>
        /// Retorna fonte apropriada baseado no tamanho da tela
        /// </summary>
        /// <param name="size">Tamanho da tela</param>
        /// <param name="style">Estilo da fonte</param>
        /// <returns>Objeto Font configurado</returns>
        public static Font GetFont(ScreenSize size, FontStyle style = FontStyle.Regular)
        {
            float fontSize = size switch
            {
                ScreenSize.Small => 9F,
                ScreenSize.Medium => 10F,
                ScreenSize.Large => 11F,
                ScreenSize.ExtraLarge => 12F,
                _ => 10F
            };

            return new Font("Segoe UI", fontSize, style);
        }

        /// <summary>
        /// Retorna fonte para títulos baseado no tamanho da tela
        /// </summary>
        /// <param name="size">Tamanho da tela</param>
        /// <param name="level">Nível do título (1=maior, 6=menor)</param>
        /// <returns>Objeto Font configurado</returns>
        public static Font GetHeadingFont(ScreenSize size, int level = 1)
        {
            if (level < 1) level = 1;
            if (level > 6) level = 6;

            // Tamanhos base para H1-H6
            float[] baseSizes = { 24F, 20F, 18F, 16F, 14F, 12F };
            float baseSize = baseSizes[level - 1];

            // Ajuste baseado no tamanho da tela
            float multiplier = size switch
            {
                ScreenSize.Small => 0.9f,
                ScreenSize.Medium => 1.0f,
                ScreenSize.Large => 1.1f,
                ScreenSize.ExtraLarge => 1.2f,
                _ => 1.0f
            };

            return new Font("Segoe UI", baseSize * multiplier, FontStyle.Bold);
        }

        /// <summary>
        /// Aplica margens responsivas a um controle
        /// </summary>
        /// <param name="control">Controle a ser ajustado</param>
        /// <param name="size">Tamanho da tela</param>
        public static void ApplyResponsiveMargins(Control control, ScreenSize size)
        {
            int spacing = GetSpacing(size);
            control.Margin = new Padding(spacing);
        }

        /// <summary>
        /// Aplica margens responsivas personalizadas a um controle
        /// </summary>
        /// <param name="control">Controle a ser ajustado</param>
        /// <param name="size">Tamanho da tela</param>
        /// <param name="top">Margem superior em unidades relativas</param>
        /// <param name="right">Margem direita em unidades relativas</param>
        /// <param name="bottom">Margem inferior em unidades relativas</param>
        /// <param name="left">Margem esquerda em unidades relativas</param>
        public static void ApplyResponsiveMargins(Control control, ScreenSize size, int top, int right, int bottom, int left)
        {
            int spacing = GetSpacing(size);
            control.Margin = new Padding(
                left * spacing,
                top * spacing,
                right * spacing,
                bottom * spacing
            );
        }

        /// <summary>
        /// Aplica padding responsivo a um controle
        /// </summary>
        /// <param name="control">Controle a ser ajustado</param>
        /// <param name="size">Tamanho da tela</param>
        public static void ApplyResponsivePadding(Control control, ScreenSize size)
        {
            int spacing = GetSpacing(size);
            control.Padding = new Padding(spacing);
        }

        /// <summary>
        /// Configura FlowLayoutPanel para layout responsivo
        /// </summary>
        /// <param name="panel">Panel a ser configurado</param>
        /// <param name="size">Tamanho da tela</param>
        public static void ApplyFlowLayout(FlowLayoutPanel panel, ScreenSize size)
        {
            // Ajusta direção do fluxo baseado no tamanho
            if (size == ScreenSize.Small)
            {
                panel.FlowDirection = FlowDirection.TopDown;
                panel.WrapContents = false;
            }
            else
            {
                panel.FlowDirection = FlowDirection.LeftToRight;
                panel.WrapContents = true;
            }

            // Aplica espaçamento responsivo
            ApplyResponsivePadding(panel, size);
        }

        /// <summary>
        /// Configura TableLayoutPanel para layout responsivo
        /// </summary>
        /// <param name="panel">Panel a ser configurado</param>
        /// <param name="size">Tamanho da tela</param>
        public static void ApplyTableLayout(TableLayoutPanel panel, ScreenSize size)
        {
            // Aplica espaçamento entre células
            int spacing = GetSpacing(size);
            panel.Padding = new Padding(spacing);

            // Ajusta espaçamento entre colunas e linhas
            panel.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
        }

        /// <summary>
        /// Retorna número de colunas ideal para um grid responsivo
        /// </summary>
        /// <param name="size">Tamanho da tela</param>
        /// <returns>Número de colunas</returns>
        public static int GetGridColumns(ScreenSize size)
        {
            return size switch
            {
                ScreenSize.Small => 1,        // Uma coluna em telas pequenas
                ScreenSize.Medium => 2,       // Duas colunas em telas médias
                ScreenSize.Large => 3,        // Três colunas em telas grandes
                ScreenSize.ExtraLarge => 4,   // Quatro colunas em 4K
                _ => 2
            };
        }

        /// <summary>
        /// Retorna largura ideal para cards em um layout de grid
        /// </summary>
        /// <param name="containerWidth">Largura do container</param>
        /// <param name="size">Tamanho da tela</param>
        /// <returns>Largura do card em pixels</returns>
        public static int GetCardWidth(int containerWidth, ScreenSize size)
        {
            int columns = GetGridColumns(size);
            int spacing = GetSpacing(size);
            int totalSpacing = spacing * (columns + 1); // Espaçamento entre e nas bordas

            return (containerWidth - totalSpacing) / columns;
        }

        /// <summary>
        /// Aplica configurações responsivas a um formulário
        /// </summary>
        /// <param name="form">Formulário a ser configurado</param>
        public static void MakeFormResponsive(Form form)
        {
            // Habilita auto-escala por DPI
            form.AutoScaleMode = AutoScaleMode.Dpi;
            form.AutoScaleDimensions = new SizeF(96F, 96F);

            // Define tamanho mínimo baseado em tela pequena
            form.MinimumSize = new Size(1280, 720);

            // Adiciona handler para redimensionamento
            var currentSize = GetScreenSize(form.Width);
            form.Resize += (sender, e) =>
            {
                var newSize = GetScreenSize(form.Width);
                if (newSize != currentSize)
                {
                    currentSize = newSize;
                    // Disparar evento customizado se necessário
                    OnScreenSizeChanged(form, newSize);
                }
            };
        }

        /// <summary>
        /// Método auxiliar para notificar mudanças de tamanho de tela
        /// </summary>
        private static void OnScreenSizeChanged(Form form, ScreenSize newSize)
        {
            // Pode ser expandido para notificar controles filhos
            // Por enquanto, apenas força refresh visual
            form.Refresh();
        }

        /// <summary>
        /// Calcula largura de sidebar baseado no tamanho da tela
        /// </summary>
        /// <param name="size">Tamanho da tela</param>
        /// <param name="collapsed">Se a sidebar está colapsada</param>
        /// <returns>Largura da sidebar em pixels</returns>
        public static int GetSidebarWidth(ScreenSize size, bool collapsed = false)
        {
            if (collapsed || size == ScreenSize.Small)
                return 60; // Apenas ícones

            return size switch
            {
                ScreenSize.Medium => 220,
                ScreenSize.Large => 250,
                ScreenSize.ExtraLarge => 280,
                _ => 250
            };
        }

        /// <summary>
        /// Retorna tamanho de ícone apropriado baseado no tamanho da tela
        /// </summary>
        /// <param name="size">Tamanho da tela</param>
        /// <returns>Tamanho do ícone em pixels</returns>
        public static int GetIconSize(ScreenSize size)
        {
            return size switch
            {
                ScreenSize.Small => 16,
                ScreenSize.Medium => 20,
                ScreenSize.Large => 24,
                ScreenSize.ExtraLarge => 28,
                _ => 20
            };
        }

        /// <summary>
        /// Retorna raio de borda apropriado para elementos arredondados
        /// </summary>
        /// <param name="size">Tamanho da tela</param>
        /// <returns>Raio em pixels</returns>
        public static int GetBorderRadius(ScreenSize size)
        {
            return size switch
            {
                ScreenSize.Small => 4,
                ScreenSize.Medium => 6,
                ScreenSize.Large => 8,
                ScreenSize.ExtraLarge => 10,
                _ => 6
            };
        }
    }
}
