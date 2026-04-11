using MediatR;
using MusicShop.Domain.Common;

namespace MusicShop.Application.UseCases.Catalog.Artists.Commands.DeleteArtist;

public record DeleteArtistCommand(string Slug) : IRequest<Result>;
