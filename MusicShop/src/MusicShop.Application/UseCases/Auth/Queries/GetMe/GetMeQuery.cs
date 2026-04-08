using MediatR;
using MusicShop.Application.DTOs.Auth;
using MusicShop.Domain.Common;

namespace MusicShop.Application.UseCases.Auth.Queries.GetMe;

public record GetMeQuery : IRequest<Result<UserResponse>>;
