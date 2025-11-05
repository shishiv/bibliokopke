using MySql.Data.MySqlClient;

namespace BibliotecaJK
{
    public class Conexao
    {
        // String de conexão com o banco MySQL
        private static string connectionString = "server=localhost;database=bibliokopke;uid=root;pwd=;";
        private static MySqlConnection? connection = null;

        // Retorna uma instância da conexão (não abre automaticamente)
        public static MySqlConnection GetConnection()
        {
            if (connection == null)
                connection = new MySqlConnection(connectionString);

            return connection;
        }
    }
}
