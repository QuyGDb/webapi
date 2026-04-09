using MediatR;
using MusicShop.Domain.Common;
using MusicShop.Domain.Interfaces;
using MusicShop.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;

namespace MusicShop.Application.UseCases.Catalog.Genres.Commands.DeleteGenre;

public sealed class DeleteGenreCommandHandler(
    IRepository<Genre> genreRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteGenreCommand, Result>
{
    public async Task<Result> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = await genreRepository.AsQueryable()
            .Include(x => x.ArtistGenres)
            .Include(x => x.ReleaseGenres)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (genre is null)
        {
            return Result.Failure(new Error("Genre.NotFound", "Genre not found."));
        }

        if (genre.ArtistGenres.Any() || genre.ReleaseGenres.Any())
        {
            return Result.Failure(new Error("Genre.HasAssociations", "Cannot delete genre with existing associations."));
        }

        genreRepository.Delete(genre);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
