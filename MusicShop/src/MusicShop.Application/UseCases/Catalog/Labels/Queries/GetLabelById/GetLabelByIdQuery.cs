using MediatR;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;

namespace MusicShop.Application.UseCases.Catalog.Labels.Queries.GetLabelById;

public record GetLabelByIdQuery(Guid Id) : IRequest<Result<LabelResponse>>;
