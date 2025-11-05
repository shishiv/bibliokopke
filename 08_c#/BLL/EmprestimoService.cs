using System;
using System.Collections.Generic;
using System.Linq;
using BibliotecaJK.Model;
using BibliotecaJK.DAL;

namespace BibliotecaJK.BLL
{
    /// <summary>
    /// Serviço de gerenciamento de empréstimos de livros
    /// </summary>
    public class EmprestimoService
    {
        private readonly EmprestimoDAL _emprestimoDAL;
        private readonly LivroDAL _livroDAL;
        private readonly AlunoDAL _alunoDAL;
        private readonly LogService _logService;

        // Constantes de regras de negócio
        private const int PRAZO_DIAS = 7;
        private const int MAX_EMPRESTIMOS_SIMULTANEOS = 3;
        private const int MAX_RENOVACOES = 2;
        private const decimal MULTA_POR_DIA = 2.00m;

        public EmprestimoService()
        {
            _emprestimoDAL = new EmprestimoDAL();
            _livroDAL = new LivroDAL();
            _alunoDAL = new AlunoDAL();
            _logService = new LogService();
        }

        /// <summary>
        /// Registra um novo empréstimo
        /// </summary>
        /// <param name="idAluno">ID do aluno</param>
        /// <param name="idLivro">ID do livro</param>
        /// <param name="idFuncionario">ID do funcionário que está registrando (opcional)</param>
        /// <returns>Resultado da operação</returns>
        public ResultadoOperacao RegistrarEmprestimo(int idAluno, int idLivro, int? idFuncionario = null)
        {
            try
            {
                // 1. Validar se aluno existe
                var aluno = _alunoDAL.ObterPorId(idAluno);
                if (aluno == null)
                    return ResultadoOperacao.Erro("Aluno não encontrado.");

                // 2. Validar se livro existe
                var livro = _livroDAL.ObterPorId(idLivro);
                if (livro == null)
                    return ResultadoOperacao.Erro("Livro não encontrado.");

                // 3. Validar disponibilidade
                if (livro.QuantidadeDisponivel <= 0)
                    return ResultadoOperacao.Erro($"Livro '{livro.Titulo}' está indisponível no momento.");

                // 4. Validar empréstimos atrasados
                var emprestimosAtrasados = ObterEmprestimosAtrasados(idAluno);
                if (emprestimosAtrasados.Any())
                {
                    return ResultadoOperacao.Erro(
                        $"Aluno '{aluno.Nome}' possui {emprestimosAtrasados.Count} empréstimo(s) atrasado(s). " +
                        $"É necessário regularizar a situação antes de realizar novos empréstimos.");
                }

                // 5. Validar limite de empréstimos simultâneos
                var emprestimosAtivos = ObterEmprestimosAtivos(idAluno);
                if (emprestimosAtivos.Count >= MAX_EMPRESTIMOS_SIMULTANEOS)
                {
                    return ResultadoOperacao.Erro(
                        $"Aluno já possui o máximo de {MAX_EMPRESTIMOS_SIMULTANEOS} empréstimos ativos.");
                }

                // 6. Criar empréstimo
                var emprestimo = new Emprestimo
                {
                    IdAluno = idAluno,
                    IdLivro = idLivro,
                    DataEmprestimo = DateTime.Now,
                    DataPrevista = DateTime.Now.AddDays(PRAZO_DIAS),
                    DataDevolucao = null,
                    Multa = 0
                };

                // 7. Decrementar quantidade disponível
                livro.QuantidadeDisponivel--;
                _livroDAL.Atualizar(livro);

                // 8. Salvar empréstimo
                _emprestimoDAL.Inserir(emprestimo);

                // 9. Registrar log
                _logService.Registrar(idFuncionario, "EMPRESTIMO_REGISTRADO",
                    $"Aluno: {aluno.Nome} (ID: {idAluno}) | Livro: {livro.Titulo} (ID: {idLivro}) | " +
                    $"Prazo: {emprestimo.DataPrevista:dd/MM/yyyy}");

                return ResultadoOperacao.Ok(
                    $"Empréstimo registrado com sucesso!\n" +
                    $"Livro: {livro.Titulo}\n" +
                    $"Aluno: {aluno.Nome}\n" +
                    $"Data de devolução: {emprestimo.DataPrevista:dd/MM/yyyy}");
            }
            catch (Exception ex)
            {
                _logService.Registrar(idFuncionario, "ERRO_EMPRESTIMO",
                    $"Erro ao registrar empréstimo: {ex.Message}");
                return ResultadoOperacao.Erro($"Erro ao registrar empréstimo: {ex.Message}");
            }
        }

        /// <summary>
        /// Registra a devolução de um empréstimo
        /// </summary>
        /// <param name="idEmprestimo">ID do empréstimo</param>
        /// <param name="idFuncionario">ID do funcionário que está registrando (opcional)</param>
        /// <returns>Resultado da operação com valor da multa (se houver)</returns>
        public ResultadoOperacao RegistrarDevolucao(int idEmprestimo, int? idFuncionario = null)
        {
            try
            {
                // 1. Buscar empréstimo
                var emprestimo = _emprestimoDAL.ObterPorId(idEmprestimo);
                if (emprestimo == null)
                    return ResultadoOperacao.Erro("Empréstimo não encontrado.");

                // 2. Validar se ainda está ativo
                if (emprestimo.DataDevolucao != null)
                {
                    return ResultadoOperacao.Erro(
                        $"Este empréstimo já foi devolvido em {emprestimo.DataDevolucao:dd/MM/yyyy}.");
                }

                // 3. Calcular atraso e multa
                var hoje = DateTime.Now.Date;
                var dataPrevista = emprestimo.DataPrevista.Date;
                var diasAtraso = (hoje - dataPrevista).Days;
                decimal multa = 0;

                if (diasAtraso > 0)
                {
                    multa = diasAtraso * MULTA_POR_DIA;
                }

                // 4. Atualizar empréstimo
                emprestimo.DataDevolucao = DateTime.Now;
                emprestimo.Multa = multa;
                _emprestimoDAL.Atualizar(emprestimo);

                // 5. Incrementar quantidade disponível
                var livro = _livroDAL.ObterPorId(emprestimo.IdLivro);
                if (livro != null)
                {
                    livro.QuantidadeDisponivel++;
                    _livroDAL.Atualizar(livro);
                }

                // 6. Buscar informações do aluno para o log
                var aluno = _alunoDAL.ObterPorId(emprestimo.IdAluno);

                // 7. Registrar log
                var mensagemLog = multa > 0
                    ? $"Devolução com {diasAtraso} dia(s) de atraso. Multa: R$ {multa:F2}"
                    : "Devolução no prazo";
                _logService.Registrar(idFuncionario, "EMPRESTIMO_DEVOLVIDO",
                    $"Aluno: {aluno?.Nome ?? "Desconhecido"} | Livro: {livro?.Titulo ?? "Desconhecido"} | {mensagemLog}");

                // 8. Preparar mensagem de retorno
                var mensagem = multa > 0
                    ? $"Devolução registrada com sucesso!\n\n" +
                      $"⚠️ ATENÇÃO: Empréstimo atrasado!\n" +
                      $"Dias de atraso: {diasAtraso}\n" +
                      $"Valor da multa: R$ {multa:F2}\n" +
                      $"(R$ {MULTA_POR_DIA:F2} por dia)"
                    : $"Devolução registrada com sucesso!\n" +
                      $"Livro devolvido no prazo. Sem multa.";

                return ResultadoOperacao.OkComMulta(mensagem, multa);
            }
            catch (Exception ex)
            {
                _logService.Registrar(idFuncionario, "ERRO_DEVOLUCAO",
                    $"Erro ao registrar devolução: {ex.Message}");
                return ResultadoOperacao.Erro($"Erro ao registrar devolução: {ex.Message}");
            }
        }

        /// <summary>
        /// Renova um empréstimo ativo
        /// </summary>
        /// <param name="idEmprestimo">ID do empréstimo</param>
        /// <param name="idFuncionario">ID do funcionário (opcional)</param>
        /// <returns>Resultado da operação</returns>
        public ResultadoOperacao RenovarEmprestimo(int idEmprestimo, int? idFuncionario = null)
        {
            try
            {
                // 1. Buscar empréstimo
                var emprestimo = _emprestimoDAL.ObterPorId(idEmprestimo);
                if (emprestimo == null)
                    return ResultadoOperacao.Erro("Empréstimo não encontrado.");

                // 2. Validar se está ativo
                if (emprestimo.DataDevolucao != null)
                    return ResultadoOperacao.Erro("Não é possível renovar um empréstimo já devolvido.");

                // 3. Validar se não está atrasado
                if (DateTime.Now.Date > emprestimo.DataPrevista.Date)
                {
                    var diasAtraso = (DateTime.Now.Date - emprestimo.DataPrevista.Date).Days;
                    return ResultadoOperacao.Erro(
                        $"Não é possível renovar empréstimo com {diasAtraso} dia(s) de atraso. " +
                        $"Realize a devolução primeiro.");
                }

                // 4. Validar limite de renovações (campo não existe, mas podemos adicionar lógica simples)
                // Por enquanto, vamos permitir apenas se a data prevista ainda não foi estendida muito
                var diasDesdeEmprestimo = (DateTime.Now.Date - emprestimo.DataEmprestimo.Date).Days;
                if (diasDesdeEmprestimo > (PRAZO_DIAS * (MAX_RENOVACOES + 1)))
                {
                    return ResultadoOperacao.Erro(
                        $"Limite de renovações atingido (máximo {MAX_RENOVACOES} renovações).");
                }

                // 5. Estender prazo
                var novoPrazo = emprestimo.DataPrevista.AddDays(PRAZO_DIAS);
                emprestimo.DataPrevista = novoPrazo;
                _emprestimoDAL.Atualizar(emprestimo);

                // 6. Buscar informações para log
                var aluno = _alunoDAL.ObterPorId(emprestimo.IdAluno);
                var livro = _livroDAL.ObterPorId(emprestimo.IdLivro);

                // 7. Registrar log
                _logService.Registrar(idFuncionario, "EMPRESTIMO_RENOVADO",
                    $"Aluno: {aluno?.Nome ?? "Desconhecido"} | Livro: {livro?.Titulo ?? "Desconhecido"} | " +
                    $"Novo prazo: {novoPrazo:dd/MM/yyyy}");

                return ResultadoOperacao.Ok(
                    $"Empréstimo renovado com sucesso!\n" +
                    $"Nova data de devolução: {novoPrazo:dd/MM/yyyy}\n" +
                    $"(+{PRAZO_DIAS} dias)");
            }
            catch (Exception ex)
            {
                _logService.Registrar(idFuncionario, "ERRO_RENOVACAO",
                    $"Erro ao renovar empréstimo: {ex.Message}");
                return ResultadoOperacao.Erro($"Erro ao renovar empréstimo: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtém todos os empréstimos ativos de um aluno
        /// </summary>
        public List<Emprestimo> ObterEmprestimosAtivos(int idAluno)
        {
            return _emprestimoDAL.Listar()
                .Where(e => e.IdAluno == idAluno && e.DataDevolucao == null)
                .OrderBy(e => e.DataPrevista)
                .ToList();
        }

        /// <summary>
        /// Obtém todos os empréstimos atrasados de um aluno
        /// </summary>
        public List<Emprestimo> ObterEmprestimosAtrasados(int idAluno)
        {
            var hoje = DateTime.Now.Date;
            return _emprestimoDAL.Listar()
                .Where(e => e.IdAluno == idAluno &&
                           e.DataDevolucao == null &&
                           e.DataPrevista.Date < hoje)
                .OrderBy(e => e.DataPrevista)
                .ToList();
        }

        /// <summary>
        /// Obtém todos os empréstimos atrasados do sistema
        /// </summary>
        public List<Emprestimo> ObterTodosEmprestimosAtrasados()
        {
            var hoje = DateTime.Now.Date;
            return _emprestimoDAL.Listar()
                .Where(e => e.DataDevolucao == null &&
                           e.DataPrevista.Date < hoje)
                .OrderBy(e => e.DataPrevista)
                .ToList();
        }

        /// <summary>
        /// Obtém histórico de empréstimos de um aluno
        /// </summary>
        public List<Emprestimo> ObterHistoricoAluno(int idAluno)
        {
            return _emprestimoDAL.Listar()
                .Where(e => e.IdAluno == idAluno)
                .OrderByDescending(e => e.DataEmprestimo)
                .ToList();
        }

        /// <summary>
        /// Obtém histórico de empréstimos de um livro
        /// </summary>
        public List<Emprestimo> ObterHistoricoLivro(int idLivro)
        {
            return _emprestimoDAL.Listar()
                .Where(e => e.IdLivro == idLivro)
                .OrderByDescending(e => e.DataEmprestimo)
                .ToList();
        }

        /// <summary>
        /// Calcula estatísticas de empréstimos por período
        /// </summary>
        public (int Total, int Ativos, int Atrasados, decimal MultaTotal) ObterEstatisticas(DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            var emprestimos = _emprestimoDAL.Listar();

            if (dataInicio.HasValue)
                emprestimos = emprestimos.Where(e => e.DataEmprestimo >= dataInicio.Value).ToList();

            if (dataFim.HasValue)
                emprestimos = emprestimos.Where(e => e.DataEmprestimo <= dataFim.Value).ToList();

            var total = emprestimos.Count;
            var ativos = emprestimos.Count(e => e.DataDevolucao == null);
            var atrasados = emprestimos.Count(e => e.DataDevolucao == null && e.DataPrevista.Date < DateTime.Now.Date);
            var multaTotal = emprestimos.Sum(e => e.Multa);

            return (total, ativos, atrasados, multaTotal);
        }
    }
}
