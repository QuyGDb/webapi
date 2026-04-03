using MediatR;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;

namespace MusicShop.Application.UseCases.Catalog.Artists.Commands.UpdateArtist;

public sealed record UpdateArtistCommand(
    Guid Id,
    string Name,
    string? Bio,
    string? Genre,
    string? Country,
    string? ImageUrl) : IRequest<Result<ArtistResponse>>;
