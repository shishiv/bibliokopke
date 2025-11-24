using BibliotecaJK.AcessoDados;
using BibliotecaJK.Utilitarios;
using BibliotecaJK.Modelos;

namespace BibliotecaJK.Servicos;

public class ServicoReserva
{
    private readonly RepositorioReserva _reservaDal = new();
    private readonly RepositorioAluno _alunoDal = new();
    private readonly RepositorioLivro _livroDal = new();
    private readonly RepositorioLogAcao _logDal = new();

    public IEnumerable<Reserva> ListarAtivas()
    {
        return _reservaDal.ListarAtivas();
    }

    public IEnumerable<Reserva> ListarPorAluno(string matricula)
    {
        var aluno = ObterAluno(matricula);
        return _reservaDal.ListarPorAluno(aluno.Id);
    }

    public Reserva CriarReserva(string matriculaAluno, string codigoLivro, DateTime dataReserva, int? executorId = null)
    {
        var aluno = ObterAluno(matriculaAluno);
        var livro = ObterLivro(codigoLivro);

        var reserva = new Reserva
        {
            IdAluno = aluno.Id,
            IdLivro = livro.Id,
            DataReserva = dataReserva.Date,
            Status = "ATIVA"
        };

        reserva.Id = _reservaDal.Inserir(reserva);
        RegistrarLog(executorId, "Reserva", $"Livro {livro.Titulo} reservado por {aluno.Nome}.");
        return reserva;
    }

    public void Cancelar(int reservaId, int? executorId = null)
    {
        Validador.GarantirNumeroPositivo(reservaId, "Reserva");
        _reservaDal.AtualizarStatus(reservaId, "CANCELADA");
        RegistrarLog(executorId, "Cancelamento de Reserva", $"Reserva ID {reservaId} cancelada.");
    }

    private Aluno ObterAluno(string matricula)
    {
        Validador.GarantirNaoVazio(matricula, "Matrícula");
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
