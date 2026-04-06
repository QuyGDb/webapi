using MediatR;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Errors;
using MusicShop.Domain.Interfaces;

namespace MusicShop.Application.UseCases.Catalog.MasterReleases.Commands.CreateMasterRelease;

public sealed class CreateMasterReleaseCommandHandler(
    IRepository<Artist> artistRepository,
    IRepository<MasterRelease> masterRepository, // Thêm repository cho MasterRelease
    IUnitOfWork unitOfWork)                       // Thêm UnitOfWork để lưu DB
    : IRequestHandler<CreateMasterReleaseCommand, Result<MasterReleaseResponse>>
{
    public async Task<Result<MasterReleaseResponse>> Handle(
        CreateMasterReleaseCommand request,
        CancellationToken cancellationToken)
    {
        // Bước 1: Kiểm tra Artist có tồn tại không?
        // Chúng ta cần đảm bảo album được gán cho một nghệ sĩ "có thật".
        var artist = await artistRepository.GetByIdAsync(request.ArtistId);
        if (artist == null)
        {
            return Result<MasterReleaseResponse>.Failure(MasterReleaseErrors.ArtistNotFound);
        }

        // Bước 2: Tạo Entity MasterRelease (Map từ Command sang Entity)
        var masterRelease = new MasterRelease
        {
            Title = request.Title,
            Year = request.Year,
            Genre = request.Genre,
            CoverUrl = request.CoverUrl,
            Description = request.Description,
            ArtistId = request.ArtistId
        };

        // Bước 3: Lưu vào Repository và Commit
        masterRepository.Add(masterRelease);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // Bước 4: Trả về kết quả (Map thủ công từ Entity sang DTO Response)
        return Result<MasterReleaseResponse>.Success(new MasterReleaseResponse
        {
            Id = masterRelease.Id,
            Title = masterRelease.Title,
            Year = masterRelease.Year,
            Genre = masterRelease.Genre,
            CoverUrl = masterRelease.CoverUrl,
            Description = masterRelease.Description,
            ArtistId = masterRelease.ArtistId,
            ArtistName = artist.Name // Chúng ta lấy Name từ object artist đã tìm thấy ở Bước 1
        });
    }
}
