========================================================
  PROTÓTIPO C# - Sistema BibliotecaJK v2.0
  COMPLETO: Model + DAL + BLL
========================================================

📁 ESTRUTURA DO PROJETO
------------------------------------------------------------
Model/
  ├── Pessoa.cs           → Classe base abstrata (Id, Nome, CPF)
  ├── Aluno.cs            → Herda de Pessoa (Matricula, Turma, Telefone, Email)
  ├── Funcionario.cs      → Herda de Pessoa (Cargo, Login, SenhaHash, Perfil)
  ├── Livro.cs            → Entidade de livros do acervo
  ├── Emprestimo.cs       → Entidade de empréstimos
  ├── Reserva.cs          → Entidade de reservas
  └── LogAcao.cs          → Entidade de logs do sistema

DAL/
  ├── AlunoDAL.cs         → CRUD completo de alunos
  ├── FuncionarioDAL.cs   → CRUD completo de funcionários
  ├── LivroDAL.cs         → CRUD completo de livros
  ├── EmprestimoDAL.cs    → CRUD completo de empréstimos
  ├── ReservaDAL.cs       → CRUD completo de reservas
  └── LogAcaoDAL.cs       → CRUD completo de logs

BLL/ ⭐ NOVO!
  ├── ResultadoOperacao.cs → Padronização de retornos
  ├── Exceptions.cs        → Exceções personalizadas
  ├── Validadores.cs       → Validações (CPF, ISBN, Email)
  ├── LogService.cs        → Gerenciamento de logs
  ├── EmprestimoService.cs → Regras de empréstimos ⭐
  ├── ReservaService.cs    → Sistema de reservas (fila FIFO)
  ├── LivroService.cs      → Gerenciamento de livros
  ├── AlunoService.cs      → Gerenciamento de alunos
  └── README_BLL.md        → Documentação da camada BLL

Conexao.cs                → Gerenciador de conexões MySQL
Program.cs                → Menu interativo para testar BLL
schema.sql                → Script de criação do banco de dados
BibliotecaJK.csproj       → Configuração do projeto (.NET 8.0)

🎯 CARACTERÍSTICAS
------------------------------------------------------------
✅ Arquitetura em 3 camadas (Model → DAL → BLL)
✅ Herança OOP com classe base Pessoa
✅ CRUD completo para todas as entidades (DAL)
✅ Lógica de negócio completa (BLL)
✅ Regras de empréstimo (prazo 7 dias, máx 3 simultâneos, multa R$ 2/dia)
✅ Sistema de reservas com fila FIFO
✅ Validações (CPF, ISBN, Email, Matrícula)
✅ Sistema de logs e auditoria
✅ Tratamento de valores nulos (Nullable types)
✅ Uso de using statements para gerenciamento de recursos
✅ Connection pooling com criação de novas conexões
✅ Prepared statements para prevenir SQL Injection
✅ Menu interativo para testes de BLL

🚀 COMO USAR
------------------------------------------------------------
1. CONFIGURAR O BANCO DE DADOS
   - Instale o MySQL Server (versão 5.7 ou superior)
   - Execute o script: mysql -u root < schema.sql
   - Isso criará o banco 'bibliokopke' com dados de teste

2. CONFIGURAR O PROJETO
   - Abra o projeto no Visual Studio 2022 ou VS Code
   - Restaure os pacotes NuGet: dotnet restore
   - Ajuste a connection string em Conexao.cs se necessário

3. EXECUTAR O PROTÓTIPO
   - Compile: dotnet build
   - Execute: dotnet run
   - Use o menu interativo para testar as funcionalidades

⚙️ CONFIGURAÇÃO
------------------------------------------------------------
Connection String (Conexao.cs):
  server=localhost;database=bibliokopke;uid=root;pwd=;

Para alterar:
  - server: endereço do servidor MySQL
  - database: nome do banco de dados
  - uid: usuário do MySQL
  - pwd: senha do MySQL

📊 BANCO DE DADOS
------------------------------------------------------------
Database: bibliokopke

Tabelas:
  - Aluno              (alunos do sistema)
  - Funcionario        (funcionários/bibliotecários)
  - Livro              (acervo de livros)
  - Emprestimo         (empréstimos realizados)
  - Reserva            (reservas de livros)
  - Log_Acao           (auditoria do sistema)

Views:
  - vw_emprestimos_ativos
  - vw_livros_disponiveis
  - vw_reservas_ativas

🔧 TECNOLOGIAS UTILIZADAS
------------------------------------------------------------
- C# 12 (.NET 8.0)
- ADO.NET
- MySQL 8.0
- MySql.Data 9.0.0

📝 MELHORIAS IMPLEMENTADAS
------------------------------------------------------------
v2.0 (Atual):
  ✅ Implementada camada BLL completa (Lógica de Negócio)
  ✅ EmprestimoService com todas regras de negócio
  ✅ ReservaService com sistema de fila FIFO
  ✅ LivroService e AlunoService com validações
  ✅ Validadores (CPF, ISBN, Email)
  ✅ Sistema de logs e auditoria
  ✅ Program.cs atualizado para testar BLL
  ✅ Documentação completa (README_BLL.md)

v1.0:
  ✅ Implementada herança com classe Pessoa
  ✅ Corrigido padrão de conexão (não reutiliza instância)
  ✅ Criado script SQL completo do protótipo
  ✅ Menu interativo para testes
  ✅ Documentação atualizada

🎓 OBSERVAÇÕES
------------------------------------------------------------
- Este é um PROTÓTIPO para fins educacionais e testes
- Não use em produção sem implementar:
  * Hash de senhas (bcrypt/argon2)
  * Validação de dados
  * Tratamento robusto de erros
  * Logging estruturado
  * Testes unitários
  * Pattern Repository/Unit of Work
  * Dependency Injection

📧 SUPORTE
------------------------------------------------------------
Para dúvidas ou problemas, verifique:
  1. Se o MySQL está rodando
  2. Se o banco foi criado (schema.sql)
  3. Se a connection string está correta
  4. Se os pacotes NuGet foram restaurados
