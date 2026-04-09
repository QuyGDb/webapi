using MediatR;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Interfaces;

namespace MusicShop.Application.UseCases.Catalog.ReleaseVersions.Commands.DeleteReleaseVersion;

public sealed class DeleteReleaseVersionCommandHandler(
    IRepository<ReleaseVersion> releaseVersionRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteReleaseVersionCommand, Result>
{
    public async Task<Result> Handle(
        DeleteReleaseVersionCommand request, 
        CancellationToken cancellationToken)
    {
        var version = await releaseVersionRepository.GetByIdAsync(request.Id, cancellationToken);
        if (version == null)
            return Result.Failure(new Error("ReleaseVersion.NotFound", "Release version not found."));

        // Placeholder for Shop Integration: Check if inventory exists
        // if (await inventoryRepository.AnyAsync(x => x.ReleaseVersionId == request.Id)) ...

        releaseVersionRepository.Delete(version);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
