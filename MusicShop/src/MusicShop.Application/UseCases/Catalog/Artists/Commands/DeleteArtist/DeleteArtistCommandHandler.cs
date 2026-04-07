using MediatR;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Errors;
using MusicShop.Domain.Interfaces;

namespace MusicShop.Application.UseCases.Catalog.Artists.Commands.DeleteArtist;

public sealed class DeleteArtistCommandHandler(
    IRepository<Artist> artistRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteArtistCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        DeleteArtistCommand request, 
        CancellationToken cancellationToken)
    {
        Artist? artist = await artistRepository.GetByIdAsync(request.Id);
        if (artist == null)
        {
            return Result<bool>.Failure(ArtistErrors.NotFound);
        }

        artistRepository.Delete(artist);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
