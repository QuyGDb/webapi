using MediatR;
using MusicShop.Domain.Common;

namespace MusicShop.Application.UseCases.Catalog.Artists.Commands.UpdateArtist;

public record UpdateArtistCommand(
    string OldSlug,
    string Name,
    string Slug,
    string? Bio,
    string? Country,
    string? ImageUrl,
    List<Guid>? GenreIds = null) : IRequest<Result<string>>;
