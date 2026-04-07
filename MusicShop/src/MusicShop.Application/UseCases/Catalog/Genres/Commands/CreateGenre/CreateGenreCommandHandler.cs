using MediatR;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Errors;
using MusicShop.Domain.Interfaces;

namespace MusicShop.Application.UseCases.Catalog.Genres.Commands.CreateGenre;

public sealed class CreateGenreCommandHandler(
    IRepository<Genre> genreRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateGenreCommand, Result<GenreResponse>>
{
    public async Task<Result<GenreResponse>> Handle(
        CreateGenreCommand request, 
        CancellationToken cancellationToken)
    {
        var existing = await genreRepository.FirstOrDefaultAsync(x => x.Slug == request.Slug);
        if (existing != null)
        {
            return Result<GenreResponse>.Failure(GenreErrors.DuplicateSlug);
        }

        var genre = new Genre
        {
            Name = request.Name,
            Slug = request.Slug
        };

        genreRepository.Add(genre);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<GenreResponse>.Success(new GenreResponse
        {
            Id = genre.Id,
            Name = genre.Name,
            Slug = genre.Slug
        });
    }
}
