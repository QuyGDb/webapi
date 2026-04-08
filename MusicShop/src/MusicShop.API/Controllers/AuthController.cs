using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using MusicShop.Application.DTOs.Auth;
using MusicShop.Application.UseCases.Auth.Commands.TokenRefresh;
using MusicShop.Application.UseCases.Auth.Commands.Register;
using MusicShop.Application.UseCases.Auth.Commands.Logout;
using MusicShop.Application.UseCases.Auth.Commands.ChangePassword;
using MusicShop.Application.UseCases.Auth.Queries.Login;
using MusicShop.Application.UseCases.Auth.Queries.GetMe;
using MusicShop.API.Infrastructure;

namespace MusicShop.API.Controllers;

public class AuthController(IMediator mediator) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Register([FromBody] RegisterCommand command)
    {
        var result = await mediator.Send(command);
        return HandleResult(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Login([FromBody] LoginQuery query)
    {
        var result = await mediator.Send(query);
        return HandleResult(result);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> RefreshToken([FromBody] RefreshTokenCommand command)
    {
        var result = await mediator.Send(command);
        return HandleResult(result);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<ApiResponse<UserResponse>>> GetMe()
    {
        var result = await mediator.Send(new GetMeQuery());
        return HandleResult(result);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult<ApiResponse<Unit>>> Logout([FromBody] LogoutCommand command)
    {
        var result = await mediator.Send(command);
        return HandleResult(result);
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<ActionResult<ApiResponse<Unit>>> ChangePassword([FromBody] ChangePasswordCommand command)
    {
        var result = await mediator.Send(command);
        return HandleResult(result);
    }
}
