using MediatR;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MusicShop.Application.UseCases.Catalog.Labels.Commands.DeleteLabel;

public sealed class DeleteLabelCommandHandler(
    IRepository<Label> labelRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteLabelCommand, Result>
{
    public async Task<Result> Handle(
        DeleteLabelCommand request, 
        CancellationToken cancellationToken)
    {
        Label? label = await labelRepository.AsQueryable()
            .Include(x => x.ReleaseVersions)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (label is null)
        {
            return Result.Failure(new Error("Label.NotFound", "Label not found."));
        }

        if (label.ReleaseVersions.Any())
        {
            return Result.Failure(new Error("Label.HasAssociations", "Cannot delete label with existing release versions."));
        }

        labelRepository.Delete(label);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
