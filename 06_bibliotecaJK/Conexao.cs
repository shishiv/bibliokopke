using Npgsql;
using System.Text.Json;

namespace BibliotecaJK
{
    public class Conexao
    {
        // Arquivo de configuracao armazenado localmente
        private static readonly string ConfigFilePath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        Constants.CONFIG_FOLDER_NAME, Constants.CONFIG_FILE_NAME);

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
                // Converter URI do Supabase para formato padrão, se necessário
                string connStrFinal = ConverterSupabaseURI(connectionString);

                // Criar diretorio se nao existir
                string? directory = Path.GetDirectoryName(ConfigFilePath);
                if (directory != null && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Salvar configuracao
                var config = new DatabaseConfig { ConnectionString = connStrFinal };
                string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ConfigFilePath, json);

                // Atualizar cache
                _connectionString = connStrFinal;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao salvar configuracao: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Converte URI do Supabase (postgresql://...) para formato Npgsql padrão
        /// Preserva parâmetros importantes como sslmode, pgbouncer, etc.
        /// </summary>
        private static string ConverterSupabaseURI(string connectionString)
        {
            // Se já está no formato padrão (Host=...), retornar como está
            if (connectionString.Contains("Host=") || connectionString.Contains("host="))
            {
                return connectionString;
            }

            // Se está no formato URI (postgresql://...)
            if (connectionString.StartsWith("postgresql://") || connectionString.StartsWith("postgres://"))
            {
                try
                {
                    var uri = new Uri(connectionString);

                    // Extrair componentes básicos
                    string host = uri.Host;
                    int port = uri.Port > 0 ? uri.Port : 5432;
                    string database = uri.AbsolutePath.TrimStart('/');
                    string username = uri.UserInfo.Split(':')[0];
                    string password = uri.UserInfo.Contains(':') ? uri.UserInfo.Split(':')[1] : "";

                    // Construir connection string base
                    var connStringBuilder = new System.Text.StringBuilder();
                    connStringBuilder.Append($"Host={host};Port={port};Database={database};Username={username};Password={password}");

                    // Processar query parameters (ex: ?sslmode=require&pgbouncer=true)
                    if (!string.IsNullOrEmpty(uri.Query))
                    {
                        var queryParams = ParseQueryString(uri.Query.TrimStart('?'));

                        // Mapear parâmetros comuns de URI para formato Npgsql
                        foreach (var param in queryParams)
                        {
                            string key = param.Key.ToLower();
                            string value = param.Value;

                            // Mapear parâmetros conhecidos
                            if (key == "sslmode")
                            {
                                // sslmode: disable, allow, prefer, require, verify-ca, verify-full
                                connStringBuilder.Append($";SSL Mode={value}");
                            }
                            else if (key == "application_name")
                            {
                                connStringBuilder.Append($";Application Name={value}");
                            }
                            else if (key == "connect_timeout")
                            {
                                connStringBuilder.Append($";Timeout={value}");
                            }
                            else if (key == "sslrootcert")
                            {
                                connStringBuilder.Append($";SSL Certificate={value}");
                            }
                            // pgbouncer é apenas informativo, Npgsql não precisa dele
                            // Outros parâmetros podem ser adicionados conforme necessário
                        }
                    }

                    // Se não há parâmetro SSL explícito e é Supabase (detectar pelo host)
                    // adicionar SSL Mode=Require por padrão
                    string connStr = connStringBuilder.ToString();
                    if (!connStr.Contains("SSL Mode") &&
                        (host.Contains("supabase.co") || host.Contains("supabase.com")))
                    {
                        connStringBuilder.Append(";SSL Mode=Require");
                    }

                    return connStringBuilder.ToString();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro ao converter URI do Supabase: {ex.Message}. " +
                        "Verifique se o formato está correto: postgresql://user:password@host:port/database?params", ex);
                }
            }

            // Retornar como está se não for nenhum dos formatos reconhecidos
            return connectionString;
        }

        /// <summary>
        /// Parse query string em dicionário (ex: "sslmode=require&pgbouncer=true")
        /// </summary>
        private static Dictionary<string, string> ParseQueryString(string query)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            if (string.IsNullOrEmpty(query))
                return result;

            foreach (var pair in query.Split('&'))
            {
                var parts = pair.Split('=');
                if (parts.Length == 2)
                {
                    string key = Uri.UnescapeDataString(parts[0]);
                    string value = Uri.UnescapeDataString(parts[1]);
                    result[key] = value;
                }
            }

            return result;
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
                // Converter URI para formato padrão, se necessário
                string connStrFinal = ConverterSupabaseURI(connectionString);

                using var conn = new NpgsqlConnection(connStrFinal);
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
