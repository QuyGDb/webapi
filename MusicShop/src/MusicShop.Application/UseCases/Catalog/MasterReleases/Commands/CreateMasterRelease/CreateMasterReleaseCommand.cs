using MediatR;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;

namespace MusicShop.Application.UseCases.Catalog.MasterReleases.Commands.CreateMasterRelease;

public record CreateMasterReleaseCommand(
    string Title,
    int Year,
    string? Genre,
    string? CoverUrl,
    string? Description,
    Guid ArtistId) : IRequest<Result<MasterReleaseResponse>>;
