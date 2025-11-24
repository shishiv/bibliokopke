namespace BibliotecaJK.Modelos
{
    public class Livro
    {
        public int Id { get; set; } // id_livro
        public string Titulo { get; set; } = string.Empty;
        public string? Autor { get; set; }
        public string? ISBN { get; set; }
        public string? Editora { get; set; }
        public int? AnoPublicacao { get; set; }
        public int QuantidadeTotal { get; set; }
        public int QuantidadeDisponivel { get; set; }
        public string? Localizacao { get; set; }
    }
}
