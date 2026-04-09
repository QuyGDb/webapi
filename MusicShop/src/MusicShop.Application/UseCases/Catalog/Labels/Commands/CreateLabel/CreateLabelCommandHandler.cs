using MediatR;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Interfaces;

namespace MusicShop.Application.UseCases.Catalog.Labels.Commands.CreateLabel;

public sealed class CreateLabelCommandHandler(
    IRepository<Label> labelRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateLabelCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateLabelCommand request, 
        CancellationToken cancellationToken)
    {
        var label = new Label
        {
            Name = request.Name,
            Country = request.Country,
            FoundedYear = request.FoundedYear,
            Website = request.Website
        };

        labelRepository.Add(label);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(label.Id);
    }
}
