using MediatR;
using MusicShop.Application.Common;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;

namespace MusicShop.Application.UseCases.Catalog.Releases.Queries.GetReleases;

public record GetReleasesQuery(
    int PageNumber = 1,
    int PageSize = 20,
    Guid? ArtistId = null,
    string? GenreSlug = null,
    int? Year = null,
    string? Q = null) : IRequest<Result<PaginatedResult<ReleaseResponse>>>;
