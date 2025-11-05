namespace BibliotecaJK.Model
{
    public class Funcionario
    {
        public int Id { get; set; } // id_funcionario
        public string Nome { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string? Cargo { get; set; }
        public string Login { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public string Perfil { get; set; } = string.Empty; // ADMIN, BIBLIOTECARIO, OPERADOR
    }
}
