using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicShop.Application.UseCases.Catalog.Genres.Commands.CreateGenre;
using MusicShop.Application.UseCases.Catalog.Genres.Queries.GetGenres;

namespace MusicShop.API.Controllers;

public class GenresController(IMediator mediator) : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetGenres()
    {
        MusicShop.Domain.Common.Result<IReadOnlyList<MusicShop.Application.DTOs.Catalog.GenreResponse>> result = await mediator.Send(new GetGenresQuery());
        return HandleResult(result);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateGenre([FromBody] CreateGenreCommand command)
    {
        MusicShop.Domain.Common.Result<MusicShop.Application.DTOs.Catalog.GenreResponse> result = await mediator.Send(command);
        return HandleResult(result);
    }
}
