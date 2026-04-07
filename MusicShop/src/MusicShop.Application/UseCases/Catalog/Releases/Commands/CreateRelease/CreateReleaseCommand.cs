using MediatR;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;

namespace MusicShop.Application.UseCases.Catalog.Releases.Commands.CreateRelease;

public record TrackCreateDto(
    int Position, 
    string Title, 
    int? DurationSeconds, 
    string? Side);

public record CreateReleaseCommand(
    string Title,
    int Year,
    Guid ArtistId,
    string? CoverUrl,
    string? Description,
    List<Guid>? GenreIds,
    List<TrackCreateDto>? Tracks) : IRequest<Result<ReleaseResponse>>;
