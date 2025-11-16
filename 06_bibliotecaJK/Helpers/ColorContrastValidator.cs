using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using BibliotecaJK.Components;

namespace BibliotecaJK.Helpers
{
    /// <summary>
    /// Validador de contraste de cores conforme WCAG 2.1 (Web Content Accessibility Guidelines)
    /// Implementa cálculos de luminância relativa e razão de contraste para garantir acessibilidade visual
    /// </summary>
    /// <remarks>
    /// WCAG 2.1 define três níveis de conformidade:
    /// - WCAG AA (Normal Text): Razão de contraste >= 4.5:1
    /// - WCAG AAA (Normal Text): Razão de contraste >= 7:1
    /// - WCAG AA (Large Text 18pt+): Razão de contraste >= 3:1
    ///
    /// Referência: https://www.w3.org/WAI/WCAG21/Understanding/contrast-minimum.html
    ///
    /// Exemplos de uso:
    /// <code>
    /// // Verificar se um par de cores é acessível
    /// bool isAccessible = ColorContrastValidator.MeetsWCAG_AA(Color.White, Color.Black);
    ///
    /// // Calcular razão de contraste
    /// double ratio = ColorContrastValidator.CalculateContrastRatio(foreground, background);
    ///
    /// // Sugerir cor acessível
    /// Color suggested = ColorContrastValidator.SuggestAccessibleColor(Color.Yellow, Color.White);
    ///
    /// // Validar todas as cores do ThemeManager
    /// var results = ColorContrastValidator.ValidateThemeColors(verbose: true);
    /// </code>
    /// </remarks>
    public static class ColorContrastValidator
    {
        // === CONSTANTES WCAG 2.1 ===

        /// <summary>
        /// Razão de contraste mínima para WCAG AA (texto normal)
        /// </summary>
        public const double MIN_CONTRAST_AA = 4.5;

        /// <summary>
        /// Razão de contraste mínima para WCAG AAA (texto normal)
        /// </summary>
        public const double MIN_CONTRAST_AAA = 7.0;

        /// <summary>
        /// Razão de contraste mínima para WCAG AA (texto grande >= 18pt ou 14pt bold)
        /// </summary>
        public const double MIN_CONTRAST_AA_LARGE = 3.0;

        // === MÉTODOS PÚBLICOS DE VALIDAÇÃO ===

        /// <summary>
        /// Verifica se o par de cores atende ao padrão WCAG AA (razão >= 4.5:1)
        /// Adequado para texto normal (menor que 18pt regular ou 14pt bold)
        /// </summary>
        /// <param name="foreground">Cor do texto ou elemento em primeiro plano</param>
        /// <param name="background">Cor de fundo</param>
        /// <returns>True se a razão de contraste for >= 4.5:1</returns>
        /// <example>
        /// <code>
        /// // Testa se texto preto em fundo branco é acessível
        /// bool result = ColorContrastValidator.MeetsWCAG_AA(Color.Black, Color.White);
        /// // result = true (contraste 21:1)
        ///
        /// // Testa se cinza claro em branco é acessível
        /// bool result2 = ColorContrastValidator.MeetsWCAG_AA(Color.LightGray, Color.White);
        /// // result2 = false (contraste insuficiente)
        /// </code>
        /// </example>
        public static bool MeetsWCAG_AA(Color foreground, Color background)
        {
            double ratio = CalculateContrastRatio(foreground, background);
            return ratio >= MIN_CONTRAST_AA;
        }

        /// <summary>
        /// Verifica se o par de cores atende ao padrão WCAG AAA (razão >= 7:1)
        /// Padrão de acessibilidade aprimorado para maior inclusão
        /// </summary>
        /// <param name="foreground">Cor do texto ou elemento em primeiro plano</param>
        /// <param name="background">Cor de fundo</param>
        /// <returns>True se a razão de contraste for >= 7:1</returns>
        /// <example>
        /// <code>
        /// // Testa conformidade AAA
        /// bool result = ColorContrastValidator.MeetsWCAG_AAA(Color.Black, Color.White);
        /// // result = true (contraste 21:1, muito acima de 7:1)
        ///
        /// bool result2 = ColorContrastValidator.MeetsWCAG_AAA(Color.DarkGray, Color.White);
        /// // result2 depende da tonalidade exata do DarkGray
        /// </code>
        /// </example>
        public static bool MeetsWCAG_AAA(Color foreground, Color background)
        {
            double ratio = CalculateContrastRatio(foreground, background);
            return ratio >= MIN_CONTRAST_AAA;
        }

        /// <summary>
        /// Verifica se o par de cores atende ao padrão WCAG AA para texto grande (razão >= 3:1)
        /// Texto grande é definido como >= 18pt regular ou >= 14pt bold
        /// </summary>
        /// <param name="foreground">Cor do texto grande em primeiro plano</param>
        /// <param name="background">Cor de fundo</param>
        /// <returns>True se a razão de contraste for >= 3:1</returns>
        /// <example>
        /// <code>
        /// // Para títulos grandes (H1, H2, etc.)
        /// bool result = ColorContrastValidator.MeetsWCAG_AA_Large(
        ///     ThemeManager.Colors.Primary500,
        ///     Color.White
        /// );
        /// </code>
        /// </example>
        public static bool MeetsWCAG_AA_Large(Color foreground, Color background)
        {
            double ratio = CalculateContrastRatio(foreground, background);
            return ratio >= MIN_CONTRAST_AA_LARGE;
        }

        /// <summary>
        /// Calcula a razão de contraste entre duas cores conforme a fórmula WCAG
        /// Razão de contraste = (L1 + 0.05) / (L2 + 0.05), onde L1 é a luminância relativa mais clara
        /// </summary>
        /// <param name="c1">Primeira cor</param>
        /// <param name="c2">Segunda cor</param>
        /// <returns>Razão de contraste (1:1 a 21:1)</returns>
        /// <remarks>
        /// O resultado varia de 1:1 (nenhum contraste, mesma cor) até 21:1 (contraste máximo, preto e branco).
        /// Valores típicos:
        /// - 1:1 = Sem contraste (ilegível)
        /// - 3:1 = Mínimo para texto grande (WCAG AA Large)
        /// - 4.5:1 = Mínimo para texto normal (WCAG AA)
        /// - 7:1 = Mínimo para conformidade aprimorada (WCAG AAA)
        /// - 21:1 = Contraste máximo (preto puro em branco puro)
        /// </remarks>
        /// <example>
        /// <code>
        /// // Calcula contraste de preto em branco
        /// double ratio = ColorContrastValidator.CalculateContrastRatio(Color.Black, Color.White);
        /// // ratio = 21.0 (contraste máximo)
        ///
        /// // Calcula contraste customizado
        /// double ratio2 = ColorContrastValidator.CalculateContrastRatio(
        ///     Color.FromArgb(63, 81, 181),  // Primary500
        ///     Color.White
        /// );
        /// // ratio2 ≈ 8.6:1 (excelente contraste)
        /// </code>
        /// </example>
        public static double CalculateContrastRatio(Color c1, Color c2)
        {
            double lum1 = GetRelativeLuminance(c1);
            double lum2 = GetRelativeLuminance(c2);

            // A luminância mais clara sempre no numerador
            double lighter = Math.Max(lum1, lum2);
            double darker = Math.Min(lum1, lum2);

            // Fórmula WCAG: (L1 + 0.05) / (L2 + 0.05)
            return (lighter + 0.05) / (darker + 0.05);
        }

        /// <summary>
        /// Calcula a luminância relativa de uma cor conforme a especificação WCAG
        /// </summary>
        /// <param name="c">Cor para calcular luminância</param>
        /// <returns>Valor de luminância relativa (0.0 para preto puro, 1.0 para branco puro)</returns>
        /// <remarks>
        /// A luminância relativa é calculada usando a fórmula:
        /// L = 0.2126 * R + 0.7152 * G + 0.0722 * B
        ///
        /// Onde R, G, B são valores linearizados (convertidos de sRGB).
        /// Esta fórmula reflete a sensibilidade do olho humano a diferentes comprimentos de onda:
        /// - Verde (0.7152) tem maior peso pois o olho humano é mais sensível
        /// - Vermelho (0.2126) tem peso moderado
        /// - Azul (0.0722) tem menor peso
        ///
        /// Referência: https://www.w3.org/TR/WCAG21/#dfn-relative-luminance
        /// </remarks>
        /// <example>
        /// <code>
        /// double lumBlack = ColorContrastValidator.GetRelativeLuminance(Color.Black);
        /// // lumBlack = 0.0
        ///
        /// double lumWhite = ColorContrastValidator.GetRelativeLuminance(Color.White);
        /// // lumWhite = 1.0
        ///
        /// double lumGray = ColorContrastValidator.GetRelativeLuminance(Color.Gray);
        /// // lumGray ≈ 0.215 (cinza médio)
        /// </code>
        /// </example>
        public static double GetRelativeLuminance(Color c)
        {
            // Converter componentes sRGB (0-255) para valores decimais (0-1)
            double r = c.R / 255.0;
            double g = c.G / 255.0;
            double b = c.B / 255.0;

            // Converter de sRGB para linear RGB (gamma correction)
            r = GetSRGBValue(r);
            g = GetSRGBValue(g);
            b = GetSRGBValue(b);

            // Fórmula de luminância relativa (pesos baseados em percepção humana)
            return 0.2126 * r + 0.7152 * g + 0.0722 * b;
        }

        /// <summary>
        /// Sugere uma cor acessível similar à cor original que atenda ao padrão WCAG AA
        /// </summary>
        /// <param name="original">Cor original desejada</param>
        /// <param name="background">Cor de fundo contra a qual o contraste será validado</param>
        /// <param name="forLargeText">Se true, usa critério WCAG AA Large (3:1), caso contrário usa AA (4.5:1)</param>
        /// <returns>Cor ajustada que atende aos critérios de acessibilidade</returns>
        /// <remarks>
        /// Este método tenta preservar o matiz (hue) e saturação da cor original,
        /// ajustando apenas a luminosidade (lightness) até que o contraste adequado seja atingido.
        ///
        /// O algoritmo:
        /// 1. Converte a cor original para HSL
        /// 2. Testa aumentar e diminuir a luminosidade em incrementos
        /// 3. Retorna a primeira cor que atende ao critério de contraste
        /// 4. Se nenhuma cor intermediária funcionar, retorna preto ou branco
        /// </remarks>
        /// <example>
        /// <code>
        /// // Cor amarela em fundo branco (baixo contraste)
        /// Color yellow = Color.Yellow;
        /// Color accessible = ColorContrastValidator.SuggestAccessibleColor(yellow, Color.White);
        /// // Retorna um amarelo mais escuro que atende WCAG AA
        ///
        /// // Para texto grande, critério mais permissivo
        /// Color accessibleLarge = ColorContrastValidator.SuggestAccessibleColor(
        ///     yellow,
        ///     Color.White,
        ///     forLargeText: true
        /// );
        /// </code>
        /// </example>
        public static Color SuggestAccessibleColor(Color original, Color background, bool forLargeText = false)
        {
            double targetRatio = forLargeText ? MIN_CONTRAST_AA_LARGE : MIN_CONTRAST_AA;

            // Se já atende, retorna a original
            if (CalculateContrastRatio(original, background) >= targetRatio)
                return original;

            // Converte para HSL para ajustar apenas lightness
            GetHSL(original, out double h, out double s, out double l);

            // Tenta ajustar a luminosidade em passos de 5%
            // Primeiro tenta escurecer, depois clarear
            for (int step = 1; step <= 20; step++)
            {
                double adjustment = step * 0.05; // 5% por passo

                // Tenta escurecer
                double newL = Math.Max(0, l - adjustment);
                Color darker = FromHSL(h, s, newL);
                if (CalculateContrastRatio(darker, background) >= targetRatio)
                    return darker;

                // Tenta clarear
                newL = Math.Min(1, l + adjustment);
                Color lighter = FromHSL(h, s, newL);
                if (CalculateContrastRatio(lighter, background) >= targetRatio)
                    return lighter;
            }

            // Se nada funcionou, retorna preto ou branco baseado no fundo
            double bgLuminance = GetRelativeLuminance(background);
            return bgLuminance > 0.5 ? Color.Black : Color.White;
        }

        /// <summary>
        /// Valida todas as combinações de cores do ThemeManager
        /// </summary>
        /// <param name="verbose">Se true, gera saída de Debug com avisos para combinações que não atendem WCAG AA</param>
        /// <returns>Dicionário com nome da combinação e resultado da validação (true = acessível)</returns>
        /// <remarks>
        /// Valida as principais combinações:
        /// - Texto em backgrounds (Light/Dark)
        /// - Cores primárias e secundárias em fundos claros/escuros
        /// - Cores semânticas em seus fundos específicos
        /// - Botões e elementos interativos
        ///
        /// Este método é útil para auditorias de acessibilidade e testes automatizados.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Validar tema completo
        /// var results = ColorContrastValidator.ValidateThemeColors(verbose: true);
        ///
        /// // Verificar resultados
        /// foreach (var kvp in results)
        /// {
        ///     if (!kvp.Value)
        ///     {
        ///         Console.WriteLine($"FALHA: {kvp.Key}");
        ///     }
        /// }
        ///
        /// // Em testes unitários:
        /// Assert.IsTrue(results["Light: Text on Background"], "Texto deve ser legível em fundo claro");
        /// </code>
        /// </example>
        public static Dictionary<string, bool> ValidateThemeColors(bool verbose = false)
        {
            var results = new Dictionary<string, bool>();

            // === TEMA CLARO ===
            ValidateAndAdd(results, "Light: Text on Background",
                ThemeManager.Light.Text, ThemeManager.Light.Background, verbose);

            ValidateAndAdd(results, "Light: Text on Surface",
                ThemeManager.Light.Text, ThemeManager.Light.Surface, verbose);

            ValidateAndAdd(results, "Light: TextSecondary on Background",
                ThemeManager.Light.TextSecondary, ThemeManager.Light.Background, verbose);

            ValidateAndAdd(results, "Light: Primary on Background",
                ThemeManager.Light.Primary, ThemeManager.Light.Background, verbose);

            ValidateAndAdd(results, "Light: Primary on Surface",
                ThemeManager.Light.Primary, ThemeManager.Light.Surface, verbose);

            // === TEMA ESCURO ===
            ValidateAndAdd(results, "Dark: Text on Background",
                ThemeManager.Dark.Text, ThemeManager.Dark.Background, verbose);

            ValidateAndAdd(results, "Dark: Text on Surface",
                ThemeManager.Dark.Text, ThemeManager.Dark.Surface, verbose);

            ValidateAndAdd(results, "Dark: TextSecondary on Background",
                ThemeManager.Dark.TextSecondary, ThemeManager.Dark.Background, verbose);

            ValidateAndAdd(results, "Dark: Primary on Background",
                ThemeManager.Dark.Primary, ThemeManager.Dark.Background, verbose);

            ValidateAndAdd(results, "Dark: Primary on Surface",
                ThemeManager.Dark.Primary, ThemeManager.Dark.Surface, verbose);

            // === CORES SEMÂNTICAS ===
            ValidateAndAdd(results, "Success on SuccessBackground",
                ThemeManager.Semantic.Success, ThemeManager.Semantic.SuccessBackground, verbose);

            ValidateAndAdd(results, "Warning on WarningBackground",
                ThemeManager.Semantic.Warning, ThemeManager.Semantic.WarningBackground, verbose);

            ValidateAndAdd(results, "Error on ErrorBackground",
                ThemeManager.Semantic.Error, ThemeManager.Semantic.ErrorBackground, verbose);

            ValidateAndAdd(results, "Info on InfoBackground",
                ThemeManager.Semantic.Info, ThemeManager.Semantic.InfoBackground, verbose);

            // === CORES PRIMÁRIAS EM FUNDOS ===
            ValidateAndAdd(results, "Primary500 on Light Background",
                ThemeManager.Colors.Primary500, ThemeManager.Background.LightDefault, verbose);

            ValidateAndAdd(results, "Primary500 on Dark Background",
                ThemeManager.Colors.Primary500, ThemeManager.Background.DarkDefault, verbose);

            // === SIDEBAR ===
            ValidateAndAdd(results, "Light: Sidebar Text (assuming white)",
                Color.White, ThemeManager.Light.Sidebar, verbose);

            ValidateAndAdd(results, "Dark: Sidebar Text (assuming white)",
                Color.White, ThemeManager.Dark.Sidebar, verbose);

            return results;
        }

        /// <summary>
        /// Retorna uma classificação textual da razão de contraste
        /// </summary>
        /// <param name="ratio">Razão de contraste calculada</param>
        /// <returns>String: "AAA", "AA", "AA Large", ou "Fail"</returns>
        /// <example>
        /// <code>
        /// double ratio = ColorContrastValidator.CalculateContrastRatio(Color.Black, Color.White);
        /// string rating = ColorContrastValidator.GetContrastRating(ratio);
        /// // rating = "AAA" (21:1 atende todos os critérios)
        ///
        /// ratio = 5.0;
        /// rating = ColorContrastValidator.GetContrastRating(ratio);
        /// // rating = "AA" (atende AA mas não AAA)
        ///
        /// ratio = 3.5;
        /// rating = ColorContrastValidator.GetContrastRating(ratio);
        /// // rating = "AA Large" (apenas para texto grande)
        ///
        /// ratio = 2.0;
        /// rating = ColorContrastValidator.GetContrastRating(ratio);
        /// // rating = "Fail" (não atende nenhum critério)
        /// </code>
        /// </example>
        public static string GetContrastRating(double ratio)
        {
            if (ratio >= MIN_CONTRAST_AAA)
                return "AAA";
            if (ratio >= MIN_CONTRAST_AA)
                return "AA";
            if (ratio >= MIN_CONTRAST_AA_LARGE)
                return "AA Large";
            return "Fail";
        }

        /// <summary>
        /// Verificação rápida de legibilidade (alias para MeetsWCAG_AA)
        /// </summary>
        /// <param name="foreground">Cor do texto</param>
        /// <param name="background">Cor de fundo</param>
        /// <returns>True se a combinação for legível (WCAG AA)</returns>
        /// <example>
        /// <code>
        /// if (ColorContrastValidator.IsReadable(textColor, backgroundColor))
        /// {
        ///     // Usar esta combinação de cores
        /// }
        /// else
        /// {
        ///     // Sugerir alternativa
        ///     textColor = ColorContrastValidator.SuggestAccessibleColor(textColor, backgroundColor);
        /// }
        /// </code>
        /// </example>
        public static bool IsReadable(Color foreground, Color background)
        {
            return MeetsWCAG_AA(foreground, background);
        }

        // === MÉTODOS AUXILIARES PRIVADOS ===

        /// <summary>
        /// Converte um valor de canal sRGB (0-1) para valor linear RGB
        /// Aplica gamma correction conforme especificação sRGB
        /// </summary>
        /// <param name="channel">Valor do canal em sRGB (0.0-1.0)</param>
        /// <returns>Valor linearizado para cálculo de luminância</returns>
        /// <remarks>
        /// A conversão sRGB para linear é necessária porque a luminância percebida
        /// não é linear em relação aos valores sRGB.
        ///
        /// Para valores <= 0.03928, usa divisão simples por 12.92
        /// Para valores > 0.03928, aplica a fórmula: ((V + 0.055) / 1.055) ^ 2.4
        ///
        /// Referência: https://www.w3.org/TR/WCAG21/#dfn-relative-luminance
        /// </remarks>
        private static double GetSRGBValue(double channel)
        {
            // Valores pequenos usam divisão linear
            if (channel <= 0.03928)
                return channel / 12.92;

            // Valores maiores usam função exponencial (gamma correction)
            return Math.Pow((channel + 0.055) / 1.055, 2.4);
        }

        /// <summary>
        /// Ajusta a luminosidade (lightness) de uma cor em HSL
        /// </summary>
        /// <param name="color">Cor original</param>
        /// <param name="amount">Quantidade a ajustar (-1.0 a +1.0, onde negativo escurece e positivo clareia)</param>
        /// <returns>Nova cor com luminosidade ajustada</returns>
        /// <remarks>
        /// Mantém matiz (hue) e saturação, alterando apenas lightness.
        /// Útil para criar variações de uma cor mantendo sua identidade visual.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Criar versão mais escura
        /// Color darker = ColorContrastValidator.AdjustLightness(Color.Blue, -0.2);
        ///
        /// // Criar versão mais clara
        /// Color lighter = ColorContrastValidator.AdjustLightness(Color.Blue, 0.2);
        /// </code>
        /// </example>
        private static Color AdjustLightness(Color color, double amount)
        {
            GetHSL(color, out double h, out double s, out double l);
            l = Math.Max(0, Math.Min(1, l + amount));
            return FromHSL(h, s, l);
        }

        /// <summary>
        /// Converte uma cor RGB para o espaço de cores HSL (Hue, Saturation, Lightness)
        /// </summary>
        /// <param name="color">Cor em RGB</param>
        /// <param name="h">Matiz (hue) em graus (0-360)</param>
        /// <param name="s">Saturação (saturation) (0.0-1.0)</param>
        /// <param name="l">Luminosidade (lightness) (0.0-1.0)</param>
        /// <remarks>
        /// HSL é um modelo cilíndrico de representação de cores que separa:
        /// - H (Hue/Matiz): A cor propriamente dita (0°=vermelho, 120°=verde, 240°=azul)
        /// - S (Saturation/Saturação): Intensidade da cor (0=cinza, 1=cor pura)
        /// - L (Lightness/Luminosidade): Claridade (0=preto, 0.5=cor pura, 1=branco)
        ///
        /// Este modelo é útil para ajustes de cor preservando a identidade cromática.
        /// </remarks>
        /// <example>
        /// <code>
        /// ColorContrastValidator.GetHSL(Color.Red, out double h, out double s, out double l);
        /// // h ≈ 0°, s = 1.0, l = 0.5
        ///
        /// ColorContrastValidator.GetHSL(Color.Gray, out double h2, out double s2, out double l2);
        /// // h2 = indefinido, s2 = 0, l2 ≈ 0.5
        /// </code>
        /// </example>
        private static void GetHSL(Color color, out double h, out double s, out double l)
        {
            double r = color.R / 255.0;
            double g = color.G / 255.0;
            double b = color.B / 255.0;

            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));
            double delta = max - min;

            // Lightness
            l = (max + min) / 2.0;

            if (delta == 0)
            {
                // Cor acromática (cinza)
                h = 0;
                s = 0;
            }
            else
            {
                // Saturation
                s = l > 0.5
                    ? delta / (2.0 - max - min)
                    : delta / (max + min);

                // Hue
                if (max == r)
                    h = ((g - b) / delta + (g < b ? 6 : 0)) / 6.0;
                else if (max == g)
                    h = ((b - r) / delta + 2) / 6.0;
                else
                    h = ((r - g) / delta + 4) / 6.0;

                h *= 360; // Converter para graus
            }
        }

        /// <summary>
        /// Converte valores HSL para uma cor RGB
        /// </summary>
        /// <param name="h">Matiz (hue) em graus (0-360)</param>
        /// <param name="s">Saturação (saturation) (0.0-1.0)</param>
        /// <param name="l">Luminosidade (lightness) (0.0-1.0)</param>
        /// <returns>Cor no espaço RGB</returns>
        /// <remarks>
        /// Conversão inversa de HSL para RGB usando o algoritmo padrão.
        /// Útil para criar cores programaticamente mantendo controle sobre
        /// matiz, saturação e luminosidade separadamente.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Criar vermelho puro
        /// Color red = ColorContrastValidator.FromHSL(0, 1.0, 0.5);
        ///
        /// // Criar azul claro (cyan)
        /// Color lightBlue = ColorContrastValidator.FromHSL(180, 1.0, 0.75);
        ///
        /// // Criar cinza médio
        /// Color gray = ColorContrastValidator.FromHSL(0, 0, 0.5);
        /// </code>
        /// </example>
        private static Color FromHSL(double h, double s, double l)
        {
            h = h / 360.0; // Normalizar para 0-1

            double r, g, b;

            if (s == 0)
            {
                // Acromático (cinza)
                r = g = b = l;
            }
            else
            {
                double q = l < 0.5
                    ? l * (1 + s)
                    : l + s - l * s;
                double p = 2 * l - q;

                r = HueToRGB(p, q, h + 1.0 / 3.0);
                g = HueToRGB(p, q, h);
                b = HueToRGB(p, q, h - 1.0 / 3.0);
            }

            return Color.FromArgb(
                (int)Math.Round(r * 255),
                (int)Math.Round(g * 255),
                (int)Math.Round(b * 255)
            );
        }

        /// <summary>
        /// Função auxiliar para conversão HSL -> RGB
        /// Calcula o valor de um canal RGB baseado nos parâmetros HSL
        /// </summary>
        private static double HueToRGB(double p, double q, double t)
        {
            if (t < 0) t += 1;
            if (t > 1) t -= 1;
            if (t < 1.0 / 6.0) return p + (q - p) * 6 * t;
            if (t < 1.0 / 2.0) return q;
            if (t < 2.0 / 3.0) return p + (q - p) * (2.0 / 3.0 - t) * 6;
            return p;
        }

        /// <summary>
        /// Método auxiliar para validar e adicionar resultado ao dicionário
        /// </summary>
        private static void ValidateAndAdd(Dictionary<string, bool> results, string name,
            Color foreground, Color background, bool verbose)
        {
            double ratio = CalculateContrastRatio(foreground, background);
            bool passes = ratio >= MIN_CONTRAST_AA;
            results[name] = passes;

            if (verbose && !passes)
            {
                Debug.WriteLine($"⚠️ AVISO WCAG: {name}");
                Debug.WriteLine($"   Razão: {ratio:F2}:1 (mínimo: {MIN_CONTRAST_AA}:1)");
                Debug.WriteLine($"   Foreground: RGB({foreground.R},{foreground.G},{foreground.B})");
                Debug.WriteLine($"   Background: RGB({background.R},{background.G},{background.B})");
                Debug.WriteLine($"   Classificação: {GetContrastRating(ratio)}");

                Color suggested = SuggestAccessibleColor(foreground, background);
                Debug.WriteLine($"   Sugestão: RGB({suggested.R},{suggested.G},{suggested.B})");
                Debug.WriteLine();
            }
        }
    }
}
