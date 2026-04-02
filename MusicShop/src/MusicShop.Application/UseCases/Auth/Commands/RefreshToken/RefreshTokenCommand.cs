using MediatR;
using MusicShop.Application.DTOs.Auth;

namespace MusicShop.Application.UseCases.Auth.Commands.Refresh;

public class RefreshTokenCommand : IRequest<AuthResponse>
{
    public string RefreshToken { get; set; } = string.Empty;
}
