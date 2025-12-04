# Project Context

## Purpose

BiblioKopke is a Windows desktop library management system designed for school libraries in Brazil. It manages the complete lifecycle of library operations including book cataloging, student registration, loan tracking, reservation queuing, overdue management, late fee calculation, and administrative reporting.

**Primary Goals:**
- Streamline library operations with automated loan/return workflows
- Enforce business rules (7-day loans, 3-book limit, R$ 2.00/day late fees)
- Implement FIFO reservation queue for unavailable books
- Provide complete audit trail of all library transactions
- Generate management reports for library administration
- Support role-based access (Admin, Librarian, Operator)

## Tech Stack

### Core Technologies
- **Language:** C# 12.0
- **Framework:** .NET 8.0 (net8.0-windows)
- **UI Framework:** Windows Forms (WinForms)
- **Database:** PostgreSQL 14+ / Supabase (PostgreSQL-compatible)
- **Database Driver:** Npgsql 8.0.8
- **Target Platform:** Windows 10/11 (x64)

### Key Features & Extensions
- **pgcrypto Extension** - BCrypt password hashing (factor 11)
- **PL/pgSQL Functions** - Server-side password verification and timestamp management
- **Database Triggers** - Automatic password hashing, inventory updates, timestamp maintenance
- **AES-256 Encryption** - Backup configuration security

### Built-in .NET Libraries
- System.Text.Json - Configuration persistence
- System.Security.Cryptography - Backup encryption
- System.Text.RegularExpressions - CPF/ISBN/Email validation
- System.Windows.Forms - UI components
- System.Drawing - Theming and graphics

### Development & Distribution
- **Build System:** .NET CLI, PowerShell build scripts
- **Installer:** Inno Setup 6.x (self-contained with .NET 8.0 runtime)
- **Compression:** LZMA2 Ultra (64-bit dictionary)
- **Localization:** Portuguese (Brazil)

## Project Conventions

### Code Style

#### Naming Conventions
- **Classes:** PascalCase (e.g., `AlunoService`, `EmprestimoDAL`, `FormLogin`)
- **Methods:** PascalCase (e.g., `RegistrarEmprestimo`, `ObterPorId`)
- **Private Fields:** _camelCase with underscore prefix (e.g., `_emprestimoDAL`, `_funcionarioLogado`)
- **Properties:** PascalCase (e.g., `Sucesso`, `Mensagem`, `ValorMulta`)
- **Constants:** UPPER_SNAKE_CASE (e.g., `PRAZO_EMPRESTIMO_DIAS`, `MAX_RENOVACOES`)
- **Local Variables:** camelCase (e.g., `resultado`, `idAluno`)

#### File Organization
- **One class per file** - File name must match class name exactly
- **Service classes:** Located in `BLL/` folder, filename ends with `Service.cs`
- **DAL classes:** Located in `DAL/` folder, filename ends with `DAL.cs`
- **Forms:** Located in `Forms/` folder, filename starts with `Form` prefix (e.g., `FormEmprestimo.cs`)
- **Models:** Located in `Model/` folder, simple entity name (e.g., `Aluno.cs`, `Livro.cs`)
- **Components:** Located in `Components/` folder for reusable UI elements

#### Code Organization
```
06_bibliotecaJK/                    # Main application directory
├── Model/                          # Entity POCOs (8 files)
├── DAL/                           # Data Access Layer (7 files)
├── BLL/                           # Business Logic Layer (8 files)
│   ├── *Service.cs                # Domain services
│   ├── ResultadoOperacao.cs       # Standard return type
│   ├── Validadores.cs             # Validation utilities
│   └── Exceptions.cs              # Custom exceptions
├── Forms/                         # Windows Forms (15 files)
├── Components/                    # Reusable UI (5 files)
├── Conexao.cs                     # Connection manager
├── Constants.cs                   # Business rules & constants
├── Program.cs                     # Entry point
└── schema-postgresql.sql          # Database schema
```

### Architecture Patterns

#### Strict 4-Layer Architecture

```
Forms (UI) → BLL (Services) → DAL (Data Access) → Model (Entities) → PostgreSQL
```

**Critical Architectural Rules:**
1. **Forms NEVER call DAL directly** - Always use BLL Services
2. **BLL NEVER accesses database directly** - Always use DAL
3. **DAL contains NO business logic** - Only CRUD operations
4. **Model classes are POCOs** - No methods, only properties

#### Layer Responsibilities

**Model Layer (Pure DTOs)**
- Plain Old CLR Objects with public properties only
- Zero business logic or validation
- Inheritance hierarchy: `Pessoa` (abstract) → `Aluno`, `Funcionario`
- Entities: `Livro`, `Emprestimo`, `Reserva`, `LogAcao`, `Notificacao`

**DAL Layer (Data Access)**
- Standard CRUD interface per entity: `Inserir()`, `Listar()`, `ObterPorId()`, `Atualizar()`, `Excluir()`
- Parameterized queries only (SQL injection prevention)
- Connection management via `Conexao.GetConnection()`
- `using` statements for resource disposal
- No validation or business logic

**BLL Layer (Business Logic)**
- All validation and business rules enforced here
- Standard return type: `ResultadoOperacao` (never throws exceptions for business rule violations)
- Logging of all critical operations via `LogService`
- Centralized validators: `Validadores.ValidarCPF()`, `ValidarISBN()`, `ValidarEmail()`
- Service pattern: Each domain entity has a corresponding Service class
- Constructor dependency instantiation (manual DI)

**Forms Layer (Presentation)**
- Windows Forms UI with event handlers
- Receives logged-in user (`Funcionario`) in constructor
- Instantiates required Services
- Handles `ResultadoOperacao` responses
- Displays feedback via MessageBox

#### Key Design Patterns

**1. Service Pattern**
Every domain has a Service class that orchestrates DAL operations and enforces business rules.

**2. Standard Return Type Pattern**
```csharp
public class ResultadoOperacao
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; }
    public decimal ValorMulta { get; set; }
    public object? Dados { get; set; }

    public static ResultadoOperacao Ok(string mensagem, object? dados = null)
    public static ResultadoOperacao OkComMulta(string mensagem, decimal multa)
    public static ResultadoOperacao Erro(string mensagem)
}
```
Benefits: Consistent error handling, no exception bloat in UI, optional data payload.

**3. Repository Pattern**
DAL classes act as repositories, abstracting database operations from business logic.

**4. Singleton Pattern (Static)**
- `Conexao` - Static connection string cache
- `Constants` - Centralized business rules
- `Validadores` - Utility validators
- `ThemeManager` - UI theming

**5. FIFO Queue Pattern**
`ReservaService` implements first-in-first-out queue for book reservations ordered by `data_reserva`.

**6. Audit Trail Pattern**
All critical operations logged to `Log_Acao` table with staff ID, action type, timestamp, and description.

### Testing Strategy

**Current State:**
- No automated unit tests implemented
- Architecture supports testing through layer separation
- BLL can be tested independently by mocking DAL

**Manual Testing Approach:**
- Connection testing via `Conexao.TestarConexao()`
- Schema verification on first run via `FormSetupInicial`
- Database function testing (`verificar_senha()` for password auth)
- UI testing through manual workflows

**Future Testing Recommendations:**
- Unit test BLL services with mocked DAL dependencies
- Integration tests for database operations
- `ResultadoOperacao` provides consistent assertions (`Assert.True(resultado.Sucesso)`)
- Each layer has single responsibility, making tests focused

**Validation Testing:**
- CPF check digit validation (`Validadores.ValidarCPF()`)
- ISBN-10 and ISBN-13 format validation
- Email RFC compliance
- Business rule validation (loan limits, overdue checks)

### Git Workflow

**Branch Strategy:**
- **Main Branch:** `master` (production-ready code)
- Feature branches merged via pull requests
- Recent commits show PR-based workflow: `#13`, `#10`

**Commit Conventions:**
- Descriptive commit messages (e.g., "refactor: optimize ReservaService instantiation")
- Merge commits reference PR numbers
- Claude Code commits include attribution footer

**Project Status (Current):**
- Active development on `master` branch
- Modified files: `CLAUDE.md`, new `.claude/commands/`, `AGENTS.md`, `openspec/`
- Recent work: Build fixes, setup flow cleanup, service optimization

**Workflow Notes:**
- Database schema tracked in version control (`schema-postgresql.sql`)
- Configuration files (connection string, backup config) stored in user AppData (NOT in repo)
- Build scripts included: `build-release.ps1`, `build-installer.bat`

## Domain Context

### Library Management Domain

BiblioKopke operates in the **Brazilian school library** domain with specific business workflows:

#### Core Entities & Relationships
- **Aluno (Student)** - CPF (Brazilian ID), Matrícula (unique student ID), Turma (class)
- **Funcionario (Staff)** - Three roles: ADMIN, BIBLIOTECARIO, OPERADOR
- **Livro (Book)** - ISBN, Title, Author, Quantity tracking (total vs. available)
- **Emprestimo (Loan)** - Tracks active loans, returns, late fees, status (ATIVO, DEVOLVIDO, ATRASADO)
- **Reserva (Reservation)** - FIFO queue for unavailable books, status (ATIVA, CANCELADA, CONCLUIDA, EXPIRADA)
- **Notificacao (Notification)** - System alerts for overdue loans, expired reservations, available books
- **Log_Acao (Audit Log)** - Complete transaction history with staff attribution

#### Business Rules (Constants.cs)

**Loan Management:**
- Loan period: **7 days** (`PRAZO_EMPRESTIMO_DIAS = 7`)
- Max simultaneous loans: **3 per student** (`MAX_EMPRESTIMOS_SIMULTANEOS = 3`)
- Late fee: **R$ 2.00 per day** (`MULTA_POR_DIA = 2.00m`)
- Max renewals: **2 per loan** (`MAX_RENOVACOES = 2`)
- Overdue block: Students with overdue loans cannot borrow

**Reservation System:**
- Reservation validity: **7 days** (`VALIDADE_RESERVA_DIAS = 7`)
- Max reservations: **3 per student** (`MAX_RESERVAS_POR_ALUNO = 3`)
- Queue processing: **FIFO (First-In-First-Out)** by reservation date
- Auto-expiration: Reservations expire after 7 days if not fulfilled

**Password Security:**
- Min length: **8 characters** (`SENHA_MIN_LENGTH = 8`)
- Hashing: **BCrypt factor 11** (database-side via pgcrypto)
- First login: **Forced password change** (`primeiro_login = TRUE`)

#### Critical Workflows

**1. Loan Registration (`RegistrarEmprestimo`)**
```
Validations:
├─ Student exists and is active
├─ Book exists and quantity_disponivel > 0
├─ Student has NO overdue loans (status ≠ ATRASADO)
├─ Student has < 3 active loans
└─ Student doesn't already have this book on loan

Actions:
├─ Create Emprestimo record (data_prevista = TODAY + 7 days)
├─ Decrement livro.quantidade_disponivel
├─ Log: "EMPRESTIMO_REGISTRADO" with details
└─ Return ResultadoOperacao.Ok()
```

**2. Loan Return (`RegistrarDevolucao`)**
```
Calculations:
├─ dias_atraso = MAX(0, TODAY - data_prevista)
└─ multa = dias_atraso × R$ 2.00

Actions:
├─ Set data_devolucao = NOW
├─ Set multa = calculated value
├─ Increment livro.quantidade_disponivel
├─ Process reservation queue (FIFO)
│  ├─ Mark first ATIVA Reserva as CONCLUIDA
│  └─ Create LIVRO_DISPONIVEL notification
├─ Log: "EMPRESTIMO_DEVOLVIDO" with fine amount
└─ Return ResultadoOperacao.OkComMulta(mensagem, multa)
```

**3. Reservation Queue Processing (`ProcessarFilaReservas`)**
```
Queue Logic:
├─ Get all ATIVA Reserva for book, ORDER BY data_reserva ASC (FIFO)
├─ Take first reservation in queue
├─ Update status to CONCLUIDA
├─ Create notification: LIVRO_DISPONIVEL (priority: ALTA)
├─ Student is notified book is ready for pickup
└─ Reservation holds book for student for pickup
```

#### Validation Domain Knowledge

**CPF (Cadastro de Pessoas Físicas)**
- Brazilian national ID: 11 digits formatted as 000.000.000-00
- Check digit algorithm validates last 2 digits
- Sequential patterns (111.111.111-11) are invalid

**ISBN (International Standard Book Number)**
- Supports both ISBN-10 and ISBN-13 formats
- Check digit validation for both formats
- Optional field (books without ISBN allowed)

**Matrícula (Student ID)**
- 3-20 alphanumeric characters
- Must be unique across all students
- Required field for student registration

## Important Constraints

### Technical Constraints

**Platform Requirements:**
- **Windows-only** - Uses Windows Forms (not cross-platform)
- **Desktop application** - No web interface or mobile support
- **.NET 8.0 Runtime** - Required on client machines
- **PostgreSQL 14+** - Server-side requirement
- **Network connectivity** - For remote database access (Supabase)

**Architectural Constraints:**
- **Strict layer separation** - Forms → BLL → DAL → Model flow must be maintained
- **No ORM** - Uses raw ADO.NET with parameterized queries (no Entity Framework)
- **No Dependency Injection container** - Manual dependency instantiation in constructors
- **Synchronous I/O** - All database operations are synchronous (no async/await)
- **Static configuration** - Connection string and constants use static classes

**Database Constraints:**
- **pgcrypto extension required** - For BCrypt password hashing
- **Port 5432** - Standard PostgreSQL port must be accessible
- **SSL/TLS support** - Required for Supabase connections (`SSL Mode=Require`)
- **Stored functions required** - `verificar_senha()`, `hash_senha()`, `update_data_atualizacao()`

### Business Constraints

**Library Operations:**
- **7-day loan cycle** - Fixed period, cannot be adjusted per-loan
- **3-book limit** - Hard limit per student, no exceptions in code
- **R$ 2.00 daily fine** - Fixed rate, cannot vary by book or student
- **2 renewal maximum** - Total loan period cannot exceed 21 days
- **FIFO reservation queue** - No priority reservations or queue jumping
- **Single active reservation per book per student** - Prevents duplicate reservations

**Data Integrity:**
- **CPF uniqueness** - One student record per CPF
- **Matrícula uniqueness** - One student per enrollment number
- **ISBN uniqueness** - One book record per ISBN (if provided)
- **Login uniqueness** - One staff account per login name
- **Referential integrity** - Foreign key constraints enforced by PostgreSQL

**Security Constraints:**
- **First login password change** - Cannot be bypassed for new staff accounts
- **BCrypt only** - No alternative hashing algorithms supported
- **Role-based permissions** - ADMIN, BIBLIOTECARIO, OPERADOR hierarchy
- **Audit logging required** - All critical operations must log to Log_Acao

### Regulatory/Operational Constraints

**Brazilian Context:**
- **Portuguese language** - All UI and messages in Portuguese (Brazil)
- **CPF validation** - Must comply with Brazilian ID format and check digits
- **Currency format** - Late fees in Brazilian Real (R$)
- **Date format** - DD/MM/YYYY for UI display

**Data Retention:**
- **Loan history** - All loans retained permanently (no auto-deletion)
- **Audit logs** - Complete transaction history maintained
- **Reservation expiration** - Auto-expires after 7 days but record retained
- **Notification cleanup** - No auto-deletion of old notifications

### Performance Constraints

**Client-Side:**
- **Synchronous UI** - Long database operations may freeze UI
- **No pagination** - DataGridView loads all records (may impact large datasets)
- **In-memory filtering** - Search/filter operations performed in memory

**Database:**
- **Connection pooling** - Via Npgsql default settings
- **No caching layer** - Every operation queries database directly
- **Indexed queries** - Heavy reliance on database indexes for performance

## External Dependencies

### Primary Dependencies

**1. PostgreSQL / Supabase (Database)**
- **Type:** Relational database system
- **Version:** PostgreSQL 14+ (or Supabase PostgreSQL-compatible)
- **Connection:** TCP/IP on port 5432
- **Extensions Required:**
  - `pgcrypto` - Password hashing (BCrypt)
- **Authentication:** Username/password (stored in `%LOCALAPPDATA%\BibliotecaJK\database.config`)
- **Supported Formats:**
  - Standard Npgsql: `Host=localhost;Port=5432;Database=biblioteca;Username=user;Password=pass`
  - Supabase URI: `postgresql://user:pass@db.xxxx.supabase.co:5432/postgres?sslmode=require`
- **Auto-conversion:** Supabase URIs automatically converted to Npgsql format by `Conexao` class

**2. Npgsql (Database Driver)**
- **Package:** Npgsql 8.0.8
- **Purpose:** PostgreSQL data provider for .NET
- **Features Used:**
  - Parameterized queries
  - Connection pooling
  - SSL/TLS support
  - Type mapping (NpgsqlDbType)
- **Installation:** Via NuGet package reference in `.csproj`

### Optional Cloud Services

**Supabase (PostgreSQL-as-a-Service)**
- **Use Case:** Cloud-hosted PostgreSQL alternative to self-hosted
- **Benefits:**
  - Automatic daily backups (7-day retention on free tier)
  - Geographic replication
  - Built-in connection pooling
  - SSL/TLS enabled by default
- **Configuration:** Automatic URI conversion in `Conexao.cs`
- **No Supabase-specific APIs used** - Standard PostgreSQL compatibility only

### System Dependencies

**Windows Task Scheduler (Optional)**
- **Purpose:** Automated backup scheduling
- **Usage:** Optional feature in `BackupService.cs`
- **Configuration:** User-configurable via `FormBackup.cs`

### No External API Dependencies

BiblioKopke is a **fully self-contained** application with:
- **No cloud API calls** (except optional Supabase database hosting)
- **No third-party authentication services**
- **No external reporting services**
- **No email/SMS notification services** (notifications are in-app only)
- **No payment gateways** (late fees tracked but not collected via app)

### Development Dependencies

**Build Tools:**
- **.NET 8.0 SDK** - Required for compilation
- **PowerShell** - For build scripts (`build-release.ps1`)
- **Inno Setup 6.x** - For installer generation (optional)

**Database Tools:**
- **psql** - For manual schema application (optional)
- **pg_dump** - For backup functionality (called via Npgsql)

### Configuration Storage

**Local AppData:**
- **Location:** `%LOCALAPPDATA%\BibliotecaJK\`
- **Files:**
  - `database.config` - Connection string (JSON format)
  - `backup.config` - Backup settings (AES-256 encrypted JSON)
- **Format:** JSON serialization via `System.Text.Json`
- **Persistence:** Managed by `Conexao` and `BackupConfig` classes
