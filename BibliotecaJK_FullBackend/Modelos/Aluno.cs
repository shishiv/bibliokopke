namespace BibliotecaJK.Modelos
{
    public class Aluno
    {
        public int Id { get; set; } // id_aluno
        public string Nome { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string Matricula { get; set; } = string.Empty;
        public string? Turma { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
    }
}
