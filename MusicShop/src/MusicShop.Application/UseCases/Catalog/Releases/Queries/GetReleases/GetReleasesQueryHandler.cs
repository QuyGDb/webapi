using MediatR;
using MusicShop.Application.Common;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Application.Common.Interfaces;
using MusicShop.Application.Common.Mappings;

namespace MusicShop.Application.UseCases.Catalog.Releases.Queries.GetReleases;

public sealed class GetReleasesQueryHandler(IReleaseRepository releaseRepository)
    : IRequestHandler<GetReleasesQuery, Result<PaginatedResult<ReleaseResponse>>>
{
    public async Task<Result<PaginatedResult<ReleaseResponse>>> Handle(
        GetReleasesQuery request,
        CancellationToken cancellationToken)
    {
        var (items, totalCount) = await releaseRepository.GetPagedAsync(request, cancellationToken);

        List<ReleaseResponse> dtos = items.Select(r => r.ToResponse()).ToList();

        PaginatedResult<ReleaseResponse> result = new PaginatedResult<ReleaseResponse>(
            dtos,
            totalCount,
            request.PageNumber,
            request.PageSize);

        return Result<PaginatedResult<ReleaseResponse>>.Success(result);
    }
}
