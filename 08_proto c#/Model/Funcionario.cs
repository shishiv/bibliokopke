namespace BibliotecaJK.Model
{
    public class Funcionario : Pessoa
    {
        public string? Cargo { get; set; }
        public string Login { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public string Perfil { get; set; } = string.Empty; // ADMIN, BIBLIOTECARIO, OPERADOR
    }
}
