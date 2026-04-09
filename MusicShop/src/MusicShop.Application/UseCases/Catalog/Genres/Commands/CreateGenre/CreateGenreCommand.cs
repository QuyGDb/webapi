using MediatR;
using MusicShop.Domain.Common;

namespace MusicShop.Application.UseCases.Catalog.Genres.Commands.CreateGenre;

public record CreateGenreCommand(string Name, string Slug) : IRequest<Result<Guid>>;
