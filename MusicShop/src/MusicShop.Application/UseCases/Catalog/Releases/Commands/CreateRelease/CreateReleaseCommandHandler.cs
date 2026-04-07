using MediatR;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Errors;
using MusicShop.Domain.Interfaces;

namespace MusicShop.Application.UseCases.Catalog.Releases.Commands.CreateRelease;

public sealed class CreateReleaseCommandHandler(
    IRepository<Artist> artistRepository,
    IRepository<Release> releaseRepository,
    IRepository<Genre> genreRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateReleaseCommand, Result<ReleaseResponse>>
{
    public async Task<Result<ReleaseResponse>> Handle(
        CreateReleaseCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Verify Artist
        var artist = await artistRepository.GetByIdAsync(request.ArtistId);
        if (artist == null)
            return Result<ReleaseResponse>.Failure(ReleaseErrors.ArtistNotFound);

        // 2. Map Release
        var release = new Release
        {
            Title = request.Title,
            Year = request.Year,
            CoverUrl = request.CoverUrl,
            Description = request.Description,
            ArtistId = request.ArtistId
        };

        // 3. Map Tracks
        if (request.Tracks != null)
        {
            foreach (var t in request.Tracks)
            {
                release.Tracks.Add(new Track
                {
                    Position = t.Position,
                    Title = t.Title,
                    DurationSeconds = t.DurationSeconds,
                    Side = t.Side
                });
            }
        }

        // 4. Map Genres (Junction Table)
        if (request.GenreIds != null && request.GenreIds.Count > 0)
        {
            foreach (var genreId in request.GenreIds)
            {
                var genre = await genreRepository.GetByIdAsync(genreId);
                if (genre != null)
                {
                    release.ReleaseGenres.Add(new ReleaseGenre { GenreId = genreId });
                }
            }
        }

        releaseRepository.Add(release);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // 5. Build Response
        return Result<ReleaseResponse>.Success(new ReleaseResponse
        {
            Id = release.Id,
            Title = release.Title,
            Year = release.Year,
            CoverUrl = release.CoverUrl,
            Description = release.Description,
            ArtistId = release.ArtistId,
            ArtistName = artist.Name,
            Genres = release.ReleaseGenres.Select(rg => new GenreResponse
            {
                Id = rg.GenreId,
                Name = rg.Genre?.Name ?? string.Empty,
                Slug = rg.Genre?.Slug ?? string.Empty
            }).ToList()
        });
    }
}
