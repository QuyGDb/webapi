using Microsoft.EntityFrameworkCore;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Interfaces;

namespace MusicShop.Infrastructure.Persistence.Repositories;

public sealed class MasterReleaseRepository : GenericRepository<MasterRelease>, IMasterReleaseRepository
{
    public MasterReleaseRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<MasterRelease?> GetWithArtistAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Set<MasterRelease>()
            .Include(m => m.Artist)
            .FirstOrDefaultAsync(m => m.Id == id, ct);
    }
}
