using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using MusicShop.Application.Common;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Application.UseCases.Catalog.Genres.Queries.GetGenres;
using MusicShop.Application.UseCases.Catalog.Genres.Queries.GetGenreById;
using MusicShop.Application.UseCases.Catalog.Genres.Commands.CreateGenre;
using MusicShop.Application.UseCases.Catalog.Genres.Commands.UpdateGenre;
using MusicShop.Application.UseCases.Catalog.Genres.Commands.DeleteGenre;
using MusicShop.API.Infrastructure;

namespace MusicShop.API.Controllers;

public class GenresController(IMediator mediator) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<GenreResponse>>>> GetGenres([FromQuery] GetGenresQuery query)
    {
        var result = await mediator.Send(query);
        return HandlePaginatedResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<GenreResponse>>> GetGenre(Guid id)
    {
        var result = await mediator.Send(new GetGenreByIdQuery(id));
        return HandleResult(result);
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<Guid>>> CreateGenre([FromBody] CreateGenreCommand command)
    {
        var result = await mediator.Send(command);
        return HandleCreatedResult(result, nameof(GetGenre), new { id = result.Value });
    }

    [Authorize(Roles = "admin")]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<Guid>>> UpdateGenre(Guid id, [FromBody] UpdateGenreCommand command)
    {
        if (id != command.Id) return BadRequest();
        
        var result = await mediator.Send(command);
        return HandleResult(result);
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteGenre(Guid id)
    {
        var result = await mediator.Send(new DeleteGenreCommand(id));
        return HandleNonGenericResult(result);
    }
}
