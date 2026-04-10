using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Errors;
using MusicShop.Domain.Interfaces;

namespace MusicShop.Application.UseCases.Catalog.Artists.Commands.CreateArtist;

public sealed class CreateArtistCommandHandler(
    IRepository<Artist> artistRepository,
    IRepository<Genre> genreRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateArtistCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateArtistCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Check for duplicate name
        Artist? existing = await artistRepository.FirstOrDefaultAsync(x => x.Name == request.Name, cancellationToken);
        if (existing != null)
        {
            return Result<Guid>.Failure(ArtistErrors.DuplicateName);
        }

        // 2. Validate Genres existence
        if (request.GenreIds != null && request.GenreIds.Any())
        {
            // Remove duplicates from request to avoid double count
            List<Guid> distinctGenreIds = request.GenreIds.Distinct().ToList();
            
            int existingGenresCount = await genreRepository.AsQueryable()
                .CountAsync(g => distinctGenreIds.Contains(g.Id), cancellationToken);

            if (existingGenresCount != distinctGenreIds.Count)
            {
                return Result<Guid>.Failure(GenreErrors.NotFound);
            }
        }

        // 3. Create Artist entity
        Artist artist = new Artist
        {
            Name = request.Name,
            Bio = request.Bio,
            Country = request.Country,
            ImageUrl = request.ImageUrl
        };

        // 4. Handle Genres many-to-many
        if (request.GenreIds != null && request.GenreIds.Any())
        {
            foreach (Guid genreId in request.GenreIds.Distinct())
            {
                artist.ArtistGenres.Add(new ArtistGenre
                {
                    GenreId = genreId
                });
            }
        }

        artistRepository.Add(artist);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(artist.Id);
    }
}
