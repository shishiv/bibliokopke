using MySql.Data.MySqlClient;

namespace BibliotecaJK
{
    public class Conexao
    {
        // String de conexão com o banco MySQL
        private static string connectionString = "server=localhost;database=bibliokopke;uid=root;pwd=;";

        // Retorna uma nova instância da conexão (não abre automaticamente)
        // Cada chamada cria uma nova conexão para evitar conflitos
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
