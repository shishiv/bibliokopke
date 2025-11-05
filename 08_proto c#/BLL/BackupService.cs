using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using MySql.Data.MySqlClient;

namespace BibliotecaJK.BLL
{
    /// <summary>
    /// Serviço de backup do banco de dados MySQL
    /// </summary>
    public class BackupService
    {
        private readonly BackupConfig _config;

        public BackupService(BackupConfig config)
        {
            _config = config;
        }

        /// <summary>
        /// Testa conexão com MySQL usando as credenciais configuradas
        /// </summary>
        public ResultadoOperacao TestarConexao()
        {
            try
            {
                using var conn = new MySqlConnection(_config.GetConnectionString());
                conn.Open();
                return ResultadoOperacao.Ok("Conexão estabelecida com sucesso!");
            }
            catch (MySqlException ex)
            {
                return ResultadoOperacao.Erro($"Erro de conexão MySQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                return ResultadoOperacao.Erro($"Erro ao testar conexão: {ex.Message}");
            }
        }

        /// <summary>
        /// Executa backup manual do banco de dados
        /// </summary>
        public ResultadoOperacao ExecutarBackup()
        {
            try
            {
                // Validar caminho de backup
                if (string.IsNullOrWhiteSpace(_config.BackupPath))
                    return ResultadoOperacao.Erro("Caminho de backup não configurado.");

                if (!Directory.Exists(_config.BackupPath))
                {
                    try
                    {
                        Directory.CreateDirectory(_config.BackupPath);
                    }
                    catch
                    {
                        return ResultadoOperacao.Erro($"Não foi possível criar o diretório: {_config.BackupPath}");
                    }
                }

                // Nome do arquivo de backup
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var fileName = $"bibliokopke_backup_{timestamp}.sql";
                var filePath = Path.Combine(_config.BackupPath, fileName);

                // Executar mysqldump
                var resultado = ExecutarMySqlDump(filePath);

                if (!resultado.Sucesso)
                    return resultado;

                // Limpar backups antigos
                LimparBackupsAntigos();

                return ResultadoOperacao.Ok($"Backup criado com sucesso!\n\nArquivo: {fileName}", filePath);
            }
            catch (Exception ex)
            {
                return ResultadoOperacao.Erro($"Erro ao executar backup: {ex.Message}");
            }
        }

        /// <summary>
        /// Executa o comando mysqldump para criar backup
        /// </summary>
        private ResultadoOperacao ExecutarMySqlDump(string filePath)
        {
            try
            {
                // Procurar mysqldump.exe
                var mysqldumpPath = EncontrarMySqlDump();

                if (string.IsNullOrEmpty(mysqldumpPath))
                {
                    return ResultadoOperacao.Erro(
                        "mysqldump.exe não encontrado.\n\n" +
                        "Certifique-se de que o MySQL está instalado.\n" +
                        "Adicione o diretório do MySQL ao PATH do sistema.");
                }

                // Configurar processo
                var startInfo = new ProcessStartInfo
                {
                    FileName = mysqldumpPath,
                    Arguments = $"--host={_config.MySqlHost} " +
                               $"--port={_config.MySqlPort} " +
                               $"--user={_config.MySqlUser} " +
                               $"--password={_config.MySqlPassword} " +
                               $"--databases {_config.MySqlDatabase} " +
                               $"--result-file=\"{filePath}\" " +
                               $"--single-transaction " +
                               $"--quick",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using var process = Process.Start(startInfo);
                if (process == null)
                    return ResultadoOperacao.Erro("Não foi possível iniciar mysqldump.");

                process.WaitForExit();

                // Verificar se houve erro
                if (process.ExitCode != 0)
                {
                    var error = process.StandardError.ReadToEnd();
                    return ResultadoOperacao.Erro($"Erro no mysqldump: {error}");
                }

                // Verificar se arquivo foi criado
                if (!File.Exists(filePath) || new FileInfo(filePath).Length == 0)
                {
                    return ResultadoOperacao.Erro("Arquivo de backup não foi criado ou está vazio.");
                }

                return ResultadoOperacao.Ok("Backup executado com sucesso.");
            }
            catch (Exception ex)
            {
                return ResultadoOperacao.Erro($"Erro ao executar mysqldump: {ex.Message}");
            }
        }

        /// <summary>
        /// Procura mysqldump.exe no sistema
        /// </summary>
        private string? EncontrarMySqlDump()
        {
            // Tentar no PATH
            var pathDirs = Environment.GetEnvironmentVariable("PATH")?.Split(';') ?? Array.Empty<string>();
            foreach (var dir in pathDirs)
            {
                try
                {
                    var mysqldumpPath = Path.Combine(dir.Trim(), "mysqldump.exe");
                    if (File.Exists(mysqldumpPath))
                        return mysqldumpPath;
                }
                catch { }
            }

            // Locais comuns de instalação
            var commonPaths = new[]
            {
                @"C:\Program Files\MySQL\MySQL Server 8.0\bin\mysqldump.exe",
                @"C:\Program Files\MySQL\MySQL Server 5.7\bin\mysqldump.exe",
                @"C:\Program Files (x86)\MySQL\MySQL Server 8.0\bin\mysqldump.exe",
                @"C:\mysql\bin\mysqldump.exe"
            };

            foreach (var path in commonPaths)
            {
                if (File.Exists(path))
                    return path;
            }

            return null;
        }

        /// <summary>
        /// Remove backups antigos conforme política de retenção
        /// </summary>
        private void LimparBackupsAntigos()
        {
            try
            {
                if (!Directory.Exists(_config.BackupPath))
                    return;

                var dataLimite = DateTime.Now.AddDays(-_config.DiasRetencao);

                var arquivosAntigos = Directory.GetFiles(_config.BackupPath, "bibliokopke_backup_*.sql")
                    .Select(f => new FileInfo(f))
                    .Where(f => f.CreationTime < dataLimite)
                    .ToList();

                foreach (var arquivo in arquivosAntigos)
                {
                    try
                    {
                        arquivo.Delete();
                    }
                    catch
                    {
                        // Ignorar erros ao excluir arquivos antigos
                    }
                }
            }
            catch
            {
                // Ignorar erros na limpeza
            }
        }

        /// <summary>
        /// Agenda backup automático diário no Windows Task Scheduler
        /// </summary>
        public ResultadoOperacao AgendarBackupAutomatico()
        {
            try
            {
                // Caminho do executável atual
                var exePath = Process.GetCurrentProcess().MainModule?.FileName;
                if (string.IsNullOrEmpty(exePath))
                    return ResultadoOperacao.Erro("Não foi possível determinar o caminho do executável.");

                // Nome da tarefa
                var taskName = "BibliotecaJK_Backup_Diario";

                // Remover tarefa existente (se houver)
                RemoverTarefaAgendada(taskName);

                // Criar nova tarefa usando schtasks
                var horario = _config.HorarioBackup;
                var startInfo = new ProcessStartInfo
                {
                    FileName = "schtasks.exe",
                    Arguments = $"/Create /TN \"{taskName}\" " +
                               $"/TR \"\\\"{exePath}\\\" /backup\" " +
                               $"/SC DAILY " +
                               $"/ST {horario} " +
                               $"/F",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    Verb = "runas" // Executar como administrador
                };

                using var process = Process.Start(startInfo);
                if (process == null)
                    return ResultadoOperacao.Erro("Não foi possível agendar tarefa.");

                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    var error = process.StandardError.ReadToEnd();
                    return ResultadoOperacao.Erro($"Erro ao agendar tarefa: {error}");
                }

                return ResultadoOperacao.Ok(
                    $"Backup automático agendado com sucesso!\n\n" +
                    $"Horário: {horario} (diariamente)\n" +
                    $"Tarefa: {taskName}");
            }
            catch (Exception ex)
            {
                return ResultadoOperacao.Erro($"Erro ao agendar backup: {ex.Message}");
            }
        }

        /// <summary>
        /// Remove tarefa agendada do Windows
        /// </summary>
        private void RemoverTarefaAgendada(string taskName)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = "schtasks.exe",
                    Arguments = $"/Delete /TN \"{taskName}\" /F",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using var process = Process.Start(startInfo);
                process?.WaitForExit();
            }
            catch
            {
                // Ignorar erros ao remover (tarefa pode não existir)
            }
        }

        /// <summary>
        /// Cancela agendamento de backup automático
        /// </summary>
        public ResultadoOperacao CancelarBackupAutomatico()
        {
            try
            {
                var taskName = "BibliotecaJK_Backup_Diario";
                RemoverTarefaAgendada(taskName);
                return ResultadoOperacao.Ok("Backup automático cancelado.");
            }
            catch (Exception ex)
            {
                return ResultadoOperacao.Erro($"Erro ao cancelar backup: {ex.Message}");
            }
        }

        /// <summary>
        /// Verifica se backup automático está agendado
        /// </summary>
        public bool VerificarSeEstaAgendado()
        {
            try
            {
                var taskName = "BibliotecaJK_Backup_Diario";
                var startInfo = new ProcessStartInfo
                {
                    FileName = "schtasks.exe",
                    Arguments = $"/Query /TN \"{taskName}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using var process = Process.Start(startInfo);
                if (process == null)
                    return false;

                process.WaitForExit();
                return process.ExitCode == 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
