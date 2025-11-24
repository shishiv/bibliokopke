using System;
using System.Collections.Generic;
using BibliotecaJK.Modelos;
using BibliotecaJK.Utilitarios;
using System.Data.Common;

namespace BibliotecaJK.AcessoDados;

public class RepositorioReserva
{
    public int Inserir(Reserva reserva)
    {
        using var conn = Conexao.ObterConexao();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO Reserva (id_aluno, id_livro, data_reserva, status) VALUES (@idaluno,@idlivro,@datares,@status)";
        PreencherParametros(cmd, reserva);

        conn.Open();
        cmd.ExecuteNonQuery();
        return (int)Conexao.ObterUltimoId(conn);
    }

    public void Atualizar(Reserva reserva)
    {
        using var conn = Conexao.ObterConexao();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE Reserva SET id_aluno=@idaluno, id_livro=@idlivro, data_reserva=@datares, status=@status WHERE id_reserva=@id";
        PreencherParametros(cmd, reserva);
        cmd.AdicionarParametro("@id", reserva.Id);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void AtualizarStatus(int reservaId, string status)
    {
        using var conn = Conexao.ObterConexao();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE Reserva SET status=@status WHERE id_reserva=@id";
        cmd.AdicionarParametro("@status", status);
        cmd.AdicionarParametro("@id", reservaId);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void Excluir(int id)
    {
        using var conn = Conexao.ObterConexao();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Reserva WHERE id_reserva=@id";
        cmd.AdicionarParametro("@id", id);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public List<Reserva> Listar()
    {
        return Buscar("SELECT * FROM Reserva ORDER BY data_reserva DESC");
    }

    public List<Reserva> ListarAtivas()
    {
        return Buscar("SELECT * FROM Reserva WHERE status='ATIVA' ORDER BY data_reserva DESC");
    }

    public List<Reserva> ListarPorAluno(int alunoId)
    {
        return Buscar("SELECT * FROM Reserva WHERE id_aluno=@id ORDER BY data_reserva DESC", ("@id", alunoId));
    }

    public Reserva? ObterPorId(int id)
    {
        const string sql = "SELECT * FROM Reserva WHERE id_reserva=@id LIMIT 1";
        return ObterReserva(sql, ("@id", id));
    }

    private static void PreencherParametros(DbCommand cmd, Reserva reserva)
    {
        cmd.AdicionarParametro("@idaluno", reserva.IdAluno);
        cmd.AdicionarParametro("@idlivro", reserva.IdLivro);
        cmd.AdicionarParametro("@datares", reserva.DataReserva.Date);
        cmd.AdicionarParametro("@status", reserva.Status);
    }

    private static List<Reserva> Buscar(string sql, params (string Nome, object? Valor)[] parametros)
    {
        var lista = new List<Reserva>();
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

    private static Reserva? ObterReserva(string sql, params (string Nome, object? Valor)[] parametros)
    {
        using var conn = Conexao.ObterConexao();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        AdicionarParametros(cmd, parametros);

        conn.Open();
        using var reader = cmd.ExecuteReader();
        return reader.Read() ? Map(reader) : null;
    }

    private static Reserva Map(DbDataReader reader)
    {
        int idx(string nome) => reader.GetOrdinal(nome);
        return new Reserva
        {
            Id = reader.GetInt32(idx("id_reserva")),
            IdAluno = reader.GetInt32(idx("id_aluno")),
            IdLivro = reader.GetInt32(idx("id_livro")),
            DataReserva = reader.GetDateTime(idx("data_reserva")),
            Status = reader.GetString(idx("status"))
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
