using System;
using System.Collections.Generic;
using BibliotecaJK.Modelos;
using BibliotecaJK.Utilitarios;
using System.Data.Common;

namespace BibliotecaJK.AcessoDados;

public class RepositorioLogAcao
{
    public int Inserir(LogAcao log)
    {
        using var conn = Conexao.ObterConexao();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO Log_Acao (id_funcionario, acao, descricao, data_hora) VALUES (@idfunc,@acao,@desc,@datahora)";
        cmd.AdicionarParametro("@idfunc", (object?)log.IdFuncionario ?? DBNull.Value);
        cmd.AdicionarParametro("@acao", (object?)log.Acao ?? DBNull.Value);
        cmd.AdicionarParametro("@desc", (object?)log.Descricao ?? DBNull.Value);
        cmd.AdicionarParametro("@datahora", log.DataHora);

        conn.Open();
        cmd.ExecuteNonQuery();
        return (int)Conexao.ObterUltimoId(conn);
    }

    public List<LogAcao> Listar()
    {
        var lista = new List<LogAcao>();
        using var conn = Conexao.ObterConexao();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Log_Acao ORDER BY data_hora DESC";

        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(Map(reader));
        }

        return lista;
    }

    public LogAcao? ObterPorId(int id)
    {
        const string sql = "SELECT * FROM Log_Acao WHERE id_log=@id LIMIT 1";
        return ObterLog(sql, ("@id", id));
    }

    public void Excluir(int id)
    {
        using var conn = Conexao.ObterConexao();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Log_Acao WHERE id_log=@id";
        cmd.AdicionarParametro("@id", id);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    private static LogAcao? ObterLog(string sql, params (string Nome, object? Valor)[] parametros)
    {
        using var conn = Conexao.ObterConexao();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        AdicionarParametros(cmd, parametros);

        conn.Open();
        using var reader = cmd.ExecuteReader();
        return reader.Read() ? Map(reader) : null;
    }

    private static LogAcao Map(DbDataReader reader)
    {
        int idx(string nome) => reader.GetOrdinal(nome);
        return new LogAcao
        {
            Id = reader.GetInt32(idx("id_log")),
            IdFuncionario = reader.IsDBNull(idx("id_funcionario")) ? null : reader.GetInt32(idx("id_funcionario")),
            Acao = reader.IsDBNull(idx("acao")) ? null : reader.GetString(idx("acao")),
            Descricao = reader.IsDBNull(idx("descricao")) ? null : reader.GetString(idx("descricao")),
            DataHora = reader.GetDateTime(idx("data_hora"))
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
