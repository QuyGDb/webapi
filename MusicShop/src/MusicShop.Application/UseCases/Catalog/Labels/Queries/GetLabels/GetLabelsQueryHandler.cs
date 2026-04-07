using MediatR;
using MusicShop.Application.Common;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Interfaces;
using System.Linq.Expressions;

namespace MusicShop.Application.UseCases.Catalog.Labels.Queries.GetLabels;

public sealed class GetLabelsQueryHandler(IRepository<Label> labelRepository)
    : IRequestHandler<GetLabelsQuery, Result<PaginatedResult<LabelResponse>>>
{
    public async Task<Result<PaginatedResult<LabelResponse>>> Handle(
        GetLabelsQuery request, 
        CancellationToken cancellationToken)
    {
        Expression<Func<Label, bool>>? predicate = null;

        if (!string.IsNullOrWhiteSpace(request.Q) || !string.IsNullOrWhiteSpace(request.Country))
        {
            predicate = x => 
                (string.IsNullOrWhiteSpace(request.Q) || x.Name.Contains(request.Q)) &&
                (string.IsNullOrWhiteSpace(request.Country) || x.Country == request.Country);
        }

        (IReadOnlyList<Label> items, int totalCount) = await labelRepository.GetPagedAsync(
            request.PageNumber, 
            request.PageSize,
            predicate);

        List<LabelResponse> labelResponses = items.Select(label => new LabelResponse
        {
            Id = label.Id,
            Name = label.Name,
            Country = label.Country,
            FoundedYear = label.FoundedYear,
            Website = label.Website
        }).ToList();

        PaginatedResult<LabelResponse> result = new PaginatedResult<LabelResponse>(
            labelResponses, 
            totalCount, 
            request.PageNumber, 
            request.PageSize);

        return Result<PaginatedResult<LabelResponse>>.Success(result);
    }
}
