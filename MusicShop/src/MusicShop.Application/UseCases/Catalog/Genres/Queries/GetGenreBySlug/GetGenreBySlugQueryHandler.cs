using MediatR;
using MusicShop.Application.Common.Mappings;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Errors;
using MusicShop.Domain.Interfaces;

namespace MusicShop.Application.UseCases.Catalog.Genres.Queries.GetGenreBySlug;

public sealed class GetGenreBySlugQueryHandler(IRepository<Genre> genreRepository) 
    : IRequestHandler<GetGenreBySlugQuery, Result<GenreResponse>>
{
    public async Task<Result<GenreResponse>> Handle(GetGenreBySlugQuery request, CancellationToken cancellationToken)
    {
        Genre? genre = await genreRepository.FirstOrDefaultAsync(
            x => x.Slug == request.Slug, 
            cancellationToken);

        if (genre is null)
        {
            return Result<GenreResponse>.Failure(GenreErrors.NotFound);
        }

        return Result<GenreResponse>.Success(genre.ToResponse());
    }
}
