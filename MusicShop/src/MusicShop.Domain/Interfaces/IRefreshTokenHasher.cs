namespace MusicShop.Domain.Interfaces;

public interface IRefreshTokenHasher
{
    string Hash(string token);
}
