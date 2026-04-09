using MediatR;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Interfaces;
using MusicShop.Application.Common.Mappings;
using Microsoft.EntityFrameworkCore;

namespace MusicShop.Application.UseCases.Catalog.CatalogSearch;

public sealed class SearchCatalogQueryHandler(
    IRepository<Artist> artistRepository,
    IRepository<Release> releaseRepository)
    : IRequestHandler<SearchCatalogQuery, Result<CatalogSearchResult>>
{
    public async Task<Result<CatalogSearchResult>> Handle(
        SearchCatalogQuery request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Q))
        {
            return Result<CatalogSearchResult>.Success(new CatalogSearchResult());
        }

        var searchTerm = request.Q.ToLower();

        // 1. Search Artists
        var artists = await artistRepository.AsQueryable()
            .Include(a => a.ArtistGenres)
                .ThenInclude(ag => ag.Genre)
            .Where(a => a.Name.ToLower().Contains(searchTerm))
            .Take(5)
            .ToListAsync(cancellationToken);

        var artistDtos = artists.Select(a => a.ToResponse()).ToList();

        // 2. Search Releases
        var releases = await releaseRepository.AsQueryable()
            .Include(r => r.Artist)
            .Include(r => r.ReleaseGenres)
                .ThenInclude(rg => rg.Genre)
            .Where(r => r.Title.ToLower().Contains(searchTerm))
            .Take(10)
            .ToListAsync(cancellationToken);

        var releaseDtos = releases.Select(r => r.ToResponse()).ToList();

        var result = new CatalogSearchResult
        {
            Artists = artistDtos,
            Releases = releaseDtos
        };

        return Result<CatalogSearchResult>.Success(result);
    }
}
