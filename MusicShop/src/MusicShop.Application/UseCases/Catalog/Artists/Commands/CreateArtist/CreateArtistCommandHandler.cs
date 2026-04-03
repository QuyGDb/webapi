using MediatR;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Errors;
using MusicShop.Domain.Interfaces;

namespace MusicShop.Application.UseCases.Catalog.Artists.Commands.CreateArtist;

public sealed class CreateArtistCommandHandler(
    IRepository<Artist> artistRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateArtistCommand, Result<ArtistResponse>>
{
    public async Task<Result<ArtistResponse>> Handle(
        CreateArtistCommand request, 
        CancellationToken cancellationToken)
    {
        // 1. Kiểm tra xem nghệ sĩ đã tồn tại chưa (Dựa vào tên)
        var existingArtist = await artistRepository.FirstOrDefaultAsync(x => x.Name == request.Name);
        if (existingArtist != null)
        {
            return Result<ArtistResponse>.Failure(ArtistErrors.DuplicateName);
        }

        // 2. Tạo Entity mới từ Command
        var artist = new Artist
        {
            Name = request.Name,
            Bio = request.Bio,
            Genre = request.Genre,
            Country = request.Country,
            ImageUrl = request.ImageUrl
        };

        // 3. Thêm vào Repository
        artistRepository.Add(artist);

        // 4. Lưu thay đổi vào Database
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // 5. Trả về thông tin nghệ sĩ đã tạo
        return Result<ArtistResponse>.Success(new ArtistResponse
        {
            Id = artist.Id,
            Name = artist.Name,
            Bio = artist.Bio,
            Genre = artist.Genre,
            Country = artist.Country,
            ImageUrl = artist.ImageUrl
        });
    }
}
