using MediatR;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Interfaces;

namespace MusicShop.Application.UseCases.Catalog.Labels.Commands.UpdateLabel;

public sealed class UpdateLabelCommandHandler(
    IRepository<Label> labelRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateLabelCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        UpdateLabelCommand request, 
        CancellationToken cancellationToken)
    {
        var label = await labelRepository.GetByIdAsync(request.Id, cancellationToken);
        if (label == null)
        {
            return Result<Guid>.Failure(new Error("Label.NotFound", "Label not found."));
        }

        label.Name = request.Name;
        label.Country = request.Country;
        label.FoundedYear = request.FoundedYear;
        label.Website = request.Website;

        labelRepository.Update(label);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(label.Id);
    }
}
