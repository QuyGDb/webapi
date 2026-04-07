using MediatR;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;
using MusicShop.Domain.Interfaces;

namespace MusicShop.Application.UseCases.Catalog.ReleaseVersions.Queries.GetReleaseVersionsByRelease;

public sealed class GetReleaseVersionsByReleaseQueryHandler(IReleaseVersionRepository releaseVersionRepository)
    : IRequestHandler<GetReleaseVersionsByReleaseQuery, Result<IReadOnlyList<ReleaseVersionDto>>>
{
    public async Task<Result<IReadOnlyList<ReleaseVersionDto>>> Handle(
        GetReleaseVersionsByReleaseQuery request, 
        CancellationToken cancellationToken)
    {
        IReadOnlyList<MusicShop.Domain.Entities.Catalog.ReleaseVersion> versions = await releaseVersionRepository.GetByReleaseIdAsync(request.ReleaseId, cancellationToken);

        List<ReleaseVersionDto> response = versions.Select(v => new ReleaseVersionDto
        {
            Id = v.Id,
            PressingCountry = v.PressingCountry,
            PressingYear = v.PressingYear,
            Format = v.Format.ToString().ToLower(),
            CatalogNumber = v.CatalogNumber,
            Notes = v.Notes,
            LabelName = v.Label?.Name ?? "Unknown Label"
        }).ToList();

        return Result<IReadOnlyList<ReleaseVersionDto>>.Success(response);
    }
}
