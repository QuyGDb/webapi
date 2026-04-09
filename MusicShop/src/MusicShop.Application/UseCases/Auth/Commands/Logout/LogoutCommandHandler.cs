using MediatR;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.System;
using MusicShop.Domain.Errors;
using MusicShop.Domain.Interfaces;

namespace MusicShop.Application.UseCases.Auth.Commands.Logout;

public class LogoutCommandHandler(
    IRepository<RefreshToken> refreshTokenRepository,
    IRefreshTokenHasher refreshTokenHasher,
    IUnitOfWork unitOfWork) : IRequestHandler<LogoutCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        string tokenHash = refreshTokenHasher.Hash(request.RefreshToken);
        
        RefreshToken? refreshToken = await refreshTokenRepository.FirstOrDefaultAsync(
            x => x.TokenHash == tokenHash, 
            cancellationToken);

        if (refreshToken == null)
        {
            return Result<Unit>.Failure(AuthErrors.InvalidToken);
        }

        // We revoke it instead of deleting to keep history if needed, 
        // or we could just remove it. Let's revoke it for now.
        refreshToken.RevokedAt = DateTime.UtcNow;
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}
