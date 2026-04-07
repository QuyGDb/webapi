using MusicShop.Domain.Entities.Catalog;

namespace MusicShop.Domain.Interfaces;

public interface IReleaseRepository : IRepository<Release>
{
    // Lấy thông tin Album kèm theo thông tin Nghệ sĩ (Eager Loading)
    Task<Release?> GetWithArtistAsync(Guid id, CancellationToken ct = default);

    Task<(IReadOnlyList<Release> Items, int TotalCount)> GetPagedWithFiltersAsync(
        int pageNumber,
        int pageSize,
        Guid? artistId = null,
        string? genreSlug = null,
        int? year = null,
        string? q = null,
        CancellationToken ct = default);

    Task<Release?> GetWithDetailsAsync(Guid id, CancellationToken ct = default);
}
