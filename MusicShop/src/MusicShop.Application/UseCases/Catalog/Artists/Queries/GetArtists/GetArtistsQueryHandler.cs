using MediatR;
using MusicShop.Application.Common;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Interfaces;
using MusicShop.Application.Common.Mappings;
using Microsoft.EntityFrameworkCore;

namespace MusicShop.Application.UseCases.Catalog.Artists.Queries.GetArtists;

public sealed class GetArtistsQueryHandler(IArtistRepository artistRepository)
    : IRequestHandler<GetArtistsQuery, Result<PaginatedResult<ArtistResponse>>>
{
    public async Task<Result<PaginatedResult<ArtistResponse>>> Handle(
        GetArtistsQuery request,
        CancellationToken cancellationToken)
    {
        IQueryable<Artist> query = artistRepository.AsQueryable();

        // 1. Apply Filtering
        if (!string.IsNullOrWhiteSpace(request.Q))
        {
            query = query.Where(a => a.Name.Contains(request.Q));
        }

        if (request.GenreId.HasValue)
        {
            query = query.Where(a => a.ArtistGenres.Any(ag => ag.GenreId == request.GenreId.Value));
        }

        // 2. Wrap into TotalCount and Paging
        int totalCount = await query.CountAsync(cancellationToken);

        List<Artist> items = await query
            .Include(a => a.ArtistGenres)
                .ThenInclude(ag => ag.Genre)
            .OrderBy(a => a.Name)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        List<ArtistResponse> dtos = items.Select(a => a.ToResponse()).ToList();

        // 3. Wrap result
        PaginatedResult<ArtistResponse> result = new PaginatedResult<ArtistResponse>(
            dtos,
            totalCount,
            request.PageNumber,
            request.PageSize);

        return Result<PaginatedResult<ArtistResponse>>.Success(result);
    }
}
