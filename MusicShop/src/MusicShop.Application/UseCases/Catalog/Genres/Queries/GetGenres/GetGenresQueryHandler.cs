using MediatR;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Interfaces;

namespace MusicShop.Application.UseCases.Catalog.Genres.Queries.GetGenres;

public sealed class GetGenresQueryHandler(IRepository<Genre> genreRepository)
    : IRequestHandler<GetGenresQuery, Result<IReadOnlyList<GenreResponse>>>
{
    public async Task<Result<IReadOnlyList<GenreResponse>>> Handle(
        GetGenresQuery request, 
        CancellationToken cancellationToken)
    {
        var genres = await genreRepository.GetPagedAsync(1, 1000); // Fetch all genres for now

        var response = genres.Items.Select(g => new GenreResponse
        {
            Id = g.Id,
            Name = g.Name,
            Slug = g.Slug
        }).ToList();

        return Result<IReadOnlyList<GenreResponse>>.Success(response);
    }
}
