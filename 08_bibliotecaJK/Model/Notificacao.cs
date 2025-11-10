using System;

namespace BibliotecaJK.Model
{
    /// <summary>
    /// Modelo para notificações do sistema
    /// Tipos: EMPRESTIMO_ATRASADO, RESERVA_EXPIRADA, LIVRO_DISPONIVEL
    /// Prioridades: BAIXA, NORMAL, ALTA, URGENTE
    /// </summary>
    public class Notificacao
    {
        public int Id { get; set; } // id_notificacao
        public string Tipo { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Mensagem { get; set; } = string.Empty;
        public int? IdAluno { get; set; }
        public int? IdFuncionario { get; set; }
        public int? IdEmprestimo { get; set; }
        public int? IdReserva { get; set; }
        public bool Lida { get; set; } = false;
        public string Prioridade { get; set; } = "NORMAL";
        public DateTime DataCriacao { get; set; }
        public DateTime? DataLeitura { get; set; }
    }

    /// <summary>
    /// Enumeração para tipos de notificação
    /// </summary>
    public static class TipoNotificacao
    {
        public const string EMPRESTIMO_ATRASADO = "EMPRESTIMO_ATRASADO";
        public const string RESERVA_EXPIRADA = "RESERVA_EXPIRADA";
        public const string LIVRO_DISPONIVEL = "LIVRO_DISPONIVEL";
    }

    /// <summary>
    /// Enumeração para prioridades de notificação
    /// </summary>
    public static class PrioridadeNotificacao
    {
        public const string BAIXA = "BAIXA";
        public const string NORMAL = "NORMAL";
        public const string ALTA = "ALTA";
        public const string URGENTE = "URGENTE";
    }
}
