using MediatR;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;

namespace MusicShop.Application.UseCases.Catalog.Labels.Queries.GetLabelBySlug;

public record GetLabelBySlugQuery(string Slug) : IRequest<Result<LabelResponse>>;
