using BibliotecaJK.AcessoDados;
using BibliotecaJK.Utilitarios;
using BibliotecaJK.Modelos;

namespace BibliotecaJK.Servicos;

public class ServicoAutenticacao
{
    private readonly RepositorioFuncionario _funcionarioDal = new();

    public Funcionario Autenticar(string login, string senha)
    {
        Validador.GarantirNaoVazio(login, "Login");
        Validador.GarantirNaoVazio(senha, "Senha");

        var funcionario = _funcionarioDal.ObterPorLogin(login!)
            ?? throw new ExcecaoValidacao("Usuário ou senha inválidos.");

        if (!GeradorHashSenha.Verificar(senha!, funcionario.SenhaHash))
        {
            throw new ExcecaoValidacao("Usuário ou senha inválidos.");
        }

        return funcionario;
    }
}
