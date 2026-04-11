using MediatR;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using MusicShop.Domain.Errors;

namespace MusicShop.Application.UseCases.Catalog.Artists.Commands.DeleteArtist;

public sealed class DeleteArtistCommandHandler(
    IRepository<Artist> artistRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteArtistCommand, Result>
{
    public async Task<Result> Handle(
        DeleteArtistCommand request, 
        CancellationToken cancellationToken)
    {
        Artist? artist = await artistRepository.AsQueryable()
            .Include(x => x.Releases)
            .FirstOrDefaultAsync(x => x.Slug == request.Slug, cancellationToken);

        if (artist == null)
        {
            return Result.Failure(ArtistErrors.NotFound);
        }

        if (artist.Releases.Any())
        {
            return Result.Failure(ArtistErrors.HasAssociations);
        }

        artistRepository.Delete(artist);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
