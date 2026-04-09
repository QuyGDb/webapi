using MediatR;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;
using MusicShop.Domain.Interfaces;
using MusicShop.Application.Common.Mappings;
using MusicShop.Domain.Entities.Catalog;

namespace MusicShop.Application.UseCases.Catalog.Genres.Queries.GetGenreById;

public record GetGenreByIdQuery(Guid Id) : IRequest<Result<GenreResponse>>;

public sealed class GetGenreByIdQueryHandler(IRepository<Genre> genreRepository) 
    : IRequestHandler<GetGenreByIdQuery, Result<GenreResponse>>
{
    public async Task<Result<GenreResponse>> Handle(GetGenreByIdQuery request, CancellationToken cancellationToken)
    {
        var genre = await genreRepository.GetByIdAsync(request.Id, cancellationToken);

        return Result<GenreResponse>.Success(genre!.ToResponse());
    }
}
