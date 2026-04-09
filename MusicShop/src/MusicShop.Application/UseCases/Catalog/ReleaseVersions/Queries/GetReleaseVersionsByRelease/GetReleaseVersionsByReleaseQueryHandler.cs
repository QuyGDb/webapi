using MediatR;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace MusicShop.Application.UseCases.Catalog.ReleaseVersions.Queries.GetReleaseVersionsByRelease;

public sealed class GetReleaseVersionsByReleaseQueryHandler(
    IRepository<ReleaseVersion> releaseVersionRepository,
    IMapper mapper)
    : IRequestHandler<GetReleaseVersionsByReleaseQuery, Result<IReadOnlyList<ReleaseVersionDto>>>
{
    public async Task<Result<IReadOnlyList<ReleaseVersionDto>>> Handle(
        GetReleaseVersionsByReleaseQuery request, 
        CancellationToken cancellationToken)
    {
        var versions = await releaseVersionRepository.AsQueryable()
            .Include(v => v.Label)
            .Where(v => v.ReleaseId == request.ReleaseId)
            .ToListAsync(cancellationToken);

        var dtos = mapper.Map<List<ReleaseVersionDto>>(versions);

        return Result<IReadOnlyList<ReleaseVersionDto>>.Success(dtos.AsReadOnly());
    }
}
