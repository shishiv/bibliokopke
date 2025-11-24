# BiblioKopke - Sistema de Gerenciamento de Biblioteca

Sistema desktop de gerenciamento de biblioteca desenvolvido em C# .NET 8.0 com Windows Forms e SQLite.

## 🚀 Início Rápido

### Pré-requisitos
- .NET 8.0 SDK
- Windows 7 ou superior

### Build e Execução

```bash
# Restaurar dependências
dotnet restore BibliotecaJK.csproj

# Build
dotnet build BibliotecaJK.csproj

# Executar
dotnet run --project BibliotecaJK.csproj

# Build de Release
dotnet publish -c Release -r win-x64 --self-contained false
```

## 📦 Estrutura do Projeto

Este é um projeto **unificado** (não há projetos separados):

```
BibliotecaJK_FullBackend/
├── BibliotecaJK.csproj          # Projeto único (WinForms + Backend)
├── Program.cs                   # Entry point da aplicação
├── DiagnosticosBanco.cs         # Utilitários de diagnóstico
├── Conexao.cs                   # Gerenciador de conexão SQLite
├── InicializadorSqlite.cs       # Inicialização do schema
│
├── Modelos/                     # Entidades (POCOs)
├── AcessoDados/                 # Camada de acesso a dados
├── Servicos/                    # Lógica de negócio
├── Utilitarios/                 # Validadores e Exceções
│
├── Form*.cs                     # Formulários Windows Forms
├── *.Designer.cs                # Designer files (gerados)
└── *.resx                       # Recursos (imagens, etc.)
```

## 🎯 Funcionalidades

- ✅ Cadastro de Alunos, Livros e Funcionários
- ✅ Gerenciamento de Empréstimos e Devoluções
- ✅ Sistema de Reservas (FIFO)
- ✅ Pesquisa de Acervo
- ✅ Cálculo automático de multas
- ✅ Validação de CPF, ISBN e Email
- ✅ Logs de auditoria

## 🗄️ Banco de Dados

**Tecnologia:** SQLite (embedded)
**Localização:** `./dados/biblioteca.sqlite`

O banco é criado automaticamente na primeira execução.

### Schema Principal

- `Aluno` - Estudantes cadastrados
- `Funcionario` - Funcionários do sistema
- `Livro` - Acervo da biblioteca
- `Emprestimo` - Controle de empréstimos
- `Reserva` - Fila de reservas
- `Notificacao` - Sistema de notificações
- `Log_Acao` - Auditoria de ações

## 🏗️ Arquitetura

### Camadas

```
Forms (UI) → Servicos (BLL) → AcessoDados (DAL) → SQLite
```

**Regras:**
1. Forms **nunca** acessam DAL diretamente
2. Servicos contêm toda lógica de negócio
3. AcessoDados **apenas** operações de banco
4. Modelos são POCOs (sem lógica)

### Tratamento de Erros

```csharp
try
{
    var result = _servicoAluno.Criar(aluno, idFuncionario);
    MessageBox.Show("Sucesso!", "Alunos", MessageBoxButtons.OK, MessageBoxIcon.Information);
}
catch (ExcecaoValidacao ex)
{
    MessageBox.Show(ex.Message, "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
}
catch (Exception ex)
{
    MessageBox.Show($"Erro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
}
```

## 📋 Regras de Negócio

- **Prazo de empréstimo:** 7 dias
- **Máximo de empréstimos simultâneos:** 3 por aluno
- **Multa por atraso:** R$ 2,00/dia
- **Máximo de renovações:** 2 por empréstimo
- **Validade de reserva:** 7 dias

## 🔐 Segurança

- Validação de CPF com dígitos verificadores
- Proteção contra SQL Injection (queries parametrizadas)
- Perfis de usuário: ADMIN, BIBLIOTECARIO, OPERADOR
- Auditoria completa de ações

## 🛠️ Desenvolvimento

### Convenções de Código

**Nomenclatura:**
- Classes: `PascalCase` (ServicoAluno, AlunoDAL)
- Métodos: `PascalCase` (ObterPorId, Criar)
- Campos privados: `_camelCase` (_servicoAluno)
- Variáveis: `camelCase` (aluno, livro)

**Organização:**
- 1 classe por arquivo
- Servicos começam com `Servico` (ServicoAluno)
- DAL termina com `DAL` (AlunoDAL)
- Forms no root directory

### Adicionando Nova Funcionalidade

1. **Modelo** → `Modelos/NomeEntidade.cs`
2. **DAL** → `AcessoDados/NomeEntidadeDAL.cs`
3. **Service** → `Servicos/ServicoNomeEntidade.cs`
4. **Form** → Root directory

## 📝 Changelog Recente (2025-11-24)

### Qualidade de Código
- ✅ Corrigido 18/22 issues identificadas em auditoria
- ✅ Validação de inputs em todos os Forms
- ✅ Textos em português correto (Usuário, Empréstimo)
- ✅ Nomenclatura consistente (txt_editora, btn_sair)

### Estrutura do Projeto
- ✅ Mesclados 2 projetos em 1 único
- ✅ Removido `telalogin.csproj` duplicado
- ✅ Limpeza de arquivos temporários (bin, obj, .vs)
- ✅ Estrutura simplificada e organizada

## 📚 Documentação

- [CLAUDE.md](../CLAUDE.md) - Guia completo do projeto
- [AUDIT_ISSUES.md](../AUDIT_ISSUES.md) - Auditoria de código
- [CONTRIBUTING.md](../CONTRIBUTING.md) - Guia de contribuição

## 🐛 Troubleshooting

**Build falha:**
```bash
dotnet clean
dotnet restore
dotnet build
```

**Banco não inicializa:**
- Delete `./dados/biblioteca.sqlite`
- Reinicie a aplicação

**Erro de conexão:**
- Verifique se Microsoft.Data.Sqlite está instalado
- Execute `dotnet restore`

## 📄 Licença

Projeto acadêmico - UEMG
