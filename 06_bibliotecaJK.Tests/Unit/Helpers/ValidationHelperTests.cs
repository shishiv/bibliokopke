using System;
using Xunit;
using FluentAssertions;
using BibliotecaJK.Helpers;

namespace BibliotecaJK.Tests.Unit.Helpers
{
    /// <summary>
    /// Testes unitários para ValidationHelper
    /// Valida todos os validadores e utilitários de composição
    /// </summary>
    public class ValidationHelperTests
    {
        #region Required Validator Tests

        [Theory]
        [InlineData("Valor válido", true, "")]
        [InlineData("A", true, "")]
        [InlineData("", false, "Nome é obrigatório")]
        [InlineData("   ", false, "Nome é obrigatório")]
        [InlineData(null, false, "Nome é obrigatório")]
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void Required_DeveValidarCamposObrigatorios(
            string valor,
            bool isValidEsperado,
            string mensagemEsperada)
        {
            // Arrange
            var validator = ValidationHelper.Required("Nome");

            // Act
            var (isValid, errorMessage) = validator(valor);

            // Assert
            isValid.Should().Be(isValidEsperado);
            errorMessage.Should().Be(mensagemEsperada);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Required_ComMensagemCustomizada_DeveUsarMensagem()
        {
            // Arrange
            var customMessage = "Este campo não pode ficar vazio!";
            var validator = ValidationHelper.Required("Campo", customMessage);

            // Act
            var (isValid, errorMessage) = validator("");

            // Assert
            isValid.Should().BeFalse();
            errorMessage.Should().Be(customMessage);
        }

        #endregion

        #region MinLength Validator Tests

        [Theory]
        [InlineData("123", 3, true, "")]
        [InlineData("1234", 3, true, "")]
        [InlineData("12", 3, false, "Nome deve ter no mínimo 3 caracteres")]
        [InlineData("", 3, true, "")]  // Empty passes - use Required() to enforce non-empty
        [InlineData(null, 3, true, "")]
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void MinLength_DeveValidarComprimentoMinimo(
            string valor,
            int minLength,
            bool isValidEsperado,
            string mensagemEsperada)
        {
            // Arrange
            var validator = ValidationHelper.MinLength(minLength, "Nome");

            // Act
            var (isValid, errorMessage) = validator(valor);

            // Assert
            isValid.Should().Be(isValidEsperado);
            errorMessage.Should().Be(mensagemEsperada);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void MinLength_ComMensagemCustomizada_DeveUsarMensagem()
        {
            // Arrange
            var customMessage = "Muito curto!";
            var validator = ValidationHelper.MinLength(5, "Campo", customMessage);

            // Act
            var (isValid, errorMessage) = validator("AB");

            // Assert
            isValid.Should().BeFalse();
            errorMessage.Should().Be(customMessage);
        }

        #endregion

        #region MaxLength Validator Tests

        [Theory]
        [InlineData("123", 5, true, "")]
        [InlineData("12345", 5, true, "")]
        [InlineData("123456", 5, false, "Nome deve ter no máximo 5 caracteres")]
        [InlineData("", 5, true, "")]
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void MaxLength_DeveValidarComprimentoMaximo(
            string valor,
            int maxLength,
            bool isValidEsperado,
            string mensagemEsperada)
        {
            // Arrange
            var validator = ValidationHelper.MaxLength(maxLength, "Nome");

            // Act
            var (isValid, errorMessage) = validator(valor);

            // Assert
            isValid.Should().Be(isValidEsperado);
            errorMessage.Should().Be(mensagemEsperada);
        }

        #endregion

        #region LengthRange Validator Tests

        [Theory]
        [InlineData("123", 3, 10, true, "")]
        [InlineData("1234567890", 3, 10, true, "")]
        [InlineData("12", 3, 10, false, "Matrícula deve ter entre 3 e 10 caracteres")]
        [InlineData("12345678901", 3, 10, false, "Matrícula deve ter entre 3 e 10 caracteres")]
        [InlineData("", 3, 10, true, "")]
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void LengthRange_DeveValidarIntervaloDeComprimento(
            string valor,
            int min,
            int max,
            bool isValidEsperado,
            string mensagemEsperada)
        {
            // Arrange
            var validator = ValidationHelper.LengthRange(min, max, "Matrícula");

            // Act
            var (isValid, errorMessage) = validator(valor);

            // Assert
            isValid.Should().Be(isValidEsperado);
            errorMessage.Should().Be(mensagemEsperada);
        }

        #endregion

        #region Pattern Validator Tests

        [Theory]
        [InlineData(@"^\d{5}-\d{3}$", "12345-678", true, "")]
        [InlineData(@"^\d{5}-\d{3}$", "12345678", false, "CEP inválido")]
        [InlineData(@"^[A-Z]{3}-\d{4}$", "ABC-1234", true, "")]
        [InlineData(@"^[A-Z]{3}-\d{4}$", "abc-1234", false, "Código inválido")]
        [InlineData(@"^\d+$", "", true, "")]  // Empty passes
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void Pattern_DeveValidarPadroesRegex(
            string pattern,
            string valor,
            bool isValidEsperado,
            string mensagemEsperada)
        {
            // Arrange
            var invalidMsg = mensagemEsperada == "" ? mensagemEsperada : mensagemEsperada;
            var validator = ValidationHelper.Pattern(pattern, "Campo", invalidMsg == "" ? "Formato inválido" : invalidMsg);

            // Act
            var (isValid, errorMessage) = validator(valor);

            // Assert
            isValid.Should().Be(isValidEsperado);
            if (mensagemEsperada != "")
            {
                errorMessage.Should().Contain(mensagemEsperada);
            }
        }

        #endregion

        #region Email Validator Tests

        [Theory]
        [InlineData("usuario@exemplo.com", true, "")]
        [InlineData("nome.sobrenome@empresa.com.br", true, "")]
        [InlineData("teste123@dominio.co", true, "")]
        [InlineData("invalido@", false, "E-mail inválido")]
        [InlineData("@invalido.com", false, "E-mail inválido")]
        [InlineData("sem-arroba.com", false, "E-mail inválido")]
        [InlineData("", true, "")]  // Empty passes - use Required() to enforce non-empty
        [InlineData(null, true, "")]
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void Email_DelegaParaValidadores_ValidarEmail(
            string valor,
            bool isValidEsperado,
            string mensagemEsperada)
        {
            // Arrange
            var validator = ValidationHelper.Email();

            // Act
            var (isValid, errorMessage) = validator(valor);

            // Assert
            isValid.Should().Be(isValidEsperado);
            errorMessage.Should().Be(mensagemEsperada);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Email_ComNomeCampoCustomizado_DeveUsarNome()
        {
            // Arrange
            var validator = ValidationHelper.Email("E-mail Corporativo");

            // Act
            var (isValid, errorMessage) = validator("invalido");

            // Assert
            isValid.Should().BeFalse();
            errorMessage.Should().Contain("E-mail Corporativo");
        }

        #endregion

        #region CPF Validator Tests

        [Theory]
        [InlineData("123.456.789-09", true, "")]
        [InlineData("12345678909", true, "")]
        [InlineData("52998224725", true, "")]
        [InlineData("000.000.000-00", false, "CPF inválido")]
        [InlineData("111.111.111-11", false, "CPF inválido")]
        [InlineData("12345678901", false, "CPF inválido")]
        [InlineData("", true, "")]  // Empty passes
        [InlineData(null, true, "")]
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void CPF_DelegaParaValidadores_ValidarCPF(
            string valor,
            bool isValidEsperado,
            string mensagemEsperada)
        {
            // Arrange
            var validator = ValidationHelper.CPF();

            // Act
            var (isValid, errorMessage) = validator(valor);

            // Assert
            isValid.Should().Be(isValidEsperado);
            errorMessage.Should().Be(mensagemEsperada);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void CPF_ComMensagemCustomizada_DeveUsarMensagem()
        {
            // Arrange
            var customMessage = "CPF fornecido é inválido!";
            var validator = ValidationHelper.CPF("CPF", customMessage);

            // Act
            var (isValid, errorMessage) = validator("111.111.111-11");

            // Assert
            isValid.Should().BeFalse();
            errorMessage.Should().Be(customMessage);
        }

        #endregion

        #region ISBN Validator Tests

        [Theory]
        [InlineData("978-0-13-110362-7", true, "")]
        [InlineData("978-0-596-52068-7", true, "")]
        [InlineData("9780136083238", true, "")]
        [InlineData("978-0-00-000000-0", false, "ISBN inválido")]
        [InlineData("12345", false, "ISBN inválido")]
        [InlineData("", true, "")]
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void ISBN_DelegaParaValidadores_ValidarISBN(
            string valor,
            bool isValidEsperado,
            string mensagemEsperada)
        {
            // Arrange
            var validator = ValidationHelper.ISBN();

            // Act
            var (isValid, errorMessage) = validator(valor);

            // Assert
            isValid.Should().Be(isValidEsperado);
            errorMessage.Should().Be(mensagemEsperada);
        }

        #endregion

        #region Matricula Validator Tests

        [Theory]
        [InlineData("ALU2023001", true, "")]
        [InlineData("12345", true, "")]
        [InlineData("ABC", true, "")]
        [InlineData("AB", false, "Matrícula inválida (deve ter entre 3 e 20 caracteres alfanuméricos)")]
        [InlineData("123456789012345678901", false, "Matrícula inválida (deve ter entre 3 e 20 caracteres alfanuméricos)")]
        [InlineData("", true, "")]
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void Matricula_DelegaParaValidadores_ValidarMatricula(
            string valor,
            bool isValidEsperado,
            string mensagemEsperada)
        {
            // Arrange
            var validator = ValidationHelper.Matricula();

            // Act
            var (isValid, errorMessage) = validator(valor);

            // Assert
            isValid.Should().Be(isValidEsperado);
            errorMessage.Should().Be(mensagemEsperada);
        }

        #endregion

        #region Numeric Validator Tests

        [Theory]
        [InlineData("123", true, "")]
        [InlineData("123.45", true, "")]
        [InlineData("-50.5", true, "")]
        [InlineData("0", true, "")]
        [InlineData("abc", false, "Preço deve ser um número")]
        [InlineData("12a34", false, "Preço deve ser um número")]
        [InlineData("", true, "")]
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void Numeric_DeveValidarNumeros(
            string valor,
            bool isValidEsperado,
            string mensagemEsperada)
        {
            // Arrange
            var validator = ValidationHelper.Numeric("Preço");

            // Act
            var (isValid, errorMessage) = validator(valor);

            // Assert
            isValid.Should().Be(isValidEsperado);
            errorMessage.Should().Be(mensagemEsperada);
        }

        #endregion

        #region IntegerRange Validator Tests

        [Theory]
        [InlineData("50", 1, 100, true, "")]
        [InlineData("1", 1, 100, true, "")]
        [InlineData("100", 1, 100, true, "")]
        [InlineData("0", 1, 100, false, "Quantidade deve estar entre 1 e 100")]
        [InlineData("101", 1, 100, false, "Quantidade deve estar entre 1 e 100")]
        [InlineData("abc", 1, 100, false, "Quantidade deve ser um número inteiro")]
        [InlineData("50.5", 1, 100, false, "Quantidade deve ser um número inteiro")]
        [InlineData("", 1, 100, true, "")]
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void IntegerRange_DeveValidarIntervaloDeInteiros(
            string valor,
            int min,
            int max,
            bool isValidEsperado,
            string mensagemEsperada)
        {
            // Arrange
            var validator = ValidationHelper.IntegerRange(min, max, "Quantidade");

            // Act
            var (isValid, errorMessage) = validator(valor);

            // Assert
            isValid.Should().Be(isValidEsperado);
            errorMessage.Should().Be(mensagemEsperada);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void IntegerRange_ComMensagemCustomizada_DeveUsarMensagem()
        {
            // Arrange
            var customMessage = "Fora do intervalo permitido!";
            var validator = ValidationHelper.IntegerRange(1, 10, "Campo", customMessage);

            // Act
            var (isValid, errorMessage) = validator("50");

            // Assert
            isValid.Should().BeFalse();
            errorMessage.Should().Be(customMessage);
        }

        #endregion

        #region DecimalRange Validator Tests

        [Theory]
        [InlineData("50.5", 0.01, 100.0, true, "")]
        [InlineData("0.01", 0.01, 100.0, true, "")]
        [InlineData("100.00", 0.01, 100.0, true, "")]
        [InlineData("0", 0.01, 100.0, false, "Preço deve estar entre 0,01 e 100,00")]
        [InlineData("100.01", 0.01, 100.0, false, "Preço deve estar entre 0,01 e 100,00")]
        [InlineData("abc", 0.01, 100.0, false, "Preço deve ser um número")]
        [InlineData("", 0.01, 100.0, true, "")]
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void DecimalRange_DeveValidarIntervaloDeDecimais(
            string valor,
            double min,
            double max,
            bool isValidEsperado,
            string mensagemEsperada)
        {
            // Arrange
            var validator = ValidationHelper.DecimalRange((decimal)min, (decimal)max, "Preço");

            // Act
            var (isValid, errorMessage) = validator(valor);

            // Assert
            isValid.Should().Be(isValidEsperado);
            errorMessage.Should().Be(mensagemEsperada);
        }

        #endregion

        #region Date Validator Tests

        [Theory]
        [InlineData("01/01/2000", true, "")]
        [InlineData("31/12/2023", true, "")]
        [InlineData("2023-12-31", true, "")]
        [InlineData("32/01/2000", false, "Data de Nascimento deve ser uma data válida")]
        [InlineData("abc", false, "Data de Nascimento deve ser uma data válida")]
        [InlineData("", true, "")]
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void Date_DeveValidarDatas(
            string valor,
            bool isValidEsperado,
            string mensagemEsperada)
        {
            // Arrange
            var validator = ValidationHelper.Date("Data de Nascimento");

            // Act
            var (isValid, errorMessage) = validator(valor);

            // Assert
            isValid.Should().Be(isValidEsperado);
            errorMessage.Should().Be(mensagemEsperada);
        }

        #endregion

        #region DateRange Validator Tests

        [Fact]
        [Trait("Category", "Unit")]
        public void DateRange_DeveValidarIntervalosDeDatas()
        {
            // Arrange
            var min = new DateTime(1900, 1, 1);
            var max = new DateTime(2023, 12, 31);
            var validator = ValidationHelper.DateRange(min, max, "Data de Nascimento");

            // Act & Assert
            var (valid1, _) = validator("01/01/2000");
            valid1.Should().BeTrue("data dentro do intervalo");

            var (valid2, msg2) = validator("01/01/1899");
            valid2.Should().BeFalse("data antes do mínimo");
            msg2.Should().Contain("deve estar entre");

            var (valid3, msg3) = validator("01/01/2024");
            valid3.Should().BeFalse("data depois do máximo");
            msg3.Should().Contain("deve estar entre");

            var (valid4, _) = validator("");
            valid4.Should().BeTrue("vazio passa");
        }

        #endregion

        #region Custom Validator Tests

        [Fact]
        [Trait("Category", "Unit")]
        public void Custom_DeveExecutarPredicadoCustomizado()
        {
            // Arrange
            var validator = ValidationHelper.Custom(
                value => value.StartsWith("BIB-"),
                "Código deve começar com BIB-"
            );

            // Act & Assert
            var (valid1, _) = validator("BIB-001");
            valid1.Should().BeTrue();

            var (valid2, msg2) = validator("LIV-001");
            valid2.Should().BeFalse();
            msg2.Should().Be("Código deve começar com BIB-");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Custom_ComValorVazio_DevePpassar()
        {
            // Arrange
            var validator = ValidationHelper.Custom(
                value => value.Length > 5,
                "Deve ter mais de 5 caracteres"
            );

            // Act
            var (isValid, _) = validator("");

            // Assert
            isValid.Should().BeTrue("valores vazios passam por padrão");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Custom_ComExcecaoNoPredicado_DeveRetornarErro()
        {
            // Arrange
            var validator = ValidationHelper.Custom(
                value => throw new InvalidOperationException("Erro de teste"),
                "Mensagem de erro"
            );

            // Act
            var (isValid, errorMessage) = validator("valor");

            // Assert
            isValid.Should().BeFalse();
            errorMessage.Should().Contain("Erro na validação");
            errorMessage.Should().Contain("Erro de teste");
        }

        #endregion

        #region Composite Validator Tests

        [Fact]
        [Trait("Category", "Unit")]
        public void Composite_DeveCombinarMultiplosValidadores()
        {
            // Arrange
            var validator = ValidationHelper.Composite(
                ValidationHelper.Required("Nome"),
                ValidationHelper.MinLength(3, "Nome"),
                ValidationHelper.MaxLength(50, "Nome")
            );

            // Act & Assert

            // Valid case
            var (valid1, _) = validator("João Silva");
            valid1.Should().BeTrue();

            // Empty - fails Required
            var (valid2, msg2) = validator("");
            valid2.Should().BeFalse();
            msg2.Should().Contain("obrigatório");

            // Too short - fails MinLength
            var (valid3, msg3) = validator("Ab");
            valid3.Should().BeFalse();
            msg3.Should().Contain("mínimo");

            // Too long - fails MaxLength
            var (valid4, msg4) = validator(new string('A', 51));
            valid4.Should().BeFalse();
            msg4.Should().Contain("máximo");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Composite_DeveRetornarPrimeiroErro()
        {
            // Arrange
            var validator = ValidationHelper.Composite(
                ValidationHelper.Required("Campo"),
                ValidationHelper.MinLength(5, "Campo")
            );

            // Act - Empty value fails both, but should return first error
            var (isValid, errorMessage) = validator("");

            // Assert
            isValid.Should().BeFalse();
            errorMessage.Should().Contain("obrigatório", "deve retornar o primeiro erro (Required)");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Composite_CPFCompleto_DeveValidarCampoObrigatorioEFormato()
        {
            // Arrange
            var validator = ValidationHelper.Composite(
                ValidationHelper.Required("CPF"),
                ValidationHelper.CPF()
            );

            // Act & Assert
            var (valid1, _) = validator("123.456.789-09");
            valid1.Should().BeTrue("CPF válido");

            var (valid2, msg2) = validator("");
            valid2.Should().BeFalse();
            msg2.Should().Contain("obrigatório");

            var (valid3, msg3) = validator("111.111.111-11");
            valid3.Should().BeFalse();
            msg3.Should().Contain("inválido");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Composite_ComUmValidator_DeveFuncionar()
        {
            // Arrange
            var validator = ValidationHelper.Composite(
                ValidationHelper.Required("Campo")
            );

            // Act
            var (isValid, errorMessage) = validator("");

            // Assert
            isValid.Should().BeFalse();
            errorMessage.Should().Contain("obrigatório");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Composite_ComTodosValidadoresPassando_DeveRetornarSucesso()
        {
            // Arrange
            var validator = ValidationHelper.Composite(
                ValidationHelper.Required("Email"),
                ValidationHelper.Email()
            );

            // Act
            var (isValid, errorMessage) = validator("usuario@exemplo.com");

            // Assert
            isValid.Should().BeTrue();
            errorMessage.Should().BeEmpty();
        }

        #endregion

        #region Integration Tests - Real World Scenarios

        [Fact]
        [Trait("Category", "Unit")]
        public void CenarioReal_ValidacaoFormularioAluno_DeveFuncionar()
        {
            // Arrange
            var validatorNome = ValidationHelper.Composite(
                ValidationHelper.Required("Nome"),
                ValidationHelper.MinLength(3, "Nome")
            );

            var validatorCPF = ValidationHelper.Composite(
                ValidationHelper.Required("CPF"),
                ValidationHelper.CPF()
            );

            var validatorEmail = ValidationHelper.Composite(
                ValidationHelper.Required("E-mail"),
                ValidationHelper.Email()
            );

            var validatorMatricula = ValidationHelper.Composite(
                ValidationHelper.Required("Matrícula"),
                ValidationHelper.Matricula()
            );

            // Act & Assert - Valid student
            validatorNome("João Silva").IsValid.Should().BeTrue();
            validatorCPF("123.456.789-09").IsValid.Should().BeTrue();
            validatorEmail("joao@escola.com").IsValid.Should().BeTrue();
            validatorMatricula("ALU2023001").IsValid.Should().BeTrue();

            // Invalid cases
            validatorNome("Jo").IsValid.Should().BeFalse("nome muito curto");
            validatorCPF("111.111.111-11").IsValid.Should().BeFalse("CPF inválido");
            validatorEmail("email-invalido").IsValid.Should().BeFalse("email sem @");
            validatorMatricula("AB").IsValid.Should().BeFalse("matrícula muito curta");
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void CenarioReal_ValidacaoFormularioLivro_DeveFuncionar()
        {
            // Arrange
            var validatorTitulo = ValidationHelper.Composite(
                ValidationHelper.Required("Título"),
                ValidationHelper.MinLength(2, "Título"),
                ValidationHelper.MaxLength(200, "Título")
            );

            var validatorISBN = ValidationHelper.Composite(
                ValidationHelper.Required("ISBN"),
                ValidationHelper.ISBN()
            );

            var validatorQuantidade = ValidationHelper.Composite(
                ValidationHelper.Required("Quantidade"),
                ValidationHelper.IntegerRange(1, 1000, "Quantidade")
            );

            // Act & Assert
            validatorTitulo("Clean Code").IsValid.Should().BeTrue();
            validatorISBN("978-0-13-110362-7").IsValid.Should().BeTrue();
            validatorQuantidade("5").IsValid.Should().BeTrue();

            validatorTitulo("A").IsValid.Should().BeFalse("título muito curto");
            validatorISBN("123").IsValid.Should().BeFalse("ISBN inválido");
            validatorQuantidade("0").IsValid.Should().BeFalse("quantidade fora do range");
        }

        #endregion

        #region Performance Tests

        [Fact]
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void Required_Performance_DeveDemorarMenosDe1Milissegundo()
        {
            // Arrange
            var validator = ValidationHelper.Required("Campo");
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Act
            for (int i = 0; i < 10000; i++)
            {
                validator("valor de teste");
            }
            stopwatch.Stop();

            // Assert
            var tempoMedioPorChamada = stopwatch.ElapsedMilliseconds / 10000.0;
            tempoMedioPorChamada.Should().BeLessThan(1,
                "validação Required deve ser extremamente rápida");
        }

        [Fact]
        [Trait("Category", "Unit")]
        [Trait("Speed", "Fast")]
        public void Composite_Performance_ComMultiplosValidadores_DeveDemorarMenosDe10ms()
        {
            // Arrange
            var validator = ValidationHelper.Composite(
                ValidationHelper.Required("Campo"),
                ValidationHelper.MinLength(3, "Campo"),
                ValidationHelper.MaxLength(50, "Campo"),
                ValidationHelper.Email()
            );
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Act
            for (int i = 0; i < 1000; i++)
            {
                validator("usuario@exemplo.com");
            }
            stopwatch.Stop();

            // Assert
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(10,
                "validação composta deve ser razoavelmente rápida");
        }

        #endregion

        #region Edge Cases

        [Fact]
        [Trait("Category", "Unit")]
        public void TodosValidadores_ComNull_DevemRetornarComportamentoEsperado()
        {
            // Arrange & Act & Assert
            ValidationHelper.Required("Campo")(null).IsValid.Should().BeFalse();
            ValidationHelper.MinLength(3, "Campo")(null).IsValid.Should().BeTrue();
            ValidationHelper.MaxLength(10, "Campo")(null).IsValid.Should().BeTrue();
            ValidationHelper.Email()(null).IsValid.Should().BeTrue();
            ValidationHelper.CPF()(null).IsValid.Should().BeTrue();
            ValidationHelper.Numeric("Campo")(null).IsValid.Should().BeTrue();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TodosValidadores_ComStringVazia_DevemRetornarComportamentoEsperado()
        {
            // Arrange & Act & Assert
            ValidationHelper.Required("Campo")("").IsValid.Should().BeFalse();
            ValidationHelper.MinLength(3, "Campo")("").IsValid.Should().BeTrue();
            ValidationHelper.MaxLength(10, "Campo")("").IsValid.Should().BeTrue();
            ValidationHelper.Email()("").IsValid.Should().BeTrue();
            ValidationHelper.CPF()("").IsValid.Should().BeTrue();
            ValidationHelper.Numeric("Campo")("").IsValid.Should().BeTrue();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TodosValidadores_ComWhitespace_DevemRetornarComportamentoEsperado()
        {
            // Arrange & Act & Assert
            ValidationHelper.Required("Campo")("   ").IsValid.Should().BeFalse();
            ValidationHelper.MinLength(3, "Campo")("   ").IsValid.Should().BeTrue();
            ValidationHelper.Email()("   ").IsValid.Should().BeTrue();
        }

        #endregion
    }
}
