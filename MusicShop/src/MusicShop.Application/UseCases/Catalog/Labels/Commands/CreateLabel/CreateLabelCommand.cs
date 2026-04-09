using MediatR;
using MusicShop.Domain.Common;

namespace MusicShop.Application.UseCases.Catalog.Labels.Commands.CreateLabel;

public record CreateLabelCommand(
    string Name, 
    string? Country, 
    int? FoundedYear, 
    string? Website) : IRequest<Result<Guid>>;
