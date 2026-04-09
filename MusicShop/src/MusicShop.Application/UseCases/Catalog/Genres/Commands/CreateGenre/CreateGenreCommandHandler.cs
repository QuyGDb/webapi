using MediatR;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Interfaces;

namespace MusicShop.Application.UseCases.Catalog.Genres.Commands.CreateGenre;

public sealed class CreateGenreCommandHandler(
    IRepository<Genre> genreRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateGenreCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateGenreCommand request, 
        CancellationToken cancellationToken)
    {
        Genre? existing = await genreRepository.FirstOrDefaultAsync(x => x.Slug == request.Slug, cancellationToken);
        if (existing != null)
        {
            return Result<Guid>.Failure(new Error("Genre.DuplicateSlug", "Slug already exists."));
        }

        Genre genre = new Genre
        {
            Name = request.Name,
            Slug = request.Slug
        };

        genreRepository.Add(genre);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(genre.Id);
    }
}
