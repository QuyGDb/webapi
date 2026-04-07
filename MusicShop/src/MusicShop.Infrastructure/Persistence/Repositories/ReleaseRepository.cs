using Microsoft.EntityFrameworkCore;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Interfaces;

namespace MusicShop.Infrastructure.Persistence.Repositories;

public sealed class ReleaseRepository : GenericRepository<Release>, IReleaseRepository
{
    public ReleaseRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Release?> GetWithArtistAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Set<Release>()
            .Include(m => m.Artist)
            .FirstOrDefaultAsync(m => m.Id == id, ct);
    }

    public async Task<(IReadOnlyList<Release> Items, int TotalCount)> GetPagedWithFiltersAsync(
        int pageNumber,
        int pageSize,
        Guid? artistId = null,
        string? genreSlug = null,
        int? year = null,
        string? q = null,
        CancellationToken ct = default)
    {
        var query = _context.Set<Release>()
            .Include(x => x.Artist)
            .Include(x => x.ReleaseGenres)
                .ThenInclude(x => x.Genre)
            .AsNoTracking();

        if (artistId.HasValue)
            query = query.Where(x => x.ArtistId == artistId);

        if (!string.IsNullOrWhiteSpace(genreSlug))
            query = query.Where(x => x.ReleaseGenres.Any(rg => rg.Genre.Slug == genreSlug));

        if (year.HasValue)
            query = query.Where(x => x.Year == year);

        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(x => x.Title.Contains(q));

        int total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(x => x.Year)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task<Release?> GetWithDetailsAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Set<Release>()
            .Include(x => x.Artist)
            .Include(x => x.ReleaseGenres)
                .ThenInclude(x => x.Genre)
            .Include(x => x.Tracks.OrderBy(t => t.Position))
            .Include(x => x.Versions)
                .ThenInclude(x => x.Label)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }
}
