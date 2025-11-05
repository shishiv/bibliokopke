using System;
using MySql.Data.MySqlClient;

namespace BibliotecaJK
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testando conexão com o banco de dados MySQL...");

            try
            {
                var conn = Conexao.GetConnection();
                conn.Open();
                Console.WriteLine("✅ Conexão estabelecida com sucesso!");
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Erro ao conectar: " + ex.Message);
            }

            Console.WriteLine("\nPressione qualquer tecla para sair...");
            Console.ReadKey();
        }
    }
}
