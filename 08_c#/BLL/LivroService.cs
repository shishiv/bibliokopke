using System;
using System.Collections.Generic;
using System.Linq;
using BibliotecaJK.Model;
using BibliotecaJK.DAL;

namespace BibliotecaJK.BLL
{
    /// <summary>
    /// Serviço de gerenciamento de livros
    /// </summary>
    public class LivroService
    {
        private readonly LivroDAL _livroDAL;
        private readonly EmprestimoDAL _emprestimoDAL;
        private readonly LogService _logService;

        public LivroService()
        {
            _livroDAL = new LivroDAL();
            _emprestimoDAL = new EmprestimoDAL();
            _logService = new LogService();
        }

        /// <summary>
        /// Cadastra um novo livro com validações
        /// </summary>
        public ResultadoOperacao CadastrarLivro(Livro livro, int? idFuncionario = null)
        {
            try
            {
                // Validações
                if (!Validadores.CampoObrigatorio(livro.Titulo, "Título", out string mensagemErro))
                    return ResultadoOperacao.Erro(mensagemErro);

                if (!string.IsNullOrWhiteSpace(livro.ISBN) && !Validadores.ValidarISBN(livro.ISBN))
                    return ResultadoOperacao.Erro("ISBN inválido.");

                if (livro.QuantidadeTotal <= 0)
                    return ResultadoOperacao.Erro("Quantidade total deve ser maior que zero.");

                if (livro.QuantidadeDisponivel < 0)
                    return ResultadoOperacao.Erro("Quantidade disponível não pode ser negativa.");

                if (livro.QuantidadeDisponivel > livro.QuantidadeTotal)
                    return ResultadoOperacao.Erro("Quantidade disponível não pode ser maior que a quantidade total.");

                // Verificar ISBN duplicado (se fornecido)
                if (!string.IsNullOrWhiteSpace(livro.ISBN))
                {
                    var livroExistente = _livroDAL.Listar()
                        .FirstOrDefault(l => l.ISBN == livro.ISBN);

                    if (livroExistente != null)
                        return ResultadoOperacao.Erro($"Já existe um livro cadastrado com o ISBN {livro.ISBN}.");
                }

                // Inserir livro
                _livroDAL.Inserir(livro);

                // Registrar log
                _logService.Registrar(idFuncionario, "LIVRO_CADASTRADO",
                    $"Título: {livro.Titulo} | Autor: {livro.Autor ?? "Não informado"} | ISBN: {livro.ISBN ?? "Não informado"}");

                return ResultadoOperacao.Ok($"Livro '{livro.Titulo}' cadastrado com sucesso!");
            }
            catch (Exception ex)
            {
                _logService.Registrar(idFuncionario, "ERRO_CADASTRO_LIVRO",
                    $"Erro ao cadastrar livro: {ex.Message}");
                return ResultadoOperacao.Erro($"Erro ao cadastrar livro: {ex.Message}");
            }
        }

        /// <summary>
        /// Atualiza dados de um livro
        /// </summary>
        public ResultadoOperacao AtualizarLivro(Livro livro, int? idFuncionario = null)
        {
            try
            {
                // Verificar se livro existe
                var livroExistente = _livroDAL.ObterPorId(livro.Id);
                if (livroExistente == null)
                    return ResultadoOperacao.Erro("Livro não encontrado.");

                // Validações
                if (!Validadores.CampoObrigatorio(livro.Titulo, "Título", out string mensagemErro))
                    return ResultadoOperacao.Erro(mensagemErro);

                if (!string.IsNullOrWhiteSpace(livro.ISBN) && !Validadores.ValidarISBN(livro.ISBN))
                    return ResultadoOperacao.Erro("ISBN inválido.");

                // Atualizar
                _livroDAL.Atualizar(livro);

                // Registrar log
                _logService.Registrar(idFuncionario, "LIVRO_ATUALIZADO",
                    $"ID: {livro.Id} | Título: {livro.Titulo}");

                return ResultadoOperacao.Ok("Livro atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                _logService.Registrar(idFuncionario, "ERRO_ATUALIZAR_LIVRO",
                    $"Erro ao atualizar livro: {ex.Message}");
                return ResultadoOperacao.Erro($"Erro ao atualizar livro: {ex.Message}");
            }
        }

        /// <summary>
        /// Verifica se um livro está disponível para empréstimo
        /// </summary>
        public bool VerificarDisponibilidade(int idLivro)
        {
            var livro = _livroDAL.ObterPorId(idLivro);
            return livro != null && livro.QuantidadeDisponivel > 0;
        }

        /// <summary>
        /// Busca livros por título (busca parcial)
        /// </summary>
        public List<Livro> BuscarPorTitulo(string termo)
        {
            if (string.IsNullOrWhiteSpace(termo))
                return new List<Livro>();

            return _livroDAL.Listar()
                .Where(l => l.Titulo.Contains(termo, StringComparison.OrdinalIgnoreCase))
                .OrderBy(l => l.Titulo)
                .ToList();
        }

        /// <summary>
        /// Busca livros por autor (busca parcial)
        /// </summary>
        public List<Livro> BuscarPorAutor(string termo)
        {
            if (string.IsNullOrWhiteSpace(termo))
                return new List<Livro>();

            return _livroDAL.Listar()
                .Where(l => l.Autor != null && l.Autor.Contains(termo, StringComparison.OrdinalIgnoreCase))
                .OrderBy(l => l.Autor)
                .ToList();
        }

        /// <summary>
        /// Busca livro por ISBN exato
        /// </summary>
        public Livro? BuscarPorISBN(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                return null;

            // Normalizar ISBN (remover hífens)
            isbn = isbn.Replace("-", "").Replace(" ", "");

            return _livroDAL.Listar()
                .FirstOrDefault(l => l.ISBN != null &&
                                    l.ISBN.Replace("-", "").Replace(" ", "") == isbn);
        }

        /// <summary>
        /// Obtém os livros mais emprestados
        /// </summary>
        /// <param name="top">Quantidade de livros a retornar</param>
        /// <returns>Lista de livros ordenados por quantidade de empréstimos</returns>
        public List<(Livro Livro, int TotalEmprestimos)> ObterMaisEmprestados(int top = 10)
        {
            var todosEmprestimos = _emprestimoDAL.Listar();

            var estatisticas = todosEmprestimos
                .GroupBy(e => e.IdLivro)
                .Select(g => new
                {
                    IdLivro = g.Key,
                    TotalEmprestimos = g.Count()
                })
                .OrderByDescending(x => x.TotalEmprestimos)
                .Take(top)
                .ToList();

            var resultado = new List<(Livro, int)>();

            foreach (var stat in estatisticas)
            {
                var livro = _livroDAL.ObterPorId(stat.IdLivro);
                if (livro != null)
                {
                    resultado.Add((livro, stat.TotalEmprestimos));
                }
            }

            return resultado;
        }

        /// <summary>
        /// Obtém todos os livros disponíveis
        /// </summary>
        public List<Livro> ObterDisponiveis()
        {
            return _livroDAL.Listar()
                .Where(l => l.QuantidadeDisponivel > 0)
                .OrderBy(l => l.Titulo)
                .ToList();
        }

        /// <summary>
        /// Obtém todos os livros indisponíveis
        /// </summary>
        public List<Livro> ObterIndisponiveis()
        {
            return _livroDAL.Listar()
                .Where(l => l.QuantidadeDisponivel == 0)
                .OrderBy(l => l.Titulo)
                .ToList();
        }

        /// <summary>
        /// Obtém estatísticas do acervo
        /// </summary>
        public (int TotalLivros, int TotalExemplares, int ExemplaresDisponiveis, int ExemplaresEmprestados) ObterEstatisticas()
        {
            var livros = _livroDAL.Listar();

            var totalLivros = livros.Count;
            var totalExemplares = livros.Sum(l => l.QuantidadeTotal);
            var exemplaresDisponiveis = livros.Sum(l => l.QuantidadeDisponivel);
            var exemplaresEmprestados = totalExemplares - exemplaresDisponiveis;

            return (totalLivros, totalExemplares, exemplaresDisponiveis, exemplaresEmprestados);
        }
    }
}
