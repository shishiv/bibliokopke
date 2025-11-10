# 🤝 Contribuindo para o BiblioKopke

Obrigado por considerar contribuir para o BiblioKopke! Este documento fornece diretrizes para contribuir com o projeto.

---

## 📋 Índice

1. [Código de Conduta](#código-de-conduta)
2. [Como Contribuir](#como-contribuir)
3. [Padrões de Código](#padrões-de-código)
4. [Processo de Pull Request](#processo-de-pull-request)
5. [Estrutura do Projeto](#estrutura-do-projeto)
6. [Boas Práticas](#boas-práticas)

---

## 📜 Código de Conduta

Este projeto segue um código de conduta baseado em respeito mútuo:

- Seja respeitoso e construtivo
- Aceite críticas construtivas
- Foque no que é melhor para a comunidade
- Mostre empatia com outros membros

---

## 🚀 Como Contribuir

### 1. Reportar Bugs

Se encontrar um bug, crie uma issue com:

- **Título claro e descritivo**
- **Passos para reproduzir o problema**
- **Comportamento esperado vs. comportamento atual**
- **Screenshots** (se aplicável)
- **Versão do Windows e .NET**

### 2. Sugerir Melhorias

Para sugerir novas funcionalidades:

- Verifique se já não existe uma issue similar
- Descreva claramente a funcionalidade
- Explique por que seria útil
- Forneça exemplos de uso

### 3. Contribuir com Código

1. **Fork o repositório**
2. **Clone seu fork**:
   ```bash
   git clone https://github.com/seu-usuario/bibliokopke.git
   cd bibliokopke
   ```

3. **Crie uma branch**:
   ```bash
   git checkout -b feature/minha-feature
   # ou
   git checkout -b fix/meu-bug-fix
   ```

4. **Faça suas alterações**
5. **Teste suas alterações**
6. **Commit suas mudanças**:
   ```bash
   git commit -m "feat: adiciona nova funcionalidade X"
   ```

7. **Push para seu fork**:
   ```bash
   git push origin feature/minha-feature
   ```

8. **Abra um Pull Request**

---

## 💻 Padrões de Código

### C# / .NET

#### Nomenclatura

```csharp
// Classes: PascalCase
public class AlunoService { }

// Métodos: PascalCase
public void RegistrarEmprestimo() { }

// Propriedades: PascalCase
public string Nome { get; set; }

// Variáveis privadas: _camelCase
private readonly string _connectionString;

// Variáveis locais: camelCase
var livroAtual = new Livro();

// Constantes: UPPER_SNAKE_CASE
private const int MAX_EMPRESTIMOS = 3;
```

#### Estrutura de Arquivos

```csharp
// 1. Using statements (agrupados e ordenados)
using System;
using System.Collections.Generic;
using System.Linq;
using BibliotecaJK.Model;
using BibliotecaJK.DAL;

// 2. Namespace
namespace BibliotecaJK.BLL
{
    // 3. XML Documentation
    /// <summary>
    /// Descrição da classe
    /// </summary>
    public class MinhaClasse
    {
        // 4. Campos privados
        private readonly MyDAL _dal;

        // 5. Constantes
        private const int MAX_VALUE = 100;

        // 6. Construtor
        public MinhaClasse()
        {
            _dal = new MyDAL();
        }

        // 7. Propriedades
        public string Nome { get; set; }

        // 8. Métodos públicos
        public void MetodoPublico() { }

        // 9. Métodos privados
        private void MetodoPrivado() { }
    }
}
```

#### Boas Práticas C#

- ✅ Use `var` quando o tipo é óbvio
- ✅ Prefira `string.IsNullOrEmpty()` para validações
- ✅ Use `using` statements para recursos descartáveis
- ✅ Adicione XML documentation em métodos públicos
- ✅ Valide parâmetros de entrada
- ✅ Use constantes em vez de magic numbers
- ❌ Evite métodos muito longos (>50 linhas)
- ❌ Evite classes muito grandes (>500 linhas)

### SQL

```sql
-- Use UPPER CASE para palavras-chave SQL
SELECT nome, cpf, matricula
FROM Aluno
WHERE ativo = TRUE
ORDER BY nome;

-- Formate queries complexas
SELECT
    a.nome AS nome_aluno,
    l.titulo AS titulo_livro,
    e.data_emprestimo,
    e.data_prevista
FROM Emprestimo e
INNER JOIN Aluno a ON e.id_aluno = a.id_aluno
INNER JOIN Livro l ON e.id_livro = l.id_livro
WHERE e.status = 'ATIVO'
ORDER BY e.data_prevista;
```

### Commits

Siga o padrão **Conventional Commits**:

```
<tipo>(<escopo>): <descrição>

[corpo opcional]

[rodapé opcional]
```

#### Tipos de Commit

- `feat`: Nova funcionalidade
- `fix`: Correção de bug
- `docs`: Mudanças na documentação
- `style`: Formatação, indentação (sem mudança de código)
- `refactor`: Refatoração de código
- `test`: Adição ou correção de testes
- `chore`: Manutenção geral (build, configs)

#### Exemplos

```bash
# Bom
git commit -m "feat(emprestimo): adiciona validação de limite de livros"
git commit -m "fix(login): corrige validação de senha"
git commit -m "docs: atualiza README com instruções de instalação"
git commit -m "refactor(dal): extrai lógica de conexão para classe base"

# Ruim
git commit -m "mudanças"
git commit -m "fix bug"
git commit -m "WIP"
```

---

## 🔄 Processo de Pull Request

### Checklist Antes de Enviar

- [ ] Código compila sem erros
- [ ] Testes passam (se aplicável)
- [ ] Código segue os padrões do projeto
- [ ] Documentação atualizada (se necessário)
- [ ] Commits seguem o padrão Conventional Commits
- [ ] Branch está atualizada com `main`

### Template de Pull Request

```markdown
## Descrição
Breve descrição das mudanças

## Tipo de Mudança
- [ ] Bug fix
- [ ] Nova funcionalidade
- [ ] Breaking change
- [ ] Documentação

## Como Testar
1. Passo 1
2. Passo 2
3. Passo 3

## Screenshots (se aplicável)

## Checklist
- [ ] Código compila
- [ ] Testes passam
- [ ] Documentação atualizada
```

---

## 📁 Estrutura do Projeto

### Camadas do Sistema

```
08_c#/
├── Model/              # Entidades do domínio (POCOs)
│   ├── Pessoa.cs      # Classe abstrata
│   ├── Aluno.cs
│   ├── Funcionario.cs
│   ├── Livro.cs
│   └── ...
│
├── DAL/                # Data Access Layer
│   ├── AlunoDAL.cs    # CRUD de Aluno
│   ├── LivroDAL.cs
│   └── ...
│
├── BLL/                # Business Logic Layer
│   ├── EmprestimoService.cs  # Regras de empréstimo
│   ├── ReservaService.cs
│   └── ...
│
├── Forms/              # Interface (Windows Forms)
│   ├── FormPrincipal.cs
│   ├── FormLogin.cs
│   └── ...
│
├── Components/         # Componentes reutilizáveis
│   ├── ToastNotification.cs
│   ├── ThemeManager.cs
│   └── ...
│
├── Constants.cs        # Constantes centralizadas
├── Conexao.cs          # Gerenciador de conexão
└── Program.cs          # Entry point
```

### Onde Adicionar Novo Código

| O que você quer adicionar | Onde colocar |
|---------------------------|--------------|
| Nova entidade | `Model/MinhaEntidade.cs` |
| CRUD para entidade | `DAL/MinhaEntidadeDAL.cs` |
| Regra de negócio | `BLL/MeuService.cs` |
| Validação | `BLL/Validacoes/MinhaValidacao.cs` |
| Nova tela | `Forms/FormMinhaTela.cs` |
| Componente reutilizável | `Components/MeuComponente.cs` |
| Constante | `Constants.cs` |

---

## ✅ Boas Práticas

### 1. Mantenha as Camadas Separadas

```csharp
// ❌ Ruim: Form acessando DAL diretamente
public class FormEmprestimo : Form
{
    private void Salvar()
    {
        var dal = new EmprestimoDAL();
        dal.Inserir(emprestimo);  // Pula a camada BLL!
    }
}

// ✅ Bom: Form usa Service (BLL)
public class FormEmprestimo : Form
{
    private readonly EmprestimoService _service;

    private void Salvar()
    {
        var resultado = _service.RegistrarEmprestimo(idAluno, idLivro);
        if (!resultado.Sucesso)
        {
            MessageBox.Show(resultado.Mensagem);
        }
    }
}
```

### 2. Use Constantes Centralizadas

```csharp
// ❌ Ruim: Magic numbers
if (diasAtraso > 0)
{
    multa = diasAtraso * 2.00m;  // De onde veio esse 2.00?
}

// ✅ Bom: Usa Constants.cs
if (diasAtraso > 0)
{
    multa = diasAtraso * Constants.MULTA_POR_DIA;
}
```

### 3. Valide Entrada do Usuário

```csharp
// ✅ Sempre valide entrada
public ResultadoOperacao RegistrarEmprestimo(int idAluno, int idLivro)
{
    // Validar parâmetros
    if (idAluno <= 0)
        return ResultadoOperacao.Erro("ID do aluno inválido");

    if (idLivro <= 0)
        return ResultadoOperacao.Erro("ID do livro inválido");

    // Continuar com lógica...
}
```

### 4. Trate Exceções Apropriadamente

```csharp
// ✅ Capture e trate exceções específicas
try
{
    using var conn = Conexao.GetConnection();
    conn.Open();
    // operação...
}
catch (NpgsqlException ex)
{
    // Erro específico de banco
    return ResultadoOperacao.Erro($"Erro no banco: {ex.Message}");
}
catch (Exception ex)
{
    // Erro genérico
    return ResultadoOperacao.Erro($"Erro inesperado: {ex.Message}");
}
```

### 5. Use `using` para Recursos

```csharp
// ✅ Sempre use 'using' para IDisposable
using var conn = Conexao.GetConnection();
using var cmd = new NpgsqlCommand(sql, conn);
using var reader = cmd.ExecuteReader();
// Recursos são automaticamente liberados
```

---

## 🧪 Testes

Embora o projeto atualmente não tenha testes automatizados, ao adicionar testes:

```csharp
using Xunit;

public class EmprestimoServiceTests
{
    [Fact]
    public void DeveCalcularMultaCorretamente()
    {
        // Arrange
        var service = new EmprestimoService();
        var diasAtraso = 5;

        // Act
        var multa = service.CalcularMulta(diasAtraso);

        // Assert
        Assert.Equal(10.00m, multa);  // 5 dias * R$ 2,00
    }
}
```

---

## 📚 Recursos Úteis

- [Microsoft C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- [Conventional Commits](https://www.conventionalcommits.org/)
- [Clean Code C#](https://github.com/thangchung/clean-code-dotnet)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)

---

## 💬 Dúvidas?

Se tiver dúvidas:

1. Consulte a [documentação](README.md)
2. Procure em [issues existentes](https://github.com/shishiv/bibliokopke/issues)
3. Abra uma nova issue com a tag `question`

---

**Obrigado por contribuir! 🎉**
