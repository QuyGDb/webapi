using MediatR;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;

namespace MusicShop.Application.UseCases.Catalog.Genres.Queries.GetGenres;

public record GetGenresQuery() : IRequest<Result<IReadOnlyList<GenreResponse>>>;
