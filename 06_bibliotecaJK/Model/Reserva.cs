using System;

namespace BibliotecaJK.Model
{
    public class Reserva
    {
        public int Id { get; set; } // id_reserva
        public int IdAluno { get; set; }
        public int IdLivro { get; set; }
        public DateTime DataReserva { get; set; }
        public string Status { get; set; } = "ATIVA"; // ATIVA, CANCELADA, CONCLUIDA
    }
}
