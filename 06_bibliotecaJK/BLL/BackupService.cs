using System;
using BibliotecaJK;

namespace BibliotecaJK.BLL
{
    /// <summary>
    /// Servico de backup simplificado para Supabase/PostgreSQL
    /// NOTA: Supabase ja possui backup automatico (7 dias no plano gratuito)
    /// Este servico e mantido para compatibilidade, mas nao e mais necessario
    /// </summary>
    public class BackupService
    {
        public BackupService(BackupConfig? config = null)
        {
            // Construtor mantido para compatibilidade
        }

        /// <summary>
        /// Testa conexao com banco de dados
        /// </summary>
        public ResultadoOperacao TestarConexao()
        {
            try
            {
                // Usa a conexao configurada globalmente
                using var conn = Conexao.GetConnection();
                conn.Open();
                return ResultadoOperacao.Ok("Conexao estabelecida com sucesso!");
            }
            catch (Exception ex)
            {
                return ResultadoOperacao.Erro($"Erro ao testar conexao: {ex.Message}");
            }
        }

        /// <summary>
        /// Executa backup manual do banco de dados
        /// NOTA: Com Supabase, backups sao automaticos. Esta funcao retorna mensagem informativa.
        /// </summary>
        public ResultadoOperacao ExecutarBackup()
        {
            return ResultadoOperacao.Ok(
                "Backup com Supabase/PostgreSQL:\n\n" +
                "O Supabase realiza backups automaticos diariamente.\n" +
                "Seus dados estao seguros e podem ser restaurados a qualquer momento.\n\n" +
                "Recursos de backup do Supabase:\n" +
                "- Backups automaticos diarios\n" +
                "- Retencao de 7 dias (plano gratuito)\n" +
                "- Restauracao via painel Supabase\n" +
                "- Replicacao geografica\n\n" +
                "Para realizar backup manual:\n" +
                "1. Acesse o painel do Supabase\n" +
                "2. Va em Database > Backups\n" +
                "3. Clique em 'Create backup'\n\n" +
                "Ou exporte dados via SQL Editor:\n" +
                "pg_dump para backup completo do schema e dados."
            );
        }

        /// <summary>
        /// Agenda backup automatico
        /// NOTA: Com Supabase, backups ja sao agendados automaticamente
        /// </summary>
        public ResultadoOperacao AgendarBackupAutomatico()
        {
            return ResultadoOperacao.Ok(
                "Backup automatico ja esta ativo!\n\n" +
                "O Supabase realiza backups automaticos diariamente.\n" +
                "Nenhuma configuracao adicional e necessaria.\n\n" +
                "Para visualizar seus backups:\n" +
                "1. Acesse https://supabase.com\n" +
                "2. Selecione seu projeto\n" +
                "3. Va em Database > Backups\n" +
                "4. Visualize historico e restaure se necessario"
            );
        }

        /// <summary>
        /// Remove agendamento de backup
        /// NOTA: Backups do Supabase nao podem ser desabilitados (recurso automatico)
        /// </summary>
        public ResultadoOperacao RemoverAgendamento()
        {
            return ResultadoOperacao.Ok(
                "Backups automaticos do Supabase nao podem ser desabilitados.\n" +
                "Este e um recurso de seguranca integrado."
            );
        }

        /// <summary>
        /// Cancela agendamento de backup automatico
        /// ALIAS de RemoverAgendamento() - mantido para compatibilidade com FormBackup
        /// </summary>
        public ResultadoOperacao CancelarBackupAutomatico()
        {
            return RemoverAgendamento();
        }

        /// <summary>
        /// Verifica se backup automatico esta agendado
        /// NOTA: Com Supabase, backups sempre estao ativos (automaticos)
        /// </summary>
        public bool VerificarSeEstaAgendado()
        {
            // Supabase sempre tem backup automatico ativo
            return true;
        }
    }
}
