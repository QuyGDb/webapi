using MediatR;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using MusicShop.Domain.Errors;

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
            .FirstOrDefaultAsync(x => x.Slug == request.Slug, cancellationToken);

        if (label is null)
        {
            return Result.Failure(LabelErrors.NotFound);
        }

        if (label.ReleaseVersions.Any())
        {
            return Result.Failure(LabelErrors.HasAssociations);
        }

        labelRepository.Delete(label);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
