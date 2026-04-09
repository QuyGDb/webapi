using MediatR;
using MusicShop.Application.Common;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Interfaces;
using MusicShop.Domain.Enums;
using MusicShop.Application.Common.Mappings;
using Microsoft.EntityFrameworkCore;

namespace MusicShop.Application.UseCases.Catalog.Releases.Queries.GetReleases;

public sealed class GetReleasesQueryHandler(IRepository<Release> releaseRepository)
    : IRequestHandler<GetReleasesQuery, Result<PaginatedResult<ReleaseResponse>>>
{
    public async Task<Result<PaginatedResult<ReleaseResponse>>> Handle(
        GetReleasesQuery request,
        CancellationToken cancellationToken)
    {
        var query = releaseRepository.AsQueryable();

        // 1. Filtering
        if (!string.IsNullOrWhiteSpace(request.Q))
        {
            query = query.Where(r => r.Title.Contains(request.Q));
        }

        if (request.ArtistId.HasValue)
        {
            query = query.Where(r => r.ArtistId == request.ArtistId.Value);
        }

        if (request.GenreId.HasValue)
        {
            query = query.Where(r => r.ReleaseGenres.Any(rg => rg.GenreId == request.GenreId.Value));
        }

        if (!string.IsNullOrWhiteSpace(request.Type))
        {
            query = query.Where(r => r.Type == request.Type);
        }

        if (!string.IsNullOrWhiteSpace(request.Format))
        {
            if (Enum.TryParse<ReleaseFormat>(request.Format, true, out var formatEnum))
            {
                query = query.Where(r => r.Versions.Any(v => v.Format == formatEnum));
            }
        }

        // 2. Count Total
        var totalCount = await query.CountAsync(cancellationToken);

        // 3. Paging and Projection
        var items = await query
            .Include(r => r.Artist)
            .Include(r => r.ReleaseGenres)
                .ThenInclude(rg => rg.Genre)
            .OrderByDescending(r => r.Year)
            .ThenBy(r => r.Title)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var dtos = items.Select(r => r.ToResponse()).ToList();

        var result = new PaginatedResult<ReleaseResponse>(
            dtos,
            totalCount,
            request.PageNumber,
            request.PageSize);

        return Result<PaginatedResult<ReleaseResponse>>.Success(result);
    }
}
