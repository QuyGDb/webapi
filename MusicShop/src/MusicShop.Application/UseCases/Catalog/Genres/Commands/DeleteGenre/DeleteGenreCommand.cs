using MediatR;
using MusicShop.Domain.Common;
using MusicShop.Domain.Interfaces;
using MusicShop.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;

namespace MusicShop.Application.UseCases.Catalog.Genres.Commands.DeleteGenre;

public record DeleteGenreCommand(string Slug) : IRequest<Result>;
