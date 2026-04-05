using Microsoft.AspNetCore.Mvc;
using MediatR;
using MusicShop.Application.DTOs.Auth;
using MusicShop.Application.UseCases.Auth.Commands.TokenRefresh;
using MusicShop.Application.UseCases.Auth.Commands.Register;
using MusicShop.Application.UseCases.Auth.Queries.Login;
using MusicShop.Domain.Common;

namespace MusicShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    // Inject IMediator to route requests to the application layer
    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        Result<AuthResponse> result = await _mediator.Send(command);
        return result.Match<IActionResult>(
            Ok,
            error => BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Auth Error",
                Detail = error.Message,
                Extensions = { ["errorCode"] = error.Code }
            }));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginQuery query)
    {
        Result<AuthResponse> result = await _mediator.Send(query);
        return result.Match<IActionResult>(
            Ok,
            error => BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Auth Error",
                Detail = error.Message,
                Extensions = { ["errorCode"] = error.Code }
            }));
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
    {
        Result<AuthResponse> result = await _mediator.Send(command);
        return result.Match<IActionResult>(
            Ok,
            error => BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Auth Error",
                Detail = error.Message,
                Extensions = { ["errorCode"] = error.Code }
            }));
    }

}
