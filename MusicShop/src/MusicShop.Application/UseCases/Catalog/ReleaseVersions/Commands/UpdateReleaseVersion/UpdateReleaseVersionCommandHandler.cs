using MediatR;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Interfaces;

namespace MusicShop.Application.UseCases.Catalog.ReleaseVersions.Commands.UpdateReleaseVersion;

public sealed class UpdateReleaseVersionCommandHandler(
    IRepository<ReleaseVersion> releaseVersionRepository,
    IRepository<Label> labelRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateReleaseVersionCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        UpdateReleaseVersionCommand request, 
        CancellationToken cancellationToken)
    {
        // 1. Fetch Version
        var version = await releaseVersionRepository.GetByIdAsync(request.Id, cancellationToken);
        if (version == null)
            return Result<Guid>.Failure(new Error("ReleaseVersion.NotFound", "Release version not found."));

        // 2. Verify Label if changed
        if (version.LabelId != request.LabelId)
        {
            var label = await labelRepository.GetByIdAsync(request.LabelId, cancellationToken);
            if (label == null)
                return Result<Guid>.Failure(new Error("Label.NotFound", "Label not found."));
            
            version.LabelId = request.LabelId;
        }

        // 3. Update properties
        version.PressingCountry = request.PressingCountry;
        version.PressingYear = request.PressingYear;
        version.Format = request.Format;
        version.CatalogNumber = request.CatalogNumber;
        version.Notes = request.Notes;

        releaseVersionRepository.Update(version);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(version.Id);
    }
}
