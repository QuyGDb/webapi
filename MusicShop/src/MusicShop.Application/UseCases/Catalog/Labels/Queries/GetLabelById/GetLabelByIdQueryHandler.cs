using MediatR;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Errors;
using MusicShop.Domain.Interfaces;

namespace MusicShop.Application.UseCases.Catalog.Labels.Queries.GetLabelById;

public sealed class GetLabelByIdQueryHandler(IRepository<Label> labelRepository)
    : IRequestHandler<GetLabelByIdQuery, Result<LabelResponse>>
{
    public async Task<Result<LabelResponse>> Handle(
        GetLabelByIdQuery request, 
        CancellationToken cancellationToken)
    {
        Label? label = await labelRepository.GetByIdAsync(request.Id);

        if (label == null)
        {
            return Result<LabelResponse>.Failure(LabelErrors.NotFound);
        }

        return Result<LabelResponse>.Success(new LabelResponse
        {
            Id = label.Id,
            Name = label.Name,
            Country = label.Country,
            FoundedYear = label.FoundedYear,
            Website = label.Website
        });
    }
}
