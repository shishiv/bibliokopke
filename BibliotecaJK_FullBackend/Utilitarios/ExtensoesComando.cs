using System.Data.Common;

namespace BibliotecaJK.Utilitarios;

public static class ExtensoesComando
{
    public static void AdicionarParametro(this DbCommand comando, string nome, object? valor)
    {
        var parametro = comando.CreateParameter();
        parametro.ParameterName = nome;
        parametro.Value = valor ?? DBNull.Value;
        comando.Parameters.Add(parametro);
    }
}
