using System;
using System.Collections.Generic;
using BibliotecaJK.Modelos;
using BibliotecaJK.Utilitarios;
using System.Data.Common;

namespace BibliotecaJK.AcessoDados;

public class RepositorioEmprestimo
{
    public int Inserir(Emprestimo emprestimo)
    {
        using var conn = Conexao.ObterConexao();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO Emprestimo (id_aluno, id_livro, data_emprestimo, data_prevista, data_devolucao, multa)
                            VALUES (@idaluno,@idlivro,@dataemp,@dataprev,@datadev,@multa)";
        PreencherParametros(cmd, emprestimo);

        conn.Open();
        cmd.ExecuteNonQuery();
        return (int)Conexao.ObterUltimoId(conn);
    }

    public void Atualizar(Emprestimo emprestimo)
    {
        using var conn = Conexao.ObterConexao();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"UPDATE Emprestimo SET id_aluno=@idaluno, id_livro=@idlivro, data_emprestimo=@dataemp, data_prevista=@dataprev,
                            data_devolucao=@datadev, multa=@multa WHERE id_emprestimo=@id";
        PreencherParametros(cmd, emprestimo);
        cmd.AdicionarParametro("@id", emprestimo.Id);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void RegistrarDevolucao(int emprestimoId, DateTime dataDevolucao, decimal multa)
    {
        using var conn = Conexao.ObterConexao();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE Emprestimo SET data_devolucao=@datadev, multa=@multa WHERE id_emprestimo=@id";
        cmd.AdicionarParametro("@datadev", dataDevolucao.Date);
        cmd.AdicionarParametro("@multa", multa);
        cmd.AdicionarParametro("@id", emprestimoId);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void Excluir(int id)
    {
        using var conn = Conexao.ObterConexao();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Emprestimo WHERE id_emprestimo=@id";
        cmd.AdicionarParametro("@id", id);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public Emprestimo? ObterPorId(int id)
    {
        const string sql = "SELECT * FROM Emprestimo WHERE id_emprestimo=@id LIMIT 1";
        return ObterEmprestimo(sql, ("@id", id));
    }

    public Emprestimo? ObterEmprestimoAtivoPorLivro(int livroId)
    {
        const string sql = "SELECT * FROM Emprestimo WHERE id_livro=@id AND data_devolucao IS NULL LIMIT 1";
        return ObterEmprestimo(sql, ("@id", livroId));
    }

    public List<Emprestimo> Listar()
    {
        return Buscar("SELECT * FROM Emprestimo ORDER BY data_emprestimo DESC");
    }

    public List<Emprestimo> ListarAtivos()
    {
        return Buscar("SELECT * FROM Emprestimo WHERE data_devolucao IS NULL ORDER BY data_emprestimo DESC");
    }

    public List<Emprestimo> ListarPorAluno(int alunoId, bool somenteAtivos = false)
    {
        var sql = "SELECT * FROM Emprestimo WHERE id_aluno=@id" + (somenteAtivos ? " AND data_devolucao IS NULL" : string.Empty) + " ORDER BY data_emprestimo DESC";
        return Buscar(sql, ("@id", alunoId));
    }

    private static List<Emprestimo> Buscar(string sql, params (string Nome, object? Valor)[] parametros)
    {
        var lista = new List<Emprestimo>();
        using var conn = Conexao.ObterConexao();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        AdicionarParametros(cmd, parametros);

        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(Map(reader));
        }

        return lista;
    }

    private static Emprestimo? ObterEmprestimo(string sql, params (string Nome, object? Valor)[] parametros)
    {
        using var conn = Conexao.ObterConexao();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        AdicionarParametros(cmd, parametros);

        conn.Open();
        using var reader = cmd.ExecuteReader();
        return reader.Read() ? Map(reader) : null;
    }

    private static void PreencherParametros(DbCommand cmd, Emprestimo emprestimo)
    {
        cmd.AdicionarParametro("@idaluno", emprestimo.IdAluno);
        cmd.AdicionarParametro("@idlivro", emprestimo.IdLivro);
        cmd.AdicionarParametro("@dataemp", emprestimo.DataEmprestimo.Date);
        cmd.AdicionarParametro("@dataprev", emprestimo.DataPrevista.Date);
        cmd.AdicionarParametro("@datadev", (object?)emprestimo.DataDevolucao?.Date ?? DBNull.Value);
        cmd.AdicionarParametro("@multa", emprestimo.Multa);
    }

    private static Emprestimo Map(DbDataReader reader)
    {
        int idx(string nome) => reader.GetOrdinal(nome);
        return new Emprestimo
        {
            Id = reader.GetInt32(idx("id_emprestimo")),
            IdAluno = reader.GetInt32(idx("id_aluno")),
            IdLivro = reader.GetInt32(idx("id_livro")),
            DataEmprestimo = reader.GetDateTime(idx("data_emprestimo")),
            DataPrevista = reader.GetDateTime(idx("data_prevista")),
            DataDevolucao = reader.IsDBNull(idx("data_devolucao")) ? null : reader.GetDateTime(idx("data_devolucao")),
            Multa = reader.GetDecimal(idx("multa"))
        };
    }

    private static void AdicionarParametros(DbCommand cmd, params (string Nome, object? Valor)[] parametros)
    {
        foreach (var (nome, valor) in parametros)
        {
            cmd.AdicionarParametro(nome, valor);
        }
    }
}
