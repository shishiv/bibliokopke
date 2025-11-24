using System.IO;
using Microsoft.Data.Sqlite;

namespace BibliotecaJK;

public static class InicializadorSqlite
{
    private const string HashSenhaAdmin = "7676AAAFB027C825BD9ABAB78B234070E702752F625B752E55E55B48E607E358";

    private const string ScriptCriacao = @"
CREATE TABLE IF NOT EXISTS Aluno (
    id_aluno INTEGER PRIMARY KEY AUTOINCREMENT,
    nome TEXT NOT NULL,
    cpf TEXT UNIQUE NOT NULL,
    matricula TEXT NOT NULL,
    turma TEXT,
    telefone TEXT,
    email TEXT
);
CREATE TABLE IF NOT EXISTS Funcionario (
    id_funcionario INTEGER PRIMARY KEY AUTOINCREMENT,
    nome TEXT NOT NULL,
    cpf TEXT UNIQUE NOT NULL,
    cargo TEXT,
    login TEXT UNIQUE NOT NULL,
    senha_hash TEXT NOT NULL,
    perfil TEXT NOT NULL
);
CREATE TABLE IF NOT EXISTS Livro (
    id_livro INTEGER PRIMARY KEY AUTOINCREMENT,
    titulo TEXT NOT NULL,
    autor TEXT,
    isbn TEXT UNIQUE,
    editora TEXT,
    ano_publicacao INTEGER,
    quantidade_total INTEGER NOT NULL,
    quantidade_disponivel INTEGER NOT NULL,
    localizacao TEXT
);
CREATE TABLE IF NOT EXISTS Emprestimo (
    id_emprestimo INTEGER PRIMARY KEY AUTOINCREMENT,
    id_aluno INTEGER NOT NULL,
    id_livro INTEGER NOT NULL,
    data_emprestimo TEXT NOT NULL,
    data_prevista TEXT NOT NULL,
    data_devolucao TEXT,
    multa REAL DEFAULT 0,
    FOREIGN KEY (id_aluno) REFERENCES Aluno(id_aluno),
    FOREIGN KEY (id_livro) REFERENCES Livro(id_livro)
);
CREATE TABLE IF NOT EXISTS Reserva (
    id_reserva INTEGER PRIMARY KEY AUTOINCREMENT,
    id_aluno INTEGER NOT NULL,
    id_livro INTEGER NOT NULL,
    data_reserva TEXT NOT NULL,
    status TEXT DEFAULT 'ATIVA',
    FOREIGN KEY (id_aluno) REFERENCES Aluno(id_aluno),
    FOREIGN KEY (id_livro) REFERENCES Livro(id_livro)
);
CREATE TABLE IF NOT EXISTS Log_Acao (
    id_log INTEGER PRIMARY KEY AUTOINCREMENT,
    id_funcionario INTEGER,
    acao TEXT,
    descricao TEXT,
    data_hora TEXT DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_funcionario) REFERENCES Funcionario(id_funcionario)
);
";

    private const string ScriptDadosIniciais = @"
INSERT INTO Aluno (nome, cpf, matricula, turma, telefone, email)
SELECT 'João Silva', '12345678901', '2025A001', '3A', '11999998888', 'joao@email.com'
WHERE NOT EXISTS (SELECT 1 FROM Aluno WHERE cpf = '12345678901');
INSERT INTO Aluno (nome, cpf, matricula, turma, telefone, email)
SELECT 'Maria Souza', '98765432100', '2025B002', '2B', '11988887777', 'maria@email.com'
WHERE NOT EXISTS (SELECT 1 FROM Aluno WHERE cpf = '98765432100');
INSERT INTO Funcionario (nome, cpf, cargo, login, senha_hash, perfil)
SELECT 'Administrador', '11122233344', 'Administrador', 'admin', '7676AAAFB027C825BD9ABAB78B234070E702752F625B752E55E55B48E607E358', 'ADMIN'
WHERE NOT EXISTS (SELECT 1 FROM Funcionario WHERE login = 'admin');
INSERT INTO Livro (titulo, autor, isbn, editora, ano_publicacao, quantidade_total, quantidade_disponivel, localizacao)
SELECT 'Dom Casmurro', 'Machado de Assis', '9788535910665', 'Companhia das Letras', 2019, 3, 3, 'Estante A'
WHERE NOT EXISTS (SELECT 1 FROM Livro WHERE isbn = '9788535910665');
INSERT INTO Livro (titulo, autor, isbn, editora, ano_publicacao, quantidade_total, quantidade_disponivel, localizacao)
SELECT 'O Pequeno Príncipe', 'Antoine de Saint-Exupéry', '9788525056019', 'HarperCollins', 2020, 2, 2, 'Estante B'
WHERE NOT EXISTS (SELECT 1 FROM Livro WHERE isbn = '9788525056019');
";

    public static void GarantirEstrutura(string caminhoArquivo)
    {
        var novoArquivo = !File.Exists(caminhoArquivo);

        using var conexao = new SqliteConnection($"Data Source={caminhoArquivo}");
        conexao.Open();

        using (var comando = conexao.CreateCommand())
        {
            comando.CommandText = ScriptCriacao;
            comando.ExecuteNonQuery();
        }

        if (novoArquivo)
        {
            using var comando = conexao.CreateCommand();
            comando.CommandText = ScriptDadosIniciais;
            comando.ExecuteNonQuery();
        }

        GarantirSenhaAdmin(conexao);
    }

    private static void GarantirSenhaAdmin(SqliteConnection conexao)
    {
        using var comando = conexao.CreateCommand();
        comando.CommandText = @"UPDATE Funcionario
                                SET senha_hash = @hash
                                WHERE login = 'admin'";
        comando.Parameters.AddWithValue("@hash", HashSenhaAdmin);
        comando.ExecuteNonQuery();
    }
}
