using System;
using System.IO;
using System.Windows.Forms;
using BibliotecaJK;

namespace BibliotecaJK
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ConfigurarBancoSqlitePadrao();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static void ConfigurarBancoSqlitePadrao()
        {
            var caminho = Path.Combine(AppContext.BaseDirectory, "dados", "biblioteca.sqlite");
            Conexao.ConfigurarSqlite(caminho);
        }
    }
}
