using MediatR;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;

namespace MusicShop.Application.UseCases.Catalog.Artists.Queries.GetArtistById;

public sealed record GetArtistByIdQuery(Guid Id) : IRequest<Result<ArtistResponse>>;
