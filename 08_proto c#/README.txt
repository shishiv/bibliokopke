BibliotecaJK - Backend completo (C# + ADO.NET + MySQL)
------------------------------------------------------------

Conteúdo atualizado:
- Model/: Aluno, Funcionario, Livro, Emprestimo, Reserva, LogAcao
- DAL/: implementações CRUD para todas as entidades
- Conexao.cs: conexão com MySQL (root sem senha)
- Program.cs: teste de conexão
- BibliotecaJK.csproj: referência MySql.Data

Observações:
- O nome dos campos dos modelos segue o schema SQL presente no arquivo 'BD Biblioteca JK.sql' que você forneceu.
- Para usar, extraia o ZIP e abra no Visual Studio 2022. Restaure pacotes NuGet (MySql.Data).
- Caso o banco esteja em outro host/usuário/senha, ajuste a connectionString em Conexao.cs.
