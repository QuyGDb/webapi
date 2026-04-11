using MusicShop.Application.Common;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Application.UseCases.Catalog.Releases.Queries.GetReleases;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Interfaces;

namespace MusicShop.Application.Common.Interfaces;

public interface IReleaseRepository : IRepository<Release>
{
    Task<(IReadOnlyList<Release> Items, int TotalCount)> GetPagedAsync(
        GetReleasesQuery query, 
        CancellationToken ct = default);

    Task<Release?> GetWithArtistAsync(Guid id, CancellationToken ct = default);

    Task<Release?> GetWithDetailsAsync(Guid id, CancellationToken ct = default);
}
