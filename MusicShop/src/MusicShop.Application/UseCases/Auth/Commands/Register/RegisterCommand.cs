using MediatR;
using MusicShop.Application.DTOs.Auth;
using MusicShop.Domain.Common;

namespace MusicShop.Application.UseCases.Auth.Commands.Register;

public sealed record RegisterCommand(
    string Email,
    string Password,
    string FullName) : IRequest<Result<AuthResponse>>;
