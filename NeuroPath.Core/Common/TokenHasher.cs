using System.Security.Cryptography;
using System.Text;

namespace NeuroPath.Core.Common;

public static class TokenHasher
{
    public static string NewToken() => Convert.ToHexString(RandomNumberGenerator.GetBytes(32));

    public static string Hash(string token) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
}
