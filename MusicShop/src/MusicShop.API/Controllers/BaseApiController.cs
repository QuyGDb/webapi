using Microsoft.AspNetCore.Mvc;
using MusicShop.API.Infrastructure;
using MusicShop.Application.Common;
using MusicShop.Domain.Common;

namespace MusicShop.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    protected IActionResult HandleResult<T>(Result<T> result)
    {
        return result.Match(
            value => Ok(ApiResponse<T>.SuccessResult(value)),
            MapError
        );
    }

    protected IActionResult HandlePaginatedResult<T>(Result<PaginatedResult<T>> result)
    {
        return result.Match(
            value =>
            {
                var meta = new MetaData
                {
                    Page = value.PageNumber,
                    Limit = value.PageSize,
                    Total = value.TotalCount
                };
                // We return the Items directly as the 'data' but wrapped in ApiResponse
                return Ok(ApiResponse<IReadOnlyList<T>>.SuccessResult(value.Items, meta));
            },
            MapError
        );
    }

    private IActionResult MapError(Error error)
    {
        var response = ApiResponse<object>.FailureResult(error.Code, error.Message);

        return error.Code switch
        {
            var code when code.EndsWith(".NotFound") => NotFound(response),
            var code when code.EndsWith(".Unauthorized") => Unauthorized(response),
            var code when code.EndsWith(".Forbidden") => Forbid(), // Note: Forbid doesn't usually take a body with standard JWT
            var code when code.EndsWith(".Conflict") => Conflict(response),
            _ => BadRequest(response)
        };
    }
}
