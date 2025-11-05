using Npgsql;
using BibliotecaJK.Model;
using System.Collections.Generic;
using System;

namespace BibliotecaJK.DAL
{
    public class ReservaDAL
    {
        public void Inserir(Reserva r)
        {
            var conn = Conexao.GetConnection();
            string sql = "INSERT INTO Reserva (id_aluno, id_livro, data_reserva, status) VALUES (@idaluno,@idlivro,@datares,@status)";
            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@idaluno", r.IdAluno);
                cmd.Parameters.AddWithValue("@idlivro", r.IdLivro);
                cmd.Parameters.AddWithValue("@datares", r.DataReserva.Date);
                cmd.Parameters.AddWithValue("@status", r.Status);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public List<Reserva> Listar()
        {
            var lista = new List<Reserva>();
            var conn = Conexao.GetConnection();
            string sql = "SELECT * FROM Reserva";
            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Reserva
                    {
                        Id = reader.GetInt32("id_reserva"),
                        IdAluno = reader.GetInt32("id_aluno"),
                        IdLivro = reader.GetInt32("id_livro"),
                        DataReserva = reader.GetDateTime("data_reserva"),
                        Status = reader.GetString("status")
                    });
                }
                conn.Close();
            }
            return lista;
        }

        public Reserva? ObterPorId(int id)
        {
            var conn = Conexao.GetConnection();
            string sql = "SELECT * FROM Reserva WHERE id_reserva=@id";
            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    var r = new Reserva
                    {
                        Id = reader.GetInt32("id_reserva"),
                        IdAluno = reader.GetInt32("id_aluno"),
                        IdLivro = reader.GetInt32("id_livro"),
                        DataReserva = reader.GetDateTime("data_reserva"),
                        Status = reader.GetString("status")
                    };
                    conn.Close();
                    return r;
                }
                conn.Close();
            }
            return null;
        }

        public void Atualizar(Reserva r)
        {
            var conn = Conexao.GetConnection();
            string sql = "UPDATE Reserva SET id_aluno=@idaluno, id_livro=@idlivro, data_reserva=@datares, status=@status WHERE id_reserva=@id";
            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@idaluno", r.IdAluno);
                cmd.Parameters.AddWithValue("@idlivro", r.IdLivro);
                cmd.Parameters.AddWithValue("@datares", r.DataReserva.Date);
                cmd.Parameters.AddWithValue("@status", r.Status);
                cmd.Parameters.AddWithValue("@id", r.Id);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void Excluir(int id)
        {
            var conn = Conexao.GetConnection();
            string sql = "DELETE FROM Reserva WHERE id_reserva=@id";
            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}
