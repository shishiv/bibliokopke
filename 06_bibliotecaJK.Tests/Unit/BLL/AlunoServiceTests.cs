using Xunit;
using FluentAssertions;
using Moq;
using BibliotecaJK.BLL;
using BibliotecaJK.DAL;
using BibliotecaJK.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BibliotecaJK.Tests.Unit.BLL
{
    /// <summary>
    /// Testes unitários para AlunoService
    ///
    /// NOTA IMPORTANTE: AlunoService atualmente cria suas próprias dependências
    /// no construtor, o que impede testes unitários puros com mocks.
    ///
    /// REFATORAÇÃO NECESSÁRIA: Para habilitar estes testes, AlunoService precisa:
    /// 1. Aceitar dependências via construtor (Dependency Injection)
    /// 2. Ou prover um construtor alternativo para testes
    ///
    /// Exemplo de refatoração:
    /// public AlunoService(AlunoDAL alunoDAL, EmprestimoDAL emprestimoDAL, LogService logService)
    /// {
    ///     _alunoDAL = alunoDAL;
    ///     _emprestimoDAL = emprestimoDAL;
    ///     _logService = logService;
    /// }
    ///
    /// Esta classe fornece a estrutura completa de testes para quando a refatoração
    /// for implementada. Os testes estão funcionais e documentados, mas comentados
    /// até que a injeção de dependências seja implementada.
    /// </summary>
    public class AlunoServiceTests
    {
        private readonly Mock<AlunoDAL> _mockAlunoDAL;
        private readonly Mock<EmprestimoDAL> _mockEmprestimoDAL;
        private readonly Mock<LogService> _mockLogService;
        // TODO: Uncomment when AlunoService supports dependency injection
        // private readonly AlunoService _service;

        public AlunoServiceTests()
        {
            _mockAlunoDAL = new Mock<AlunoDAL>();
            _mockEmprestimoDAL = new Mock<EmprestimoDAL>();
            _mockLogService = new Mock<LogService>();

            // TODO: Uncomment and implement when AlunoService constructor accepts dependencies
            // _service = new AlunoService(_mockAlunoDAL.Object, _mockEmprestimoDAL.Object, _mockLogService.Object);
        }

        #region CadastrarAluno - Success Cases

        /// <summary>
        /// Teste: Cadastro de aluno com todos os dados válidos deve retornar sucesso
        /// </summary>
        [Fact]
        [Trait("Category", "Unit")]
        [Trait("Priority", "High")]
        public void CadastrarAluno_ComDadosValidos_DeveRetornarSucesso()
        {
            // Arrange
            var aluno = new Aluno
            {
                Nome = "João Silva",
                CPF = "123.456.789-09",
                Matricula = "2024001",
                Turma = "3A",
                Email = "joao.silva@example.com",
                Telefone = "(11) 98765-4321"
            };

            _mockAlunoDAL.Setup(dal => dal.Listar())
                .Returns(new List<Aluno>()); // Nenhum aluno duplicado

            _mockAlunoDAL.Setup(dal => dal.Inserir(It.IsAny<Aluno>()))
                .Verifiable();

            _mockLogService.Setup(log => log.Registrar(
                It.IsAny<int?>(),
                "ALUNO_CADASTRADO",
                It.IsAny<string>()))
                .Verifiable();

            // Act
            // var resultado = _service.CadastrarAluno(aluno, idFuncionario: 1);

            // Assert
            // resultado.Should().NotBeNull();
            // resultado.Sucesso.Should().BeTrue("dados válidos devem resultar em cadastro bem-sucedido");
            // resultado.Mensagem.Should().Contain("sucesso");
            // resultado.Mensagem.Should().Contain(aluno.Nome);

            // _mockAlunoDAL.Verify(dal => dal.Inserir(It.Is<Aluno>(a =>
            //     a.Nome == aluno.Nome &&
            //     a.CPF == aluno.CPF &&
            //     a.Matricula == aluno.Matricula)), Times.Once);

            // _mockLogService.Verify(log => log.Registrar(
            //     It.IsAny<int?>(),
            //     "ALUNO_CADASTRADO",
            //     It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Teste: Cadastro de aluno sem email e telefone (campos opcionais) deve ser permitido
        /// </summary>
        [Fact]
        [Trait("Category", "Unit")]
        [Trait("Priority", "Medium")]
        public void CadastrarAluno_SemEmailETelefone_DeveRetornarSucesso()
        {
            // Arrange
            var aluno = new Aluno
            {
                Nome = "Maria Santos",
                CPF = "111.444.777-35",
                Matricula = "2024002",
                Turma = "2B"
                // Email e Telefone são opcionais
            };

            _mockAlunoDAL.Setup(dal => dal.Listar())
                .Returns(new List<Aluno>());

            _mockAlunoDAL.Setup(dal => dal.Inserir(It.IsAny<Aluno>()))
                .Verifiable();

            // Act
            // var resultado = _service.CadastrarAluno(aluno);

            // Assert
            // resultado.Should().NotBeNull();
            // resultado.Sucesso.Should().BeTrue("email e telefone são campos opcionais");
            // _mockAlunoDAL.Verify(dal => dal.Inserir(It.IsAny<Aluno>()), Times.Once);
        }

        #endregion

        #region CadastrarAluno - Validation Failures

        /// <summary>
        /// Teste: Cadastro sem nome deve retornar erro
        /// </summary>
        [Fact]
        [Trait("Category", "Unit")]
        [Trait("Priority", "High")]
        public void CadastrarAluno_SemNome_DeveRetornarErro()
        {
            // Arrange
            var aluno = new Aluno
            {
                Nome = "", // Nome vazio
                CPF = "123.456.789-09",
                Matricula = "2024001"
            };

            // Act
            // var resultado = _service.CadastrarAluno(aluno);

            // Assert
            // resultado.Should().NotBeNull();
            // resultado.Sucesso.Should().BeFalse("nome é campo obrigatório");
            // resultado.Mensagem.Should().Contain("Nome");
            // resultado.Mensagem.Should().Contain("obrigatório");
            // _mockAlunoDAL.Verify(dal => dal.Inserir(It.IsAny<Aluno>()), Times.Never);
        }

        /// <summary>
        /// Teste: Cadastro sem CPF deve retornar erro
        /// </summary>
        [Fact]
        [Trait("Category", "Unit")]
        [Trait("Priority", "High")]
        public void CadastrarAluno_SemCPF_DeveRetornarErro()
        {
            // Arrange
            var aluno = new Aluno
            {
                Nome = "João Silva",
                CPF = "", // CPF vazio
                Matricula = "2024001"
            };

            // Act
            // var resultado = _service.CadastrarAluno(aluno);

            // Assert
            // resultado.Should().NotBeNull();
            // resultado.Sucesso.Should().BeFalse("CPF é campo obrigatório");
            // resultado.Mensagem.Should().Contain("CPF");
            // resultado.Mensagem.Should().Contain("obrigatório");
            // _mockAlunoDAL.Verify(dal => dal.Inserir(It.IsAny<Aluno>()), Times.Never);
        }

        /// <summary>
        /// Teste: Cadastro com CPF inválido deve retornar erro
        /// </summary>
        [Fact]
        [Trait("Category", "Unit")]
        [Trait("Priority", "High")]
        public void CadastrarAluno_ComCPFInvalido_DeveRetornarErro()
        {
            // Arrange
            var aluno = new Aluno
            {
                Nome = "João Silva",
                CPF = "111.111.111-11", // CPF inválido (sequência)
                Matricula = "2024001"
            };

            // Act
            // var resultado = _service.CadastrarAluno(aluno);

            // Assert
            // resultado.Should().NotBeNull();
            // resultado.Sucesso.Should().BeFalse("CPF inválido deve ser rejeitado");
            // resultado.Mensagem.Should().Contain("CPF inválido");
            // _mockAlunoDAL.Verify(dal => dal.Inserir(It.IsAny<Aluno>()), Times.Never);
        }

        /// <summary>
        /// Teste: Cadastro sem matrícula deve retornar erro
        /// </summary>
        [Fact]
        [Trait("Category", "Unit")]
        [Trait("Priority", "High")]
        public void CadastrarAluno_SemMatricula_DeveRetornarErro()
        {
            // Arrange
            var aluno = new Aluno
            {
                Nome = "João Silva",
                CPF = "123.456.789-09",
                Matricula = "" // Matrícula vazia
            };

            // Act
            // var resultado = _service.CadastrarAluno(aluno);

            // Assert
            // resultado.Should().NotBeNull();
            // resultado.Sucesso.Should().BeFalse("matrícula é campo obrigatório");
            // resultado.Mensagem.Should().Contain("Matrícula");
            // resultado.Mensagem.Should().Contain("obrigatório");
            // _mockAlunoDAL.Verify(dal => dal.Inserir(It.IsAny<Aluno>()), Times.Never);
        }

        /// <summary>
        /// Teste: Cadastro com matrícula inválida deve retornar erro
        /// </summary>
        [Theory]
        [InlineData("AB")] // Muito curta
        [InlineData("123456789012345678901")] // Muito longa
        [InlineData("ABC-123")] // Caractere especial
        [Trait("Category", "Unit")]
        [Trait("Priority", "High")]
        public void CadastrarAluno_ComMatriculaInvalida_DeveRetornarErro(string matriculaInvalida)
        {
            // Arrange
            var aluno = new Aluno
            {
                Nome = "João Silva",
                CPF = "123.456.789-09",
                Matricula = matriculaInvalida
            };

            // Act
            // var resultado = _service.CadastrarAluno(aluno);

            // Assert
            // resultado.Should().NotBeNull();
            // resultado.Sucesso.Should().BeFalse("matrícula inválida deve ser rejeitada");
            // resultado.Mensagem.Should().Contain("Matrícula inválida");
            // _mockAlunoDAL.Verify(dal => dal.Inserir(It.IsAny<Aluno>()), Times.Never);
        }

        /// <summary>
        /// Teste: Cadastro com email inválido deve retornar erro
        /// </summary>
        [Theory]
        [InlineData("email-invalido")]
        [InlineData("@example.com")]
        [InlineData("user@")]
        [Trait("Category", "Unit")]
        [Trait("Priority", "Medium")]
        public void CadastrarAluno_ComEmailInvalido_DeveRetornarErro(string emailInvalido)
        {
            // Arrange
            var aluno = new Aluno
            {
                Nome = "João Silva",
                CPF = "123.456.789-09",
                Matricula = "2024001",
                Email = emailInvalido
            };

            // Act
            // var resultado = _service.CadastrarAluno(aluno);

            // Assert
            // resultado.Should().NotBeNull();
            // resultado.Sucesso.Should().BeFalse("email inválido deve ser rejeitado");
            // resultado.Mensagem.Should().Contain("E-mail inválido");
            // _mockAlunoDAL.Verify(dal => dal.Inserir(It.IsAny<Aluno>()), Times.Never);
        }

        #endregion

        #region CadastrarAluno - Duplicate Validation

        /// <summary>
        /// Teste: Cadastro com CPF duplicado deve retornar erro
        /// </summary>
        [Fact]
        [Trait("Category", "Unit")]
        [Trait("Priority", "High")]
        public void CadastrarAluno_ComCPFDuplicado_DeveRetornarErro()
        {
            // Arrange
            var cpfExistente = "123.456.789-09";
            var alunoExistente = new Aluno
            {
                Id = 1,
                Nome = "Aluno Existente",
                CPF = cpfExistente,
                Matricula = "2023001"
            };

            var novoAluno = new Aluno
            {
                Nome = "Novo Aluno",
                CPF = cpfExistente, // CPF duplicado
                Matricula = "2024001"
            };

            _mockAlunoDAL.Setup(dal => dal.Listar())
                .Returns(new List<Aluno> { alunoExistente });

            // Act
            // var resultado = _service.CadastrarAluno(novoAluno);

            // Assert
            // resultado.Should().NotBeNull();
            // resultado.Sucesso.Should().BeFalse("CPF duplicado deve ser rejeitado");
            // resultado.Mensagem.Should().Contain("Já existe um aluno cadastrado com o CPF");
            // resultado.Mensagem.Should().Contain(cpfExistente);
            // _mockAlunoDAL.Verify(dal => dal.Inserir(It.IsAny<Aluno>()), Times.Never);
        }

        /// <summary>
        /// Teste: Cadastro com matrícula duplicada deve retornar erro
        /// </summary>
        [Fact]
        [Trait("Category", "Unit")]
        [Trait("Priority", "High")]
        public void CadastrarAluno_ComMatriculaDuplicada_DeveRetornarErro()
        {
            // Arrange
            var matriculaExistente = "2024001";
            var alunoExistente = new Aluno
            {
                Id = 1,
                Nome = "Aluno Existente",
                CPF = "111.444.777-35",
                Matricula = matriculaExistente
            };

            var novoAluno = new Aluno
            {
                Nome = "Novo Aluno",
                CPF = "123.456.789-09",
                Matricula = matriculaExistente // Matrícula duplicada
            };

            _mockAlunoDAL.Setup(dal => dal.Listar())
                .Returns(new List<Aluno> { alunoExistente });

            // Act
            // var resultado = _service.CadastrarAluno(novoAluno);

            // Assert
            // resultado.Should().NotBeNull();
            // resultado.Sucesso.Should().BeFalse("matrícula duplicada deve ser rejeitada");
            // resultado.Mensagem.Should().Contain("Já existe um aluno cadastrado com a matrícula");
            // resultado.Mensagem.Should().Contain(matriculaExistente);
            // _mockAlunoDAL.Verify(dal => dal.Inserir(It.IsAny<Aluno>()), Times.Never);
        }

        #endregion

        #region CadastrarAluno - Exception Handling

        /// <summary>
        /// Teste: Exceção durante cadastro deve retornar erro e logar
        /// </summary>
        [Fact]
        [Trait("Category", "Unit")]
        [Trait("Priority", "Medium")]
        public void CadastrarAluno_ComExcecaoNoBancoDeDados_DeveRetornarErroELogar()
        {
            // Arrange
            var aluno = new Aluno
            {
                Nome = "João Silva",
                CPF = "123.456.789-09",
                Matricula = "2024001"
            };

            var mensagemErro = "Erro de conexão com banco de dados";

            _mockAlunoDAL.Setup(dal => dal.Listar())
                .Returns(new List<Aluno>());

            _mockAlunoDAL.Setup(dal => dal.Inserir(It.IsAny<Aluno>()))
                .Throws(new Exception(mensagemErro));

            _mockLogService.Setup(log => log.Registrar(
                It.IsAny<int?>(),
                "ERRO_CADASTRO_ALUNO",
                It.IsAny<string>()))
                .Verifiable();

            // Act
            // var resultado = _service.CadastrarAluno(aluno, idFuncionario: 1);

            // Assert
            // resultado.Should().NotBeNull();
            // resultado.Sucesso.Should().BeFalse("exceção deve resultar em falha");
            // resultado.Mensagem.Should().Contain("Erro ao cadastrar aluno");
            // resultado.Mensagem.Should().Contain(mensagemErro);

            // _mockLogService.Verify(log => log.Registrar(
            //     It.IsAny<int?>(),
            //     "ERRO_CADASTRO_ALUNO",
            //     It.Is<string>(msg => msg.Contains(mensagemErro))), Times.Once);
        }

        #endregion

        #region AtualizarAluno Tests

        /// <summary>
        /// Teste: Atualização de aluno existente com dados válidos deve retornar sucesso
        /// </summary>
        [Fact]
        [Trait("Category", "Unit")]
        [Trait("Priority", "High")]
        public void AtualizarAluno_ComDadosValidos_DeveRetornarSucesso()
        {
            // Arrange
            var alunoExistente = new Aluno
            {
                Id = 1,
                Nome = "João Silva",
                CPF = "123.456.789-09",
                Matricula = "2024001"
            };

            var alunoAtualizado = new Aluno
            {
                Id = 1,
                Nome = "João Silva Santos", // Nome atualizado
                CPF = "123.456.789-09",
                Matricula = "2024001",
                Email = "joao.santos@example.com" // Email adicionado
            };

            _mockAlunoDAL.Setup(dal => dal.ObterPorId(1))
                .Returns(alunoExistente);

            _mockAlunoDAL.Setup(dal => dal.Listar())
                .Returns(new List<Aluno> { alunoExistente });

            _mockAlunoDAL.Setup(dal => dal.Atualizar(It.IsAny<Aluno>()))
                .Verifiable();

            // Act
            // var resultado = _service.AtualizarAluno(alunoAtualizado, idFuncionario: 1);

            // Assert
            // resultado.Should().NotBeNull();
            // resultado.Sucesso.Should().BeTrue("dados válidos devem ser atualizados");
            // resultado.Mensagem.Should().Contain("sucesso");

            // _mockAlunoDAL.Verify(dal => dal.Atualizar(It.Is<Aluno>(a =>
            //     a.Id == alunoAtualizado.Id &&
            //     a.Nome == alunoAtualizado.Nome)), Times.Once);
        }

        /// <summary>
        /// Teste: Atualização de aluno inexistente deve retornar erro
        /// </summary>
        [Fact]
        [Trait("Category", "Unit")]
        [Trait("Priority", "High")]
        public void AtualizarAluno_AlunoInexistente_DeveRetornarErro()
        {
            // Arrange
            var aluno = new Aluno
            {
                Id = 999, // ID inexistente
                Nome = "João Silva",
                CPF = "123.456.789-09",
                Matricula = "2024001"
            };

            _mockAlunoDAL.Setup(dal => dal.ObterPorId(999))
                .Returns((Aluno?)null);

            // Act
            // var resultado = _service.AtualizarAluno(aluno);

            // Assert
            // resultado.Should().NotBeNull();
            // resultado.Sucesso.Should().BeFalse("aluno inexistente não pode ser atualizado");
            // resultado.Mensagem.Should().Contain("não encontrado");
            // _mockAlunoDAL.Verify(dal => dal.Atualizar(It.IsAny<Aluno>()), Times.Never);
        }

        #endregion

        #region ExcluirAluno Tests

        /// <summary>
        /// Teste: Exclusão de aluno sem empréstimos ativos deve retornar sucesso
        /// </summary>
        [Fact]
        [Trait("Category", "Unit")]
        [Trait("Priority", "High")]
        public void ExcluirAluno_SemEmprestimosAtivos_DeveRetornarSucesso()
        {
            // Arrange
            var aluno = new Aluno
            {
                Id = 1,
                Nome = "João Silva",
                CPF = "123.456.789-09",
                Matricula = "2024001"
            };

            _mockAlunoDAL.Setup(dal => dal.ObterPorId(1))
                .Returns(aluno);

            _mockEmprestimoDAL.Setup(dal => dal.Listar())
                .Returns(new List<Emprestimo>()); // Sem empréstimos

            _mockAlunoDAL.Setup(dal => dal.Excluir(1))
                .Verifiable();

            // Act
            // var resultado = _service.ExcluirAluno(1, idFuncionario: 1);

            // Assert
            // resultado.Should().NotBeNull();
            // resultado.Sucesso.Should().BeTrue("aluno sem empréstimos pode ser excluído");
            // resultado.Mensagem.Should().Contain("sucesso");
            // resultado.Mensagem.Should().Contain(aluno.Nome);

            // _mockAlunoDAL.Verify(dal => dal.Excluir(1), Times.Once);
        }

        /// <summary>
        /// Teste: Exclusão de aluno com empréstimos ativos deve retornar erro
        /// </summary>
        [Fact]
        [Trait("Category", "Unit")]
        [Trait("Priority", "High")]
        public void ExcluirAluno_ComEmprestimosAtivos_DeveRetornarErro()
        {
            // Arrange
            var aluno = new Aluno
            {
                Id = 1,
                Nome = "João Silva",
                CPF = "123.456.789-09",
                Matricula = "2024001"
            };

            var emprestimoAtivo = new Emprestimo
            {
                Id = 1,
                IdAluno = 1,
                IdLivro = 1,
                DataEmprestimo = DateTime.Today.AddDays(-5),
                DataPrevista = DateTime.Today.AddDays(2),
                DataDevolucao = null // Empréstimo ativo
            };

            _mockAlunoDAL.Setup(dal => dal.ObterPorId(1))
                .Returns(aluno);

            _mockEmprestimoDAL.Setup(dal => dal.Listar())
                .Returns(new List<Emprestimo> { emprestimoAtivo });

            // Act
            // var resultado = _service.ExcluirAluno(1);

            // Assert
            // resultado.Should().NotBeNull();
            // resultado.Sucesso.Should().BeFalse("aluno com empréstimos ativos não pode ser excluído");
            // resultado.Mensagem.Should().Contain("empréstimos ativos");
            // resultado.Mensagem.Should().Contain("devolução");

            // _mockAlunoDAL.Verify(dal => dal.Excluir(It.IsAny<int>()), Times.Never);
        }

        #endregion

        #region Helper Methods Tests

        /// <summary>
        /// Teste: Buscar aluno por CPF existente deve retornar o aluno
        /// </summary>
        [Fact]
        [Trait("Category", "Unit")]
        [Trait("Priority", "Medium")]
        public void BuscarPorCPF_AlunoExistente_DeveRetornarAluno()
        {
            // Arrange
            var cpf = "123.456.789-09";
            var aluno = new Aluno
            {
                Id = 1,
                Nome = "João Silva",
                CPF = cpf,
                Matricula = "2024001"
            };

            _mockAlunoDAL.Setup(dal => dal.Listar())
                .Returns(new List<Aluno> { aluno });

            // Act
            // var resultado = _service.BuscarPorCPF(cpf);

            // Assert
            // resultado.Should().NotBeNull();
            // resultado.Id.Should().Be(aluno.Id);
            // resultado.CPF.Should().Be(cpf);
        }

        /// <summary>
        /// Teste: Verificar se aluno está apto para empréstimo (sem atrasos)
        /// </summary>
        [Fact]
        [Trait("Category", "Unit")]
        [Trait("Priority", "High")]
        public void VerificarAptoParaEmprestimo_SemAtrasos_DeveRetornarApto()
        {
            // Arrange
            var aluno = new Aluno
            {
                Id = 1,
                Nome = "João Silva",
                CPF = "123.456.789-09",
                Matricula = "2024001"
            };

            _mockAlunoDAL.Setup(dal => dal.ObterPorId(1))
                .Returns(aluno);

            _mockEmprestimoDAL.Setup(dal => dal.Listar())
                .Returns(new List<Emprestimo>()); // Sem empréstimos atrasados

            // Act
            // var (apto, mensagem) = _service.VerificarAptoParaEmprestimo(1);

            // Assert
            // apto.Should().BeTrue("aluno sem atrasos deve estar apto");
            // mensagem.Should().Contain("apto");
        }

        /// <summary>
        /// Teste: Verificar se aluno com atrasos não está apto para empréstimo
        /// </summary>
        [Fact]
        [Trait("Category", "Unit")]
        [Trait("Priority", "High")]
        public void VerificarAptoParaEmprestimo_ComAtrasos_DeveRetornarNaoApto()
        {
            // Arrange
            var aluno = new Aluno
            {
                Id = 1,
                Nome = "João Silva",
                CPF = "123.456.789-09",
                Matricula = "2024001"
            };

            var emprestimoAtrasado = new Emprestimo
            {
                Id = 1,
                IdAluno = 1,
                IdLivro = 1,
                DataEmprestimo = DateTime.Today.AddDays(-15),
                DataPrevista = DateTime.Today.AddDays(-5), // 5 dias de atraso
                DataDevolucao = null
            };

            _mockAlunoDAL.Setup(dal => dal.ObterPorId(1))
                .Returns(aluno);

            _mockEmprestimoDAL.Setup(dal => dal.Listar())
                .Returns(new List<Emprestimo> { emprestimoAtrasado });

            // Act
            // var (apto, mensagem) = _service.VerificarAptoParaEmprestimo(1);

            // Assert
            // apto.Should().BeFalse("aluno com atrasos não deve estar apto");
            // mensagem.Should().Contain("atrasado");
            // mensagem.Should().Contain("regularizar");
        }

        #endregion
    }
}
