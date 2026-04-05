using MediatR;
using MusicShop.Application.DTOs.Auth;
using MusicShop.Domain.Common;

namespace MusicShop.Application.UseCases.Auth.Queries.Login;

public sealed record LoginQuery(
    string Email,
    string Password) : IRequest<Result<AuthResponse>>;
