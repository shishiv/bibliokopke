using System;

namespace BibliotecaJK.Model
{
    public class Emprestimo
    {
        public int Id { get; set; } // id_emprestimo
        public int IdAluno { get; set; }
        public int IdLivro { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public DateTime DataPrevista { get; set; }
        public DateTime? DataDevolucao { get; set; }
        public decimal Multa { get; set; }
    }
}
