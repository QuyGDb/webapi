using MediatR;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Errors;
using MusicShop.Domain.Interfaces;

namespace MusicShop.Application.UseCases.Catalog.Labels.Commands.CreateLabel;

public sealed class CreateLabelCommandHandler(
    IRepository<Label> labelRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateLabelCommand, Result<LabelResponse>>
{
    public async Task<Result<LabelResponse>> Handle(
        CreateLabelCommand request, 
        CancellationToken cancellationToken)
    {
        // 1. Check if label already exists (by name)
        Label? existingLabel = await labelRepository.FirstOrDefaultAsync(x => x.Name == request.Name);
        if (existingLabel != null)
        {
            return Result<LabelResponse>.Failure(LabelErrors.DuplicateName);
        }

        // 2. Create new entity from command
        Label label = new Label
        {
            Name = request.Name,
            Country = request.Country,
            FoundedYear = request.FoundedYear,
            Website = request.Website
        };

        // 3. Add to Repository
        labelRepository.Add(label);

        // 4. Save changes to Database
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // 5. Return the created label response
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
