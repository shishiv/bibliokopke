using BibliotecaJK.AcessoDados;
using BibliotecaJK.Utilitarios;
using BibliotecaJK.Modelos;

namespace BibliotecaJK.Servicos;

public class ServicoAluno
{
    private readonly RepositorioAluno _alunoDal = new();
    private readonly RepositorioLogAcao _logDal = new();

    public IEnumerable<Aluno> Listar(string? termo = null)
    {
        return string.IsNullOrWhiteSpace(termo)
            ? _alunoDal.Listar()
            : _alunoDal.Buscar(termo);
    }

    public Aluno Criar(Aluno aluno, int? executorId = null)
    {
        Validar(aluno);
        GarantirAlunoUnico(aluno);
        aluno.Id = _alunoDal.Inserir(aluno);
        RegistrarLog(executorId, "Cadastro de Aluno", $"Aluno {aluno.Nome} cadastrado (matrícula {aluno.Matricula}).");
        return aluno;
    }

    public Aluno Atualizar(Aluno aluno, int? executorId = null)
    {
        if (aluno.Id <= 0)
        {
            var existente = _alunoDal.ObterPorMatricula(aluno.Matricula)
                           ?? throw new ExcecaoValidacao("Selecione um aluno existente para editar.");
            aluno.Id = existente.Id;
        }

        Validar(aluno);
        _alunoDal.Atualizar(aluno);
        RegistrarLog(executorId, "Atualização de Aluno", $"Aluno {aluno.Nome} atualizado.");
        return aluno;
    }

    public void Remover(int alunoId, int? executorId = null)
    {
        Validador.GarantirNumeroPositivo(alunoId, "Aluno");
        _alunoDal.Excluir(alunoId);
        RegistrarLog(executorId, "Exclusão de Aluno", $"Aluno ID {alunoId} foi excluído.");
    }

    public Aluno? ObterPorMatricula(string matricula)
    {
        return _alunoDal.ObterPorMatricula(matricula);
    }

    private static void Validar(Aluno aluno)
    {
        Validador.GarantirNaoVazio(aluno.Nome, "Nome");
        Validador.GarantirCpfValido(aluno.CPF);
        Validador.GarantirNaoVazio(aluno.Matricula, "Matrícula");
    }

    private void GarantirAlunoUnico(Aluno aluno)
    {
        if (_alunoDal.ObterPorMatricula(aluno.Matricula) != null)
        {
            throw new ExcecaoValidacao("Já existe um aluno com essa matrícula.");
        }

        var cpfLimpo = Validador.ExtrairDigitos(aluno.CPF!);
        if (_alunoDal.ObterPorCpf(cpfLimpo) != null)
        {
            throw new ExcecaoValidacao("Já existe um aluno com esse CPF.");
        }
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
