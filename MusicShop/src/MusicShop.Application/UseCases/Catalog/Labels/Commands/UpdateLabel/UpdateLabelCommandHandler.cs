using MediatR;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Errors;
using MusicShop.Domain.Interfaces;

namespace MusicShop.Application.UseCases.Catalog.Labels.Commands.UpdateLabel;

public sealed class UpdateLabelCommandHandler(
    IRepository<Label> labelRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateLabelCommand, Result<LabelResponse>>
{
    public async Task<Result<LabelResponse>> Handle(
        UpdateLabelCommand request, 
        CancellationToken cancellationToken)
    {
        var label = await labelRepository.GetByIdAsync(request.Id);
        if (label == null)
        {
            return Result<LabelResponse>.Failure(LabelErrors.NotFound);
        }

        var existingWithSameName = await labelRepository.FirstOrDefaultAsync(
            x => x.Name == request.Name && x.Id != request.Id);
        
        if (existingWithSameName != null)
        {
            return Result<LabelResponse>.Failure(LabelErrors.DuplicateName);
        }

        label.Name = request.Name;
        label.Country = request.Country;
        label.FoundedYear = request.FoundedYear;
        label.Website = request.Website;

        labelRepository.Update(label);
        await unitOfWork.SaveChangesAsync(cancellationToken);

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
