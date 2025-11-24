using System.Security.Cryptography;
using System.Text;

namespace BibliotecaJK.Utilitarios;

public static class GeradorHashSenha
{
    public static string GerarHash(string senha)
    {
        if (string.IsNullOrWhiteSpace(senha))
        {
            return string.Empty;
        }

        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(senha);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToHexString(hash);
    }

    public static bool Verificar(string senha, string hashArmazenado)
    {
        if (string.IsNullOrEmpty(hashArmazenado))
        {
            return false;
        }

        if (string.Equals(senha, hashArmazenado, StringComparison.Ordinal))
        {
            return true;
        }

        return string.Equals(GerarHash(senha), hashArmazenado, StringComparison.OrdinalIgnoreCase);
    }
}
