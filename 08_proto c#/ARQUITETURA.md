# 🏗️ DOCUMENTAÇÃO DE ARQUITETURA - BibliotecaJK v3.0

## Sumário
1. [Visão Geral](#visão-geral)
2. [Arquitetura em Camadas](#arquitetura-em-camadas)
3. [Diagrama de Componentes](#diagrama-de-componentes)
4. [Modelo de Dados](#modelo-de-dados)
5. [Padrões de Projeto](#padrões-de-projeto)
6. [Fluxo de Dados](#fluxo-de-dados)
7. [Decisões Arquiteturais](#decisões-arquiteturais)
8. [Escalabilidade e Manutenção](#escalabilidade-e-manutenção)

---

## Visão Geral

### Informações do Sistema

- **Nome:** BibliotecaJK
- **Versão:** 3.0
- **Tipo:** Aplicação Desktop (Windows Forms)
- **Tecnologia:** C# .NET 8.0
- **Banco de Dados:** MySQL 8.0
- **Arquitetura:** 4 camadas (Model → DAL → BLL → UI)

### Objetivo

Sistema completo de gerenciamento de bibliotecas escolares com funcionalidades de:
- Controle de empréstimos e devoluções
- Gerenciamento de acervo
- Sistema de reservas FIFO
- Cálculo automático de multas
- Relatórios gerenciais

---

## Arquitetura em Camadas

### Diagrama de Camadas

```
┌─────────────────────────────────────────┐
│          CAMADA DE APRESENTAÇÃO         │
│              (UI - WinForms)            │
│  FormLogin, FormPrincipal, Form...     │
└──────────────────┬──────────────────────┘
                   │ Interage com
                   ↓
┌─────────────────────────────────────────┐
│       CAMADA DE LÓGICA DE NEGÓCIO       │
│              (BLL - Services)           │
│  EmprestimoService, LivroService...    │
└──────────────────┬──────────────────────┘
                   │ Usa
                   ↓
┌─────────────────────────────────────────┐
│      CAMADA DE ACESSO A DADOS           │
│             (DAL - Data Access)         │
│  EmprestimoDAL, LivroDAL...           │
└──────────────────┬──────────────────────┘
                   │ Acessa
                   ↓
┌─────────────────────────────────────────┐
│          CAMADA DE MODELO               │
│            (Model - Entidades)          │
│  Emprestimo, Livro, Aluno...          │
└──────────────────┬──────────────────────┘
                   │ Mapeia
                   ↓
┌─────────────────────────────────────────┐
│           BANCO DE DADOS                │
│            (MySQL 8.0)                  │
│  bibliokopke (6 tabelas, 3 views)     │
└─────────────────────────────────────────┘
```

### Responsabilidades das Camadas

#### 1. Model (Entidades)

**Localização:** `/Model/*.cs`

**Responsabilidade:**
- Definir estrutura das entidades
- Propriedades e tipos de dados
- Herança (ex: Pessoa → Aluno/Funcionario)

**Classes:**
```csharp
- Pessoa (abstrata)
  ├── Aluno
  └── Funcionario
- Livro
- Emprestimo
- Reserva
- LogAcao
```

**Exemplo:**
```csharp
public class Emprestimo
{
    public int Id { get; set; }
    public int IdAluno { get; set; }
    public int IdLivro { get; set; }
    public DateTime DataEmprestimo { get; set; }
    public DateTime DataPrevista { get; set; }
    public DateTime? DataDevolucao { get; set; }
    public int NumeroRenovacoes { get; set; }
    public int? IdFuncionario { get; set; }
}
```

**Características:**
- ✅ POCOs (Plain Old CLR Objects)
- ✅ Sem lógica de negócio
- ✅ Apenas propriedades
- ✅ Suporte a nullable types (`DateTime?`)

#### 2. DAL (Data Access Layer)

**Localização:** `/DAL/*DAL.cs`

**Responsabilidade:**
- CRUD completo (Create, Read, Update, Delete)
- Comunicação direta com banco de dados
- Conversão entre DataReader e objetos Model
- Prepared statements (prevenção SQL Injection)

**Classes:**
```
AlunoDAL.cs
FuncionarioDAL.cs
LivroDAL.cs
EmprestimoDAL.cs
ReservaDAL.cs
LogAcaoDAL.cs
```

**Exemplo de Método:**
```csharp
public List<Emprestimo> Listar()
{
    var emprestimos = new List<Emprestimo>();
    using var conn = Conexao.GetConnection();
    conn.Open();

    var cmd = new MySqlCommand(
        "SELECT * FROM Emprestimo", conn);
    using var reader = cmd.ExecuteReader();

    while (reader.Read())
    {
        emprestimos.Add(new Emprestimo
        {
            Id = reader.GetInt32("id"),
            IdAluno = reader.GetInt32("id_aluno"),
            // ...
        });
    }
    return emprestimos;
}
```

**Características:**
- ✅ Uso de `using` para dispose automático
- ✅ Prepared statements com parâmetros
- ✅ Conversão segura de tipos
- ✅ Tratamento de valores NULL
- ❌ **SEM lógica de negócio** (apenas acesso a dados)

#### 3. BLL (Business Logic Layer)

**Localização:** `/BLL/*Service.cs`

**Responsabilidade:**
- **Todas as regras de negócio**
- Validações de dados
- Cálculos (multas, prazos)
- Coordenação entre DALs
- Logging de ações

**Classes:**
```
EmprestimoService.cs  → Regras de empréstimos
LivroService.cs       → Gerenciamento de livros
AlunoService.cs       → Gerenciamento de alunos
ReservaService.cs     → Sistema FIFO de reservas
LogService.cs         → Auditoria
Validadores.cs        → CPF, ISBN, Email
```

**Exemplo - Regras de Negócio:**
```csharp
public ResultadoOperacao RegistrarEmprestimo(int idAluno, int idLivro, int? idFuncionario)
{
    // 1. Aluno existe?
    var aluno = _alunoDAL.ObterPorId(idAluno);
    if (aluno == null)
        return ResultadoOperacao.Erro("Aluno não encontrado.");

    // 2. Livro existe?
    var livro = _livroDAL.ObterPorId(idLivro);
    if (livro == null)
        return ResultadoOperacao.Erro("Livro não encontrado.");

    // 3. Livro disponível?
    if (livro.QuantidadeDisponivel <= 0)
        return ResultadoOperacao.Erro("Livro indisponível.");

    // 4. Aluno tem empréstimos atrasados?
    if (VerificarEmprestimosAtrasados(idAluno))
        return ResultadoOperacao.Erro("Aluno possui empréstimos atrasados.");

    // 5. Limite de 3 empréstimos simultâneos?
    var ativos = ObterEmprestimosAtivos(idAluno).Count;
    if (ativos >= 3)
        return ResultadoOperacao.Erro("Limite de 3 empréstimos simultâneos atingido.");

    // 6. Criar empréstimo
    var emprestimo = new Emprestimo
    {
        IdAluno = idAluno,
        IdLivro = idLivro,
        DataEmprestimo = DateTime.Now,
        DataPrevista = DateTime.Now.AddDays(7), // Regra: 7 dias
        IdFuncionario = idFuncionario
    };

    _emprestimoDAL.Inserir(emprestimo);

    // 7. Decrementar quantidade disponível
    livro.QuantidadeDisponivel--;
    _livroDAL.Atualizar(livro);

    // 8. Registrar log
    _logService.Registrar(idFuncionario, "EMPRESTIMO_CRIADO", $"...");

    return ResultadoOperacao.Ok("Empréstimo registrado com sucesso!");
}
```

**Padrão de Retorno:**
```csharp
public class ResultadoOperacao
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; }
    public decimal ValorMulta { get; set; }
    public object? Dados { get; set; }
}
```

**Características:**
- ✅ **Centraliza todas as regras de negócio**
- ✅ Valida antes de persistir
- ✅ Coordena múltiplas DALs
- ✅ Retorna ResultadoOperacao padronizado
- ✅ Registra logs de ações críticas
- ❌ **Nunca acessa banco diretamente** (usa DAL)

#### 4. UI (User Interface - WinForms)

**Localização:** `/Forms/Form*.cs`

**Responsabilidade:**
- Captura de entrada do usuário
- Apresentação de dados
- Feedback visual (cores, mensagens)
- Navegação entre telas

**Formulários:**
```
FormLogin.cs              → Autenticação
FormPrincipal.cs          → Dashboard e menu
FormCadastroAluno.cs      → CRUD alunos
FormCadastroLivro.cs      → CRUD livros
FormEmprestimo.cs         → Registro de empréstimos
FormDevolucao.cs          → Devolução e multas
FormReserva.cs            → Sistema de reservas
FormConsultaEmprestimos.cs → Consultas
FormRelatorios.cs         → Relatórios gerenciais
```

**Exemplo - Integração com BLL:**
```csharp
private void BtnRegistrar_Click(object sender, EventArgs e)
{
    // UI apenas chama o serviço
    var resultado = _emprestimoService.RegistrarEmprestimo(
        _alunoSelecionadoId.Value,
        _livroSelecionadoId.Value,
        _funcionarioLogado.Id
    );

    // UI apenas apresenta o resultado
    if (resultado.Sucesso)
    {
        MessageBox.Show(resultado.Mensagem, "Sucesso",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
        LimparFormulario();
    }
    else
    {
        MessageBox.Show(resultado.Mensagem, "Atenção",
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
```

**Características:**
- ✅ **Nunca acessa DAL diretamente**
- ✅ **Sempre usa BLL (Services)**
- ✅ Responsável apenas por UI/UX
- ✅ Valida entrada básica (campos vazios)
- ✅ Feedback visual (cores para atrasados)

---

## Diagrama de Componentes

```
┌─────────────────────────────────────────────────────────┐
│                    WINFORMS UI                          │
├───────────┬─────────┬────────┬─────────┬───────────────┤
│ FormLogin │ FormPrincipal │ Forms CRUD │ FormRelatorios│
└─────┬─────┴─────────┴────┬───┴─────────┴───────┬───────┘
      │                     │                      │
      ↓                     ↓                      ↓
┌─────────────────────────────────────────────────────────┐
│                   BUSINESS LOGIC (BLL)                  │
├────────────────┬──────────────┬────────────────────────┤
│EmprestimoService│LivroService │ AlunoService  │ etc... │
│ (5 validações)  │             │               │        │
└────────┬────────┴──────┬──────┴────────┬──────────────┘
         │                │                │
         ↓                ↓                ↓
┌─────────────────────────────────────────────────────────┐
│                  DATA ACCESS (DAL)                      │
├───────────────┬──────────────┬────────────────────────┬┤
│ EmprestimoDAL │ LivroDAL     │ AlunoDAL    │ etc...  ││
└───────┬───────┴──────┬───────┴────────┬───────────────┘
        │              │                 │
        ↓              ↓                 ↓
┌─────────────────────────────────────────────────────────┐
│                     MYSQL DATABASE                       │
│  Emprestimo │ Livro │ Aluno │ Reserva │ Log_Acao      │
└─────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────┐
│                 COMPONENTES AUXILIARES                   │
├─────────────┬────────────┬────────────┬─────────────────┤
│  Conexao.cs │Validadores │ Exceptions │ResultadoOperacao│
└─────────────┴────────────┴────────────┴─────────────────┘
```

---

## Modelo de Dados

### Diagrama Entidade-Relacionamento

```
┌─────────────────┐         ┌──────────────────┐
│    Pessoa       │         │      Livro       │
│   (abstrata)    │         │                  │
│  - Id           │         │  - Id            │
│  - Nome         │         │  - Titulo        │
│  - CPF          │         │  - Autor         │
└────────┬────────┘         │  - ISBN          │
         │                  │  - Editora       │
    ┌────┴─────┐            │  - Categoria     │
    │          │            │  - QtdTotal      │
┌───┴──┐  ┌───┴────┐       │  - QtdDisponivel │
│Aluno │  │Funciona│       └───┬──────────────┘
│      │  │rio     │           │
│-Matri│  │-Cargo  │           │
│cula  │  │-Login  │           │
│-Turma│  │-Senha  │           │
└──┬───┘  └───┬────┘           │
   │          │                │
   │          │                │
   │      ┌───┴────────────────┴───┐
   │      │                        │
   └─────►│     Emprestimo         │◄───────┐
          │                        │        │
          │  - Id                  │        │
          │  - IdAluno      (FK)   │        │
          │  - IdLivro      (FK)   │        │
          │  - DataEmprestimo      │        │
          │  - DataPrevista        │        │
          │  - DataDevolucao       │        │
          │  - NumeroRenovacoes    │        │
          │  - IdFuncionario (FK)  │◄───────┘
          └─────────┬──────────────┘
                    │
   ┌────────────────┴────────────────┐
   │                                 │
   ↓                                 ↓
┌────────────┐              ┌──────────────┐
│  Reserva   │              │   Log_Acao   │
│            │              │              │
│ - IdAluno  │              │ - IdFuncion. │
│ - IdLivro  │              │ - Acao       │
│ - DataRes. │              │ - Descricao  │
│ - Status   │              │ - Data       │
└────────────┘              └──────────────┘
```

### Relacionamentos

1. **Pessoa → Aluno/Funcionario** (Herança)
   - Tipo: Generalização/Especialização
   - Aluno e Funcionario herdam propriedades de Pessoa

2. **Aluno → Emprestimo** (1:N)
   - Um aluno pode ter vários empréstimos
   - Chave estrangeira: `id_aluno`

3. **Livro → Emprestimo** (1:N)
   - Um livro pode ter vários empréstimos
   - Chave estrangeira: `id_livro`

4. **Funcionario → Emprestimo** (1:N)
   - Um funcionário registra vários empréstimos
   - Chave estrangeira: `id_funcionario` (opcional)

5. **Aluno → Reserva** (1:N)
   - Um aluno pode ter várias reservas
   - Chave estrangeira: `id_aluno`

6. **Livro → Reserva** (1:N)
   - Um livro pode ter várias reservas
   - Chave estrangeira: `id_livro`

### Tabelas Principais

#### Aluno
```sql
CREATE TABLE Aluno (
    id INT PRIMARY KEY AUTO_INCREMENT,
    nome VARCHAR(100) NOT NULL,
    cpf VARCHAR(14) UNIQUE NOT NULL,
    matricula VARCHAR(20) UNIQUE NOT NULL,
    turma VARCHAR(50),
    telefone VARCHAR(15),
    email VARCHAR(100)
);
```

#### Livro
```sql
CREATE TABLE Livro (
    id INT PRIMARY KEY AUTO_INCREMENT,
    titulo VARCHAR(200) NOT NULL,
    autor VARCHAR(100),
    isbn VARCHAR(20) UNIQUE,
    editora VARCHAR(100),
    ano_publicacao INT,
    categoria VARCHAR(50),
    quantidade_total INT NOT NULL,
    quantidade_disponivel INT NOT NULL
);
```

#### Emprestimo
```sql
CREATE TABLE Emprestimo (
    id INT PRIMARY KEY AUTO_INCREMENT,
    id_aluno INT NOT NULL,
    id_livro INT NOT NULL,
    data_emprestimo DATETIME NOT NULL,
    data_prevista_devolucao DATETIME NOT NULL,
    data_devolucao DATETIME,
    numero_renovacoes INT DEFAULT 0,
    id_funcionario INT,
    FOREIGN KEY (id_aluno) REFERENCES Aluno(id),
    FOREIGN KEY (id_livro) REFERENCES Livro(id),
    FOREIGN KEY (id_funcionario) REFERENCES Funcionario(id)
);
```

### Views

#### vw_emprestimos_ativos
```sql
CREATE VIEW vw_emprestimos_ativos AS
SELECT e.id, a.nome AS aluno, l.titulo AS livro,
       e.data_emprestimo, e.data_prevista_devolucao
FROM Emprestimo e
JOIN Aluno a ON e.id_aluno = a.id
JOIN Livro l ON e.id_livro = l.id
WHERE e.data_devolucao IS NULL;
```

---

## Padrões de Projeto

### 1. Layered Architecture (Arquitetura em Camadas)

**Objetivo:** Separar responsabilidades

**Implementação:**
- Model: Entidades
- DAL: Acesso a dados
- BLL: Lógica de negócio
- UI: Apresentação

**Benefícios:**
- ✅ Manutenibilidade
- ✅ Testabilidade
- ✅ Reusabilidade

### 2. Repository Pattern (no DAL)

**Objetivo:** Abstrair acesso ao banco

**Implementação:**
```csharp
public class AlunoDAL
{
    public List<Aluno> Listar() { }
    public Aluno ObterPorId(int id) { }
    public void Inserir(Aluno aluno) { }
    public void Atualizar(Aluno aluno) { }
    public void Excluir(int id) { }
}
```

**Benefícios:**
- ✅ Centraliza queries
- ✅ Facilita mudança de banco

### 3. Service Layer Pattern (no BLL)

**Objetivo:** Encapsular lógica de negócio

**Implementação:**
```csharp
public class EmprestimoService
{
    private readonly EmprestimoDAL _emprestimoDAL;
    private readonly LivroDAL _livroDAL;
    private readonly AlunoDAL _alunoDAL;

    public ResultadoOperacao RegistrarEmprestimo() { }
    public ResultadoOperacao RegistrarDevolucao() { }
    public ResultadoOperacao RenovarEmprestimo() { }
}
```

**Benefícios:**
- ✅ Regras centralizadas
- ✅ Reutilização
- ✅ UI enxuta

### 4. Data Transfer Object (ResultadoOperacao)

**Objetivo:** Padronizar retornos

**Implementação:**
```csharp
public class ResultadoOperacao
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; }
    public decimal ValorMulta { get; set; }
    public object? Dados { get; set; }

    public static ResultadoOperacao Ok(string msg) => new() { Sucesso = true, Mensagem = msg };
    public static ResultadoOperacao Erro(string msg) => new() { Sucesso = false, Mensagem = msg };
}
```

**Benefícios:**
- ✅ Retorno consistente
- ✅ Facilita tratamento na UI

### 5. Singleton (evitado em Conexao.cs)

**⚠️ Anti-pattern identificado e corrigido:**

Versão **incorreta** (não usar):
```csharp
private static MySqlConnection? _instance;
public static MySqlConnection Instance => _instance ??= new MySqlConnection(connectionString);
```

Versão **correta** (usar):
```csharp
public static MySqlConnection GetConnection()
{
    return new MySqlConnection(connectionString); // Nova instância sempre
}
```

**Motivo:** Connection pooling é gerenciado pelo ADO.NET automaticamente.

---

## Fluxo de Dados

### Exemplo: Registro de Empréstimo

```
[UI] FormEmprestimo
  │
  │ 1. Usuário seleciona aluno e livro
  │ 2. Clica em "Registrar Empréstimo"
  │
  └─→ [BLL] EmprestimoService.RegistrarEmprestimo(idAluno, idLivro, idFuncionario)
        │
        ├─→ [DAL] AlunoDAL.ObterPorId(idAluno)
        │     └─→ [DB] SELECT * FROM Aluno WHERE id = @id
        │           └─→ Retorna: Aluno ou null
        │
        ├─→ [DAL] LivroDAL.ObterPorId(idLivro)
        │     └─→ [DB] SELECT * FROM Livro WHERE id = @id
        │           └─→ Retorna: Livro ou null
        │
        ├─→ [BLL] Validações:
        │     - Aluno existe?
        │     - Livro existe?
        │     - Livro disponível?
        │     - Aluno sem atrasos?
        │     - Limite de 3 empréstimos?
        │
        ├─→ [DAL] EmprestimoDAL.Inserir(emprestimo)
        │     └─→ [DB] INSERT INTO Emprestimo (...) VALUES (...)
        │
        ├─→ [DAL] LivroDAL.Atualizar(livro)
        │     └─→ [DB] UPDATE Livro SET quantidade_disponivel = @qtd WHERE id = @id
        │
        ├─→ [BLL] LogService.Registrar(...)
        │     └─→ [DAL] LogAcaoDAL.Inserir(log)
        │           └─→ [DB] INSERT INTO Log_Acao (...) VALUES (...)
        │
        └─→ Retorna: ResultadoOperacao { Sucesso = true, Mensagem = "..." }
  │
  └─→ [UI] Exibe MessageBox com resultado
```

---

## Decisões Arquiteturais

### 1. Por que 4 camadas?

**Decisão:** Separar UI da lógica de negócio

**Alternativas consideradas:**
- ❌ 2 camadas (UI + Banco): Código duplicado, difícil manter
- ❌ 3 camadas (UI + BLL+DAL + Banco): Lógica misturada

**Escolhida:** 4 camadas (UI + BLL + DAL + Model)

**Justificativa:**
- ✅ Clara separação de responsabilidades
- ✅ Facilita testes (pode testar BLL sem UI)
- ✅ Permite trocar UI (ex: para Web) sem mudar BLL/DAL

### 2. Por que ADO.NET e não Entity Framework?

**Decisão:** Usar ADO.NET direto

**Justificativa:**
- ✅ Controle total das queries
- ✅ Performance (sem overhead do ORM)
- ✅ Didático (entende SQL melhor)
- ✅ Leve (sem dependências pesadas)

### 3. Por que WinForms e não WPF/Avalonia?

**Decisão:** Windows Forms

**Justificativa:**
- ✅ Simplicidade (rápido de desenvolver)
- ✅ Estável e maduro
- ✅ Suporte completo no .NET 8
- ✅ Menor curva de aprendizado

### 4. Por que MySQL e não SQL Server?

**Decisão:** MySQL 8.0

**Justificativa:**
- ✅ Gratuito e open-source
- ✅ Multi-plataforma
- ✅ Amplamente usado
- ✅ Bom desempenho

---

## Escalabilidade e Manutenção

### Pontos Fortes

1. **Modularidade**
   - Cada camada independente
   - Facilita manutenção

2. **Extensibilidade**
   - Adicionar novos Services é simples
   - Adicionar novos Forms é direto

3. **Testabilidade**
   - BLL pode ser testado isoladamente
   - DAL pode ser mockado

### Limitações Atuais

1. **Autenticação**
   - Senhas em texto plano
   - **Produção:** Usar BCrypt/Argon2

2. **Concorrência**
   - Aplicação desktop (single-user por instância)
   - **Melhorar:** Adicionar locks no banco

3. **Logs**
   - Logs básicos
   - **Melhorar:** Structured logging (Serilog)

4. **Validações**
   - Validações básicas
   - **Melhorar:** FluentValidation

### Evoluções Futuras

**v4.0 (Sugestões):**
- [ ] Migrar para ASP.NET Core (Web)
- [ ] Implementar API REST
- [ ] Adicionar autenticação JWT
- [ ] Usar Entity Framework Core
- [ ] Implementar CQRS
- [ ] Adicionar Redis para cache
- [ ] Containerizar com Docker

**v3.1 (Melhorias imediatas):**
- [ ] Hash de senhas (BCrypt)
- [ ] Testes unitários (xUnit)
- [ ] CI/CD (GitHub Actions)
- [ ] Backup automático do banco

---

## Referências

### Documentos Relacionados
- [Manual do Usuário](MANUAL_USUARIO.md)
- [Guia de Instalação](INSTALACAO.md)
- [Plano de Testes](TESTES.md)
- [README Principal](README.txt)

### Padrões e Conceitos
- Layered Architecture
- Repository Pattern
- Service Layer Pattern
- Dependency Injection (parcialmente implementado)
- SOLID Principles

---

**Desenvolvido por:**
Pessoa 1: Banco de Dados
Pessoa 2: Camada DAL
Pessoa 3: Camada BLL
Pessoa 4: Interface WinForms
Pessoa 5: Relatórios e Documentação

**BibliotecaJK v3.0** - Sistema Completo de Gerenciamento de Bibliotecas
© 2025 - Todos os direitos reservados
