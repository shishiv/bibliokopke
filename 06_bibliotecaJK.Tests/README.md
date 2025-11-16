# BiblioKopke Tests

Projeto de testes automatizados para o sistema BiblioKopke.

## 📋 Estrutura

```
06_bibliotecaJK.Tests/
├── Unit/                           # Testes unitários (rápidos, isolados)
│   ├── BLL/                       # Testes de serviços de negócio
│   │   ├── ValidadoresTests.cs    # ✅ Implementado
│   │   ├── EmprestimoServiceTests.cs  # 🚧 Em desenvolvimento
│   │   ├── AlunoServiceTests.cs   # ⏳ Pendente
│   │   ├── LivroServiceTests.cs   # ⏳ Pendente
│   │   └── ReservaServiceTests.cs # ⏳ Pendente
│   ├── DAL/                       # Testes de acesso a dados
│   │   └── ...                    # ⏳ Pendente
│   └── Model/                     # Testes de modelos
│       └── ...                    # ⏳ Pendente
├── Integration/                    # Testes de integração (com BD)
│   ├── DatabaseTests.cs           # ⏳ Pendente
│   └── WorkflowTests.cs           # ⏳ Pendente
├── TestHelpers/                    # Helpers e utilities
│   ├── MockDataGenerator.cs       # ⏳ Pendente
│   └── TestDatabaseFixture.cs     # ⏳ Pendente
└── BibliotecaJK.Tests.csproj      # ✅ Configurado
```

## 🚀 Como Executar os Testes

### Pré-requisitos

- .NET 8.0 SDK instalado
- Docker Desktop (para testes de integração com PostgreSQL)

### Executar Todos os Testes

```bash
cd 06_bibliotecaJK.Tests
dotnet test
```

### Executar Apenas Testes Unitários (rápidos)

```bash
dotnet test --filter "Category=Unit"
```

### Executar com Cobertura de Código

```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Executar Testes em Watch Mode (desenvolvimento)

```bash
dotnet watch test
```

## 📊 Frameworks e Bibliotecas

| Biblioteca | Versão | Propósito |
|------------|--------|-----------|
| xUnit | 2.6.2 | Framework de testes principal |
| Moq | 4.20.70 | Mock de dependências |
| FluentAssertions | 6.12.0 | Assertions legíveis |
| Testcontainers | 3.6.0 | PostgreSQL em Docker |
| Bogus | 35.0.1 | Geração de dados fake |
| Coverlet | 6.0.0 | Cobertura de código |

## ✅ Convenções de Testes

### Nomenclatura

```csharp
[Fact]
public void MetodoSendoTestado_Cenario_ResultadoEsperado()
{
    // Arrange - Preparação
    // Act - Ação
    // Assert - Verificação
}
```

### Traits (Categorização)

```csharp
[Trait("Category", "Unit")]       // Unit ou Integration
[Trait("Priority", "High")]       // High, Medium, Low
[Trait("Speed", "Fast")]          // Fast ou Slow
```

### Assertions com FluentAssertions

```csharp
// ❌ Ruim
Assert.True(resultado == true);
Assert.Equal("esperado", resultado);

// ✅ Bom
resultado.Should().BeTrue();
resultado.Should().Be("esperado");
```

## 🎯 Metas de Cobertura

| Camada | Meta de Cobertura | Status Atual |
|--------|-------------------|--------------|
| BLL (Serviços) | 80%+ | 🚧 Em progresso |
| DAL (Dados) | 70%+ | ⏳ Não iniciado |
| Model | 60%+ | ⏳ Não iniciado |
| **Total** | **70%+** | ⏳ Não iniciado |

## 🧪 Exemplos de Testes

### Teste Unitário Simples

```csharp
[Fact]
[Trait("Category", "Unit")]
public void ValidarCPF_ComCPFValido_DeveRetornarTrue()
{
    // Arrange
    var cpf = "52998224725";

    // Act
    var resultado = Validadores.ValidarCPF(cpf);

    // Assert
    resultado.Should().BeTrue();
}
```

### Teste com Theory (múltiplos casos)

```csharp
[Theory]
[InlineData("123.456.789-09", true)]
[InlineData("000.000.000-00", false)]
[Trait("Category", "Unit")]
public void ValidarCPF_DeveValidarCorretamente(string cpf, bool esperado)
{
    var resultado = Validadores.ValidarCPF(cpf);
    resultado.Should().Be(esperado);
}
```

### Teste com Mock

```csharp
[Fact]
[Trait("Category", "Unit")]
public void CadastrarAluno_DeveInserirNoBanco()
{
    // Arrange
    var mockDAL = new Mock<AlunoDAL>();
    var service = new AlunoService(mockDAL.Object);
    var aluno = new Aluno { Nome = "João" };

    // Act
    service.CadastrarAluno(aluno);

    // Assert
    mockDAL.Verify(dal => dal.Inserir(It.IsAny<Aluno>()), Times.Once);
}
```

## 🐳 Testes de Integração com Docker

Os testes de integração usam Testcontainers para criar um PostgreSQL isolado:

```csharp
public class DatabaseTests : IClassFixture<PostgreSqlContainerFixture>
{
    private readonly PostgreSqlContainerFixture _fixture;

    public DatabaseTests(PostgreSqlContainerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task DeveConectarAoBanco()
    {
        using var conn = _fixture.GetConnection();
        await conn.OpenAsync();
        conn.State.Should().Be(ConnectionState.Open);
    }
}
```

## 🔍 Executar Testes no CI/CD

Os testes são executados automaticamente no GitHub Actions:

- **Build and Test:** Executado em todo push e PR
- **Code Quality:** Verifica formatação e vulnerabilidades
- **Coverage:** Gera relatório de cobertura

Ver: `.github/workflows/build-and-test.yml`

## 📚 Recursos

- [xUnit Documentation](https://xunit.net/)
- [Moq Quickstart](https://github.com/moq/moq4/wiki/Quickstart)
- [FluentAssertions](https://fluentassertions.com/)
- [Testcontainers](https://dotnet.testcontainers.org/)

## 🤝 Contribuindo

Ao adicionar novos testes:

1. Siga a estrutura de pastas existente
2. Use AAA pattern (Arrange, Act, Assert)
3. Adicione traits apropriados
4. Mantenha testes rápidos e isolados
5. Execute todos os testes antes de commitar

## ⚠️ Status do Projeto

**Este projeto de testes está em desenvolvimento inicial.**

- ✅ Estrutura criada
- ✅ Dependências configuradas
- ✅ Exemplos de testes implementados
- 🚧 Cobertura em progresso
- ⏳ Testes de integração pendentes

Ver `openspec/changes/002-ci-cd-automated-testing.md` para detalhes do plano completo.
