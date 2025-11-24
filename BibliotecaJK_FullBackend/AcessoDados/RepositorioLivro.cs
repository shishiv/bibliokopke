using System;
using System.Collections.Generic;
using BibliotecaJK.Modelos;
using BibliotecaJK.Utilitarios;
using System.Data.Common;

namespace BibliotecaJK.AcessoDados;

public class RepositorioLivro
{
    public int Inserir(Livro livro)
    {
        using var conn = Conexao.ObterConexao();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO Livro (titulo, autor, isbn, editora, ano_publicacao, quantidade_total, quantidade_disponivel, localizacao)
                            VALUES (@titulo, @autor, @isbn, @editora, @ano, @total, @disp, @loc)";
        PreencherParametros(cmd, livro);

        conn.Open();
        cmd.ExecuteNonQuery();
        return (int)Conexao.ObterUltimoId(conn);
    }

    public void Atualizar(Livro livro)
    {
        using var conn = Conexao.ObterConexao();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"UPDATE Livro SET titulo=@titulo, autor=@autor, isbn=@isbn, editora=@editora, ano_publicacao=@ano,
                            quantidade_total=@total, quantidade_disponivel=@disp, localizacao=@loc WHERE id_livro=@id";
        PreencherParametros(cmd, livro);
        cmd.AdicionarParametro("@id", livro.Id);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void AtualizarDisponibilidade(int idLivro, int quantidadeDisponivel)
    {
        using var conn = Conexao.ObterConexao();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE Livro SET quantidade_disponivel=@disp WHERE id_livro=@id";
        cmd.AdicionarParametro("@disp", quantidadeDisponivel);
        cmd.AdicionarParametro("@id", idLivro);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void Excluir(int id)
    {
        using var conn = Conexao.ObterConexao();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Livro WHERE id_livro=@id";
        cmd.AdicionarParametro("@id", id);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public List<Livro> Listar()
    {
        return Buscar(null);
    }

    public List<Livro> Buscar(string? termo)
    {
        var lista = new List<Livro>();
        using var conn = Conexao.ObterConexao();
        using var cmd = conn.CreateCommand();

        if (string.IsNullOrWhiteSpace(termo))
        {
            cmd.CommandText = "SELECT * FROM Livro ORDER BY titulo";
        }
        else
        {
            cmd.CommandText = "SELECT * FROM Livro WHERE titulo LIKE @termo OR autor LIKE @termo OR isbn LIKE @termo ORDER BY titulo";
            cmd.AdicionarParametro("@termo", $"%{termo}%");
        }

        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(Map(reader));
        }

        return lista;
    }

    public Livro? ObterPorId(int id)
    {
        const string sql = "SELECT * FROM Livro WHERE id_livro=@id LIMIT 1";
        return ObterLivro(sql, ("@id", id));
    }

    public Livro? ObterPorIsbn(string isbn)
    {
        const string sql = "SELECT * FROM Livro WHERE isbn=@isbn LIMIT 1";
        return ObterLivro(sql, ("@isbn", isbn));
    }

    private static Livro? ObterLivro(string sql, params (string Nome, object? Valor)[] parametros)
    {
        using var conn = Conexao.ObterConexao();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        AdicionarParametros(cmd, parametros);

        conn.Open();
        using var reader = cmd.ExecuteReader();
        return reader.Read() ? Map(reader) : null;
    }

    private static void PreencherParametros(DbCommand cmd, Livro livro)
    {
        cmd.AdicionarParametro("@titulo", livro.Titulo);
        cmd.AdicionarParametro("@autor", (object?)livro.Autor ?? DBNull.Value);
        cmd.AdicionarParametro("@isbn", (object?)livro.ISBN ?? DBNull.Value);
        cmd.AdicionarParametro("@editora", (object?)livro.Editora ?? DBNull.Value);
        cmd.AdicionarParametro("@ano", (object?)livro.AnoPublicacao ?? DBNull.Value);
        cmd.AdicionarParametro("@total", livro.QuantidadeTotal);
        cmd.AdicionarParametro("@disp", livro.QuantidadeDisponivel);
        cmd.AdicionarParametro("@loc", (object?)livro.Localizacao ?? DBNull.Value);
    }

    private static Livro Map(DbDataReader reader)
    {
        int idx(string nome) => reader.GetOrdinal(nome);
        return new Livro
        {
            Id = reader.GetInt32(idx("id_livro")),
            Titulo = reader.GetString(idx("titulo")),
            Autor = reader.IsDBNull(idx("autor")) ? null : reader.GetString(idx("autor")),
            ISBN = reader.IsDBNull(idx("isbn")) ? null : reader.GetString(idx("isbn")),
            Editora = reader.IsDBNull(idx("editora")) ? null : reader.GetString(idx("editora")),
            AnoPublicacao = reader.IsDBNull(idx("ano_publicacao")) ? null : reader.GetInt32(idx("ano_publicacao")),
            QuantidadeTotal = reader.GetInt32(idx("quantidade_total")),
            QuantidadeDisponivel = reader.GetInt32(idx("quantidade_disponivel")),
            Localizacao = reader.IsDBNull(idx("localizacao")) ? null : reader.GetString(idx("localizacao"))
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
