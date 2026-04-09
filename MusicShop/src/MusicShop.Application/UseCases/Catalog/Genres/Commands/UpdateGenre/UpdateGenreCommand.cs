using MediatR;
using MusicShop.Domain.Common;
using MusicShop.Domain.Interfaces;
using MusicShop.Domain.Entities.Catalog;
using FluentValidation;

namespace MusicShop.Application.UseCases.Catalog.Genres.Commands.UpdateGenre;

public record UpdateGenreCommand(Guid Id, string Name, string Slug) : IRequest<Result<Guid>>;
