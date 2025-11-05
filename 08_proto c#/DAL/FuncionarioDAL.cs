using Npgsql;
using BibliotecaJK.Model;
using System.Collections.Generic;

namespace BibliotecaJK.DAL
{
    public class FuncionarioDAL
    {
        public void Inserir(Funcionario f)
        {
            var conn = Conexao.GetConnection();
            string sql = "INSERT INTO Funcionario (nome, cpf, cargo, login, senha_hash, perfil) VALUES (@nome,@cpf,@cargo,@login,@senha,@perfil)";
            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@nome", f.Nome);
                cmd.Parameters.AddWithValue("@cpf", f.CPF);
                cmd.Parameters.AddWithValue("@cargo", (object?)f.Cargo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@login", f.Login);
                cmd.Parameters.AddWithValue("@senha", f.SenhaHash);
                cmd.Parameters.AddWithValue("@perfil", f.Perfil);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public List<Funcionario> Listar()
        {
            var lista = new List<Funcionario>();
            var conn = Conexao.GetConnection();
            string sql = "SELECT * FROM Funcionario";
            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Funcionario
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id_funcionario")),
                        Nome = reader.GetString(reader.GetOrdinal("nome")),
                        CPF = reader.GetString(reader.GetOrdinal("cpf")),
                        Cargo = reader.IsDBNull(reader.GetOrdinal("cargo")) ? null : reader.GetString(reader.GetOrdinal("cargo")),
                        Login = reader.GetString(reader.GetOrdinal("login")),
                        SenhaHash = reader.GetString(reader.GetOrdinal("senha_hash")),
                        Perfil = reader.GetString(reader.GetOrdinal("perfil"))
                    });
                }
                conn.Close();
            }
            return lista;
        }

        public Funcionario? ObterPorId(int id)
        {
            var conn = Conexao.GetConnection();
            string sql = "SELECT * FROM Funcionario WHERE id_funcionario=@id";
            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    var f = new Funcionario
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id_funcionario")),
                        Nome = reader.GetString(reader.GetOrdinal("nome")),
                        CPF = reader.GetString(reader.GetOrdinal("cpf")),
                        Cargo = reader.IsDBNull(reader.GetOrdinal("cargo")) ? null : reader.GetString(reader.GetOrdinal("cargo")),
                        Login = reader.GetString(reader.GetOrdinal("login")),
                        SenhaHash = reader.GetString(reader.GetOrdinal("senha_hash")),
                        Perfil = reader.GetString(reader.GetOrdinal("perfil"))
                    };
                    conn.Close();
                    return f;
                }
                conn.Close();
            }
            return null;
        }

        public void Atualizar(Funcionario f)
        {
            var conn = Conexao.GetConnection();
            string sql = "UPDATE Funcionario SET nome=@nome, cpf=@cpf, cargo=@cargo, login=@login, senha_hash=@senha, perfil=@perfil WHERE id_funcionario=@id";
            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@nome", f.Nome);
                cmd.Parameters.AddWithValue("@cpf", f.CPF);
                cmd.Parameters.AddWithValue("@cargo", (object?)f.Cargo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@login", f.Login);
                cmd.Parameters.AddWithValue("@senha", f.SenhaHash);
                cmd.Parameters.AddWithValue("@perfil", f.Perfil);
                cmd.Parameters.AddWithValue("@id", f.Id);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void Excluir(int id)
        {
            var conn = Conexao.GetConnection();
            string sql = "DELETE FROM Funcionario WHERE id_funcionario=@id";
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
