namespace BibliotecaJK
{
    /// <summary>
    /// Constantes globais do sistema BibliotecaJK
    /// Centraliza regras de negócio, limites e strings reutilizáveis
    /// </summary>
    public static class Constants
    {
        // ==========================================
        // REGRAS DE NEGÓCIO - EMPRÉSTIMOS
        // ==========================================

        /// <summary>
        /// Prazo padrão para devolução de livros (em dias)
        /// </summary>
        public const int PRAZO_EMPRESTIMO_DIAS = 7;

        /// <summary>
        /// Número máximo de empréstimos simultâneos por aluno
        /// </summary>
        public const int MAX_EMPRESTIMOS_SIMULTANEOS = 3;

        /// <summary>
        /// Número máximo de renovações permitidas
        /// </summary>
        public const int MAX_RENOVACOES = 2;

        /// <summary>
        /// Valor da multa por dia de atraso (em reais)
        /// </summary>
        public const decimal MULTA_POR_DIA = 2.00m;

        // ==========================================
        // REGRAS DE NEGÓCIO - RESERVAS
        // ==========================================

        /// <summary>
        /// Validade de uma reserva (em dias)
        /// </summary>
        public const int VALIDADE_RESERVA_DIAS = 7;

        /// <summary>
        /// Máximo de reservas ativas por aluno
        /// </summary>
        public const int MAX_RESERVAS_POR_ALUNO = 3;

        // ==========================================
        // STATUS DO SISTEMA
        // ==========================================

        public static class StatusEmprestimo
        {
            public const string ATIVO = "ATIVO";
            public const string DEVOLVIDO = "DEVOLVIDO";
            public const string ATRASADO = "ATRASADO";
        }

        public static class StatusReserva
        {
            public const string ATIVA = "ATIVA";
            public const string CANCELADA = "CANCELADA";
            public const string CONCLUIDA = "CONCLUIDA";
            public const string EXPIRADA = "EXPIRADA";
        }

        public static class PerfilFuncionario
        {
            public const string ADMIN = "ADMIN";
            public const string BIBLIOTECARIO = "BIBLIOTECARIO";
            public const string OPERADOR = "OPERADOR";
        }

        public static class TipoNotificacao
        {
            public const string EMPRESTIMO_ATRASADO = "EMPRESTIMO_ATRASADO";
            public const string RESERVA_EXPIRADA = "RESERVA_EXPIRADA";
            public const string LIVRO_DISPONIVEL = "LIVRO_DISPONIVEL";
            public const string MULTA_PENDENTE = "MULTA_PENDENTE";
        }

        public static class PrioridadeNotificacao
        {
            public const string BAIXA = "BAIXA";
            public const string NORMAL = "NORMAL";
            public const string ALTA = "ALTA";
            public const string URGENTE = "URGENTE";
        }

        // ==========================================
        // VALIDAÇÃO E LIMITES
        // ==========================================

        /// <summary>
        /// Tamanho mínimo de senha
        /// </summary>
        public const int SENHA_MIN_LENGTH = 8;

        /// <summary>
        /// Tamanho máximo de senha
        /// </summary>
        public const int SENHA_MAX_LENGTH = 50;

        /// <summary>
        /// Formato esperado de CPF (com máscara)
        /// </summary>
        public const string CPF_FORMATO = "000.000.000-00";

        /// <summary>
        /// Formato esperado de telefone (com máscara)
        /// </summary>
        public const string TELEFONE_FORMATO = "(00) 00000-0000";

        // ==========================================
        // CONFIGURAÇÃO E ARQUIVOS
        // ==========================================

        /// <summary>
        /// Nome do arquivo de configuração do banco de dados
        /// </summary>
        public const string CONFIG_FILE_NAME = "database.config";

        /// <summary>
        /// Nome da pasta de configuração (em LocalAppData)
        /// </summary>
        public const string CONFIG_FOLDER_NAME = "BibliotecaJK";

        /// <summary>
        /// Nome do arquivo de schema SQL
        /// </summary>
        public const string SCHEMA_FILE_NAME = "schema-postgresql.sql";

        // ==========================================
        // MENSAGENS DO SISTEMA
        // ==========================================

        public static class Mensagens
        {
            // Sucesso
            public const string OPERACAO_SUCESSO = "Operação realizada com sucesso!";
            public const string EMPRESTIMO_REGISTRADO = "Empréstimo registrado com sucesso!";
            public const string DEVOLUCAO_REALIZADA = "Devolução realizada com sucesso!";
            public const string RESERVA_CRIADA = "Reserva criada com sucesso!";
            public const string CADASTRO_SALVO = "Cadastro salvo com sucesso!";

            // Erros
            public const string ERRO_DESCONHECIDO = "Erro desconhecido. Contate o suporte.";
            public const string ERRO_CONEXAO_BD = "Erro ao conectar ao banco de dados.";
            public const string ERRO_SALVAR_DADOS = "Erro ao salvar dados.";

            // Validações
            public const string CPF_INVALIDO = "CPF inválido.";
            public const string EMAIL_INVALIDO = "E-mail inválido.";
            public const string ISBN_INVALIDO = "ISBN inválido.";
            public const string CAMPO_OBRIGATORIO = "Este campo é obrigatório.";
            public const string SENHA_FRACA = "Senha deve ter no mínimo 8 caracteres.";

            // Empréstimos
            public const string LIVRO_INDISPONIVEL = "Livro indisponível para empréstimo.";
            public const string LIMITE_EMPRESTIMOS_ATINGIDO = "Aluno atingiu o limite de empréstimos simultâneos.";
            public const string ALUNO_COM_PENDENCIAS = "Aluno possui empréstimos em atraso.";

            // Setup
            public const string SETUP_BEM_VINDO = "Bem-vindo ao BibliotecaJK! Configure a conexão com o banco de dados.";
            public const string SETUP_TABELAS_ENCONTRADAS = "Todas as tabelas foram encontradas. Sistema pronto para uso!";
            public const string SETUP_TABELAS_FALTANDO = "Algumas tabelas estão faltando. Execute o schema SQL.";
        }

        // ==========================================
        // TABELAS DO BANCO DE DADOS
        // ==========================================

        public static class Tabelas
        {
            public const string ALUNO = "aluno";
            public const string FUNCIONARIO = "funcionario";
            public const string LIVRO = "livro";
            public const string EMPRESTIMO = "emprestimo";
            public const string RESERVA = "reserva";
            public const string LOG_ACAO = "log_acao";
            public const string NOTIFICACAO = "notificacao";
        }

        // ==========================================
        // CREDENCIAIS PADRÃO
        // ==========================================

        public static class CredenciaisPadrao
        {
            public const string LOGIN_ADMIN = "admin";
            public const string SENHA_ADMIN = "admin123";
            public const string CPF_ADMIN = "000.000.000-00";
        }
    }
}
