using System.Security.Cryptography;
using System.Text;
using MusicShop.Domain.Interfaces;

namespace MusicShop.Infrastructure.Security;

public class RefreshTokenHasher : IRefreshTokenHasher
{
    public string Hash(string token)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(token);
        byte[] hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash);
    }
}
