using System;
using System.Collections.Generic;
using System.Linq;
using BibliotecaJK.Model;
using BibliotecaJK.DAL;

namespace BibliotecaJK.BLL
{
    /// <summary>
    /// Serviço de gerenciamento de alunos
    /// </summary>
    public class AlunoService
    {
        private readonly AlunoDAL _alunoDAL;
        private readonly EmprestimoDAL _emprestimoDAL;
        private readonly LogService _logService;

        public AlunoService()
        {
            _alunoDAL = new AlunoDAL();
            _emprestimoDAL = new EmprestimoDAL();
            _logService = new LogService();
        }

        /// <summary>
        /// Cadastra um novo aluno com validações
        /// </summary>
        public ResultadoOperacao CadastrarAluno(Aluno aluno, int? idFuncionario = null)
        {
            try
            {
                // Validações de campos obrigatórios
                if (!Validadores.CampoObrigatorio(aluno.Nome, "Nome", out string mensagemErro))
                    return ResultadoOperacao.Erro(mensagemErro);

                if (!Validadores.CampoObrigatorio(aluno.CPF, "CPF", out mensagemErro))
                    return ResultadoOperacao.Erro(mensagemErro);

                if (!Validadores.CampoObrigatorio(aluno.Matricula, "Matrícula", out mensagemErro))
                    return ResultadoOperacao.Erro(mensagemErro);

                // Validar CPF
                if (!Validadores.ValidarCPF(aluno.CPF))
                    return ResultadoOperacao.Erro("CPF inválido.");

                // Validar matrícula
                if (!Validadores.ValidarMatricula(aluno.Matricula))
                    return ResultadoOperacao.Erro("Matrícula inválida. Deve conter entre 3 e 20 caracteres alfanuméricos.");

                // Validar e-mail (se fornecido)
                if (!string.IsNullOrWhiteSpace(aluno.Email) && !Validadores.ValidarEmail(aluno.Email))
                    return ResultadoOperacao.Erro("E-mail inválido.");

                // Verificar CPF duplicado
                var alunoPorCPF = _alunoDAL.Listar()
                    .FirstOrDefault(a => a.CPF == aluno.CPF);

                if (alunoPorCPF != null)
                    return ResultadoOperacao.Erro($"Já existe um aluno cadastrado com o CPF {aluno.CPF}.");

                // Verificar matrícula duplicada
                var alunoPorMatricula = _alunoDAL.Listar()
                    .FirstOrDefault(a => a.Matricula == aluno.Matricula);

                if (alunoPorMatricula != null)
                    return ResultadoOperacao.Erro($"Já existe um aluno cadastrado com a matrícula {aluno.Matricula}.");

                // Inserir aluno
                _alunoDAL.Inserir(aluno);

                // Registrar log
                _logService.Registrar(idFuncionario, "ALUNO_CADASTRADO",
                    $"Nome: {aluno.Nome} | CPF: {aluno.CPF} | Matrícula: {aluno.Matricula}");

                return ResultadoOperacao.Ok($"Aluno '{aluno.Nome}' cadastrado com sucesso!");
            }
            catch (Exception ex)
            {
                _logService.Registrar(idFuncionario, "ERRO_CADASTRO_ALUNO",
                    $"Erro ao cadastrar aluno: {ex.Message}");
                return ResultadoOperacao.Erro($"Erro ao cadastrar aluno: {ex.Message}");
            }
        }

        /// <summary>
        /// Atualiza dados de um aluno
        /// </summary>
        public ResultadoOperacao AtualizarAluno(Aluno aluno, int? idFuncionario = null)
        {
            try
            {
                // Verificar se aluno existe
                var alunoExistente = _alunoDAL.ObterPorId(aluno.Id);
                if (alunoExistente == null)
                    return ResultadoOperacao.Erro("Aluno não encontrado.");

                // Validações
                if (!Validadores.CampoObrigatorio(aluno.Nome, "Nome", out string mensagemErro))
                    return ResultadoOperacao.Erro(mensagemErro);

                if (!Validadores.CampoObrigatorio(aluno.CPF, "CPF", out mensagemErro))
                    return ResultadoOperacao.Erro(mensagemErro);

                if (!Validadores.ValidarCPF(aluno.CPF))
                    return ResultadoOperacao.Erro("CPF inválido.");

                if (!string.IsNullOrWhiteSpace(aluno.Email) && !Validadores.ValidarEmail(aluno.Email))
                    return ResultadoOperacao.Erro("E-mail inválido.");

                // Verificar CPF duplicado (exceto para o próprio aluno)
                var alunoPorCPF = _alunoDAL.Listar()
                    .FirstOrDefault(a => a.CPF == aluno.CPF && a.Id != aluno.Id);

                if (alunoPorCPF != null)
                    return ResultadoOperacao.Erro($"Já existe outro aluno cadastrado com o CPF {aluno.CPF}.");

                // Atualizar
                _alunoDAL.Atualizar(aluno);

                // Registrar log
                _logService.Registrar(idFuncionario, "ALUNO_ATUALIZADO",
                    $"ID: {aluno.Id} | Nome: {aluno.Nome}");

                return ResultadoOperacao.Ok("Aluno atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                _logService.Registrar(idFuncionario, "ERRO_ATUALIZAR_ALUNO",
                    $"Erro ao atualizar aluno: {ex.Message}");
                return ResultadoOperacao.Erro($"Erro ao atualizar aluno: {ex.Message}");
            }
        }

        /// <summary>
        /// Exclui um aluno (apenas se não tiver empréstimos ativos)
        /// </summary>
        public ResultadoOperacao ExcluirAluno(int idAluno, int? idFuncionario = null)
        {
            try
            {
                var aluno = _alunoDAL.ObterPorId(idAluno);
                if (aluno == null)
                    return ResultadoOperacao.Erro("Aluno não encontrado.");

                // Verificar se tem empréstimos ativos
                var emprestimosAtivos = _emprestimoDAL.Listar()
                    .Any(e => e.IdAluno == idAluno && e.DataDevolucao == null);

                if (emprestimosAtivos)
                {
                    return ResultadoOperacao.Erro(
                        "Não é possível excluir aluno com empréstimos ativos. " +
                        "Realize a devolução de todos os livros primeiro.");
                }

                // Excluir
                _alunoDAL.Excluir(idAluno);

                // Registrar log
                _logService.Registrar(idFuncionario, "ALUNO_EXCLUIDO",
                    $"ID: {idAluno} | Nome: {aluno.Nome}");

                return ResultadoOperacao.Ok($"Aluno '{aluno.Nome}' excluído com sucesso!");
            }
            catch (Exception ex)
            {
                _logService.Registrar(idFuncionario, "ERRO_EXCLUIR_ALUNO",
                    $"Erro ao excluir aluno: {ex.Message}");
                return ResultadoOperacao.Erro($"Erro ao excluir aluno: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtém alunos com empréstimos atrasados
        /// </summary>
        public List<Aluno> ObterAlunosComEmprestimosAtrasados()
        {
            var hoje = DateTime.Now.Date;
            var emprestimosAtrasados = _emprestimoDAL.Listar()
                .Where(e => e.DataDevolucao == null && e.DataPrevista.Date < hoje)
                .Select(e => e.IdAluno)
                .Distinct()
                .ToList();

            var alunos = new List<Aluno>();
            foreach (var idAluno in emprestimosAtrasados)
            {
                var aluno = _alunoDAL.ObterPorId(idAluno);
                if (aluno != null)
                    alunos.Add(aluno);
            }

            return alunos.OrderBy(a => a.Nome).ToList();
        }

        /// <summary>
        /// Obtém alunos com empréstimos ativos
        /// </summary>
        public List<Aluno> ObterAlunosComEmprestimosAtivos()
        {
            var emprestimosAtivos = _emprestimoDAL.Listar()
                .Where(e => e.DataDevolucao == null)
                .Select(e => e.IdAluno)
                .Distinct()
                .ToList();

            var alunos = new List<Aluno>();
            foreach (var idAluno in emprestimosAtivos)
            {
                var aluno = _alunoDAL.ObterPorId(idAluno);
                if (aluno != null)
                    alunos.Add(aluno);
            }

            return alunos.OrderBy(a => a.Nome).ToList();
        }

        /// <summary>
        /// Busca alunos por nome (busca parcial)
        /// </summary>
        public List<Aluno> BuscarPorNome(string termo)
        {
            if (string.IsNullOrWhiteSpace(termo))
                return new List<Aluno>();

            return _alunoDAL.Listar()
                .Where(a => a.Nome.Contains(termo, StringComparison.OrdinalIgnoreCase))
                .OrderBy(a => a.Nome)
                .ToList();
        }

        /// <summary>
        /// Busca aluno por CPF exato
        /// </summary>
        public Aluno? BuscarPorCPF(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return null;

            // Normalizar CPF (remover pontos e traços)
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            return _alunoDAL.Listar()
                .FirstOrDefault(a => new string(a.CPF.Where(char.IsDigit).ToArray()) == cpf);
        }

        /// <summary>
        /// Busca aluno por matrícula exata
        /// </summary>
        public Aluno? BuscarPorMatricula(string matricula)
        {
            if (string.IsNullOrWhiteSpace(matricula))
                return null;

            return _alunoDAL.Listar()
                .FirstOrDefault(a => a.Matricula.Equals(matricula, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Verifica se aluno está apto para empréstimo (sem atrasos)
        /// </summary>
        public (bool Apto, string Mensagem) VerificarAptoParaEmprestimo(int idAluno)
        {
            var aluno = _alunoDAL.ObterPorId(idAluno);
            if (aluno == null)
                return (false, "Aluno não encontrado.");

            // Verificar empréstimos atrasados
            var hoje = DateTime.Now.Date;
            var emprestimosAtrasados = _emprestimoDAL.Listar()
                .Where(e => e.IdAluno == idAluno &&
                           e.DataDevolucao == null &&
                           e.DataPrevista.Date < hoje)
                .ToList();

            if (emprestimosAtrasados.Any())
            {
                return (false,
                    $"Aluno possui {emprestimosAtrasados.Count} empréstimo(s) atrasado(s). " +
                    $"É necessário regularizar a situação.");
            }

            return (true, "Aluno apto para empréstimo.");
        }

        /// <summary>
        /// Obtém estatísticas gerais de alunos
        /// </summary>
        public (int TotalAlunos, int ComEmprestimos, int ComAtrasos) ObterEstatisticas()
        {
            var todosAlunos = _alunoDAL.Listar();
            var totalAlunos = todosAlunos.Count;

            var alunosComEmprestimos = _emprestimoDAL.Listar()
                .Where(e => e.DataDevolucao == null)
                .Select(e => e.IdAluno)
                .Distinct()
                .Count();

            var hoje = DateTime.Now.Date;
            var alunosComAtrasos = _emprestimoDAL.Listar()
                .Where(e => e.DataDevolucao == null && e.DataPrevista.Date < hoje)
                .Select(e => e.IdAluno)
                .Distinct()
                .Count();

            return (totalAlunos, alunosComEmprestimos, alunosComAtrasos);
        }
    }
}
