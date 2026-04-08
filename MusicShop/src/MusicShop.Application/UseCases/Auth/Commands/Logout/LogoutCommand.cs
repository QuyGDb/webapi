using MediatR;
using MusicShop.Domain.Common;

namespace MusicShop.Application.UseCases.Auth.Commands.Logout;

public record LogoutCommand(string RefreshToken) : IRequest<Result<Unit>>;
