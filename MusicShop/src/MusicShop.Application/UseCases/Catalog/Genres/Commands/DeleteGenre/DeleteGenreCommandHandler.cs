using MediatR;
using MusicShop.Domain.Common;
using MusicShop.Domain.Interfaces;
using MusicShop.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using MusicShop.Domain.Errors;

namespace MusicShop.Application.UseCases.Catalog.Genres.Commands.DeleteGenre;

public sealed class DeleteGenreCommandHandler(
    IRepository<Genre> genreRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteGenreCommand, Result>
{
    public async Task<Result> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
    {
        Genre? genre = await genreRepository.AsQueryable()
            .Include(x => x.ArtistGenres)
            .Include(x => x.ReleaseGenres)
            .FirstOrDefaultAsync(x => x.Slug == request.Slug, cancellationToken);

        if (genre is null)
        {
            return Result.Failure(GenreErrors.NotFound);
        }

        if (genre.ArtistGenres.Any() || genre.ReleaseGenres.Any())
        {
            return Result.Failure(GenreErrors.HasAssociations);
        }

        genreRepository.Delete(genre);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
