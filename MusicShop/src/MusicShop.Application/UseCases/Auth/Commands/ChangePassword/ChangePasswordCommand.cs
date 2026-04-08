using MediatR;
using MusicShop.Domain.Common;

namespace MusicShop.Application.UseCases.Auth.Commands.ChangePassword;

public record ChangePasswordCommand(
    string CurrentPassword, 
    string NewPassword) : IRequest<Result<Unit>>;
