using MySql.Data.MySqlClient;
using BibliotecaJK.Model;
using System.Collections.Generic;
using System;

namespace BibliotecaJK.DAL
{
    public class LogAcaoDAL
    {
        public void Inserir(LogAcao log)
        {
            var conn = Conexao.GetConnection();
            string sql = "INSERT INTO Log_Acao (id_funcionario, acao, descricao, data_hora) VALUES (@idfunc,@acao,@desc,@datahora)";
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@idfunc", (object?)log.IdFuncionario ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@acao", (object?)log.Acao ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@desc", (object?)log.Descricao ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@datahora", log.DataHora);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public List<LogAcao> Listar()
        {
            var lista = new List<LogAcao>();
            var conn = Conexao.GetConnection();
            string sql = "SELECT * FROM Log_Acao";
            using (var cmd = new MySqlCommand(sql, conn))
            {
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new LogAcao
                    {
                        Id = reader.GetInt32("id_log"),
                        IdFuncionario = reader.IsDBNull(reader.GetOrdinal("id_funcionario")) ? (int?)null : reader.GetInt32("id_funcionario"),
                        Acao = reader.IsDBNull(reader.GetOrdinal("acao")) ? null : reader.GetString("acao"),
                        Descricao = reader.IsDBNull(reader.GetOrdinal("descricao")) ? null : reader.GetString("descricao"),
                        DataHora = reader.GetDateTime("data_hora")
                    });
                }
                conn.Close();
            }
            return lista;
        }

        public LogAcao? ObterPorId(int id)
        {
            var conn = Conexao.GetConnection();
            string sql = "SELECT * FROM Log_Acao WHERE id_log=@id";
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    var l = new LogAcao
                    {
                        Id = reader.GetInt32("id_log"),
                        IdFuncionario = reader.IsDBNull(reader.GetOrdinal("id_funcionario")) ? (int?)null : reader.GetInt32("id_funcionario"),
                        Acao = reader.IsDBNull(reader.GetOrdinal("acao")) ? null : reader.GetString("acao"),
                        Descricao = reader.IsDBNull(reader.GetOrdinal("descricao")) ? null : reader.GetString("descricao"),
                        DataHora = reader.GetDateTime("data_hora")
                    };
                    conn.Close();
                    return l;
                }
                conn.Close();
            }
            return null;
        }

        public void Atualizar(LogAcao log)
        {
            var conn = Conexao.GetConnection();
            string sql = "UPDATE Log_Acao SET id_funcionario=@idfunc, acao=@acao, descricao=@desc, data_hora=@datahora WHERE id_log=@id";
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@idfunc", (object?)log.IdFuncionario ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@acao", (object?)log.Acao ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@desc", (object?)log.Descricao ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@datahora", log.DataHora);
                cmd.Parameters.AddWithValue("@id", log.Id);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void Excluir(int id)
        {
            var conn = Conexao.GetConnection();
            string sql = "DELETE FROM Log_Acao WHERE id_log=@id";
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}
