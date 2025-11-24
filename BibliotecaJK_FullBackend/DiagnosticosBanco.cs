using System.Data.Common;

namespace BibliotecaJK.Diagnosticos;

public static class DiagnosticosBanco
{
    /// <summary>
    /// Testa a conexão com o banco e retorna uma mensagem amigável
    /// para ser exibida na interface.
    /// </summary>
    public static string TestarConexao()
    {
        try
        {
            using var conn = Conexao.ObterConexaoAberta();
            conn.Close();
            return "✅ Conexão estabelecida com sucesso!";
        }
        catch (DbException ex)
        {
            return $"❌ Erro ao conectar ao banco de dados: {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"❌ Erro inesperado ao testar a conexão: {ex.Message}";
        }
    }
}
