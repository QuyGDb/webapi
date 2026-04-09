using MediatR;
using MusicShop.Domain.Common;
using MusicShop.Domain.Enums;

namespace MusicShop.Application.UseCases.Catalog.ReleaseVersions.Commands.UpdateReleaseVersion;

public record UpdateReleaseVersionCommand(
    Guid Id,
    Guid LabelId,
    string? PressingCountry,
    int? PressingYear,
    ReleaseFormat Format,
    string? CatalogNumber,
    string? Notes) : IRequest<Result<Guid>>;
