# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

BiblioKopke is a Windows desktop library management system built with C# .NET 8.0, Windows Forms, and PostgreSQL/Supabase. It manages books, students, loans, returns, reservations, and generates management reports.

## Development Commands

### Building and Running
```bash
# Navigate to main application directory
cd 06_bibliotecaJK

# Build the project
dotnet build BibliotecaJK.csproj

# Run in debug mode
dotnet run

# Create release build
dotnet publish -c Release -r win-x64 --self-contained false
```

### Database Setup
```bash
# The schema is automatically applied on first run via FormSetupInicial
# Manual execution (if needed):
psql -h <host> -U <user> -d <database> -f 06_bibliotecaJK/schema-postgresql.sql
```

## Architecture

### 4-Layer Architecture (Strict Separation)

```
Forms (UI) → BLL (Services) → DAL (Data Access) → Model (Entities) → PostgreSQL
```

**Critical Rules:**
1. **Forms NEVER call DAL directly** - Always use BLL Services
2. **BLL NEVER accesses database directly** - Always use DAL
3. **DAL contains NO business logic** - Only data operations
4. **Model classes are POCOs** - No logic, just properties

### Directory Structure

```
06_bibliotecaJK/                    # Main application (current version)
├── Model/                          # Entity classes (Aluno, Livro, Emprestimo, etc.)
├── DAL/                           # Data Access Layer (*DAL.cs files)
├── BLL/                           # Business Logic Layer (*Service.cs files)
│   ├── ResultadoOperacao.cs       # Standard return type for all operations
│   ├── Validadores.cs             # CPF, ISBN, Email validation
│   └── Exceptions.cs              # Custom exceptions
├── Forms/                         # Windows Forms UI (Form*.cs files)
├── Components/                    # Reusable UI components
├── Conexao.cs                     # Database connection manager
├── Constants.cs                   # Centralized business rules and constants
├── Program.cs                     # Application entry point
└── schema-postgresql.sql          # Database schema with triggers

07_foyer/                          # Legacy HTML prototype (archived)
01_planejamento/                   # Project documentation
02_modelagem_banco/                # Database modeling
03_requisitos/                     # Requirements and user stories
04_diagramas/                      # UML diagrams
05_relatorios/                     # Final reports
```

## Key Architectural Patterns

### Standard Operation Return Type

All BLL methods return `ResultadoOperacao`:
```csharp
public class ResultadoOperacao
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; }
    public decimal ValorMulta { get; set; }
    public object? Dados { get; set; }
}
```

**Usage in Forms:**
```csharp
var resultado = emprestimoService.RegistrarEmprestimo(idAluno, idLivro, idFuncionario);
if (resultado.Sucesso)
{
    MessageBox.Show(resultado.Mensagem, "Sucesso");
}
else
{
    MessageBox.Show(resultado.Mensagem, "Erro");
}
```

### Business Rules (Constants.cs)

All business rules are centralized in `Constants.cs`:
- Loan period: 7 days (`PRAZO_EMPRESTIMO_DIAS`)
- Max simultaneous loans: 3 per student (`MAX_EMPRESTIMOS_SIMULTANEOS`)
- Late fee: R$ 2.00/day (`MULTA_POR_DIA`)
- Max renewals: 2 (`MAX_RENOVACOES`)
- Reservation validity: 7 days (`VALIDADE_RESERVA_DIAS`)

**Always reference constants instead of hardcoding values.**

### Database Connection

Connection string is stored in:
```
%LOCALAPPDATA%\BibliotecaJK\database.config
```

The `Conexao` class handles:
- Connection string persistence
- Supabase URI conversion (postgresql:// → Npgsql format)
- SSL parameter preservation
- Connection testing

**Important:** Always use `using var conn = Conexao.GetConnection()` to ensure proper disposal.

## Core Business Logic

### EmprestimoService (Loan Management)

**Key validations before creating loan:**
1. Student exists and is active
2. Book exists and is available (quantidade_disponivel > 0)
3. Student has no overdue loans
4. Student hasn't reached limit of 3 simultaneous loans
5. Student doesn't already have this book on loan

**On loan return:**
1. Calculate days overdue (if any)
2. Calculate late fee: days * R$ 2.00
3. Update book availability (+1)
4. Process reservation queue if exists
5. Log action

### ReservaService (Reservation System)

**FIFO Queue System:**
- Students can reserve unavailable books
- When book is returned, first reservation in queue is notified
- Reservations expire after 7 days
- Status: ATIVA, CONCLUIDA, CANCELADA, EXPIRADA

### Database Features

**PostgreSQL/Supabase specific:**
- Uses pgcrypto extension for BCrypt password hashing
- Triggers for automatic timestamp updates
- Triggers for password hashing on insert/update
- Stored function `verificar_senha()` for authentication
- Indexes on frequently queried columns

## Common Development Patterns

### Adding New Functionality

1. **Create/Update Model** (if new entity)
   - Location: `Model/NomeEntidade.cs`
   - POCOs only, no logic

2. **Create DAL** (database operations)
   - Location: `DAL/NomeEntidadeDAL.cs`
   - Methods: Listar(), ObterPorId(), Inserir(), Atualizar(), Excluir()
   - Use parameterized queries

3. **Create Service** (business logic)
   - Location: `BLL/NomeEntidadeService.cs`
   - Validate all inputs
   - Return ResultadoOperacao
   - Log critical actions

4. **Create Form** (UI)
   - Location: `Forms/FormNomeFuncionalidade.cs`
   - Call Service methods only
   - Handle ResultadoOperacao responses

### Validation Pattern

Use `Validadores` class for common validations:
```csharp
if (!Validadores.ValidarCPF(cpf))
    return ResultadoOperacao.Erro("CPF inválido.");

if (!Validadores.ValidarISBN(isbn))
    return ResultadoOperacao.Erro("ISBN inválido.");

if (!Validadores.ValidarEmail(email))
    return ResultadoOperacao.Erro("E-mail inválido.");
```

### Logging Pattern

Log all critical operations:
```csharp
_logService.Registrar(
    idFuncionario,
    "EMPRESTIMO_REGISTRADO",
    $"Empréstimo #{emprestimo.Id} - Aluno: {aluno.Nome}, Livro: {livro.Titulo}"
);
```

## Database Schema Key Tables

```sql
Aluno           -- Students (id_aluno, nome, cpf, matricula, turma)
Funcionario     -- Staff (id_funcionario, nome, login, senha_hash, perfil)
Livro           -- Books (id_livro, titulo, autor, isbn, quantidade_disponivel)
Emprestimo      -- Loans (id_emprestimo, id_aluno, id_livro, data_emprestimo, data_prevista)
Reserva         -- Reservations (id_reserva, id_aluno, id_livro, status, data_reserva)
Notificacao     -- Notifications (id_notificacao, tipo, prioridade, mensagem)
Log_Acao        -- Audit log (id_log, id_funcionario, acao, descricao)
```

## Security Considerations

- Passwords are hashed with BCrypt (factor 11) via database triggers
- First login forces password change (primeiro_login flag)
- Three user profiles: ADMIN, BIBLIOTECARIO, OPERADOR
- CPF validation with check digits
- SQL injection prevention via parameterized queries
- All sensitive operations are logged

## Testing Strategy

While automated tests are not yet implemented, the architecture supports testing:
- BLL layer can be tested independently by mocking DAL
- ResultadoOperacao provides consistent assertions
- Each layer has single responsibility

## Important Notes

1. **Active Directory:** Main working code is in `06_bibliotecaJK/`, not `08_bibliotecaJK/` or `08_c#/`
2. **Connection String Format:** Supports both standard Npgsql format and Supabase URI format
3. **First Run:** Application prompts for database configuration and auto-applies schema
4. **Admin Credentials:** Default is admin/admin123 (must change on first login)
5. **Referential Integrity:** Enforced by PostgreSQL foreign keys
6. **Backup Feature:** Built-in backup service in BLL/BackupService.cs

## Code Conventions

### Naming
- Classes: PascalCase (AlunoService, EmprestimoDAL)
- Methods: PascalCase (RegistrarEmprestimo)
- Private fields: _camelCase (_emprestimoDAL)
- Constants: UPPER_SNAKE_CASE (PRAZO_EMPRESTIMO_DIAS)

### File Organization
- One class per file
- File name matches class name
- Services in BLL folder end with "Service.cs"
- DAL classes end with "DAL.cs"
- Forms start with "Form" prefix

## Troubleshooting

### Connection Issues
- Check if PostgreSQL/Supabase is accessible
- Verify connection string in `%LOCALAPPDATA%\BibliotecaJK\database.config`
- Test connection via FormConfiguracaoConexao
- For Supabase: ensure SSL Mode=Require is set

### Missing Tables
- Run schema-postgresql.sql in SQL editor
- Or use FormSetupInicial on first run
- Verify pgcrypto extension is enabled

### Password Verification Issues
- Ensure `verificar_senha()` function exists
- Function requires pgcrypto extension
- Password hashing happens automatically via trigger

## Related Documentation

- [06_bibliotecaJK/README.md](06_bibliotecaJK/README.md) - Application documentation
- [06_bibliotecaJK/ARQUITETURA.md](06_bibliotecaJK/ARQUITETURA.md) - Detailed architecture
- [06_bibliotecaJK/BLL/README_BLL.md](06_bibliotecaJK/BLL/README_BLL.md) - Business logic guide
- [CONTRIBUTING.md](CONTRIBUTING.md) - Contribution guidelines with coding standards
- [README.md](README.md) - Project overview
