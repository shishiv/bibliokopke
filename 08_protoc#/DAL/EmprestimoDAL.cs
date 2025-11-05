using MySql.Data.MySqlClient;
using BibliotecaJK.Model;
using System.Collections.Generic;
using System;

namespace BibliotecaJK.DAL
{
    public class EmprestimoDAL
    {
        public void Inserir(Emprestimo e)
        {
            var conn = Conexao.GetConnection();
            string sql = "INSERT INTO Emprestimo (id_aluno, id_livro, data_emprestimo, data_prevista, data_devolucao, multa) " +
                         "VALUES (@idaluno,@idlivro,@dataemp,@dataprev,@datadev,@multa)";
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@idaluno", e.IdAluno);
                cmd.Parameters.AddWithValue("@idlivro", e.IdLivro);
                cmd.Parameters.AddWithValue("@dataemp", e.DataEmprestimo.Date);
                cmd.Parameters.AddWithValue("@dataprev", e.DataPrevista.Date);
                cmd.Parameters.AddWithValue("@datadev", (object?)e.DataDevolucao?.Date ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@multa", e.Multa);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public List<Emprestimo> Listar()
        {
            var lista = new List<Emprestimo>();
            var conn = Conexao.GetConnection();
            string sql = "SELECT * FROM Emprestimo";
            using (var cmd = new MySqlCommand(sql, conn))
            {
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Emprestimo
                    {
                        Id = reader.GetInt32("id_emprestimo"),
                        IdAluno = reader.GetInt32("id_aluno"),
                        IdLivro = reader.GetInt32("id_livro"),
                        DataEmprestimo = reader.GetDateTime("data_emprestimo"),
                        DataPrevista = reader.GetDateTime("data_prevista"),
                        DataDevolucao = reader.IsDBNull(reader.GetOrdinal("data_devolucao")) ? (DateTime?)null : reader.GetDateTime("data_devolucao"),
                        Multa = reader.GetDecimal("multa")
                    });
                }
                conn.Close();
            }
            return lista;
        }

        public Emprestimo? ObterPorId(int id)
        {
            var conn = Conexao.GetConnection();
            string sql = "SELECT * FROM Emprestimo WHERE id_emprestimo=@id";
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    var e = new Emprestimo
                    {
                        Id = reader.GetInt32("id_emprestimo"),
                        IdAluno = reader.GetInt32("id_aluno"),
                        IdLivro = reader.GetInt32("id_livro"),
                        DataEmprestimo = reader.GetDateTime("data_emprestimo"),
                        DataPrevista = reader.GetDateTime("data_prevista"),
                        DataDevolucao = reader.IsDBNull(reader.GetOrdinal("data_devolucao")) ? (DateTime?)null : reader.GetDateTime("data_devolucao"),
                        Multa = reader.GetDecimal("multa")
                    };
                    conn.Close();
                    return e;
                }
                conn.Close();
            }
            return null;
        }

        public void Atualizar(Emprestimo e)
        {
            var conn = Conexao.GetConnection();
            string sql = "UPDATE Emprestimo SET id_aluno=@idaluno, id_livro=@idlivro, data_emprestimo=@dataemp, data_prevista=@dataprev, data_devolucao=@datadev, multa=@multa WHERE id_emprestimo=@id";
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@idaluno", e.IdAluno);
                cmd.Parameters.AddWithValue("@idlivro", e.IdLivro);
                cmd.Parameters.AddWithValue("@dataemp", e.DataEmprestimo.Date);
                cmd.Parameters.AddWithValue("@dataprev", e.DataPrevista.Date);
                cmd.Parameters.AddWithValue("@datadev", (object?)e.DataDevolucao?.Date ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@multa", e.Multa);
                cmd.Parameters.AddWithValue("@id", e.Id);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void Excluir(int id)
        {
            var conn = Conexao.GetConnection();
            string sql = "DELETE FROM Emprestimo WHERE id_emprestimo=@id";
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
