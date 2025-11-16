using Xunit;
using FluentAssertions;
using BibliotecaJK.BLL;

namespace BibliotecaJK.Tests.Unit.BLL
{
    /// <summary>
    /// Testes unitários para a classe Validadores
    /// Objetivo: 100% de cobertura em validações críticas
    /// Implementa testes abrangentes para CPF, ISBN, Email e Matrícula
    /// </summary>
    public class ValidadoresTests
    {
        #region ValidarCPF Tests

        [Theory]
        [InlineData("123.456.789-09", true)]   // CPF válido com formatação
        [InlineData("12345678909", true)]      // CPF válido sem formatação
        [InlineData("111.444.777-35", true)]   // CPF válido específico
        [InlineData("52998224725", true)]      // CPF válido sem formatação
        [InlineData("000.000.000-00", false)]  // Sequência inválida
        [InlineData("111.111.111-11", false)]  // Sequência inválida
        [InlineData("222.222.222-22", false)]  // Sequência inválida
        [InlineData("333.333.333-33", false)]  // Sequência inválida
        [InlineData("444.444.444-44", false)]  // Sequência inválida
        [InlineData("555.555.555-55", false)]  // Sequência inválida
        [InlineData("666.666.666-66", false)]  // Sequência inválida
        [InlineData("777.777.777-77", false)]  // Sequência inválida
        [InlineData("888.888.888-88", false)]  // Sequência inválida
        [InlineData("999.999.999-99", false)]  // Sequência inválida
        [InlineData("12345678901", false)]     // Dígito verificador incorreto
        [InlineData("52998224726", false)]     // Último dígito incorreto
        [InlineData("", false)]                // String vazia
        [InlineData(null, false)]              // Null
        [InlineData("   ", false)]             // Apenas espaços
        [InlineData("abc.def.ghi-jk", false)]  // Letras
        [InlineData("123.456.789", false)]     // Incompleto
        [InlineData("123.456.789-0", false)]   // Incompleto
        [InlineData("1234567890123", false)]   // Muito longo
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
        public void ValidarCPF_ComCPFValidoFormatado_DeveRetornarTrue()
        {
            // Arrange - CPF válido com formatação completa
            var cpfValido = "123.456.789-09";

            // Act
            var resultado = Validadores.ValidarCPF(cpfValido);

            // Assert
            resultado.Should().BeTrue("CPF tem dígitos verificadores corretos e formatação válida");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ValidarCPF_ComCPFValidoSemFormatacao_DeveRetornarTrue()
        {
            // Arrange - CPF válido sem formatação
            var cpfValido = "52998224725";

            // Act
            var resultado = Validadores.ValidarCPF(cpfValido);

            // Assert
            resultado.Should().BeTrue("CPF tem dígitos verificadores corretos");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ValidarCPF_ComPrimeiroDigitoVerificadorIncorreto_DeveRetornarFalse()
        {
            // Arrange - CPF com primeiro dígito verificador incorreto
            var cpfInvalido = "52998224735"; // penúltimo dígito errado

            // Act
            var resultado = Validadores.ValidarCPF(cpfInvalido);

            // Assert
            resultado.Should().BeFalse("primeiro dígito verificador está incorreto");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ValidarCPF_ComSegundoDigitoVerificadorIncorreto_DeveRetornarFalse()
        {
            // Arrange - CPF com segundo dígito verificador incorreto
            var cpfInvalido = "52998224726"; // último dígito errado

            // Act
            var resultado = Validadores.ValidarCPF(cpfInvalido);

            // Assert
            resultado.Should().BeFalse("segundo dígito verificador está incorreto");
        }

        #endregion

        #region ValidarISBN Tests

        [Theory]
        [InlineData("978-0-13-110362-7", true)]   // ISBN-13 válido com hífens
        [InlineData("978-0-596-52068-7", true)]   // ISBN-13 válido com hífens
        [InlineData("9780136083238", true)]       // ISBN-13 válido sem hífens
        [InlineData("9780201633610", true)]       // ISBN-13 válido
        [InlineData("0-13-110362-8", true)]       // ISBN-10 válido com hífens
        [InlineData("0136083234", true)]          // ISBN-10 válido sem hífens
        [InlineData("043942089X", true)]          // ISBN-10 válido com X
        [InlineData("043942089x", true)]          // ISBN-10 válido com x minúsculo
        [InlineData("978-0-00-000000-0", false)]  // ISBN-13 inválido (dígito verificador)
        [InlineData("978123456789X", false)]      // ISBN-13 não pode ter X
        [InlineData("9780136083239", false)]      // ISBN-13 dígito verificador incorreto
        [InlineData("0136083235", false)]         // ISBN-10 dígito verificador incorreto
        [InlineData("", false)]                   // String vazia
        [InlineData(null, false)]                 // Null
        [InlineData("   ", false)]                // Apenas espaços
        [InlineData("12345", false)]              // Muito curto
        [InlineData("12345678901234", false)]     // Muito longo
        [InlineData("ABC0136083234", false)]      // Contém letras (exceto X no final)
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void ValidarISBN_DeveValidarCorretamente(string isbn, bool esperado)
        {
            // Act
            var resultado = Validadores.ValidarISBN(isbn);

            // Assert
            resultado.Should().Be(esperado,
                $"ISBN '{isbn}' deveria ser {(esperado ? "válido" : "inválido")}");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ValidarISBN_ComISBN13Valido_DeveRetornarTrue()
        {
            // Arrange - ISBN-13 válido conhecido (Clean Code)
            var isbn13 = "978-0-13-235088-4";

            // Act
            var resultado = Validadores.ValidarISBN(isbn13);

            // Assert
            resultado.Should().BeTrue("ISBN-13 tem dígito verificador correto");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ValidarISBN_ComISBN10Valido_DeveRetornarTrue()
        {
            // Arrange - ISBN-10 válido
            var isbn10 = "0-13-110362-8";

            // Act
            var resultado = Validadores.ValidarISBN(isbn10);

            // Assert
            resultado.Should().BeTrue("ISBN-10 tem dígito verificador correto");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ValidarISBN_ComISBN10TerminadoEmX_DeveRetornarTrue()
        {
            // Arrange - ISBN-10 válido terminado em X (representa 10)
            var isbn10 = "043942089X";

            // Act
            var resultado = Validadores.ValidarISBN(isbn10);

            // Assert
            resultado.Should().BeTrue("ISBN-10 com X no final é válido quando X representa 10");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ValidarISBN_ComISBN10TerminadoEmXMinusculo_DeveRetornarTrue()
        {
            // Arrange - ISBN-10 válido terminado em x minúsculo
            var isbn10 = "043942089x";

            // Act
            var resultado = Validadores.ValidarISBN(isbn10);

            // Assert
            resultado.Should().BeTrue("ISBN-10 com x minúsculo no final é válido");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ValidarISBN_ComISBN13ComEspacos_DeveRetornarTrue()
        {
            // Arrange - ISBN-13 com espaços (devem ser removidos)
            var isbn13 = "978 0 13 235088 4";

            // Act
            var resultado = Validadores.ValidarISBN(isbn13);

            // Assert
            resultado.Should().BeTrue("espaços devem ser removidos na validação");
        }

        #endregion

        #region ValidarEmail Tests

        [Theory]
        [InlineData("user@example.com", true)]                      // Email básico válido
        [InlineData("usuario@exemplo.com", true)]                   // Email com caracteres acentuados não
        [InlineData("nome.sobrenome@empresa.com.br", true)]         // Email com ponto e múltiplos domínios
        [InlineData("teste123@dominio.co", true)]                   // Email com números
        [InlineData("test.user+tag@domain.co.uk", true)]            // Email com plus e múltiplos subdomínios
        [InlineData("first.last@sub.domain.com", true)]             // Email com subdomínio
        [InlineData("user_name@example.org", true)]                 // Email com underscore
        [InlineData("user-name@example.net", true)]                 // Email com hífen
        [InlineData("123@example.com", true)]                       // Email começando com número
        [InlineData("a@b.c", true)]                                 // Email minimalista válido
        [InlineData("invalid", false)]                              // Sem @ e domínio
        [InlineData("@example.com", false)]                         // Sem parte local
        [InlineData("user@", false)]                                // Sem domínio
        [InlineData("user @example.com", false)]                    // Espaço no meio
        [InlineData("user@example", false)]                         // Sem TLD
        [InlineData("", false)]                                     // String vazia
        [InlineData(null, false)]                                   // Null
        [InlineData("   ", false)]                                  // Apenas espaços
        [InlineData("user@@example.com", false)]                    // Dois @
        [InlineData("user@.com", false)]                            // Domínio começando com ponto
        [InlineData("user@example..com", false)]                    // Pontos consecutivos
        [InlineData("user name@example.com", false)]                // Espaço na parte local
        [InlineData("user@exam ple.com", false)]                    // Espaço no domínio
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

        [Fact]
        [Trait("Category", "Unit")]
        public void ValidarEmail_ComEmailValidoComplexo_DeveRetornarTrue()
        {
            // Arrange - Email com caracteres especiais permitidos
            var emailComplexo = "test.user+tag@sub.domain.co.uk";

            // Act
            var resultado = Validadores.ValidarEmail(emailComplexo);

            // Assert
            resultado.Should().BeTrue("email complexo mas válido deve ser aceito");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ValidarEmail_ComEspacosNoInicio_DeveRetornarFalse()
        {
            // Arrange - Email com espaços no início
            var emailInvalido = "  user@example.com";

            // Act
            var resultado = Validadores.ValidarEmail(emailInvalido);

            // Assert
            resultado.Should().BeFalse("espaços no início invalidam o email");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ValidarEmail_ComEspacosNoFinal_DeveRetornarFalse()
        {
            // Arrange - Email com espaços no final
            var emailInvalido = "user@example.com  ";

            // Act
            var resultado = Validadores.ValidarEmail(emailInvalido);

            // Assert
            resultado.Should().BeFalse("espaços no final invalidam o email");
        }

        #endregion

        #region ValidarMatricula Tests

        [Theory]
        [InlineData("ABC123", true)]              // Alfanumérico válido
        [InlineData("2024001", true)]             // Apenas números
        [InlineData("MAT2024", true)]             // Letras e números
        [InlineData("A1B2C3D4E5F6G7H8", true)]   // 18 caracteres (dentro do limite)
        [InlineData("12345678901234567890", true)] // 20 caracteres (limite máximo)
        [InlineData("ABC", true)]                 // 3 caracteres (limite mínimo)
        [InlineData("AB", false)]                 // Muito curto (2 caracteres)
        [InlineData("A", false)]                  // Muito curto (1 caractere)
        [InlineData("", false)]                   // String vazia
        [InlineData(null, false)]                 // Null
        [InlineData("   ", false)]                // Apenas espaços
        [InlineData("ABC-123", false)]            // Contém hífen
        [InlineData("ABC_123", false)]            // Contém underscore
        [InlineData("ABC.123", false)]            // Contém ponto
        [InlineData("ABC 123", false)]            // Contém espaço
        [InlineData("ABC@123", false)]            // Contém caractere especial
        [InlineData("ÁÉÍ123", false)]             // Contém acentos
        [InlineData("123456789012345678901", false)] // 21 caracteres (muito longo)
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", false)] // 26 caracteres (muito longo)
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void ValidarMatricula_DeveValidarCorretamente(string matricula, bool esperado)
        {
            // Act
            var resultado = Validadores.ValidarMatricula(matricula);

            // Assert
            resultado.Should().Be(esperado,
                $"Matrícula '{matricula}' deveria ser {(esperado ? "válida" : "inválida")}");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ValidarMatricula_ComMatriculaValida_DeveRetornarTrue()
        {
            // Arrange - Matrícula válida típica
            var matriculaValida = "2024MAT001";

            // Act
            var resultado = Validadores.ValidarMatricula(matriculaValida);

            // Assert
            resultado.Should().BeTrue("matrícula alfanumérica dentro dos limites é válida");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ValidarMatricula_ComApenasNumeros_DeveRetornarTrue()
        {
            // Arrange - Matrícula apenas numérica
            var matriculaValida = "20240001";

            // Act
            var resultado = Validadores.ValidarMatricula(matriculaValida);

            // Assert
            resultado.Should().BeTrue("matrícula apenas numérica é válida");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ValidarMatricula_ComApenasLetras_DeveRetornarTrue()
        {
            // Arrange - Matrícula apenas com letras
            var matriculaValida = "ALUNO";

            // Act
            var resultado = Validadores.ValidarMatricula(matriculaValida);

            // Assert
            resultado.Should().BeTrue("matrícula apenas com letras é válida");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ValidarMatricula_ComTamanhoMinimo_DeveRetornarTrue()
        {
            // Arrange - Matrícula com 3 caracteres (mínimo)
            var matriculaValida = "ABC";

            // Act
            var resultado = Validadores.ValidarMatricula(matriculaValida);

            // Assert
            resultado.Should().BeTrue("matrícula com 3 caracteres é válida");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ValidarMatricula_ComTamanhoMaximo_DeveRetornarTrue()
        {
            // Arrange - Matrícula com 20 caracteres (máximo)
            var matriculaValida = "12345678901234567890";

            // Act
            var resultado = Validadores.ValidarMatricula(matriculaValida);

            // Assert
            resultado.Should().BeTrue("matrícula com 20 caracteres é válida");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ValidarMatricula_ComCaracteresEspeciais_DeveRetornarFalse()
        {
            // Arrange - Matrícula com caracteres especiais
            var matriculaInvalida = "ABC-123";

            // Act
            var resultado = Validadores.ValidarMatricula(matriculaInvalida);

            // Assert
            resultado.Should().BeFalse("matrícula com caracteres especiais é inválida");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ValidarMatricula_MuitoCurta_DeveRetornarFalse()
        {
            // Arrange - Matrícula com 2 caracteres (abaixo do mínimo)
            var matriculaInvalida = "AB";

            // Act
            var resultado = Validadores.ValidarMatricula(matriculaInvalida);

            // Assert
            resultado.Should().BeFalse("matrícula com menos de 3 caracteres é inválida");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ValidarMatricula_MuitoLonga_DeveRetornarFalse()
        {
            // Arrange - Matrícula com 21 caracteres (acima do máximo)
            var matriculaInvalida = "123456789012345678901";

            // Act
            var resultado = Validadores.ValidarMatricula(matriculaInvalida);

            // Assert
            resultado.Should().BeFalse("matrícula com mais de 20 caracteres é inválida");
        }

        #endregion

        #region CampoObrigatorio Tests

        [Theory]
        [InlineData("Valor válido", "Campo", true, "")]
        [InlineData("", "Campo", false, "O campo 'Campo' é obrigatório.")]
        [InlineData(null, "Campo", false, "O campo 'Campo' é obrigatório.")]
        [InlineData("   ", "Campo", false, "O campo 'Campo' é obrigatório.")]
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void CampoObrigatorio_DeveValidarCorretamente(string valor, string nomeCampo, bool esperado, string mensagemEsperada)
        {
            // Act
            var resultado = Validadores.CampoObrigatorio(valor, nomeCampo, out string mensagemErro);

            // Assert
            resultado.Should().Be(esperado);
            mensagemErro.Should().Be(mensagemEsperada);
        }

        #endregion

        #region NumeroPositivo Tests

        [Theory]
        [InlineData(1, "Quantidade", true, "")]
        [InlineData(100, "Quantidade", true, "")]
        [InlineData(0, "Quantidade", false, "O campo 'Quantidade' deve ser um número positivo.")]
        [InlineData(-1, "Quantidade", false, "O campo 'Quantidade' deve ser um número positivo.")]
        [InlineData(-100, "Quantidade", false, "O campo 'Quantidade' deve ser um número positivo.")]
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void NumeroPositivo_DeveValidarCorretamente(int valor, string nomeCampo, bool esperado, string mensagemEsperada)
        {
            // Act
            var resultado = Validadores.NumeroPositivo(valor, nomeCampo, out string mensagemErro);

            // Assert
            resultado.Should().Be(esperado);
            mensagemErro.Should().Be(mensagemEsperada);
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

        [Fact]
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void ValidarISBN_Performance_DeveDemorarMenosDe1Milissegundo()
        {
            // Arrange
            var isbn = "978-0-13-110362-7";
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Act
            for (int i = 0; i < 1000; i++)
            {
                Validadores.ValidarISBN(isbn);
            }
            stopwatch.Stop();

            // Assert
            var tempoMedioPorChamada = stopwatch.ElapsedMilliseconds / 1000.0;
            tempoMedioPorChamada.Should().BeLessThan(1,
                "validação de ISBN deve ser rápida");
        }

        [Fact]
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void ValidarEmail_Performance_DeveDemorarMenosDe1Milissegundo()
        {
            // Arrange
            var email = "test.user@example.com";
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Act
            for (int i = 0; i < 1000; i++)
            {
                Validadores.ValidarEmail(email);
            }
            stopwatch.Stop();

            // Assert
            var tempoMedioPorChamada = stopwatch.ElapsedMilliseconds / 1000.0;
            tempoMedioPorChamada.Should().BeLessThan(1,
                "validação de email deve ser rápida");
        }

        #endregion
    }
}
