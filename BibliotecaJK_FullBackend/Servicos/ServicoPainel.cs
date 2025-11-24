using BibliotecaJK.AcessoDados;
using BibliotecaJK.Servicos.Modelos;

namespace BibliotecaJK.Servicos;

public class ServicoPainel
{
    private readonly RepositorioLivro _livroDal = new();
    private readonly RepositorioAluno _alunoDal = new();
    private readonly RepositorioEmprestimo _emprestimoDal = new();
    private readonly RepositorioReserva _reservaDal = new();

    public ResumoPainel ObterResumo()
    {
        var totalLivros = _livroDal.Listar().Count;
        var totalAlunos = _alunoDal.Listar().Count;
        var emprestados = _emprestimoDal.ListarAtivos().Count;
        var reservas = _reservaDal.ListarAtivas().Count;

        return new ResumoPainel(totalLivros, totalAlunos, emprestados, reservas);
    }
}
