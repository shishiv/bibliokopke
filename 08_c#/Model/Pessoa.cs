namespace BibliotecaJK.Model
{
    public abstract class Pessoa
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
    }
}
