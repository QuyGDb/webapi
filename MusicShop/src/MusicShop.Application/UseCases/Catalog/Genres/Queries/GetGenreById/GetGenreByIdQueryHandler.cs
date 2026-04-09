using MediatR;
using MusicShop.Application.Common.Mappings;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Interfaces;

namespace MusicShop.Application.UseCases.Catalog.Genres.Queries.GetGenreById;

public sealed class GetGenreByIdQueryHandler(IRepository<Genre> genreRepository) 
    : IRequestHandler<GetGenreByIdQuery, Result<GenreResponse>>
{
    public async Task<Result<GenreResponse>> Handle(GetGenreByIdQuery request, CancellationToken cancellationToken)
    {
        Genre? genre = await genreRepository.GetByIdAsync(request.Id, cancellationToken);

        return Result<GenreResponse>.Success(genre!.ToResponse());
    }
}
