using Npgsql;
using BibliotecaJK.Model;
using System.Collections.Generic;

namespace BibliotecaJK.DAL
{
    public class AlunoDAL
    {
        public void Inserir(Aluno aluno)
        {
            var conn = Conexao.GetConnection();
            string sql = "INSERT INTO Aluno (nome, cpf, matricula, turma, telefone, email) VALUES (@nome,@cpf,@matricula,@turma,@telefone,@email)";
            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@nome", aluno.Nome);
                cmd.Parameters.AddWithValue("@cpf", aluno.CPF);
                cmd.Parameters.AddWithValue("@matricula", aluno.Matricula);
                cmd.Parameters.AddWithValue("@turma", (object?)aluno.Turma ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@telefone", (object?)aluno.Telefone ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@email", (object?)aluno.Email ?? DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public List<Aluno> Listar()
        {
            var lista = new List<Aluno>();
            var conn = Conexao.GetConnection();
            string sql = "SELECT * FROM Aluno";
            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Aluno
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id_aluno")),
                        Nome = reader.GetString(reader.GetOrdinal("nome")),
                        CPF = reader.GetString(reader.GetOrdinal("cpf")),
                        Matricula = reader.GetString(reader.GetOrdinal("matricula")),
                        Turma = reader.IsDBNull(reader.GetOrdinal("turma")) ? null : reader.GetString(reader.GetOrdinal("turma")),
                        Telefone = reader.IsDBNull(reader.GetOrdinal("telefone")) ? null : reader.GetString(reader.GetOrdinal("telefone")),
                        Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email"))
                    });
                }
                conn.Close();
            }
            return lista;
        }

        public Aluno? ObterPorId(int id)
        {
            var conn = Conexao.GetConnection();
            string sql = "SELECT * FROM Aluno WHERE id_aluno=@id";
            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    var a = new Aluno
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id_aluno")),
                        Nome = reader.GetString(reader.GetOrdinal("nome")),
                        CPF = reader.GetString(reader.GetOrdinal("cpf")),
                        Matricula = reader.GetString(reader.GetOrdinal("matricula")),
                        Turma = reader.IsDBNull(reader.GetOrdinal("turma")) ? null : reader.GetString(reader.GetOrdinal("turma")),
                        Telefone = reader.IsDBNull(reader.GetOrdinal("telefone")) ? null : reader.GetString(reader.GetOrdinal("telefone")),
                        Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email"))
                    };
                    conn.Close();
                    return a;
                }
                conn.Close();
            }
            return null;
        }

        public void Atualizar(Aluno aluno)
        {
            var conn = Conexao.GetConnection();
            string sql = "UPDATE Aluno SET nome=@nome, cpf=@cpf, matricula=@matricula, turma=@turma, telefone=@telefone, email=@email WHERE id_aluno=@id";
            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@nome", aluno.Nome);
                cmd.Parameters.AddWithValue("@cpf", aluno.CPF);
                cmd.Parameters.AddWithValue("@matricula", aluno.Matricula);
                cmd.Parameters.AddWithValue("@turma", (object?)aluno.Turma ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@telefone", (object?)aluno.Telefone ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@email", (object?)aluno.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@id", aluno.Id);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void Excluir(int id)
        {
            var conn = Conexao.GetConnection();
            string sql = "DELETE FROM Aluno WHERE id_aluno=@id";
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
