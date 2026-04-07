using MediatR;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;
using MusicShop.Domain.Errors;
using MusicShop.Domain.Interfaces;

namespace MusicShop.Application.UseCases.Catalog.Releases.Queries.GetReleaseById;

public sealed class GetReleaseByIdQueryHandler(IReleaseRepository releaseRepository)
    : IRequestHandler<GetReleaseByIdQuery, Result<ReleaseDetailResponse>>
{
    public async Task<Result<ReleaseDetailResponse>> Handle(
        GetReleaseByIdQuery request, 
        CancellationToken cancellationToken)
    {
        var release = await releaseRepository.GetWithDetailsAsync(request.Id, cancellationToken);
        if (release == null)
            return Result<ReleaseDetailResponse>.Failure(ReleaseErrors.NotFound);

        var response = new ReleaseDetailResponse
        {
            Id = release.Id,
            Title = release.Title,
            Year = release.Year,
            CoverUrl = release.CoverUrl,
            Description = release.Description,
            ArtistId = release.ArtistId,
            ArtistName = release.Artist?.Name ?? string.Empty,
            Genres = release.ReleaseGenres.Select(rg => new GenreResponse
            {
                Id = rg.GenreId,
                Name = rg.Genre?.Name ?? string.Empty,
                Slug = rg.Genre?.Slug ?? string.Empty
            }).ToList(),
            Tracks = release.Tracks.Select(t => new TrackDto
            {
                Id = t.Id,
                Position = t.Position,
                Title = t.Title,
                DurationSeconds = t.DurationSeconds,
                Side = t.Side
            }).ToList(),
            Versions = release.Versions.Select(v => new ReleaseVersionDto
            {
                Id = v.Id,
                PressingCountry = v.PressingCountry,
                PressingYear = v.PressingYear,
                Format = v.Format.ToString().ToLower(),
                CatalogNumber = v.CatalogNumber,
                Notes = v.Notes,
                LabelName = v.Label?.Name ?? string.Empty
            }).ToList()
        };

        return Result<ReleaseDetailResponse>.Success(response);
    }
}
