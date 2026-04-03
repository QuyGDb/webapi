using MediatR;
using MusicShop.Domain.Common;

namespace MusicShop.Application.UseCases.Catalog.Labels.Commands.DeleteLabel;

public sealed record DeleteLabelCommand(Guid Id) : IRequest<Result<bool>>;
