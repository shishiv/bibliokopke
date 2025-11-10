namespace BibliotecaJK.Model
{
    public class Aluno : Pessoa
    {
        public string Matricula { get; set; } = string.Empty;
        public string? Turma { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
    }
}
