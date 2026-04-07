using MediatR;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Errors;
using MusicShop.Domain.Interfaces;

namespace MusicShop.Application.UseCases.Catalog.Labels.Commands.DeleteLabel;

public sealed class DeleteLabelCommandHandler(
    IRepository<Label> labelRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteLabelCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        DeleteLabelCommand request, 
        CancellationToken cancellationToken)
    {
        Label? label = await labelRepository.GetByIdAsync(request.Id);
        if (label == null)
        {
            return Result<bool>.Failure(LabelErrors.NotFound);
        }

        labelRepository.Delete(label);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
