using System;
using System.Collections.Generic;
using System.Data.Common;
using BibliotecaJK.Modelos;
using BibliotecaJK.Utilitarios;

namespace BibliotecaJK.AcessoDados;

public class RepositorioAluno
{
    public int Inserir(Aluno aluno)
    {
        using var conn = Conexao.ObterConexao();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO Aluno (nome, cpf, matricula, turma, telefone, email) VALUES (@nome,@cpf,@matricula,@turma,@telefone,@email)";
        PreencherParametrosBasicos(cmd, aluno);

        conn.Open();
        cmd.ExecuteNonQuery();
        return (int)Conexao.ObterUltimoId(conn);
    }

    public void Atualizar(Aluno aluno)
    {
        using var conn = Conexao.ObterConexao();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE Aluno SET nome=@nome, cpf=@cpf, matricula=@matricula, turma=@turma, telefone=@telefone, email=@email WHERE id_aluno=@id";
        PreencherParametrosBasicos(cmd, aluno);
        cmd.AdicionarParametro("@id", aluno.Id);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void Excluir(int id)
    {
        using var conn = Conexao.ObterConexao();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Aluno WHERE id_aluno=@id";
        cmd.AdicionarParametro("@id", id);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public List<Aluno> Listar()
    {
        return Buscar(null);
    }

    public List<Aluno> Buscar(string? termo)
    {
        var lista = new List<Aluno>();
        using var conn = Conexao.ObterConexao();
        using var cmd = conn.CreateCommand();

        if (string.IsNullOrWhiteSpace(termo))
        {
            cmd.CommandText = "SELECT * FROM Aluno ORDER BY nome";
        }
        else
        {
            cmd.CommandText = "SELECT * FROM Aluno WHERE nome LIKE @termo OR matricula LIKE @termo OR cpf LIKE @termo ORDER BY nome";
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

    public Aluno? ObterPorId(int id)
    {
        const string sql = "SELECT * FROM Aluno WHERE id_aluno=@id LIMIT 1";
        return ObterAluno(sql, ("@id", id));
    }

    public Aluno? ObterPorMatricula(string matricula)
    {
        const string sql = "SELECT * FROM Aluno WHERE matricula=@matricula LIMIT 1";
        return ObterAluno(sql, ("@matricula", matricula));
    }

    public Aluno? ObterPorCpf(string cpf)
    {
        const string sql = "SELECT * FROM Aluno WHERE cpf=@cpf LIMIT 1";
        return ObterAluno(sql, ("@cpf", cpf));
    }

    private static Aluno? ObterAluno(string sql, params (string Nome, object? Valor)[] parametros)
    {
        using var conn = Conexao.ObterConexao();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        AdicionarParametros(cmd, parametros);

        conn.Open();
        using var reader = cmd.ExecuteReader();
        return reader.Read() ? Map(reader) : null;
    }

    private static void PreencherParametrosBasicos(DbCommand cmd, Aluno aluno)
    {
        cmd.AdicionarParametro("@nome", aluno.Nome);
        cmd.AdicionarParametro("@cpf", aluno.CPF);
        cmd.AdicionarParametro("@matricula", aluno.Matricula);
        cmd.AdicionarParametro("@turma", (object?)aluno.Turma ?? DBNull.Value);
        cmd.AdicionarParametro("@telefone", (object?)aluno.Telefone ?? DBNull.Value);
        cmd.AdicionarParametro("@email", (object?)aluno.Email ?? DBNull.Value);
    }

    private static Aluno Map(DbDataReader reader)
    {
        return new Aluno
        {
            Id = reader.GetInt32(reader.GetOrdinal("id_aluno")),
            Nome = reader.GetString(reader.GetOrdinal("nome")),
            CPF = reader.GetString(reader.GetOrdinal("cpf")),
            Matricula = reader.GetString(reader.GetOrdinal("matricula")),
            Turma = reader.IsDBNull(reader.GetOrdinal("turma")) ? null : reader.GetString(reader.GetOrdinal("turma")),
            Telefone = reader.IsDBNull(reader.GetOrdinal("telefone")) ? null : reader.GetString(reader.GetOrdinal("telefone")),
            Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email"))
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
