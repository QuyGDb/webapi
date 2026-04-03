using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicShop.Application.Common;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Application.UseCases.Catalog.Artists.Commands.CreateArtist;
using MusicShop.Application.UseCases.Catalog.Artists.Commands.DeleteArtist;
using MusicShop.Application.UseCases.Catalog.Artists.Commands.UpdateArtist;
using MusicShop.Application.UseCases.Catalog.Artists.Queries.GetArtistById;
using MusicShop.Application.UseCases.Catalog.Artists.Queries.GetArtists;

namespace MusicShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArtistsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetArtists(
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 20)
    {
        var query = new GetArtistsQuery(pageNumber, pageSize);
        var result = await mediator.Send(query);
        
        return result.Match<IActionResult>(
            Ok,
            error => BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Catalog Error",
                Detail = error.Message,
                Extensions = { ["errorCode"] = error.Code }
            }));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetArtist(Guid id)
    {
        var query = new GetArtistByIdQuery(id);
        var result = await mediator.Send(query);

        return result.Match<IActionResult>(
            Ok,
            error => NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Artist Not Found",
                Detail = error.Message
            }));
    }

    [HttpPost]
    public async Task<IActionResult> CreateArtist([FromBody] CreateArtistCommand command)
    {
        var result = await mediator.Send(command);

        return result.Match<IActionResult>(
            value => CreatedAtAction(nameof(GetArtist), new { id = value.Id }, value),
            error => BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation Error",
                Detail = error.Message
            }));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateArtist(Guid id, [FromBody] UpdateArtistCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID mismatch");
        }

        var result = await mediator.Send(command);

        return result.Match<IActionResult>(
            Ok,
            error => BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Update Error",
                Detail = error.Message
            }));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteArtist(Guid id)
    {
        var command = new DeleteArtistCommand(id);
        var result = await mediator.Send(command);

        return result.Match<IActionResult>(
            _ => NoContent(),
            error => BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Delete Error",
                Detail = error.Message
            }));
    }
}
