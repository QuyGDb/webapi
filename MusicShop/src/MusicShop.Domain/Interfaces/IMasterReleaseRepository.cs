using MusicShop.Domain.Entities.Catalog;

namespace MusicShop.Domain.Interfaces;

public interface IMasterReleaseRepository : IRepository<MasterRelease>
{
    // Lấy thông tin Album kèm theo thông tin Nghệ sĩ (Eager Loading)
    Task<MasterRelease?> GetWithArtistAsync(Guid id, CancellationToken ct = default);
}
