using MediatR;
using MusicShop.Domain.Common;
using MusicShop.Domain.Interfaces;
using MusicShop.Domain.Entities.Catalog;

namespace MusicShop.Application.UseCases.Catalog.Genres.Commands.UpdateGenre;

public sealed class UpdateGenreCommandHandler(
    IRepository<Genre> genreRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateGenreCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = await genreRepository.GetByIdAsync(request.Id, cancellationToken);

        if (genre is null)
        {
            return Result<Guid>.Failure(new Error("Genre.NotFound", "Genre not found."));
        }

        genre.Name = request.Name;
        genre.Slug = request.Slug;

        genreRepository.Update(genre);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(genre.Id);
    }
}
