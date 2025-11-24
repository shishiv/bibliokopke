using BibliotecaJK.AcessoDados;
using BibliotecaJK.Utilitarios;
using BibliotecaJK.Modelos;

namespace BibliotecaJK.Servicos;

public class ServicoLivro
{
    private readonly RepositorioLivro _livroDal = new();
    private readonly RepositorioLogAcao _logDal = new();

    public IEnumerable<Livro> Listar(string? termo = null)
    {
        return string.IsNullOrWhiteSpace(termo)
            ? _livroDal.Listar()
            : _livroDal.Buscar(termo);
    }

    public Livro Criar(Livro livro, int? executorId = null)
    {
        Validar(livro);
        livro.QuantidadeDisponivel = livro.QuantidadeTotal;
        livro.Id = _livroDal.Inserir(livro);
        RegistrarLog(executorId, "Cadastro de Livro", $"Livro {livro.Titulo} cadastrado.");
        return livro;
    }

    public Livro Atualizar(Livro livro, int? executorId = null)
    {
        if (livro.Id <= 0)
        {
            throw new ExcecaoValidacao("Selecione um livro existente para edição.");
        }

        var existente = _livroDal.ObterPorId(livro.Id)
                       ?? throw new ExcecaoValidacao("Livro não encontrado.");

        Validar(livro);
        var diferenca = livro.QuantidadeTotal - existente.QuantidadeTotal;
        livro.QuantidadeDisponivel = Math.Max(0, existente.QuantidadeDisponivel + diferenca);
        livro.QuantidadeDisponivel = Math.Min(livro.QuantidadeDisponivel, livro.QuantidadeTotal);

        _livroDal.Atualizar(livro);
        RegistrarLog(executorId, "Atualização de Livro", $"Livro {livro.Titulo} atualizado.");
        return livro;
    }

    public void Remover(int livroId, int? executorId = null)
    {
        Validador.GarantirNumeroPositivo(livroId, "Livro");
        _livroDal.Excluir(livroId);
        RegistrarLog(executorId, "Exclusão de Livro", $"Livro ID {livroId} excluído.");
    }

    public Livro? ObterPorCodigo(string codigo)
    {
        if (int.TryParse(codigo, out var id))
        {
            return _livroDal.ObterPorId(id);
        }

        return _livroDal.ObterPorIsbn(codigo);
    }

    private static void Validar(Livro livro)
    {
        Validador.GarantirNaoVazio(livro.Titulo, "Título");
        Validador.GarantirNumeroPositivo(livro.QuantidadeTotal, "Quantidade Total");
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
