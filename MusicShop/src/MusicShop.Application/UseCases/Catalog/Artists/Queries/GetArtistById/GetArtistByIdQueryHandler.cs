using MediatR;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Interfaces;
using MusicShop.Application.Common.Mappings;

namespace MusicShop.Application.UseCases.Catalog.Artists.Queries.GetArtistById;

public sealed class GetArtistByIdQueryHandler(IArtistRepository artistRepository)
    : IRequestHandler<GetArtistByIdQuery, Result<ArtistResponse>>
{
    public async Task<Result<ArtistResponse>> Handle(
        GetArtistByIdQuery request,
        CancellationToken cancellationToken)
    {
        var artist = await artistRepository.GetWithGenresAsync(request.Id, cancellationToken);

        if (artist == null)
        {
            return Result<ArtistResponse>.Failure(new Error("Artist.NotFound", "Artist not found."));
        }

        var response = artist.ToResponse();

        return Result<ArtistResponse>.Success(response);
    }
}
