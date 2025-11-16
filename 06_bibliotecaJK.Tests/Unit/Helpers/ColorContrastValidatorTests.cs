using System.Drawing;
using Xunit;
using FluentAssertions;
using BibliotecaJK.Helpers;

namespace BibliotecaJK.Tests.Unit.Helpers
{
    /// <summary>
    /// Testes unitários para ColorContrastValidator
    /// Valida conformidade WCAG 2.1 para acessibilidade de cores
    /// </summary>
    public class ColorContrastValidatorTests
    {
        #region MeetsWCAG_AA Tests

        [Theory]
        [InlineData(0, 0, 0, 255, 255, 255, true)]  // Black on White: 21:1
        [InlineData(255, 255, 255, 0, 0, 0, true)]  // White on Black: 21:1
        [InlineData(63, 81, 181, 255, 255, 255, true)]  // Primary500 on White: ~8.6:1
        [InlineData(255, 255, 0, 255, 255, 255, false)]  // Yellow on White: ~1.07:1 (fails)
        [InlineData(211, 211, 211, 255, 255, 255, false)]  // LightGray on White: fails
        [InlineData(0, 0, 0, 33, 33, 33, false)]  // Dark gray on darker gray: fails
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void MeetsWCAG_AA_DeveValidarContrastesCorretamente(
            int fgR, int fgG, int fgB,
            int bgR, int bgG, int bgB,
            bool esperado)
        {
            // Arrange
            var foreground = Color.FromArgb(fgR, fgG, fgB);
            var background = Color.FromArgb(bgR, bgG, bgB);

            // Act
            var resultado = ColorContrastValidator.MeetsWCAG_AA(foreground, background);

            // Assert
            resultado.Should().Be(esperado,
                $"Contraste entre RGB({fgR},{fgG},{fgB}) e RGB({bgR},{bgG},{bgB}) deveria {(esperado ? "atender" : "não atender")} WCAG AA");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void MeetsWCAG_AA_ComPretoEmBranco_DeveRetornarTrue()
        {
            // Arrange
            var foreground = Color.Black;
            var background = Color.White;

            // Act
            var resultado = ColorContrastValidator.MeetsWCAG_AA(foreground, background);

            // Assert
            resultado.Should().BeTrue("preto em branco tem contraste máximo (21:1)");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void MeetsWCAG_AA_ComAmareloEmBranco_DeveRetornarFalse()
        {
            // Arrange
            var foreground = Color.Yellow;
            var background = Color.White;

            // Act
            var resultado = ColorContrastValidator.MeetsWCAG_AA(foreground, background);

            // Assert
            resultado.Should().BeFalse("amarelo em branco tem contraste muito baixo");
        }

        #endregion

        #region MeetsWCAG_AAA Tests

        [Theory]
        [InlineData(0, 0, 0, 255, 255, 255, true)]  // Black on White: 21:1 (exceeds 7:1)
        [InlineData(255, 255, 255, 0, 0, 0, true)]  // White on Black: 21:1
        [InlineData(63, 81, 181, 255, 255, 255, true)]  // Primary500 on White: ~8.6:1 (exceeds 7:1)
        [InlineData(128, 128, 128, 255, 255, 255, false)]  // Gray on White: ~3.95:1 (fails AAA)
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void MeetsWCAG_AAA_DeveValidarContrastesAprimorados(
            int fgR, int fgG, int fgB,
            int bgR, int bgG, int bgB,
            bool esperado)
        {
            // Arrange
            var foreground = Color.FromArgb(fgR, fgG, fgB);
            var background = Color.FromArgb(bgR, bgG, bgB);

            // Act
            var resultado = ColorContrastValidator.MeetsWCAG_AAA(foreground, background);

            // Assert
            resultado.Should().Be(esperado);
        }

        #endregion

        #region MeetsWCAG_AA_Large Tests

        [Theory]
        [InlineData(0, 0, 0, 255, 255, 255, true)]  // Black on White
        [InlineData(128, 128, 128, 255, 255, 255, true)]  // Gray on White: ~3.95:1 (passes AA Large)
        [InlineData(200, 200, 200, 255, 255, 255, false)]  // Light gray on white: ~1.5:1 (fails)
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void MeetsWCAG_AA_Large_DeveValidarParaTextoGrande(
            int fgR, int fgG, int fgB,
            int bgR, int bgG, int bgB,
            bool esperado)
        {
            // Arrange
            var foreground = Color.FromArgb(fgR, fgG, fgB);
            var background = Color.FromArgb(bgR, bgG, bgB);

            // Act
            var resultado = ColorContrastValidator.MeetsWCAG_AA_Large(foreground, background);

            // Assert
            resultado.Should().Be(esperado);
        }

        #endregion

        #region CalculateContrastRatio Tests

        [Fact]
        [Trait("Category", "Unit")]
        public void CalculateContrastRatio_ComPretoEBranco_DeveRetornar21()
        {
            // Arrange
            var black = Color.Black;
            var white = Color.White;

            // Act
            var ratio = ColorContrastValidator.CalculateContrastRatio(black, white);

            // Assert
            ratio.Should().BeApproximately(21.0, 0.01, "preto e branco têm contraste máximo de 21:1");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void CalculateContrastRatio_ComBrancoEPreto_DeveRetornar21()
        {
            // Arrange
            var white = Color.White;
            var black = Color.Black;

            // Act
            var ratio = ColorContrastValidator.CalculateContrastRatio(white, black);

            // Assert
            ratio.Should().BeApproximately(21.0, 0.01, "ordem não importa - resultado deve ser o mesmo");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void CalculateContrastRatio_ComMesmaCor_DeveRetornar1()
        {
            // Arrange
            var blue = Color.Blue;

            // Act
            var ratio = ColorContrastValidator.CalculateContrastRatio(blue, blue);

            // Assert
            ratio.Should().BeApproximately(1.0, 0.01, "mesma cor não tem contraste");
        }

        [Theory]
        [InlineData(63, 81, 181, 255, 255, 255, 8.59)]  // Primary500 on White
        [InlineData(255, 0, 0, 255, 255, 255, 3.99)]  // Red on White
        [InlineData(0, 128, 0, 255, 255, 255, 3.70)]  // Green on White
        [InlineData(0, 0, 255, 255, 255, 255, 8.59)]  // Blue on White
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void CalculateContrastRatio_ComCoresComunsEmBranco_DeveCalcularCorretamente(
            int fgR, int fgG, int fgB,
            int bgR, int bgG, int bgB,
            double razaoEsperada)
        {
            // Arrange
            var foreground = Color.FromArgb(fgR, fgG, fgB);
            var background = Color.FromArgb(bgR, bgG, bgB);

            // Act
            var ratio = ColorContrastValidator.CalculateContrastRatio(foreground, background);

            // Assert
            ratio.Should().BeApproximately(razaoEsperada, 0.1,
                $"contraste entre RGB({fgR},{fgG},{fgB}) e RGB({bgR},{bgG},{bgB})");
        }

        #endregion

        #region GetRelativeLuminance Tests

        [Fact]
        [Trait("Category", "Unit")]
        public void GetRelativeLuminance_ComPreto_DeveRetornar0()
        {
            // Arrange
            var black = Color.Black;

            // Act
            var luminance = ColorContrastValidator.GetRelativeLuminance(black);

            // Assert
            luminance.Should().BeApproximately(0.0, 0.001, "preto tem luminância 0");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void GetRelativeLuminance_ComBranco_DeveRetornar1()
        {
            // Arrange
            var white = Color.White;

            // Act
            var luminance = ColorContrastValidator.GetRelativeLuminance(white);

            // Assert
            luminance.Should().BeApproximately(1.0, 0.001, "branco tem luminância 1");
        }

        [Theory]
        [InlineData(128, 128, 128, 0.215)]  // Gray (50% brightness)
        [InlineData(255, 0, 0, 0.2126)]  // Pure Red
        [InlineData(0, 255, 0, 0.7152)]  // Pure Green (highest luminance)
        [InlineData(0, 0, 255, 0.0722)]  // Pure Blue (lowest luminance)
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void GetRelativeLuminance_ComCoresPuras_DeveCalcularCorretamente(
            int r, int g, int b,
            double luminanciaEsperada)
        {
            // Arrange
            var color = Color.FromArgb(r, g, b);

            // Act
            var luminance = ColorContrastValidator.GetRelativeLuminance(color);

            // Assert
            luminance.Should().BeApproximately(luminanciaEsperada, 0.01,
                $"luminância de RGB({r},{g},{b})");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void GetRelativeLuminance_VerdeTemMaiorPesoQueVermelhoOuAzul()
        {
            // Arrange
            var red = Color.FromArgb(255, 0, 0);
            var green = Color.FromArgb(0, 255, 0);
            var blue = Color.FromArgb(0, 0, 255);

            // Act
            var lumRed = ColorContrastValidator.GetRelativeLuminance(red);
            var lumGreen = ColorContrastValidator.GetRelativeLuminance(green);
            var lumBlue = ColorContrastValidator.GetRelativeLuminance(blue);

            // Assert
            lumGreen.Should().BeGreaterThan(lumRed, "verde tem maior peso na fórmula de luminância");
            lumGreen.Should().BeGreaterThan(lumBlue, "verde tem maior peso na fórmula de luminância");
            lumRed.Should().BeGreaterThan(lumBlue, "vermelho tem peso intermediário");
        }

        #endregion

        #region SuggestAccessibleColor Tests

        [Fact]
        [Trait("Category", "Unit")]
        public void SuggestAccessibleColor_ComCorJaAcessivel_DeveRetornarMesmaCor()
        {
            // Arrange
            var black = Color.Black;
            var white = Color.White;

            // Act
            var suggested = ColorContrastValidator.SuggestAccessibleColor(black, white);

            // Assert
            suggested.Should().Be(black, "preto em branco já é acessível");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void SuggestAccessibleColor_ComAmareloEmBranco_DeveMelhorarContraste()
        {
            // Arrange
            var yellow = Color.Yellow;
            var white = Color.White;

            // Act
            var suggested = ColorContrastValidator.SuggestAccessibleColor(yellow, white);
            var originalRatio = ColorContrastValidator.CalculateContrastRatio(yellow, white);
            var suggestedRatio = ColorContrastValidator.CalculateContrastRatio(suggested, white);

            // Assert
            suggestedRatio.Should().BeGreaterThan(originalRatio, "cor sugerida deve ter melhor contraste");
            suggestedRatio.Should().BeGreaterThanOrEqualTo(ColorContrastValidator.MIN_CONTRAST_AA,
                "cor sugerida deve atender WCAG AA");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void SuggestAccessibleColor_ParaTextoGrande_DeveUsarCriterioMenosRestritivo()
        {
            // Arrange
            var lightGray = Color.FromArgb(170, 170, 170);
            var white = Color.White;

            // Act
            var suggestedNormal = ColorContrastValidator.SuggestAccessibleColor(lightGray, white, forLargeText: false);
            var suggestedLarge = ColorContrastValidator.SuggestAccessibleColor(lightGray, white, forLargeText: true);

            var ratioNormal = ColorContrastValidator.CalculateContrastRatio(suggestedNormal, white);
            var ratioLarge = ColorContrastValidator.CalculateContrastRatio(suggestedLarge, white);

            // Assert
            ratioNormal.Should().BeGreaterThanOrEqualTo(ColorContrastValidator.MIN_CONTRAST_AA);
            ratioLarge.Should().BeGreaterThanOrEqualTo(ColorContrastValidator.MIN_CONTRAST_AA_LARGE);
        }

        [Theory]
        [InlineData(255, 0, 0)]  // Red
        [InlineData(0, 255, 0)]  // Green
        [InlineData(0, 0, 255)]  // Blue
        [InlineData(255, 255, 0)]  // Yellow
        [InlineData(255, 0, 255)]  // Magenta
        [InlineData(0, 255, 255)]  // Cyan
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void SuggestAccessibleColor_ComQualquerCorEmBranco_DeveSempreAtenderWCAG(
            int r, int g, int b)
        {
            // Arrange
            var color = Color.FromArgb(r, g, b);
            var white = Color.White;

            // Act
            var suggested = ColorContrastValidator.SuggestAccessibleColor(color, white);
            var ratio = ColorContrastValidator.CalculateContrastRatio(suggested, white);

            // Assert
            ratio.Should().BeGreaterThanOrEqualTo(ColorContrastValidator.MIN_CONTRAST_AA,
                $"cor sugerida para RGB({r},{g},{b}) deve atender WCAG AA");
        }

        #endregion

        #region GetContrastRating Tests

        [Theory]
        [InlineData(21.0, "AAA")]
        [InlineData(8.0, "AAA")]
        [InlineData(7.0, "AAA")]
        [InlineData(6.9, "AA")]
        [InlineData(5.0, "AA")]
        [InlineData(4.5, "AA")]
        [InlineData(4.4, "AA Large")]
        [InlineData(3.5, "AA Large")]
        [InlineData(3.0, "AA Large")]
        [InlineData(2.9, "Fail")]
        [InlineData(1.5, "Fail")]
        [InlineData(1.0, "Fail")]
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void GetContrastRating_DeveRetornarClassificacaoCorreta(
            double ratio,
            string classificacaoEsperada)
        {
            // Act
            var rating = ColorContrastValidator.GetContrastRating(ratio);

            // Assert
            rating.Should().Be(classificacaoEsperada,
                $"razão {ratio:F1}:1 deveria ser classificada como '{classificacaoEsperada}'");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void GetContrastRating_ComPretoEmBranco_DeveRetornarAAA()
        {
            // Arrange
            var ratio = ColorContrastValidator.CalculateContrastRatio(Color.Black, Color.White);

            // Act
            var rating = ColorContrastValidator.GetContrastRating(ratio);

            // Assert
            rating.Should().Be("AAA");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void GetContrastRating_ComAmareloEmBranco_DeveRetornarFail()
        {
            // Arrange
            var ratio = ColorContrastValidator.CalculateContrastRatio(Color.Yellow, Color.White);

            // Act
            var rating = ColorContrastValidator.GetContrastRating(ratio);

            // Assert
            rating.Should().Be("Fail");
        }

        #endregion

        #region IsReadable Tests

        [Fact]
        [Trait("Category", "Unit")]
        public void IsReadable_DeveSerAliasParaMeetsWCAG_AA()
        {
            // Arrange
            var black = Color.Black;
            var white = Color.White;
            var yellow = Color.Yellow;

            // Act & Assert
            ColorContrastValidator.IsReadable(black, white)
                .Should().Be(ColorContrastValidator.MeetsWCAG_AA(black, white));

            ColorContrastValidator.IsReadable(yellow, white)
                .Should().Be(ColorContrastValidator.MeetsWCAG_AA(yellow, white));
        }

        #endregion

        #region ValidateThemeColors Tests

        [Fact]
        [Trait("Category", "Unit")]
        public void ValidateThemeColors_DeveRetornarDicionarioDeResultados()
        {
            // Act
            var results = ColorContrastValidator.ValidateThemeColors(verbose: false);

            // Assert
            results.Should().NotBeNull();
            results.Should().NotBeEmpty();
            results.Should().ContainKey("Light: Text on Background");
            results.Should().ContainKey("Dark: Text on Background");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ValidateThemeColors_ComVerbose_NaoDeveLancarExcecao()
        {
            // Act
            Action act = () => ColorContrastValidator.ValidateThemeColors(verbose: true);

            // Assert
            act.Should().NotThrow("validação com verbose não deve lançar exceção");
        }

        #endregion

        #region Edge Cases & Validation Tests

        [Fact]
        [Trait("Category", "Unit")]
        public void CalculateContrastRatio_ComCoresAlpha_DeveIgnorarCanal()
        {
            // Arrange
            var blackOpaque = Color.FromArgb(255, 0, 0, 0);
            var blackTransparent = Color.FromArgb(128, 0, 0, 0);
            var white = Color.White;

            // Act
            var ratioOpaque = ColorContrastValidator.CalculateContrastRatio(blackOpaque, white);
            var ratioTransparent = ColorContrastValidator.CalculateContrastRatio(blackTransparent, white);

            // Assert
            ratioOpaque.Should().BeApproximately(ratioTransparent, 0.01,
                "canal alpha não deve afetar cálculo de contraste");
        }

        [Theory]
        [InlineData(0, 0, 0)]  // Black
        [InlineData(255, 255, 255)]  // White
        [InlineData(128, 128, 128)]  // Gray
        [InlineData(63, 81, 181)]  // Primary500
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void GetRelativeLuminance_DeveRetornarValorEntre0E1(int r, int g, int b)
        {
            // Arrange
            var color = Color.FromArgb(r, g, b);

            // Act
            var luminance = ColorContrastValidator.GetRelativeLuminance(color);

            // Assert
            luminance.Should().BeGreaterThanOrEqualTo(0.0);
            luminance.Should().BeLessThanOrEqualTo(1.0);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void CalculateContrastRatio_DeveRetornarValorEntre1E21()
        {
            // Arrange - Various color combinations
            var testCases = new[]
            {
                (Color.Black, Color.White),
                (Color.White, Color.Black),
                (Color.Gray, Color.White),
                (Color.Red, Color.Blue),
                (Color.Yellow, Color.Yellow)
            };

            // Act & Assert
            foreach (var (c1, c2) in testCases)
            {
                var ratio = ColorContrastValidator.CalculateContrastRatio(c1, c2);
                ratio.Should().BeGreaterThanOrEqualTo(1.0, "razão mínima é 1:1");
                ratio.Should().BeLessThanOrEqualTo(21.0, "razão máxima é 21:1");
            }
        }

        #endregion

        #region Performance Tests

        [Fact]
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void CalculateContrastRatio_Performance_DeveDemorarMenosDe1Milissegundo()
        {
            // Arrange
            var black = Color.Black;
            var white = Color.White;
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Act
            for (int i = 0; i < 1000; i++)
            {
                ColorContrastValidator.CalculateContrastRatio(black, white);
            }
            stopwatch.Stop();

            // Assert
            var tempoMedioPorChamada = stopwatch.ElapsedMilliseconds / 1000.0;
            tempoMedioPorChamada.Should().BeLessThan(1,
                "cálculo de contraste deve ser rápido");
        }

        [Fact]
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void SuggestAccessibleColor_Performance_DeveCompletarEm100ms()
        {
            // Arrange
            var yellow = Color.Yellow;
            var white = Color.White;
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Act
            for (int i = 0; i < 100; i++)
            {
                ColorContrastValidator.SuggestAccessibleColor(yellow, white);
            }
            stopwatch.Stop();

            // Assert
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(100,
                "sugestão de cor deve ser razoavelmente rápida");
        }

        #endregion
    }
}
