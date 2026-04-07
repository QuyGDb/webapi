using Microsoft.AspNetCore.Mvc;
using MediatR;
using MusicShop.Application.DTOs.Auth;
using MusicShop.Application.UseCases.Auth.Commands.TokenRefresh;
using MusicShop.Application.UseCases.Auth.Commands.Register;
using MusicShop.Application.UseCases.Auth.Queries.Login;

namespace MusicShop.API.Controllers;

public class AuthController(IMediator mediator) : BaseApiController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        MusicShop.Domain.Common.Result<AuthResponse> result = await mediator.Send(command);
        return HandleResult(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginQuery query)
    {
        MusicShop.Domain.Common.Result<AuthResponse> result = await mediator.Send(query);
        return HandleResult(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
    {
        MusicShop.Domain.Common.Result<AuthResponse> result = await mediator.Send(command);
        return HandleResult(result);
    }
}
