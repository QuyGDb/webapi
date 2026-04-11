using MediatR;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;

namespace MusicShop.Application.UseCases.Catalog.Artists.Queries.GetArtistBySlug;

public sealed record GetArtistBySlugQuery(string Slug) : IRequest<Result<ArtistResponse>>;
