using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using MusicShop.Application.Common;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Application.UseCases.Catalog.Labels.Queries.GetLabels;
using MusicShop.Application.UseCases.Catalog.Labels.Queries.GetLabelById;
using MusicShop.Application.UseCases.Catalog.Labels.Commands.CreateLabel;
using MusicShop.Application.UseCases.Catalog.Labels.Commands.UpdateLabel;
using MusicShop.Application.UseCases.Catalog.Labels.Commands.DeleteLabel;
using MusicShop.API.Infrastructure;

namespace MusicShop.API.Controllers;

public class LabelsController(IMediator mediator) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<LabelResponse>>>> GetLabels([FromQuery] GetLabelsQuery query)
    {
        var result = await mediator.Send(query);
        return HandlePaginatedResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<LabelResponse>>> GetLabel(Guid id)
    {
        var result = await mediator.Send(new GetLabelByIdQuery(id));
        return HandleResult(result);
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<Guid>>> CreateLabel([FromBody] CreateLabelCommand command)
    {
        var result = await mediator.Send(command);
        return HandleCreatedResult(result, nameof(GetLabel), new { id = result.Value });
    }

    [Authorize(Roles = "admin")]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<Guid>>> UpdateLabel(Guid id, [FromBody] UpdateLabelCommand command)
    {
        if (id != command.Id) return BadRequest();
        
        var result = await mediator.Send(command);
        return HandleResult(result);
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteLabel(Guid id)
    {
        var result = await mediator.Send(new DeleteLabelCommand(id));
        return HandleNonGenericResult(result);
    }
}
