using Xunit;
using FluentAssertions;
using BibliotecaJK.BLL;

namespace BibliotecaJK.Tests.Unit.BLL
{
    /// <summary>
    /// Testes unitários para a classe Validadores
    /// Objetivo: 100% de cobertura em validações críticas
    /// </summary>
    public class ValidadoresTests
    {
        #region ValidarCPF Tests

        [Theory]
        [InlineData("123.456.789-09", true)]
        [InlineData("12345678909", true)]
        [InlineData("000.000.000-00", false)]
        [InlineData("111.111.111-11", false)]
        [InlineData("222.222.222-22", false)]
        [InlineData("12345678901", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        [InlineData("abc.def.ghi-jk", false)]
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void ValidarCPF_DeveValidarCorretamente(string cpf, bool esperado)
        {
            // Act
            var resultado = Validadores.ValidarCPF(cpf);

            // Assert
            resultado.Should().Be(esperado,
                $"CPF '{cpf}' deveria ser {(esperado ? "válido" : "inválido")}");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ValidarCPF_ComCPFValido_DeveRetornarTrue()
        {
            // Arrange - CPF válido gerado com dígitos verificadores corretos
            var cpfValido = "52998224725";

            // Act
            var resultado = Validadores.ValidarCPF(cpfValido);

            // Assert
            resultado.Should().BeTrue("CPF tem dígitos verificadores corretos");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ValidarCPF_ComCPFInvalidoDigitoVerificador_DeveRetornarFalse()
        {
            // Arrange - CPF com dígito verificador incorreto
            var cpfInvalido = "52998224726"; // último dígito errado

            // Act
            var resultado = Validadores.ValidarCPF(cpfInvalido);

            // Assert
            resultado.Should().BeFalse("último dígito verificador está incorreto");
        }

        #endregion

        #region ValidarISBN Tests

        [Theory]
        [InlineData("978-0-13-110362-7", true)]
        [InlineData("978-0-596-52068-7", true)]
        [InlineData("9780136083238", true)]
        [InlineData("978-0-00-000000-0", false)]
        [InlineData("978123456789X", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        [InlineData("12345", false)]
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void ValidarISBN13_DeveValidarCorretamente(string isbn, bool esperado)
        {
            // Act
            var resultado = Validadores.ValidarISBN(isbn);

            // Assert
            resultado.Should().Be(esperado,
                $"ISBN '{isbn}' deveria ser {(esperado ? "válido" : "inválido")}");
        }

        #endregion

        #region ValidarEmail Tests

        [Theory]
        [InlineData("usuario@exemplo.com", true)]
        [InlineData("nome.sobrenome@empresa.com.br", true)]
        [InlineData("teste123@dominio.co", true)]
        [InlineData("invalido@", false)]
        [InlineData("@invalido.com", false)]
        [InlineData("sem-arroba.com", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        [InlineData("espaço em@branco.com", false)]
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void ValidarEmail_DeveValidarCorretamente(string email, bool esperado)
        {
            // Act
            var resultado = Validadores.ValidarEmail(email);

            // Assert
            resultado.Should().Be(esperado,
                $"Email '{email}' deveria ser {(esperado ? "válido" : "inválido")}");
        }

        #endregion

        #region Performance Tests

        [Fact]
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void ValidarCPF_Performance_DeveDemorarMenosDe1Milissegundo()
        {
            // Arrange
            var cpf = "52998224725";
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Act
            for (int i = 0; i < 1000; i++)
            {
                Validadores.ValidarCPF(cpf);
            }
            stopwatch.Stop();

            // Assert
            var tempoMedioPorChamada = stopwatch.ElapsedMilliseconds / 1000.0;
            tempoMedioPorChamada.Should().BeLessThan(1,
                "validação de CPF deve ser rápida");
        }

        #endregion
    }
}
