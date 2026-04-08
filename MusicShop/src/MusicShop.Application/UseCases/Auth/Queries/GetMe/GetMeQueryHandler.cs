using MediatR;
using MusicShop.Application.Common.Interfaces;
using MusicShop.Application.DTOs.Auth;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.System;
using MusicShop.Domain.Errors;
using MusicShop.Domain.Interfaces;

namespace MusicShop.Application.UseCases.Auth.Queries.GetMe;

public class GetMeQueryHandler(
    IRepository<User> userRepository,
    ICurrentUserService currentUserService) : IRequestHandler<GetMeQuery, Result<UserResponse>>
{
    public async Task<Result<UserResponse>> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        if (!currentUserService.IsAuthenticated || string.IsNullOrEmpty(currentUserService.UserId))
        {
            return Result<UserResponse>.Failure(AuthErrors.Unauthorized);
        }

        if (!Guid.TryParse(currentUserService.UserId, out var userId))
        {
            return Result<UserResponse>.Failure(AuthErrors.InvalidUserId);
        }

        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            return Result<UserResponse>.Failure(AuthErrors.UserNotFound);
        }

        return Result<UserResponse>.Success(new UserResponse
        {
            UserId = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role.ToString(),
            CreatedAt = user.CreatedAt
        });
    }
}
