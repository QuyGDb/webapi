using MediatR;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;

namespace MusicShop.Application.UseCases.Catalog.Labels.Commands.UpdateLabel;

public sealed record UpdateLabelCommand(
    Guid Id,
    string Name,
    string? Country,
    int? FoundedYear,
    string? Website) : IRequest<Result<LabelResponse>>;
