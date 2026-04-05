using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicShop.Application.Common;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Application.UseCases.Catalog.Labels.Commands.CreateLabel;
using MusicShop.Application.UseCases.Catalog.Labels.Commands.DeleteLabel;
using MusicShop.Application.UseCases.Catalog.Labels.Commands.UpdateLabel;
using MusicShop.Application.UseCases.Catalog.Labels.Queries.GetLabelById;
using MusicShop.Application.UseCases.Catalog.Labels.Queries.GetLabels;

namespace MusicShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LabelsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetLabels(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = new GetLabelsQuery(pageNumber, pageSize);
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
    public async Task<IActionResult> GetLabel(Guid id)
    {
        var query = new GetLabelByIdQuery(id);
        var result = await mediator.Send(query);

        return result.Match<IActionResult>(
            Ok,
            error => NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Label Not Found",
                Detail = error.Message
            }));
    }

    [HttpPost]
    public async Task<IActionResult> CreateLabel([FromBody] CreateLabelCommand command)
    {
        var result = await mediator.Send(command);

        return result.Match<IActionResult>(
            value => CreatedAtAction(nameof(GetLabel), new { id = value.Id }, value),
            error => BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation Error",
                Detail = error.Message
            }));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateLabel(Guid id, [FromBody] UpdateLabelCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "ID Mismatch",
                Detail = "Route id does not match the body id."
            });
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
    public async Task<IActionResult> DeleteLabel(Guid id)
    {
        var command = new DeleteLabelCommand(id);
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
