using BibliotecaJK.AcessoDados;
using BibliotecaJK.Utilitarios;
using BibliotecaJK.Modelos;

namespace BibliotecaJK.Servicos;

public class ServicoFuncionario
{
    private readonly RepositorioFuncionario _funcionarioDal = new();
    private readonly RepositorioLogAcao _logDal = new();

    public IEnumerable<Funcionario> Listar(string? termo = null)
    {
        return string.IsNullOrWhiteSpace(termo)
            ? _funcionarioDal.Listar()
            : _funcionarioDal.Buscar(termo);
    }

    public Funcionario Criar(Funcionario funcionario, int? executorId = null)
    {
        Validar(funcionario, true);
        GarantirUnicidade(funcionario);

        funcionario.SenhaHash = GeradorHashSenha.GerarHash(funcionario.SenhaHash);
        funcionario.Perfil = string.IsNullOrWhiteSpace(funcionario.Perfil) ? "OPERADOR" : funcionario.Perfil;
        funcionario.Id = _funcionarioDal.Inserir(funcionario);

        RegistrarLog(executorId, "Cadastro de Funcionário", $"Funcionário {funcionario.Nome} criado (login: {funcionario.Login}).");
        return funcionario;
    }

    public Funcionario Atualizar(Funcionario funcionario, bool atualizarSenha, int? executorId = null)
    {
        if (funcionario.Id <= 0)
        {
            throw new ExcecaoValidacao("É necessário informar o funcionário selecionado para atualização.");
        }

        Validar(funcionario, atualizarSenha);

        var existente = _funcionarioDal.ObterPorId(funcionario.Id)
                        ?? throw new ExcecaoValidacao("Funcionário não encontrado.");

        if (!string.Equals(existente.Login, funcionario.Login, StringComparison.OrdinalIgnoreCase))
        {
            GarantirUnicidade(funcionario);
        }

        if (atualizarSenha)
        {
            funcionario.SenhaHash = GeradorHashSenha.GerarHash(funcionario.SenhaHash);
        }
        else
        {
            funcionario.SenhaHash = existente.SenhaHash;
        }

        funcionario.Perfil = string.IsNullOrWhiteSpace(funcionario.Perfil) ? existente.Perfil : funcionario.Perfil;
        _funcionarioDal.Atualizar(funcionario);

        RegistrarLog(executorId, "Atualização de Funcionário", $"Funcionário {funcionario.Nome} atualizado.");
        return funcionario;
    }

    public void Remover(int funcionarioId, int? executorId = null)
    {
        Validador.GarantirNumeroPositivo(funcionarioId, "Funcionário");
        _funcionarioDal.Excluir(funcionarioId);
        RegistrarLog(executorId, "Exclusão de Funcionário", $"Funcionário ID {funcionarioId} foi excluído.");
    }

    private static void Validar(Funcionario funcionario, bool exigirSenha)
    {
        Validador.GarantirNaoVazio(funcionario.Nome, "Nome");
        Validador.GarantirCpfValido(funcionario.CPF);
        Validador.GarantirNaoVazio(funcionario.Login, "Login");

        if (exigirSenha)
        {
            Validador.GarantirNaoVazio(funcionario.SenhaHash, "Senha");
        }
    }

    private void GarantirUnicidade(Funcionario funcionario)
    {
        if (_funcionarioDal.ObterPorLogin(funcionario.Login!) != null)
        {
            throw new ExcecaoValidacao("Já existe um funcionário com esse login.");
        }

        var existenteCpf = _funcionarioDal.ObterPorCpf(Validador.ExtrairDigitos(funcionario.CPF!));
        if (existenteCpf != null)
        {
            throw new ExcecaoValidacao("Já existe um funcionário com esse CPF.");
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
