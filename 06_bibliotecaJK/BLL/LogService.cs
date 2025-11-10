using System;
using System.Collections.Generic;
using System.Linq;
using BibliotecaJK.Model;
using BibliotecaJK.DAL;

namespace BibliotecaJK.BLL
{
    /// <summary>
    /// Serviço de gerenciamento de logs do sistema
    /// </summary>
    public class LogService
    {
        private readonly LogAcaoDAL _logDAL;

        public LogService()
        {
            _logDAL = new LogAcaoDAL();
        }

        /// <summary>
        /// Registra uma ação no sistema
        /// </summary>
        /// <param name="idFuncionario">ID do funcionário que executou a ação (null se for ação do sistema)</param>
        /// <param name="acao">Nome da ação executada</param>
        /// <param name="descricao">Descrição detalhada da ação</param>
        public void Registrar(int? idFuncionario, string acao, string descricao)
        {
            try
            {
                var log = new LogAcao
                {
                    IdFuncionario = idFuncionario,
                    Acao = acao,
                    Descricao = descricao,
                    DataHora = DateTime.Now
                };

                _logDAL.Inserir(log);
            }
            catch (Exception ex)
            {
                // Não deve lançar exceção para não quebrar o fluxo principal
                // Em produção, registrar em arquivo ou sistema de log alternativo
                Console.WriteLine($"[ERRO AO REGISTRAR LOG] {ex.Message}");
            }
        }

        /// <summary>
        /// Obtém todos os logs de um funcionário específico
        /// </summary>
        public List<LogAcao> ObterPorFuncionario(int idFuncionario)
        {
            return _logDAL.Listar()
                .Where(l => l.IdFuncionario == idFuncionario)
                .OrderByDescending(l => l.DataHora)
                .ToList();
        }

        /// <summary>
        /// Obtém logs por período
        /// </summary>
        public List<LogAcao> ObterPorPeriodo(DateTime dataInicio, DateTime dataFim)
        {
            return _logDAL.Listar()
                .Where(l => l.DataHora >= dataInicio && l.DataHora <= dataFim)
                .OrderByDescending(l => l.DataHora)
                .ToList();
        }

        /// <summary>
        /// Obtém logs por tipo de ação
        /// </summary>
        public List<LogAcao> ObterPorAcao(string acao)
        {
            return _logDAL.Listar()
                .Where(l => l.Acao != null && l.Acao.Contains(acao, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(l => l.DataHora)
                .ToList();
        }

        /// <summary>
        /// Obtém os últimos N logs
        /// </summary>
        public List<LogAcao> ObterUltimos(int quantidade = 50)
        {
            return _logDAL.Listar()
                .OrderByDescending(l => l.DataHora)
                .Take(quantidade)
                .ToList();
        }
    }
}
