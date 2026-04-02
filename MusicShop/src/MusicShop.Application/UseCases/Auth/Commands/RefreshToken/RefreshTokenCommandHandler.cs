using MediatR;
using MusicShop.Application.DTOs.Auth;
using MusicShop.Domain.Entities.System;
using MusicShop.Domain.Interfaces;

namespace MusicShop.Application.UseCases.Auth.Commands.Refresh;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponse>
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<RefreshToken> _refreshTokenRepository;
    private readonly IRefreshTokenHasher _refreshTokenHasher;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokenCommandHandler(
        IRepository<User> userRepository,
        IRepository<RefreshToken> refreshTokenRepository,
        IRefreshTokenHasher refreshTokenHasher,
        ITokenService tokenService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _refreshTokenHasher = refreshTokenHasher;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            throw new Exception("Error: Refresh token is required!");
        }

        string currentRefreshTokenHash = _refreshTokenHasher.Hash(request.RefreshToken);

        RefreshToken? existingRefreshToken = await _refreshTokenRepository
            .FirstOrDefaultAsync(x => x.TokenHash == currentRefreshTokenHash);

        if (existingRefreshToken == null)
        {
            throw new Exception("Error: Invalid refresh token!");
        }

        if (existingRefreshToken.RevokedAt != null)
        {
            throw new Exception("Error: Refresh token has been revoked!");
        }

        if (existingRefreshToken.ExpiresAt <= DateTime.UtcNow)
        {
            throw new Exception("Error: Refresh token has expired!");
        }

        User? user = await _userRepository.GetByIdAsync(existingRefreshToken.UserId);
        if (user == null)
        {
            throw new Exception("Error: User not found!");
        }

        (string accessToken, DateTime accessTokenExpiresAtUtc) = _tokenService.GenerateAccessToken(user);
        (string newRefreshToken, DateTime newRefreshTokenExpiresAtUtc) = _tokenService.GenerateRefreshToken();
        string newRefreshTokenHash = _refreshTokenHasher.Hash(newRefreshToken);

        existingRefreshToken.RevokedAt = DateTime.UtcNow;
        existingRefreshToken.ReplacedByTokenHash = newRefreshTokenHash;
        existingRefreshToken.UpdatedAt = DateTime.UtcNow;
        _refreshTokenRepository.Update(existingRefreshToken);

        _refreshTokenRepository.Add(new RefreshToken
        {
            UserId = user.Id,
            TokenHash = newRefreshTokenHash,
            ExpiresAt = newRefreshTokenExpiresAtUtc
        });

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            AccessTokenExpiresAt = accessTokenExpiresAtUtc,
            UserId = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role.ToString()
        };
    }
}
