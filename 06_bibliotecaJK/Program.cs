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
            // High-DPI support must be set before EnableVisualStyles
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Verificar se existe configuracao de conexao
            bool primeiraExecucao = !Conexao.TemConfiguracao();
            if (primeiraExecucao)
            {
                MessageBox.Show(
                    Constants.Mensagens.SETUP_BEM_VINDO,
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

            // Conexao OK - Verificar se o schema esta instalado
            // Se for primeira execucao, sempre executar o setup wizard
            // Caso contrario, perguntar ao usuario
            bool executarSetup = primeiraExecucao;

            if (!primeiraExecucao)
            {
                var resultSetup = MessageBox.Show(
                    "Conexao estabelecida com sucesso!\n\n" +
                    "Deseja verificar se o banco de dados esta configurado corretamente?\n" +
                    "(Verifica se as tabelas existem e oferece criar automaticamente)",
                    "Setup do Banco de Dados",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                executarSetup = (resultSetup == DialogResult.Yes);
            }

            if (executarSetup)
            {
                string connStr = Conexao.GetConnectionString();
                using var formSetup = new FormSetupInicial(connStr);
                if (formSetup.ShowDialog() != DialogResult.OK)
                {
                    var sair = MessageBox.Show(
                        "Setup nao concluido.\n\n" +
                        "Deseja sair do sistema?",
                        "Setup Cancelado",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (sair == DialogResult.Yes)
                    {
                        return;
                    }
                }
            }

            // Exibir tela de login
            using (var formLogin = new FormLogin())
            {
                if (formLogin.ShowDialog() == DialogResult.OK)
                {
                    // Login bem-sucedido
                    var funcionarioLogado = formLogin.FuncionarioLogado;

                    if (funcionarioLogado == null)
                    {
                        MessageBox.Show(
                            "Erro ao recuperar dados do funcionario logado.",
                            "Erro",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    // Verificar se precisa trocar senha
                    if (formLogin.PrecisaTrocarSenha)
                    {
                        using var formTrocaSenha = new FormTrocaSenha(funcionarioLogado, obrigatorio: true);
                        if (formTrocaSenha.ShowDialog() != DialogResult.OK)
                        {
                            // Usuario cancelou a troca de senha obrigatoria
                            MessageBox.Show(
                                "Troca de senha cancelada.\n\n" +
                                "Voce nao pode acessar o sistema sem alterar a senha padrao.",
                                "Acesso Negado",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Abrir formulario principal
                    Application.Run(new FormPrincipal(funcionarioLogado));
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
