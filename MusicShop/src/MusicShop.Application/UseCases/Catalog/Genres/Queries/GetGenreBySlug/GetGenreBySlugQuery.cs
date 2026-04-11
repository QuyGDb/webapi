using MediatR;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;

namespace MusicShop.Application.UseCases.Catalog.Genres.Queries.GetGenreBySlug;

public record GetGenreBySlugQuery(string Slug) : IRequest<Result<GenreResponse>>;
