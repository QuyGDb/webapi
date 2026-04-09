using MediatR;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Interfaces;
using MusicShop.Application.Common.Mappings;
using Microsoft.EntityFrameworkCore;

namespace MusicShop.Application.UseCases.Catalog.Releases.Queries.GetReleaseById;

public sealed class GetReleaseByIdQueryHandler(IRepository<Release> releaseRepository)
    : IRequestHandler<GetReleaseByIdQuery, Result<ReleaseDetailResponse>>
{
    public async Task<Result<ReleaseDetailResponse>> Handle(
        GetReleaseByIdQuery request,
        CancellationToken cancellationToken)
    {
        Release? release = await releaseRepository.AsQueryable()
            .Include(r => r.Artist)
            .Include(r => r.ReleaseGenres)
                .ThenInclude(rg => rg.Genre)
            .Include(r => r.Tracks)
            .Include(r => r.Versions)
                .ThenInclude(v => v.Label)
            .Include(r => r.Versions)
                .ThenInclude(v => v.Products)
                    .ThenInclude(p => p.Variants)
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (release == null)
        {
            return Result<ReleaseDetailResponse>.Failure(new Error("Release.NotFound", "Release not found."));
        }

        ReleaseDetailResponse response = release.ToDetailResponse();

        return Result<ReleaseDetailResponse>.Success(response);
    }
}
