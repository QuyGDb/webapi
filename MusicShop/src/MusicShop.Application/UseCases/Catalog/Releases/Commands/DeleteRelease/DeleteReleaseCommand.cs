using MediatR;
using MusicShop.Domain.Common;

namespace MusicShop.Application.UseCases.Catalog.Releases.Commands.DeleteRelease;

public record DeleteReleaseCommand(Guid Id) : IRequest<Result>;
