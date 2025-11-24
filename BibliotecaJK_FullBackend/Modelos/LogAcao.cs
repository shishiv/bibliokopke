using System;

namespace BibliotecaJK.Modelos
{
    public class LogAcao
    {
        public int Id { get; set; } // id_log
        public int? IdFuncionario { get; set; }
        public string? Acao { get; set; }
        public string? Descricao { get; set; }
        public DateTime DataHora { get; set; }
    }
}
