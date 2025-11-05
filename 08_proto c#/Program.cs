using System;
using MySql.Data.MySqlClient;
using BibliotecaJK.Model;
using BibliotecaJK.DAL;
using BibliotecaJK.BLL;

namespace BibliotecaJK
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===========================================");
            Console.WriteLine("  PROTÓTIPO - Sistema BibliotecaJK v2.0");
            Console.WriteLine("  Com Camada BLL (Lógica de Negócio)");
            Console.WriteLine("===========================================\n");

            // Teste 1: Conexão com o banco
            Console.WriteLine("1. Testando conexão com o banco de dados...");
            if (!TestarConexao())
            {
                Console.WriteLine("\n❌ Não foi possível conectar ao banco!");
                Console.WriteLine("Verifique se o MySQL está rodando e execute o script 'schema.sql'");
                Console.WriteLine("\nPressione qualquer tecla para sair...");
                Console.ReadKey();
                return;
            }

            // Menu interativo
            bool continuar = true;
            while (continuar)
            {
                ExibirMenu();
                var opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        TestarValidadores();
                        break;
                    case "2":
                        TestarEmprestimoService();
                        break;
                    case "3":
                        TestarReservaService();
                        break;
                    case "4":
                        TestarLivroService();
                        break;
                    case "5":
                        TestarAlunoService();
                        break;
                    case "6":
                        TestarLogService();
                        break;
                    case "7":
                        TestarFluxoCompleto();
                        break;
                    case "0":
                        continuar = false;
                        Console.WriteLine("\n👋 Encerrando o sistema...");
                        break;
                    default:
                        Console.WriteLine("\n❌ Opção inválida!\n");
                        break;
                }

                if (continuar && opcao != "0")
                {
                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        static void ExibirMenu()
        {
            Console.WriteLine("\n===========================================");
            Console.WriteLine("  MENU DE TESTES - CAMADA BLL");
            Console.WriteLine("===========================================");
            Console.WriteLine("1. Testar Validadores (CPF, ISBN, Email)");
            Console.WriteLine("2. Testar EmprestimoService");
            Console.WriteLine("3. Testar ReservaService");
            Console.WriteLine("4. Testar LivroService");
            Console.WriteLine("5. Testar AlunoService");
            Console.WriteLine("6. Testar LogService");
            Console.WriteLine("7. Testar Fluxo Completo (Empréstimo → Devolução)");
            Console.WriteLine("0. Sair");
            Console.Write("\nEscolha uma opção: ");
        }

        static bool TestarConexao()
        {
            try
            {
                using var conn = Conexao.GetConnection();
                conn.Open();
                Console.WriteLine("   ✅ Conexão estabelecida com sucesso!\n");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ❌ Erro ao conectar: {ex.Message}\n");
                return false;
            }
        }

        static void TestarValidadores()
        {
            Console.WriteLine("\n=== TESTANDO VALIDADORES ===\n");

            // Teste CPF
            Console.WriteLine("📋 Teste de CPF:");
            string[] cpfs = { "111.111.111-11", "123.456.789-09", "000.000.000-00", "12345678909" };
            foreach (var cpf in cpfs)
            {
                var valido = Validadores.ValidarCPF(cpf);
                var simbolo = valido ? "✅" : "❌";
                Console.WriteLine($"   {simbolo} {cpf} → {(valido ? "VÁLIDO" : "INVÁLIDO")}");
            }

            // Teste ISBN
            Console.WriteLine("\n📚 Teste de ISBN:");
            string[] isbns = { "978-85-359-0277-4", "85-7326-981-6", "123456789X", "1234567890" };
            foreach (var isbn in isbns)
            {
                var valido = Validadores.ValidarISBN(isbn);
                var simbolo = valido ? "✅" : "❌";
                Console.WriteLine($"   {simbolo} {isbn} → {(valido ? "VÁLIDO" : "INVÁLIDO")}");
            }

            // Teste Email
            Console.WriteLine("\n📧 Teste de Email:");
            string[] emails = { "teste@email.com", "invalido@", "semdominio", "ok@dominio.com.br" };
            foreach (var email in emails)
            {
                var valido = Validadores.ValidarEmail(email);
                var simbolo = valido ? "✅" : "❌";
                Console.WriteLine($"   {simbolo} {email} → {(valido ? "VÁLIDO" : "INVÁLIDO")}");
            }
        }

        static void TestarEmprestimoService()
        {
            Console.WriteLine("\n=== TESTANDO EMPRESTIMO SERVICE ===\n");
            var service = new EmprestimoService();

            // Listar livros disponíveis
            var livroDAL = new LivroDAL();
            var livros = livroDAL.Listar();
            Console.WriteLine($"📚 Livros cadastrados: {livros.Count}");

            // Listar alunos
            var alunoDAL = new AlunoDAL();
            var alunos = alunoDAL.Listar();
            Console.WriteLine($"👤 Alunos cadastrados: {alunos.Count}\n");

            if (livros.Count == 0 || alunos.Count == 0)
            {
                Console.WriteLine("⚠️  Execute o script schema.sql primeiro para popular dados de teste!");
                return;
            }

            // Tentar registrar empréstimo
            Console.WriteLine("📖 Tentando registrar empréstimo...");
            var resultado = service.RegistrarEmprestimo(
                idAluno: alunos[0].Id,
                idLivro: livros[0].Id,
                idFuncionario: 1
            );

            if (resultado.Sucesso)
                Console.WriteLine($"✅ {resultado.Mensagem}");
            else
                Console.WriteLine($"❌ {resultado.Mensagem}");

            // Estatísticas
            Console.WriteLine("\n📊 Estatísticas de Empréstimos:");
            var stats = service.ObterEstatisticas();
            Console.WriteLine($"   Total: {stats.Total}");
            Console.WriteLine($"   Ativos: {stats.Ativos}");
            Console.WriteLine($"   Atrasados: {stats.Atrasados}");
            Console.WriteLine($"   Multa Total: R$ {stats.MultaTotal:F2}");
        }

        static void TestarReservaService()
        {
            Console.WriteLine("\n=== TESTANDO RESERVA SERVICE ===\n");
            var service = new ReservaService();

            // Tentar criar reserva (só funciona se livro estiver indisponível)
            Console.WriteLine("📅 Tentando criar reserva...");
            var alunoDAL = new AlunoDAL();
            var livroDAL = new LivroDAL();

            var alunos = alunoDAL.Listar();
            var livros = livroDAL.Listar();

            if (alunos.Count == 0 || livros.Count == 0)
            {
                Console.WriteLine("⚠️  Execute o script schema.sql primeiro!");
                return;
            }

            var resultado = service.CriarReserva(
                idAluno: alunos[0].Id,
                idLivro: livros[0].Id
            );

            Console.WriteLine(resultado.Sucesso ? $"✅ {resultado.Mensagem}" : $"❌ {resultado.Mensagem}");

            // Estatísticas
            Console.WriteLine("\n📊 Estatísticas de Reservas:");
            var stats = service.ObterEstatisticas();
            Console.WriteLine($"   Ativas: {stats.Ativas}");
            Console.WriteLine($"   Canceladas: {stats.Canceladas}");
            Console.WriteLine($"   Concluídas: {stats.Concluidas}");
        }

        static void TestarLivroService()
        {
            Console.WriteLine("\n=== TESTANDO LIVRO SERVICE ===\n");
            var service = new LivroService();

            // Buscar livros por título
            Console.WriteLine("🔍 Buscar por título 'Dom':");
            var livros = service.BuscarPorTitulo("Dom");
            foreach (var livro in livros)
            {
                Console.WriteLine($"   📚 {livro.Titulo} - {livro.Autor}");
            }

            // Livros mais emprestados
            Console.WriteLine("\n🏆 Top 5 Livros Mais Emprestados:");
            var topLivros = service.ObterMaisEmprestados(5);
            foreach (var (livro, total) in topLivros)
            {
                Console.WriteLine($"   📚 {livro.Titulo} - {total} empréstimo(s)");
            }

            // Estatísticas
            Console.WriteLine("\n📊 Estatísticas do Acervo:");
            var stats = service.ObterEstatisticas();
            Console.WriteLine($"   Total de Livros: {stats.TotalLivros}");
            Console.WriteLine($"   Total de Exemplares: {stats.TotalExemplares}");
            Console.WriteLine($"   Disponíveis: {stats.ExemplaresDisponiveis}");
            Console.WriteLine($"   Emprestados: {stats.ExemplaresEmprestados}");
        }

        static void TestarAlunoService()
        {
            Console.WriteLine("\n=== TESTANDO ALUNO SERVICE ===\n");
            var service = new AlunoService();

            // Tentar cadastrar aluno com CPF inválido
            Console.WriteLine("📝 Tentando cadastrar aluno com CPF inválido:");
            var aluno = new Aluno
            {
                Nome = "Teste Validação",
                CPF = "111.111.111-11", // CPF inválido
                Matricula = "MAT999"
            };

            var resultado = service.CadastrarAluno(aluno);
            Console.WriteLine(resultado.Sucesso ? $"✅ {resultado.Mensagem}" : $"❌ {resultado.Mensagem}");

            // Buscar alunos com empréstimos atrasados
            Console.WriteLine("\n⚠️  Alunos com Empréstimos Atrasados:");
            var alunosAtrasados = service.ObterAlunosComEmprestimosAtrasados();
            if (alunosAtrasados.Count == 0)
            {
                Console.WriteLine("   Nenhum aluno com empréstimos atrasados.");
            }
            else
            {
                foreach (var a in alunosAtrasados)
                {
                    Console.WriteLine($"   👤 {a.Nome} - {a.Matricula}");
                }
            }

            // Estatísticas
            Console.WriteLine("\n📊 Estatísticas de Alunos:");
            var stats = service.ObterEstatisticas();
            Console.WriteLine($"   Total: {stats.TotalAlunos}");
            Console.WriteLine($"   Com Empréstimos: {stats.ComEmprestimos}");
            Console.WriteLine($"   Com Atrasos: {stats.ComAtrasos}");
        }

        static void TestarLogService()
        {
            Console.WriteLine("\n=== TESTANDO LOG SERVICE ===\n");
            var service = new LogService();

            // Registrar um log de teste
            service.Registrar(1, "TESTE_SISTEMA", "Log de teste do Program.cs");

            // Obter últimos logs
            Console.WriteLine("📝 Últimos 10 Logs:");
            var logs = service.ObterUltimos(10);
            foreach (var log in logs)
            {
                Console.WriteLine($"   [{log.DataHora:dd/MM/yyyy HH:mm:ss}] {log.Acao} - {log.Descricao}");
            }
        }

        static void TestarFluxoCompleto()
        {
            Console.WriteLine("\n=== TESTANDO FLUXO COMPLETO ===\n");

            var emprestimoService = new EmprestimoService();
            var livroDAL = new LivroDAL();
            var alunoDAL = new AlunoDAL();

            var alunos = alunoDAL.Listar();
            var livros = livroDAL.Listar();

            if (alunos.Count == 0 || livros.Count == 0)
            {
                Console.WriteLine("⚠️  Execute o script schema.sql primeiro!");
                return;
            }

            var idAluno = alunos[0].Id;
            var idLivro = livros[0].Id;

            Console.WriteLine("PASSO 1: Verificar empréstimos ativos do aluno");
            var emprestimosAtivos = emprestimoService.ObterEmprestimosAtivos(idAluno);
            Console.WriteLine($"   📖 Aluno tem {emprestimosAtivos.Count} empréstimo(s) ativo(s)\n");

            Console.WriteLine("PASSO 2: Registrar novo empréstimo");
            var resultado = emprestimoService.RegistrarEmprestimo(idAluno, idLivro, 1);
            Console.WriteLine($"   {(resultado.Sucesso ? "✅" : "❌")} {resultado.Mensagem}\n");

            if (resultado.Sucesso)
            {
                Console.WriteLine("PASSO 3: Verificar empréstimos ativos após registro");
                emprestimosAtivos = emprestimoService.ObterEmprestimosAtivos(idAluno);
                Console.WriteLine($"   📖 Aluno agora tem {emprestimosAtivos.Count} empréstimo(s) ativo(s)\n");

                if (emprestimosAtivos.Count > 0)
                {
                    var ultimoEmprestimo = emprestimosAtivos[emprestimosAtivos.Count - 1];

                    Console.WriteLine("PASSO 4: Simular devolução imediata");
                    var resultadoDev = emprestimoService.RegistrarDevolucao(ultimoEmprestimo.Id, 1);
                    Console.WriteLine($"   {(resultadoDev.Sucesso ? "✅" : "❌")} {resultadoDev.Mensagem}");

                    if (resultadoDev.ValorMulta > 0)
                    {
                        Console.WriteLine($"   💰 Multa: R$ {resultadoDev.ValorMulta:F2}");
                    }
                }
            }

            Console.WriteLine("\n✅ Fluxo completo testado!");
        }
    }
}
