using BibliotecaJK.AcessoDados;
using BibliotecaJK.Utilitarios;
using BibliotecaJK.Modelos;

namespace BibliotecaJK.Servicos;

public class ServicoEmprestimo
{
    private const decimal MultaDiaria = 2.5m;
    private const int DiasPadrao = 7;

    private readonly RepositorioEmprestimo _emprestimoDal = new();
    private readonly RepositorioAluno _alunoDal = new();
    private readonly RepositorioLivro _livroDal = new();
    private readonly RepositorioLogAcao _logDal = new();

    public IEnumerable<Emprestimo> ListarAtivos()
    {
        return _emprestimoDal.ListarAtivos();
    }

    public IEnumerable<Emprestimo> ListarPorAluno(string matricula)
    {
        var aluno = ObterAluno(matricula);
        return _emprestimoDal.ListarPorAluno(aluno.Id);
    }

    public Emprestimo RegistrarEmprestimo(string matriculaAluno, string codigoLivro, DateTime? dataPrevista, int? executorId = null)
    {
        var aluno = ObterAluno(matriculaAluno);
        var livro = ObterLivro(codigoLivro);

        if (livro.QuantidadeDisponivel <= 0)
        {
            throw new ExcecaoValidacao("Não há exemplares disponíveis para este livro.");
        }

        var emprestimo = new Emprestimo
        {
            IdAluno = aluno.Id,
            IdLivro = livro.Id,
            DataEmprestimo = DateTime.Today,
            DataPrevista = dataPrevista?.Date ?? DateTime.Today.AddDays(DiasPadrao),
            Multa = 0
        };

        emprestimo.Id = _emprestimoDal.Inserir(emprestimo);
        _livroDal.AtualizarDisponibilidade(livro.Id, livro.QuantidadeDisponivel - 1);
        RegistrarLog(executorId, "Empréstimo", $"Livro {livro.Titulo} emprestado para {aluno.Nome}.");
        return emprestimo;
    }

    public Emprestimo RegistrarDevolucao(string matriculaAluno, string codigoLivro, DateTime? dataDevolucao, int? executorId = null)
    {
        var aluno = ObterAluno(matriculaAluno);
        var livro = ObterLivro(codigoLivro);
        var emprestimo = _emprestimoDal.ListarPorAluno(aluno.Id, true)
            .FirstOrDefault(e => e.IdLivro == livro.Id)
            ?? _emprestimoDal.ObterEmprestimoAtivoPorLivro(livro.Id)
            ?? throw new ExcecaoValidacao("Nenhum empréstimo ativo encontrado para este livro.");

        var devolucao = dataDevolucao?.Date ?? DateTime.Today;
        var multa = CalcularMulta(emprestimo, devolucao);

        _emprestimoDal.RegistrarDevolucao(emprestimo.Id, devolucao, multa);
        _livroDal.AtualizarDisponibilidade(livro.Id, livro.QuantidadeDisponivel + 1);
        RegistrarLog(executorId, "Devolução", $"Livro {livro.Titulo} devolvido por {aluno.Nome}.");

        emprestimo.DataDevolucao = devolucao;
        emprestimo.Multa = multa;
        return emprestimo;
    }

    private static decimal CalcularMulta(Emprestimo emprestimo, DateTime dataDevolucao)
    {
        var atraso = (dataDevolucao - emprestimo.DataPrevista).Days;
        return atraso > 0 ? atraso * MultaDiaria : 0m;
    }

    private Aluno ObterAluno(string matricula)
    {
        Validador.GarantirNaoVazio(matricula, "Matrícula do aluno");
        return _alunoDal.ObterPorMatricula(matricula!)
               ?? throw new ExcecaoValidacao("Aluno não encontrado.");
    }

    private Livro ObterLivro(string codigo)
    {
        Validador.GarantirNaoVazio(codigo, "Código do livro");
        var livro = int.TryParse(codigo, out var id)
            ? _livroDal.ObterPorId(id)
            : _livroDal.ObterPorIsbn(codigo);

        return livro ?? throw new ExcecaoValidacao("Livro não encontrado.");
    }

    private void RegistrarLog(int? executorId, string acao, string descricao)
    {
        if (executorId == null)
        {
            return;
        }

        _logDal.Inserir(new LogAcao
        {
            IdFuncionario = executorId,
            Acao = acao,
            Descricao = descricao,
            DataHora = DateTime.Now
        });
    }
}
