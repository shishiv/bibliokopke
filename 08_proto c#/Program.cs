using System;
using System.Windows.Forms;
using BibliotecaJK.Forms;

namespace BibliotecaJK
{
    internal class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo
        /// Sistema BibliotecaJK v3.0 - Com Interface WinForms
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Configuração de estilos visuais do Windows Forms
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Testar conexão com banco de dados
            if (!TestarConexaoBanco())
            {
                MessageBox.Show(
                    "Não foi possível conectar ao banco de dados!\n\n" +
                    "Verifique:\n" +
                    "1. Se o MySQL está rodando\n" +
                    "2. Se o banco 'bibliokopke' foi criado (execute schema.sql)\n" +
                    "3. Se a connection string em Conexao.cs está correta",
                    "Erro de Conexão",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // Exibir tela de login
            using (var formLogin = new FormLogin())
            {
                if (formLogin.ShowDialog() == DialogResult.OK)
                {
                    // Login bem-sucedido, abrir formulário principal
                    var funcionarioLogado = formLogin.FuncionarioLogado;

                    if (funcionarioLogado != null)
                    {
                        Application.Run(new FormPrincipal(funcionarioLogado));
                    }
                    else
                    {
                        MessageBox.Show(
                            "Erro ao recuperar dados do funcionário logado.",
                            "Erro",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Testa a conexão com o banco de dados
        /// </summary>
        private static bool TestarConexaoBanco()
        {
            try
            {
                using var conn = Conexao.GetConnection();
                conn.Open();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao conectar ao banco: {ex.Message}");
                return false;
            }
        }
    }
}
