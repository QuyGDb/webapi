using MediatR;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Interfaces;
using MusicShop.Domain.Errors;

namespace MusicShop.Application.UseCases.Catalog.ReleaseVersions.Commands.CreateReleaseVersion;

public sealed class CreateReleaseVersionCommandHandler(
    IRepository<Release> releaseRepository,
    IRepository<Label> labelRepository,
    IRepository<ReleaseVersion> releaseVersionRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateReleaseVersionCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateReleaseVersionCommand request, 
        CancellationToken cancellationToken)
    {
        // 1. Verify Release
        Release? release = await releaseRepository.GetByIdAsync(request.ReleaseId, cancellationToken);
        if (release == null)
            return Result<Guid>.Failure(ReleaseErrors.NotFound);

        // 2. Verify Label
        Label? label = await labelRepository.GetByIdAsync(request.LabelId, cancellationToken);
        if (label == null)
            return Result<Guid>.Failure(LabelErrors.NotFound);

        // 3. Create Version
        ReleaseVersion version = new ReleaseVersion
        {
            ReleaseId = request.ReleaseId,
            LabelId = request.LabelId,
            PressingCountry = request.PressingCountry,
            PressingYear = request.PressingYear,
            Format = request.Format,
            CatalogNumber = request.CatalogNumber,
            Notes = request.Notes
        };

        releaseVersionRepository.Add(version);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(version.Id);
    }
}
