using Npgsql;
using System.Text.Json;

namespace BibliotecaJK
{
    public class Conexao
    {
        // Arquivo de configuracao armazenado localmente
        private static readonly string ConfigFilePath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "BibliotecaJK", "database.config");

        // Connection string padrao (Supabase ou PostgreSQL local)
        private static string? _connectionString = null;

        // Classe para serializar/deserializar configuracao
        private class DatabaseConfig
        {
            public string? ConnectionString { get; set; }
        }

        // Carrega a connection string do arquivo de configuracao
        private static string? CarregarConnectionString()
        {
            try
            {
                if (File.Exists(ConfigFilePath))
                {
                    string json = File.ReadAllText(ConfigFilePath);
                    var config = JsonSerializer.Deserialize<DatabaseConfig>(json);
                    return config?.ConnectionString;
                }
            }
            catch
            {
                // Se falhar ao carregar, retorna null
            }

            return null;
        }

        // Salva a connection string no arquivo de configuracao
        public static void SalvarConnectionString(string connectionString)
        {
            try
            {
                // Criar diretorio se nao existir
                string? directory = Path.GetDirectoryName(ConfigFilePath);
                if (directory != null && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Salvar configuracao
                var config = new DatabaseConfig { ConnectionString = connectionString };
                string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ConfigFilePath, json);

                // Atualizar cache
                _connectionString = connectionString;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao salvar configuracao: {ex.Message}", ex);
            }
        }

        // Retorna a connection string configurada
        public static string GetConnectionString()
        {
            // Se ja carregou, retorna do cache
            if (_connectionString != null)
            {
                return _connectionString;
            }

            // Tenta carregar do arquivo
            _connectionString = CarregarConnectionString();

            // Se nao tem configuracao, lanca excecao
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException(
                    "Connection string nao configurada. Execute a configuracao inicial do banco de dados.");
            }

            return _connectionString;
        }

        // Verifica se ja existe configuracao
        public static bool TemConfiguracao()
        {
            if (_connectionString != null) return true;
            _connectionString = CarregarConnectionString();
            return !string.IsNullOrEmpty(_connectionString);
        }

        // Retorna uma nova instancia da conexao (nao abre automaticamente)
        // Cada chamada cria uma nova conexao para evitar conflitos
        public static NpgsqlConnection GetConnection()
        {
            string connStr = GetConnectionString();
            return new NpgsqlConnection(connStr);
        }

        // Testa a conexao com o banco de dados
        public static bool TestarConexao(string connectionString, out string mensagemErro)
        {
            mensagemErro = string.Empty;

            try
            {
                using var conn = new NpgsqlConnection(connectionString);
                conn.Open();

                // Testa uma query simples
                using var cmd = new NpgsqlCommand("SELECT version()", conn);
                var version = cmd.ExecuteScalar()?.ToString();

                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                mensagemErro = ex.Message;
                return false;
            }
        }
    }
}
