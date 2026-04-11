using MediatR;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Errors;
using MusicShop.Domain.Interfaces;

namespace MusicShop.Application.UseCases.Catalog.Labels.Queries.GetLabelBySlug;

public sealed class GetLabelBySlugQueryHandler(IRepository<Label> labelRepository)
    : IRequestHandler<GetLabelBySlugQuery, Result<LabelResponse>>
{
    public async Task<Result<LabelResponse>> Handle(
        GetLabelBySlugQuery request, 
        CancellationToken cancellationToken)
    {
        Label? label = await labelRepository.FirstOrDefaultAsync(x => x.Slug == request.Slug, cancellationToken);

        if (label == null)
        {
            return Result<LabelResponse>.Failure(LabelErrors.NotFound);
        }

        return Result<LabelResponse>.Success(new LabelResponse
        {
            Id = label.Id,
            Name = label.Name,
            Slug = label.Slug,
            Country = label.Country,
            FoundedYear = label.FoundedYear,
            Website = label.Website
        });
    }
}
