using System;
using System.IO;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;

namespace BibliotecaJK.BLL
{
    /// <summary>
    /// Configurações de backup armazenadas localmente
    /// </summary>
    public class BackupConfig
    {
        public string MySqlHost { get; set; } = "localhost";
        public int MySqlPort { get; set; } = 3306;
        public string MySqlUser { get; set; } = "root";
        public string MySqlPassword { get; set; } = "";
        public string MySqlDatabase { get; set; } = "bibliokopke";
        public string BackupPath { get; set; } = "";
        public bool BackupAgendado { get; set; } = false;
        public string HorarioBackup { get; set; } = "23:00";
        public int DiasRetencao { get; set; } = 30;

        private static readonly string ConfigFilePath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "BibliotecaJK", "backup.config");

        private static readonly byte[] EncryptionKey = Encoding.UTF8.GetBytes("BiblioJK2025Key!"); // 16 bytes
        private static readonly byte[] EncryptionIV = Encoding.UTF8.GetBytes("BiblioJK2025IV!!"); // 16 bytes

        /// <summary>
        /// Salva configuração de backup de forma criptografada
        /// </summary>
        public void Salvar()
        {
            try
            {
                // Garantir que diretório existe
                var directory = Path.GetDirectoryName(ConfigFilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Serializar para JSON
                var json = JsonSerializer.Serialize(this, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                // Criptografar
                var encrypted = Encrypt(json);

                // Salvar arquivo
                File.WriteAllText(ConfigFilePath, encrypted);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao salvar configurações de backup: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Carrega configuração de backup
        /// </summary>
        public static BackupConfig? Carregar()
        {
            try
            {
                if (!File.Exists(ConfigFilePath))
                    return null;

                // Ler arquivo
                var encrypted = File.ReadAllText(ConfigFilePath);

                // Descriptografar
                var json = Decrypt(encrypted);

                // Desserializar
                return JsonSerializer.Deserialize<BackupConfig>(json);
            }
            catch
            {
                // Se falhar ao carregar/descriptografar, retorna null
                return null;
            }
        }

        /// <summary>
        /// Verifica se existe configuração salva
        /// </summary>
        public static bool Existe()
        {
            return File.Exists(ConfigFilePath);
        }

        /// <summary>
        /// Exclui configuração salva
        /// </summary>
        public static void Excluir()
        {
            if (File.Exists(ConfigFilePath))
            {
                File.Delete(ConfigFilePath);
            }
        }

        /// <summary>
        /// Criptografa string usando AES
        /// </summary>
        private static string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = EncryptionKey;
            aes.IV = EncryptionIV;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using var msEncrypt = new MemoryStream();
            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }

            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        /// <summary>
        /// Descriptografa string usando AES
        /// </summary>
        private static string Decrypt(string cipherText)
        {
            using var aes = Aes.Create();
            aes.Key = EncryptionKey;
            aes.IV = EncryptionIV;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText));
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);

            return srDecrypt.ReadToEnd();
        }

        /// <summary>
        /// Gera string de conexão MySQL
        /// </summary>
        public string GetConnectionString()
        {
            return $"server={MySqlHost};port={MySqlPort};database={MySqlDatabase};uid={MySqlUser};pwd={MySqlPassword};";
        }
    }
}
