using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicShop.Application.UseCases.Catalog.Releases.Commands.CreateRelease;
using MusicShop.Application.UseCases.Catalog.Releases.Queries.GetReleaseById;
using MusicShop.Application.UseCases.Catalog.Releases.Queries.GetReleases;

namespace MusicShop.API.Controllers;

public class ReleasesController(IMediator mediator) : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetReleases(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] Guid? artistId = null,
        [FromQuery] string? genre = null,
        [FromQuery] int? year = null,
        [FromQuery] string? q = null)
    {
        var query = new GetReleasesQuery(pageNumber, pageSize, artistId, genre, year, q);
        var result = await mediator.Send(query);
        return HandlePaginatedResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetRelease(Guid id)
    {
        var result = await mediator.Send(new GetReleaseByIdQuery(id));
        return HandleResult(result);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateRelease([FromBody] CreateReleaseCommand command)
    {
        var result = await mediator.Send(command);
        return HandleResult(result);
    }
}
