using System;
using System.Collections.Generic;
using System.Linq;
using BibliotecaJK.Model;
using BibliotecaJK.DAL;

namespace BibliotecaJK.BLL
{
    /// <summary>
    /// Serviço de gerenciamento de reservas de livros
    /// </summary>
    public class ReservaService
    {
        private readonly ReservaDAL _reservaDAL;
        private readonly LivroDAL _livroDAL;
        private readonly AlunoDAL _alunoDAL;
        private readonly LogService _logService;

        public ReservaService()
        {
            _reservaDAL = new ReservaDAL();
            _livroDAL = new LivroDAL();
            _alunoDAL = new AlunoDAL();
            _logService = new LogService();
        }

        /// <summary>
        /// Cria uma nova reserva de livro
        /// </summary>
        /// <param name="idAluno">ID do aluno</param>
        /// <param name="idLivro">ID do livro</param>
        /// <param name="idFuncionario">ID do funcionário (opcional)</param>
        /// <returns>Resultado da operação</returns>
        public ResultadoOperacao CriarReserva(int idAluno, int idLivro, int? idFuncionario = null)
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

                // 3. Verificar se livro está disponível (não faz sentido reservar se está disponível)
                if (livro.QuantidadeDisponivel > 0)
                {
                    return ResultadoOperacao.Erro(
                        $"O livro '{livro.Titulo}' está disponível para empréstimo. " +
                        $"Não é necessário fazer reserva.");
                }

                // 4. Verificar se aluno já tem reserva ativa para este livro
                var reservaExistente = _reservaDAL.Listar()
                    .FirstOrDefault(r => r.IdAluno == idAluno &&
                                        r.IdLivro == idLivro &&
                                        r.Status == "ATIVA");

                if (reservaExistente != null)
                {
                    return ResultadoOperacao.Erro(
                        $"Você já possui uma reserva ativa para o livro '{livro.Titulo}'.");
                }

                // 5. Criar reserva
                var reserva = new Reserva
                {
                    IdAluno = idAluno,
                    IdLivro = idLivro,
                    DataReserva = DateTime.Now,
                    Status = "ATIVA"
                };

                _reservaDAL.Inserir(reserva);

                // 6. Calcular posição na fila
                var posicaoNaFila = ObterPosicaoNaFila(idAluno, idLivro);

                // 7. Registrar log
                _logService.Registrar(idFuncionario, "RESERVA_CRIADA",
                    $"Aluno: {aluno.Nome} | Livro: {livro.Titulo} | Posição na fila: {posicaoNaFila}");

                return ResultadoOperacao.Ok(
                    $"Reserva criada com sucesso!\n" +
                    $"Livro: {livro.Titulo}\n" +
                    $"Aluno: {aluno.Nome}\n" +
                    $"Posição na fila: {posicaoNaFila}º\n" +
                    $"Você será notificado quando o livro estiver disponível.");
            }
            catch (Exception ex)
            {
                _logService.Registrar(idFuncionario, "ERRO_RESERVA",
                    $"Erro ao criar reserva: {ex.Message}");
                return ResultadoOperacao.Erro($"Erro ao criar reserva: {ex.Message}");
            }
        }

        /// <summary>
        /// Cancela uma reserva
        /// </summary>
        public ResultadoOperacao CancelarReserva(int idReserva, int? idFuncionario = null)
        {
            try
            {
                var reserva = _reservaDAL.ObterPorId(idReserva);
                if (reserva == null)
                    return ResultadoOperacao.Erro("Reserva não encontrada.");

                if (reserva.Status != "ATIVA")
                    return ResultadoOperacao.Erro("Apenas reservas ativas podem ser canceladas.");

                // Atualizar status
                reserva.Status = "CANCELADA";
                _reservaDAL.Atualizar(reserva);

                // Buscar informações para log
                var aluno = _alunoDAL.ObterPorId(reserva.IdAluno);
                var livro = _livroDAL.ObterPorId(reserva.IdLivro);

                // Registrar log
                _logService.Registrar(idFuncionario, "RESERVA_CANCELADA",
                    $"Aluno: {aluno?.Nome ?? "Desconhecido"} | Livro: {livro?.Titulo ?? "Desconhecido"}");

                return ResultadoOperacao.Ok("Reserva cancelada com sucesso!");
            }
            catch (Exception ex)
            {
                _logService.Registrar(idFuncionario, "ERRO_CANCELAMENTO_RESERVA",
                    $"Erro ao cancelar reserva: {ex.Message}");
                return ResultadoOperacao.Erro($"Erro ao cancelar reserva: {ex.Message}");
            }
        }

        /// <summary>
        /// Processa a fila de reservas quando um livro é devolvido
        /// Deve ser chamado automaticamente após uma devolução
        /// </summary>
        /// <param name="idLivro">ID do livro devolvido</param>
        /// <returns>Informações da próxima reserva a ser atendida (se houver)</returns>
        public (bool TemReserva, Reserva? ProximaReserva, Aluno? ProximoAluno) ProcessarFilaReservas(int idLivro)
        {
            try
            {
                // Buscar reservas ativas para este livro, ordenadas por data (FIFO)
                var reservasAtivas = _reservaDAL.Listar()
                    .Where(r => r.IdLivro == idLivro && r.Status == "ATIVA")
                    .OrderBy(r => r.DataReserva)
                    .ToList();

                if (!reservasAtivas.Any())
                    return (false, null, null);

                // Pegar a primeira da fila
                var proximaReserva = reservasAtivas.First();
                var proximoAluno = _alunoDAL.ObterPorId(proximaReserva.IdAluno);

                // Marcar como atendida (ou poderia criar status "NOTIFICADA")
                proximaReserva.Status = "CONCLUIDA";
                _reservaDAL.Atualizar(proximaReserva);

                // Registrar log
                _logService.Registrar(null, "RESERVA_ATENDIDA",
                    $"Reserva ID {proximaReserva.Id} atendida. Aluno: {proximoAluno?.Nome ?? "Desconhecido"}");

                return (true, proximaReserva, proximoAluno);
            }
            catch (Exception ex)
            {
                _logService.Registrar(null, "ERRO_PROCESSAR_FILA",
                    $"Erro ao processar fila de reservas: {ex.Message}");
                return (false, null, null);
            }
        }

        /// <summary>
        /// Obtém a posição de um aluno na fila de reservas de um livro
        /// </summary>
        public int ObterPosicaoNaFila(int idAluno, int idLivro)
        {
            var reservasAtivas = _reservaDAL.Listar()
                .Where(r => r.IdLivro == idLivro && r.Status == "ATIVA")
                .OrderBy(r => r.DataReserva)
                .ToList();

            var posicao = reservasAtivas.FindIndex(r => r.IdAluno == idAluno) + 1;
            return posicao > 0 ? posicao : reservasAtivas.Count + 1;
        }

        /// <summary>
        /// Obtém todas as reservas ativas de um aluno
        /// </summary>
        public List<Reserva> ObterReservasAtivas(int idAluno)
        {
            return _reservaDAL.Listar()
                .Where(r => r.IdAluno == idAluno && r.Status == "ATIVA")
                .OrderBy(r => r.DataReserva)
                .ToList();
        }

        /// <summary>
        /// Obtém a fila de reservas de um livro
        /// </summary>
        public List<Reserva> ObterFilaReservas(int idLivro)
        {
            return _reservaDAL.Listar()
                .Where(r => r.IdLivro == idLivro && r.Status == "ATIVA")
                .OrderBy(r => r.DataReserva)
                .ToList();
        }

        /// <summary>
        /// Obtém histórico de reservas de um aluno
        /// </summary>
        public List<Reserva> ObterHistoricoReservas(int idAluno)
        {
            return _reservaDAL.Listar()
                .Where(r => r.IdAluno == idAluno)
                .OrderByDescending(r => r.DataReserva)
                .ToList();
        }

        /// <summary>
        /// Obtém quantidade de reservas por status
        /// </summary>
        public (int Ativas, int Canceladas, int Concluidas) ObterEstatisticas()
        {
            var todasReservas = _reservaDAL.Listar();
            var ativas = todasReservas.Count(r => r.Status == "ATIVA");
            var canceladas = todasReservas.Count(r => r.Status == "CANCELADA");
            var concluidas = todasReservas.Count(r => r.Status == "CONCLUIDA");

            return (ativas, canceladas, concluidas);
        }
    }
}
