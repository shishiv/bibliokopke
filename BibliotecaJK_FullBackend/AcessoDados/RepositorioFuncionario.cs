using System;
using System.Collections.Generic;
using BibliotecaJK.Modelos;
using BibliotecaJK.Utilitarios;
using System.Data.Common;

namespace BibliotecaJK.AcessoDados;

public class RepositorioFuncionario
{
    public int Inserir(Funcionario funcionario)
    {
        using var conn = Conexao.ObterConexao();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO Funcionario (nome, cpf, cargo, login, senha_hash, perfil) VALUES (@nome,@cpf,@cargo,@login,@senha,@perfil)";
        PreencherParametros(cmd, funcionario);

        conn.Open();
        cmd.ExecuteNonQuery();
        return (int)Conexao.ObterUltimoId(conn);
    }

    public void Atualizar(Funcionario funcionario)
    {
        using var conn = Conexao.ObterConexao();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE Funcionario SET nome=@nome, cpf=@cpf, cargo=@cargo, login=@login, senha_hash=@senha, perfil=@perfil WHERE id_funcionario=@id";
        PreencherParametros(cmd, funcionario);
        cmd.AdicionarParametro("@id", funcionario.Id);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void Excluir(int id)
    {
        using var conn = Conexao.ObterConexao();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Funcionario WHERE id_funcionario=@id";
        cmd.AdicionarParametro("@id", id);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public List<Funcionario> Listar()
    {
        return Buscar(null);
    }

    public List<Funcionario> Buscar(string? termo)
    {
        var lista = new List<Funcionario>();
        using var conn = Conexao.ObterConexao();
        using var cmd = conn.CreateCommand();

        if (string.IsNullOrWhiteSpace(termo))
        {
            cmd.CommandText = "SELECT * FROM Funcionario ORDER BY nome";
        }
        else
        {
            cmd.CommandText = "SELECT * FROM Funcionario WHERE nome LIKE @termo OR login LIKE @termo OR cpf LIKE @termo ORDER BY nome";
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

    public Funcionario? ObterPorId(int id)
    {
        const string sql = "SELECT * FROM Funcionario WHERE id_funcionario=@id LIMIT 1";
        return ObterFuncionario(sql, ("@id", id));
    }

    public Funcionario? ObterPorLogin(string login)
    {
        const string sql = "SELECT * FROM Funcionario WHERE login=@login LIMIT 1";
        return ObterFuncionario(sql, ("@login", login));
    }

    public Funcionario? ObterPorCpf(string cpf)
    {
        const string sql = "SELECT * FROM Funcionario WHERE cpf=@cpf LIMIT 1";
        return ObterFuncionario(sql, ("@cpf", cpf));
    }

    private static Funcionario? ObterFuncionario(string sql, params (string Nome, object? Valor)[] parametros)
    {
        using var conn = Conexao.ObterConexao();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        AdicionarParametros(cmd, parametros);

        conn.Open();
        using var reader = cmd.ExecuteReader();
        return reader.Read() ? Map(reader) : null;
    }

    private static void PreencherParametros(DbCommand cmd, Funcionario funcionario)
    {
        cmd.AdicionarParametro("@nome", funcionario.Nome);
        cmd.AdicionarParametro("@cpf", funcionario.CPF);
        cmd.AdicionarParametro("@cargo", (object?)funcionario.Cargo ?? DBNull.Value);
        cmd.AdicionarParametro("@login", funcionario.Login);
        cmd.AdicionarParametro("@senha", funcionario.SenhaHash);
        cmd.AdicionarParametro("@perfil", funcionario.Perfil);
    }

    private static Funcionario Map(DbDataReader reader)
    {
        return new Funcionario
        {
            Id = reader.GetInt32(reader.GetOrdinal("id_funcionario")),
            Nome = reader.GetString(reader.GetOrdinal("nome")),
            CPF = reader.GetString(reader.GetOrdinal("cpf")),
            Cargo = reader.IsDBNull(reader.GetOrdinal("cargo")) ? null : reader.GetString(reader.GetOrdinal("cargo")),
            Login = reader.GetString(reader.GetOrdinal("login")),
            SenhaHash = reader.GetString(reader.GetOrdinal("senha_hash")),
            Perfil = reader.GetString(reader.GetOrdinal("perfil"))
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
