using System;
using System.Windows.Forms;
using BibliotecaJK.Forms;

namespace BibliotecaJK
{
    internal class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo
        /// Sistema BibliotecaJK v3.0 - Com Interface WinForms + Supabase/PostgreSQL
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Configuracao de estilos visuais do Windows Forms
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Verificar se existe configuracao de conexao
            if (!Conexao.TemConfiguracao())
            {
                MessageBox.Show(
                    "Bem-vindo ao BibliotecaJK!\n\n" +
                    "Parece ser a primeira vez que voce esta executando o sistema.\n" +
                    "Vamos configurar a conexao com o banco de dados.",
                    "Configuracao Inicial",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Mostrar formulario de configuracao
                using var formConfig = new FormConfiguracaoConexao();
                if (formConfig.ShowDialog() != DialogResult.OK)
                {
                    // Usuario cancelou a configuracao
                    MessageBox.Show(
                        "Configuracao cancelada.\n\n" +
                        "O sistema nao pode ser iniciado sem uma conexao com o banco de dados.",
                        "Sistema Cancelado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }
            }

            // Testar conexao com banco de dados
            if (!TestarConexaoBanco())
            {
                var result = MessageBox.Show(
                    "Nao foi possivel conectar ao banco de dados!\n\n" +
                    "Verifique:\n" +
                    "1. Se o PostgreSQL/Supabase esta acessivel\n" +
                    "2. Se o schema foi executado (schema-postgresql.sql)\n" +
                    "3. Se a connection string esta correta\n\n" +
                    "Deseja reconfigurar a conexao?",
                    "Erro de Conexao",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Error);

                if (result == DialogResult.Yes)
                {
                    using var formConfig = new FormConfiguracaoConexao();
                    if (formConfig.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    // Testar novamente
                    if (!TestarConexaoBanco())
                    {
                        MessageBox.Show(
                            "Ainda nao foi possivel conectar.\n" +
                            "Verifique a connection string e tente novamente.",
                            "Erro",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            // Exibir tela de login
            using (var formLogin = new FormLogin())
            {
                if (formLogin.ShowDialog() == DialogResult.OK)
                {
                    // Login bem-sucedido, abrir formulario principal
                    var funcionarioLogado = formLogin.FuncionarioLogado;

                    if (funcionarioLogado != null)
                    {
                        Application.Run(new FormPrincipal(funcionarioLogado));
                    }
                    else
                    {
                        MessageBox.Show(
                            "Erro ao recuperar dados do funcionario logado.",
                            "Erro",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Testa a conexao com o banco de dados
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
