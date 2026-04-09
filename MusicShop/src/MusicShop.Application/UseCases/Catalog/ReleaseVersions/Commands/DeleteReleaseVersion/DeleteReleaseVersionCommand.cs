using MediatR;
using MusicShop.Domain.Common;

namespace MusicShop.Application.UseCases.Catalog.ReleaseVersions.Commands.DeleteReleaseVersion;

public record DeleteReleaseVersionCommand(Guid Id) : IRequest<Result>;
