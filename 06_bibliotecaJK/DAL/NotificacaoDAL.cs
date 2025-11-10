using Npgsql;
using BibliotecaJK.Model;
using System;
using System.Collections.Generic;

namespace BibliotecaJK.DAL
{
    public class NotificacaoDAL
    {
        /// <summary>
        /// Lista todas as notificações ordenadas por prioridade e data
        /// </summary>
        public List<Notificacao> Listar()
        {
            var lista = new List<Notificacao>();
            var conn = Conexao.GetConnection();

            string sql = @"SELECT * FROM Notificacao
                          ORDER BY
                            CASE prioridade
                              WHEN 'URGENTE' THEN 1
                              WHEN 'ALTA' THEN 2
                              WHEN 'NORMAL' THEN 3
                              WHEN 'BAIXA' THEN 4
                            END,
                            data_criacao DESC";

            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(MapearNotificacao(reader));
                }
                conn.Close();
            }
            return lista;
        }

        /// <summary>
        /// Lista notificações não lidas
        /// </summary>
        public List<Notificacao> ListarNaoLidas()
        {
            var lista = new List<Notificacao>();
            var conn = Conexao.GetConnection();

            string sql = @"SELECT * FROM Notificacao
                          WHERE lida = FALSE
                          ORDER BY
                            CASE prioridade
                              WHEN 'URGENTE' THEN 1
                              WHEN 'ALTA' THEN 2
                              WHEN 'NORMAL' THEN 3
                              WHEN 'BAIXA' THEN 4
                            END,
                            data_criacao DESC";

            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(MapearNotificacao(reader));
                }
                conn.Close();
            }
            return lista;
        }

        /// <summary>
        /// Lista notificações de um funcionário específico
        /// </summary>
        public List<Notificacao> ListarPorFuncionario(int idFuncionario)
        {
            var lista = new List<Notificacao>();
            var conn = Conexao.GetConnection();

            string sql = @"SELECT * FROM Notificacao
                          WHERE id_funcionario = @id_funcionario OR id_funcionario IS NULL
                          ORDER BY
                            CASE prioridade
                              WHEN 'URGENTE' THEN 1
                              WHEN 'ALTA' THEN 2
                              WHEN 'NORMAL' THEN 3
                              WHEN 'BAIXA' THEN 4
                            END,
                            data_criacao DESC";

            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id_funcionario", idFuncionario);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(MapearNotificacao(reader));
                }
                conn.Close();
            }
            return lista;
        }

        /// <summary>
        /// Conta quantas notificações não lidas existem
        /// </summary>
        public int ContarNaoLidas()
        {
            var conn = Conexao.GetConnection();
            string sql = "SELECT COUNT(*) FROM Notificacao WHERE lida = FALSE";

            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                conn.Open();
                var result = cmd.ExecuteScalar();
                conn.Close();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        /// <summary>
        /// Marca uma notificação como lida
        /// </summary>
        public void MarcarComoLida(int idNotificacao)
        {
            var conn = Conexao.GetConnection();
            string sql = "UPDATE Notificacao SET lida = TRUE, data_leitura = CURRENT_TIMESTAMP WHERE id_notificacao = @id";

            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", idNotificacao);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        /// <summary>
        /// Marca todas as notificações como lidas
        /// </summary>
        public void MarcarTodasComoLidas()
        {
            var conn = Conexao.GetConnection();
            string sql = "UPDATE Notificacao SET lida = TRUE, data_leitura = CURRENT_TIMESTAMP WHERE lida = FALSE";

            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        /// <summary>
        /// Obtém uma notificação por ID
        /// </summary>
        public Notificacao? ObterPorId(int id)
        {
            var conn = Conexao.GetConnection();
            string sql = "SELECT * FROM Notificacao WHERE id_notificacao = @id";

            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    var notificacao = MapearNotificacao(reader);
                    conn.Close();
                    return notificacao;
                }
                conn.Close();
            }
            return null;
        }

        /// <summary>
        /// Exclui uma notificação
        /// </summary>
        public void Excluir(int id)
        {
            var conn = Conexao.GetConnection();
            string sql = "DELETE FROM Notificacao WHERE id_notificacao = @id";

            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        /// <summary>
        /// Exclui todas as notificações lidas
        /// </summary>
        public void ExcluirLidas()
        {
            var conn = Conexao.GetConnection();
            string sql = "DELETE FROM Notificacao WHERE lida = TRUE";

            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        /// <summary>
        /// Mapeia um reader para um objeto Notificacao
        /// </summary>
        private Notificacao MapearNotificacao(NpgsqlDataReader reader)
        {
            return new Notificacao
            {
                Id = reader.GetInt32(reader.GetOrdinal("id_notificacao")),
                Tipo = reader.GetString(reader.GetOrdinal("tipo")),
                Titulo = reader.GetString(reader.GetOrdinal("titulo")),
                Mensagem = reader.GetString(reader.GetOrdinal("mensagem")),
                IdAluno = reader.IsDBNull(reader.GetOrdinal("id_aluno")) ? null : reader.GetInt32(reader.GetOrdinal("id_aluno")),
                IdFuncionario = reader.IsDBNull(reader.GetOrdinal("id_funcionario")) ? null : reader.GetInt32(reader.GetOrdinal("id_funcionario")),
                IdEmprestimo = reader.IsDBNull(reader.GetOrdinal("id_emprestimo")) ? null : reader.GetInt32(reader.GetOrdinal("id_emprestimo")),
                IdReserva = reader.IsDBNull(reader.GetOrdinal("id_reserva")) ? null : reader.GetInt32(reader.GetOrdinal("id_reserva")),
                Lida = reader.GetBoolean(reader.GetOrdinal("lida")),
                Prioridade = reader.GetString(reader.GetOrdinal("prioridade")),
                DataCriacao = reader.GetDateTime(reader.GetOrdinal("data_criacao")),
                DataLeitura = reader.IsDBNull(reader.GetOrdinal("data_leitura")) ? null : reader.GetDateTime(reader.GetOrdinal("data_leitura"))
            };
        }
    }
}
