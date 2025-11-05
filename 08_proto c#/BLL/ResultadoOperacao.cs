namespace BibliotecaJK.BLL
{
    /// <summary>
    /// Classe auxiliar para padronizar retornos das operações da camada de negócio
    /// </summary>
    public class ResultadoOperacao
    {
        /// <summary>
        /// Indica se a operação foi bem-sucedida
        /// </summary>
        public bool Sucesso { get; set; }

        /// <summary>
        /// Mensagem descritiva do resultado (sucesso ou erro)
        /// </summary>
        public string Mensagem { get; set; } = string.Empty;

        /// <summary>
        /// Valor da multa calculada (usado em devoluções)
        /// </summary>
        public decimal ValorMulta { get; set; }

        /// <summary>
        /// Dados adicionais que podem ser retornados
        /// </summary>
        public object? Dados { get; set; }

        /// <summary>
        /// Cria um resultado de sucesso
        /// </summary>
        public static ResultadoOperacao Ok(string mensagem, object? dados = null)
        {
            return new ResultadoOperacao
            {
                Sucesso = true,
                Mensagem = mensagem,
                Dados = dados
            };
        }

        /// <summary>
        /// Cria um resultado de sucesso com valor de multa
        /// </summary>
        public static ResultadoOperacao OkComMulta(string mensagem, decimal multa)
        {
            return new ResultadoOperacao
            {
                Sucesso = true,
                Mensagem = mensagem,
                ValorMulta = multa
            };
        }

        /// <summary>
        /// Cria um resultado de erro
        /// </summary>
        public static ResultadoOperacao Erro(string mensagem)
        {
            return new ResultadoOperacao
            {
                Sucesso = false,
                Mensagem = mensagem
            };
        }
    }
}
