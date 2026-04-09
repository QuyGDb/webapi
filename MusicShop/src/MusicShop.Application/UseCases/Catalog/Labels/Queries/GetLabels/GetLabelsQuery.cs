using MediatR;
using MusicShop.Application.Common;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;

namespace MusicShop.Application.UseCases.Catalog.Labels.Queries.GetLabels;

public record GetLabelsQuery(
    string? Q = null,
    string? Country = null,
    int PageNumber = 1, 
    int PageSize = 20) : IRequest<Result<PaginatedResult<LabelResponse>>>;
