using MediatR;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace MusicShop.Application.UseCases.Catalog.CatalogSearch;

public sealed class SearchCatalogQueryHandler(
    IRepository<Artist> artistRepository,
    IRepository<Release> releaseRepository,
    IMapper mapper)
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
            .Where(a => a.Name.ToLower().Contains(searchTerm))
            .Take(5)
            .ProjectTo<ArtistResponse>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        // 2. Search Releases
        var releases = await releaseRepository.AsQueryable()
            .Where(r => r.Title.ToLower().Contains(searchTerm))
            .Take(10)
            .ProjectTo<ReleaseResponse>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        var result = new CatalogSearchResult
        {
            Artists = artists,
            Releases = releases
        };

        return Result<CatalogSearchResult>.Success(result);
    }
}
