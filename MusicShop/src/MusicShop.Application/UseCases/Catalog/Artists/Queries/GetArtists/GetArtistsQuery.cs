using MediatR;
using MusicShop.Application.Common;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;

namespace MusicShop.Application.UseCases.Catalog.Artists.Queries.GetArtists;

public sealed record GetArtistsQuery(int PageNumber = 1, int PageSize = 20) 
    : IRequest<Result<PaginatedResult<ArtistResponse>>>;
