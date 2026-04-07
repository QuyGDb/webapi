using MediatR;
using MusicShop.Application.Common;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;
using MusicShop.Domain.Interfaces;

namespace MusicShop.Application.UseCases.Catalog.Releases.Queries.GetReleases;

public sealed class GetReleasesQueryHandler(IReleaseRepository releaseRepository)
    : IRequestHandler<GetReleasesQuery, Result<PaginatedResult<ReleaseResponse>>>
{
    public async Task<Result<PaginatedResult<ReleaseResponse>>> Handle(
        GetReleasesQuery request, 
        CancellationToken cancellationToken)
    {
        var (items, total) = await releaseRepository.GetPagedWithFiltersAsync(
            request.PageNumber,
            request.PageSize,
            request.ArtistId,
            request.GenreSlug,
            request.Year,
            request.Q,
            cancellationToken);

        var responseItems = items.Select(r => new ReleaseResponse
        {
            Id = r.Id,
            Title = r.Title,
            Year = r.Year,
            CoverUrl = r.CoverUrl,
            Description = r.Description,
            ArtistId = r.ArtistId,
            ArtistName = r.Artist?.Name ?? string.Empty,
            Genres = r.ReleaseGenres.Select(rg => new GenreResponse
            {
                Id = rg.GenreId,
                Name = rg.Genre?.Name ?? string.Empty,
                Slug = rg.Genre?.Slug ?? string.Empty
            }).ToList()
        }).ToList();

        var result = new PaginatedResult<ReleaseResponse>(
            responseItems, 
            total, 
            request.PageNumber, 
            request.PageSize);

        return Result<PaginatedResult<ReleaseResponse>>.Success(result);
    }
}
