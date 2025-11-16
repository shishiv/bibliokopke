# Change Proposal: CI/CD Pipeline and Automated Testing

**Status:** Draft
**Created:** 2025-11-16
**Author:** AI Assistant
**Change ID:** 002-ci-cd-automated-testing
**Depends On:** 001-bibliokopke-build-review

## Overview

Implement comprehensive automated testing and CI/CD pipeline for BiblioKopke to ensure code quality, prevent regressions, and automate the build/release process.

## Current State

### Existing Infrastructure
- **Version Control:** Git + GitHub
- **Build System:** .NET 8.0 SDK with dotnet CLI
- **Testing:** None implemented
- **CI/CD:** None implemented
- **Code Quality:** Manual review only

### Pain Points
1. No automated testing - bugs only caught in production
2. No CI/CD - manual build and deployment process
3. No quality gates - compilation errors reach repository
4. No test coverage metrics
5. No automated release process

## Proposed Changes

### Task 1: Setup Testing Infrastructure

#### 1.1 Create Test Projects
Create three test projects following .NET best practices:

```
06_bibliotecaJK.Tests/
├── BibliotecaJK.Tests.csproj          # Main test project
├── Unit/                               # Unit tests
│   ├── BLL/
│   │   ├── AlunoServiceTests.cs
│   │   ├── EmprestimoServiceTests.cs
│   │   ├── LivroServiceTests.cs
│   │   ├── ReservaServiceTests.cs
│   │   └── ValidadoresTests.cs
│   ├── DAL/
│   │   ├── AlunoDALTests.cs
│   │   ├── EmprestimoDALTests.cs
│   │   └── LivroDALTests.cs
│   └── Model/
│       └── ValidationTests.cs
├── Integration/                        # Integration tests
│   ├── DatabaseTests.cs
│   ├── EndToEndWorkflowTests.cs
│   └── ConnectionTests.cs
└── TestHelpers/
    ├── MockDataGenerator.cs
    ├── TestDatabaseFixture.cs
    └── InMemoryDatabase.cs
```

#### 1.2 Testing Frameworks & Libraries
- **xUnit** - Primary testing framework (industry standard)
- **Moq** - Mocking framework for isolating units
- **FluentAssertions** - Readable assertions
- **Testcontainers** - Docker containers for PostgreSQL integration tests
- **Bogus** - Fake data generation
- **Coverlet** - Code coverage

**Dependencies to Add:**
```xml
<ItemGroup>
  <PackageReference Include="xunit" Version="2.6.2" />
  <PackageReference Include="xunit.runner.visualstudio" Version="2.5.4" />
  <PackageReference Include="Moq" Version="4.20.70" />
  <PackageReference Include="FluentAssertions" Version="6.12.0" />
  <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.0" />
  <PackageReference Include="Testcontainers.PostgreSql" Version="3.6.0" />
  <PackageReference Include="Bogus" Version="35.0.1" />
  <PackageReference Include="coverlet.collector" Version="6.0.0" />
</ItemGroup>
```

### Task 2: Implement Unit Tests

#### 2.1 BLL Layer Tests
Test all business logic services with 80%+ coverage:

**AlunoServiceTests.cs** - Example structure:
```csharp
public class AlunoServiceTests
{
    [Fact]
    public void CadastrarAluno_ComDadosValidos_DeveRetornarSucesso()
    {
        // Arrange
        var mockAlunoDAL = new Mock<AlunoDAL>();
        var service = new AlunoService(mockAlunoDAL.Object);
        var aluno = new Aluno { Nome = "João", CPF = "12345678901" };

        // Act
        var resultado = service.CadastrarAluno(aluno);

        // Assert
        resultado.Sucesso.Should().BeTrue();
        mockAlunoDAL.Verify(dal => dal.Inserir(It.IsAny<Aluno>()), Times.Once);
    }

    [Fact]
    public void CadastrarAluno_ComCPFInvalido_DeveRetornarErro()
    {
        // Arrange, Act, Assert...
    }
}
```

**ValidadoresTests.cs** - Critical validation tests:
```csharp
public class ValidadoresTests
{
    [Theory]
    [InlineData("123.456.789-09", true)]
    [InlineData("111.111.111-11", false)]
    [InlineData("000.000.000-00", false)]
    [InlineData("12345678901", false)]
    public void ValidarCPF_DeveValidarCorretamente(string cpf, bool esperado)
    {
        var resultado = Validadores.ValidarCPF(cpf);
        resultado.Should().Be(esperado);
    }

    [Theory]
    [InlineData("978-0-13-110362-7", true)]
    [InlineData("978-0-00-000000-0", false)]
    public void ValidarISBN_DeveValidarCorretamente(string isbn, bool esperado)
    {
        var resultado = Validadores.ValidarISBN13(isbn);
        resultado.Should().Be(esperado);
    }
}
```

**EmprestimoServiceTests.cs** - Core business rules:
```csharp
public class EmprestimoServiceTests
{
    [Fact]
    public void RegistrarEmprestimo_AlunoComMaisDe3Emprestimos_DeveRetornarErro()
    {
        // Test MAX_EMPRESTIMOS_SIMULTANEOS constraint
    }

    [Fact]
    public void CalcularMulta_LivroAtrasado5Dias_DeveRetornar10Reais()
    {
        // Test MULTA_POR_DIA calculation (5 days * R$ 2.00 = R$ 10.00)
    }

    [Fact]
    public void ProcessarDevolucao_ComReservaAtiva_DeveNotificarProximoNaFila()
    {
        // Test reservation queue processing
    }
}
```

#### 2.2 DAL Layer Tests (with Test Containers)
```csharp
public class AlunoDALTests : IClassFixture<PostgreSqlContainerFixture>
{
    private readonly PostgreSqlContainerFixture _fixture;

    public AlunoDALTests(PostgreSqlContainerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Inserir_NovoAluno_DeveInserirNoBanco()
    {
        // Arrange
        var dal = new AlunoDAL();
        var aluno = new Aluno { Nome = "Test", CPF = "12345678901" };

        // Act
        dal.Inserir(aluno);
        var resultado = dal.ObterPorCPF("12345678901");

        // Assert
        resultado.Should().NotBeNull();
        resultado.Nome.Should().Be("Test");
    }
}
```

### Task 3: Implement Integration Tests

#### 3.1 End-to-End Workflow Tests
```csharp
public class EndToEndWorkflowTests : IClassFixture<DatabaseFixture>
{
    [Fact]
    public void FluxoCompleto_EmprestimoComDevolucaoAtrasada_CalculaMultaCorretamente()
    {
        // 1. Cadastrar aluno
        // 2. Cadastrar livro
        // 3. Registrar empréstimo
        // 4. Simular atraso
        // 5. Processar devolução
        // 6. Verificar multa calculada
    }

    [Fact]
    public void FluxoReserva_LivroIndisponivelComReserva_NotificaQuandoDisponivel()
    {
        // Test complete reservation workflow
    }
}
```

### Task 4: CI/CD Pipeline with GitHub Actions

#### 4.1 Build and Test Workflow
Create `.github/workflows/build-and-test.yml`:

```yaml
name: Build and Test

on:
  push:
    branches: [ main, develop, 'claude/**' ]
  pull_request:
    branches: [ main, develop ]

jobs:
  build-and-test:
    runs-on: windows-latest

    services:
      postgres:
        image: postgres:15
        env:
          POSTGRES_PASSWORD: postgres
          POSTGRES_DB: bibliokopke_test
        options: >-
          --health-cmd pg_isready
          --health-interval 10s
          --health-timeout 5s
          --health-retries 5
        ports:
          - 5432:5432

    steps:
    - name: Checkout code
      uses: actions/checkout@v4

    - name: Setup .NET 8.0
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: '8.0.x'

    - name: Restore dependencies
      run: |
        dotnet restore 06_bibliotecaJK/BibliotecaJK.csproj
        dotnet restore 06_bibliotecaJK.Tests/BibliotecaJK.Tests.csproj

    - name: Build solution
      run: dotnet build 06_bibliotecaJK/BibliotecaJK.csproj --configuration Release --no-restore

    - name: Run unit tests
      run: dotnet test 06_bibliotecaJK.Tests/BibliotecaJK.Tests.csproj --configuration Release --no-build --filter "Category=Unit" --verbosity normal

    - name: Run integration tests
      run: dotnet test 06_bibliotecaJK.Tests/BibliotecaJK.Tests.csproj --configuration Release --no-build --filter "Category=Integration" --verbosity normal
      env:
        ConnectionStrings__DefaultConnection: "Host=localhost;Port=5432;Database=bibliokopke_test;Username=postgres;Password=postgres"

    - name: Generate code coverage
      run: dotnet test 06_bibliotecaJK.Tests/BibliotecaJK.Tests.csproj --configuration Release --no-build --collect:"XPlat Code Coverage" --results-directory ./coverage

    - name: Upload coverage to Codecov
      uses: codecov/codecov-action@v3
      with:
        directory: ./coverage
        flags: unittests
        fail_ci_if_error: true

    - name: Check code coverage threshold
      run: |
        dotnet tool install --global dotnet-reportgenerator-globaltool
        reportgenerator -reports:./coverage/**/coverage.cobertura.xml -targetdir:./coveragereport -reporttypes:TextSummary
        $coverage = Get-Content ./coveragereport/Summary.txt | Select-String "Line coverage:" | ForEach-Object { ($_ -split ":")[1].Trim().TrimEnd('%') }
        if ([double]$coverage -lt 70) {
          Write-Error "Code coverage ($coverage%) is below 70% threshold"
          exit 1
        }
```

#### 4.2 Code Quality Workflow
Create `.github/workflows/code-quality.yml`:

```yaml
name: Code Quality

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main, develop ]

jobs:
  analyze:
    runs-on: windows-latest

    steps:
    - name: Checkout code
      uses: actions/checkout@v4

    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: '8.0.x'

    - name: Restore dependencies
      run: dotnet restore 06_bibliotecaJK/BibliotecaJK.csproj

    - name: Run dotnet format
      run: dotnet format 06_bibliotecaJK/BibliotecaJK.csproj --verify-no-changes --verbosity diagnostic

    - name: Run security analysis
      run: dotnet list package --vulnerable --include-transitive

    - name: Check for nullable warnings
      run: dotnet build 06_bibliotecaJK/BibliotecaJK.csproj -warnaserror:CS8600,CS8601,CS8602,CS8603,CS8604,CS8618
```

#### 4.3 Release Workflow
Create `.github/workflows/release.yml`:

```yaml
name: Release

on:
  push:
    tags:
      - 'v*'

jobs:
  release:
    runs-on: windows-latest

    steps:
    - name: Checkout code
      uses: actions/checkout@v4

    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: '8.0.x'

    - name: Publish Windows x64
      run: dotnet publish 06_bibliotecaJK/BibliotecaJK.csproj -c Release -r win-x64 --self-contained false -o ./publish/win-x64

    - name: Zip release
      run: Compress-Archive -Path ./publish/win-x64/* -DestinationPath ./BiblioKopke-${{ github.ref_name }}-win-x64.zip

    - name: Create GitHub Release
      uses: softprops/action-gh-release@v1
      with:
        files: BiblioKopke-${{ github.ref_name }}-win-x64.zip
        generate_release_notes: true
      env:
        GITHUB_TOKEN: ${{ secrets.GITHUB_TOKEN }}
```

### Task 5: Pre-commit Hooks

Create `.husky/pre-commit`:
```bash
#!/bin/sh
. "$(dirname "$0")/_/husky.sh"

# Run dotnet format
dotnet format 06_bibliotecaJK/BibliotecaJK.csproj --verify-no-changes

# Run unit tests (fast only)
dotnet test 06_bibliotecaJK.Tests/BibliotecaJK.Tests.csproj --filter "Category=Unit&Speed=Fast" --no-build
```

### Task 6: Test Data Helpers

Create `06_bibliotecaJK.Tests/TestHelpers/MockDataGenerator.cs`:
```csharp
public static class MockDataGenerator
{
    public static Faker<Aluno> AlunoFaker => new Faker<Aluno>("pt_BR")
        .RuleFor(a => a.Nome, f => f.Name.FullName())
        .RuleFor(a => a.CPF, f => GerarCPFValido())
        .RuleFor(a => a.Matricula, f => f.Random.Int(1000, 9999).ToString())
        .RuleFor(a => a.Turma, f => f.PickRandom("6A", "7B", "8C", "9D"))
        .RuleFor(a => a.Email, (f, a) => f.Internet.Email(a.Nome));

    public static Faker<Livro> LivroFaker => new Faker<Livro>("pt_BR")
        .RuleFor(l => l.Titulo, f => f.Lorem.Sentence(3, 5))
        .RuleFor(l => l.Autor, f => f.Name.FullName())
        .RuleFor(l => l.ISBN, f => GerarISBNValido())
        .RuleFor(l => l.QuantidadeTotal, f => f.Random.Int(1, 5))
        .RuleFor(l => l.QuantidadeDisponivel, (f, l) => l.QuantidadeTotal);

    private static string GerarCPFValido() { /* ... */ }
    private static string GerarISBNValido() { /* ... */ }
}
```

## Success Criteria

- [ ] Test project created with all dependencies
- [ ] Unit tests implemented for BLL layer (80%+ coverage)
- [ ] Integration tests implemented for critical workflows
- [ ] GitHub Actions workflows created and functional
- [ ] All tests passing in CI pipeline
- [ ] Code coverage above 70% threshold
- [ ] Pre-commit hooks configured
- [ ] Documentation updated with testing guidelines

## Implementation Plan

### Phase 1: Test Infrastructure (Week 1)
1. Create test project structure
2. Add testing framework dependencies
3. Setup Testcontainers for PostgreSQL
4. Create mock data generators
5. Implement test fixtures

### Phase 2: Unit Tests (Week 2)
1. Implement Validadores tests (100% coverage target)
2. Implement BLL service tests (80% coverage target)
3. Implement DAL tests with test containers
4. Verify all tests pass locally

### Phase 3: Integration Tests (Week 3)
1. Implement end-to-end workflow tests
2. Implement database integration tests
3. Test reservation queue processing
4. Test multa calculation workflows

### Phase 4: CI/CD Setup (Week 4)
1. Create GitHub Actions workflows
2. Configure PostgreSQL service container
3. Setup code coverage reporting
4. Configure quality gates
5. Test full pipeline

### Phase 5: Pre-commit Hooks (Week 5)
1. Install Husky
2. Configure pre-commit hooks
3. Test hook execution
4. Document for team

## Benefits

### Immediate Benefits
- ✅ Catch bugs before they reach production
- ✅ Prevent compilation errors in repository
- ✅ Automated build verification
- ✅ Code quality enforcement

### Long-term Benefits
- 📈 Improved code quality over time
- 🚀 Faster development cycles
- 🛡️ Regression prevention
- 📊 Measurable test coverage metrics
- 🔄 Automated release process

## Dependencies

- .NET 8.0 SDK
- Docker Desktop (for Testcontainers)
- GitHub repository with Actions enabled
- PostgreSQL 15+ (for integration tests)

## Risks and Mitigation

| Risk | Impact | Mitigation |
|------|--------|-----------|
| Windows Forms hard to test | High | Focus on BLL/DAL layers, mock Forms dependencies |
| CI/CD costs on GitHub | Medium | Use free tier (2000 minutes/month), optimize workflows |
| Test maintenance overhead | Medium | Start with critical paths, expand gradually |
| Team learning curve | Low | Provide documentation and examples |

## Rollback Plan

If CI/CD causes issues:
1. Disable GitHub Actions workflows
2. Remove test project from solution
3. Continue manual testing
4. No impact on production code

## Testing Strategy Summary

### Unit Tests (Fast, Isolated)
- **Target:** BLL, Validadores, Model
- **Mocking:** DAL layer mocked
- **Coverage Goal:** 80%+
- **Execution Time:** < 30 seconds

### Integration Tests (Slower, Real DB)
- **Target:** DAL, end-to-end workflows
- **Database:** Testcontainers PostgreSQL
- **Coverage Goal:** Critical paths
- **Execution Time:** < 2 minutes

### Manual Tests (UI)
- **Target:** Windows Forms UI
- **When:** Before releases
- **Coverage:** User acceptance scenarios

## Documentation Requirements

- [ ] Testing guidelines in CONTRIBUTING.md
- [ ] CI/CD pipeline documentation
- [ ] How to run tests locally
- [ ] How to write new tests
- [ ] Mock data generation guide

## Related Changes

- **Depends on:** 001-bibliokopke-build-review (must build successfully first)
- **Enables:** Future changes with confidence (regression prevention)

## Notes

- Windows Forms UI testing is complex - focus on business logic
- Testcontainers requires Docker Desktop installed
- GitHub Actions free tier sufficient for this project size
- Code coverage threshold set to 70% (industry standard for line coverage)
- Integration tests run against isolated PostgreSQL container
