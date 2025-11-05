using System;

namespace BibliotecaJK.BLL
{
    /// <summary>
    /// Exceção lançada quando uma regra de negócio é violada
    /// </summary>
    public class RegraDeNegocioException : Exception
    {
        public RegraDeNegocioException(string mensagem) : base(mensagem) { }
    }

    /// <summary>
    /// Exceção lançada quando uma entidade não é encontrada
    /// </summary>
    public class EntidadeNaoEncontradaException : Exception
    {
        public EntidadeNaoEncontradaException(string entidade, int id)
            : base($"{entidade} com ID {id} não foi encontrado(a).") { }

        public EntidadeNaoEncontradaException(string mensagem) : base(mensagem) { }
    }

    /// <summary>
    /// Exceção lançada quando a validação de dados falha
    /// </summary>
    public class ValidacaoException : Exception
    {
        public ValidacaoException(string mensagem) : base(mensagem) { }
    }
}
