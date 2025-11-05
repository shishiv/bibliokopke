using System;
using MySql.Data.MySqlClient;
using BibliotecaJK.Model;
using BibliotecaJK.DAL;

namespace BibliotecaJK
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===========================================");
            Console.WriteLine("  PROTÓTIPO - Sistema BibliotecaJK v1.0");
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
                        TestarAlunos();
                        break;
                    case "2":
                        TestarFuncionarios();
                        break;
                    case "3":
                        TestarLivros();
                        break;
                    case "4":
                        TestarEmprestimos();
                        break;
                    case "5":
                        TestarReservas();
                        break;
                    case "6":
                        TestarLogs();
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
            Console.WriteLine("  MENU PRINCIPAL");
            Console.WriteLine("===========================================");
            Console.WriteLine("1. Testar CRUD de Alunos");
            Console.WriteLine("2. Testar CRUD de Funcionários");
            Console.WriteLine("3. Testar CRUD de Livros");
            Console.WriteLine("4. Testar CRUD de Empréstimos");
            Console.WriteLine("5. Testar CRUD de Reservas");
            Console.WriteLine("6. Testar CRUD de Logs");
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

        static void TestarAlunos()
        {
            Console.WriteLine("\n--- TESTANDO CRUD DE ALUNOS ---");
            var dal = new AlunoDAL();

            try
            {
                // Listar todos
                Console.WriteLine("\n📋 Listando todos os alunos:");
                var alunos = dal.Listar();
                foreach (var aluno in alunos)
                {
                    Console.WriteLine($"   ID: {aluno.Id} | Nome: {aluno.Nome} | CPF: {aluno.CPF} | Matrícula: {aluno.Matricula} | Turma: {aluno.Turma ?? "N/A"}");
                }
                Console.WriteLine($"   Total: {alunos.Count} aluno(s)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro: {ex.Message}");
            }
        }

        static void TestarFuncionarios()
        {
            Console.WriteLine("\n--- TESTANDO CRUD DE FUNCIONÁRIOS ---");
            var dal = new FuncionarioDAL();

            try
            {
                // Listar todos
                Console.WriteLine("\n📋 Listando todos os funcionários:");
                var funcionarios = dal.Listar();
                foreach (var func in funcionarios)
                {
                    Console.WriteLine($"   ID: {func.Id} | Nome: {func.Nome} | CPF: {func.CPF} | Login: {func.Login} | Perfil: {func.Perfil}");
                }
                Console.WriteLine($"   Total: {funcionarios.Count} funcionário(s)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro: {ex.Message}");
            }
        }

        static void TestarLivros()
        {
            Console.WriteLine("\n--- TESTANDO CRUD DE LIVROS ---");
            var dal = new LivroDAL();

            try
            {
                // Listar todos
                Console.WriteLine("\n📚 Listando todos os livros:");
                var livros = dal.Listar();
                foreach (var livro in livros)
                {
                    Console.WriteLine($"   ID: {livro.Id} | Título: {livro.Titulo} | Autor: {livro.Autor ?? "N/A"} | Disponíveis: {livro.QuantidadeDisponivel}/{livro.QuantidadeTotal}");
                }
                Console.WriteLine($"   Total: {livros.Count} livro(s)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro: {ex.Message}");
            }
        }

        static void TestarEmprestimos()
        {
            Console.WriteLine("\n--- TESTANDO CRUD DE EMPRÉSTIMOS ---");
            var dal = new EmprestimoDAL();

            try
            {
                // Listar todos
                Console.WriteLine("\n📖 Listando todos os empréstimos:");
                var emprestimos = dal.Listar();
                foreach (var emp in emprestimos)
                {
                    var status = emp.DataDevolucao == null ? "ATIVO" : "DEVOLVIDO";
                    Console.WriteLine($"   ID: {emp.Id} | Aluno ID: {emp.IdAluno} | Livro ID: {emp.IdLivro} | Empréstimo: {emp.DataEmprestimo:dd/MM/yyyy} | Prevista: {emp.DataPrevista:dd/MM/yyyy} | Status: {status}");
                }
                Console.WriteLine($"   Total: {emprestimos.Count} empréstimo(s)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro: {ex.Message}");
            }
        }

        static void TestarReservas()
        {
            Console.WriteLine("\n--- TESTANDO CRUD DE RESERVAS ---");
            var dal = new ReservaDAL();

            try
            {
                // Listar todas
                Console.WriteLine("\n📅 Listando todas as reservas:");
                var reservas = dal.Listar();
                foreach (var res in reservas)
                {
                    Console.WriteLine($"   ID: {res.Id} | Aluno ID: {res.IdAluno} | Livro ID: {res.IdLivro} | Data: {res.DataReserva:dd/MM/yyyy} | Status: {res.Status}");
                }
                Console.WriteLine($"   Total: {reservas.Count} reserva(s)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro: {ex.Message}");
            }
        }

        static void TestarLogs()
        {
            Console.WriteLine("\n--- TESTANDO CRUD DE LOGS ---");
            var dal = new LogAcaoDAL();

            try
            {
                // Listar todos
                Console.WriteLine("\n📝 Listando todos os logs:");
                var logs = dal.Listar();
                foreach (var log in logs)
                {
                    Console.WriteLine($"   ID: {log.Id} | Funcionário ID: {log.IdFuncionario?.ToString() ?? "N/A"} | Ação: {log.Acao ?? "N/A"} | Data: {log.DataHora:dd/MM/yyyy HH:mm:ss}");
                }
                Console.WriteLine($"   Total: {logs.Count} log(s)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro: {ex.Message}");
            }
        }
    }
}
