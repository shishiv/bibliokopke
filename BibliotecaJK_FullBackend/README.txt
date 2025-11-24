BibliotecaJK - Backend completo (C# + ADO.NET + SQLite)
------------------------------------------------------------

Conteúdo atualizado:
- Modelos/: entidades Aluno, Funcionario, Livro, Emprestimo, Reserva, LogAcao
- AcessoDados/: repositórios genéricos sobre SQLite (DbCommand/DbConnection)
- Utilitarios/: validações, hashing de senha, extensões de comando
- Servicos/: regras de negócio (autenticação, cadastros, empréstimos, reservas, painel)
- Conexao.cs / InicializadorSqlite.cs: gerenciamento do arquivo SQLite e seed automático
- Program.cs: inicializa o banco simplificado antes de abrir o WinForms

Uso do banco de dados:
- O aplicativo sempre utiliza SQLite. O arquivo `./dados/biblioteca.sqlite` é criado automaticamente com o schema e dados básicos (incluindo usuário `admin` com senha `admin@123`).
- Para armazenar o arquivo em outro local, altere `Conexao.ConfigurarSqlite` (ver `Programa.cs`).

Observações:
- O nome dos campos segue o schema original presente em `BD Biblioteca JK.sql`.
- Para usar no Visual Studio/CLI, restaure os pacotes NuGet (`Microsoft.Data.Sqlite`).
- Todos os formulários WinForms já consomem as camadas de serviço em português.
