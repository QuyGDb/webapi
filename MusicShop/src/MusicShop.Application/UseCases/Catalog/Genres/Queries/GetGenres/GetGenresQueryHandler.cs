using MediatR;
using MusicShop.Application.Common;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Interfaces;
using AutoMapper;

namespace MusicShop.Application.UseCases.Catalog.Genres.Queries.GetGenres;

public sealed class GetGenresQueryHandler(
    IRepository<Genre> genreRepository,
    IMapper mapper)
    : IRequestHandler<GetGenresQuery, Result<PaginatedResult<GenreResponse>>>
{
    public async Task<Result<PaginatedResult<GenreResponse>>> Handle(
        GetGenresQuery request, 
        CancellationToken cancellationToken)
    {
        var (items, totalCount) = await genreRepository.GetPagedAsync(
            request.PageNumber, 
            request.PageSize, 
            cancellationToken: cancellationToken);

        var dtos = mapper.Map<List<GenreResponse>>(items);

        var result = new PaginatedResult<GenreResponse>(
            dtos,
            totalCount,
            request.PageNumber,
            request.PageSize);

        return Result<PaginatedResult<GenreResponse>>.Success(result);
    }
}
