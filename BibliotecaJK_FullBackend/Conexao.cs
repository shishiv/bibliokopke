using System.Data.Common;
using System.IO;
using Microsoft.Data.Sqlite;

namespace BibliotecaJK;

public static class Conexao
{
    private static string _caminhoSqlite = Path.Combine(AppContext.BaseDirectory, "dados", "biblioteca.sqlite");

    public static void ConfigurarSqlite(string caminhoArquivo)
    {
        _caminhoSqlite = caminhoArquivo;
        Directory.CreateDirectory(Path.GetDirectoryName(_caminhoSqlite)!);
        InicializadorSqlite.GarantirEstrutura(_caminhoSqlite);
    }

    public static DbConnection ObterConexao()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_caminhoSqlite)!);
        return new SqliteConnection($"Data Source={_caminhoSqlite}");
    }

    public static DbConnection ObterConexaoAberta()
    {
        var conexao = ObterConexao();
        conexao.Open();
        return conexao;
    }

    public static long ObterUltimoId(DbConnection conexao)
    {
        using var comando = conexao.CreateCommand();
        comando.CommandText = "SELECT last_insert_rowid();";
        var resultado = comando.ExecuteScalar();
        return resultado is null ? 0 : Convert.ToInt64(resultado);
    }
}
