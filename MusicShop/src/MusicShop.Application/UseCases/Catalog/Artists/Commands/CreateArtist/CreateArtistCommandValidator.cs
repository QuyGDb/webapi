using FluentValidation;

namespace MusicShop.Application.UseCases.Catalog.Artists.Commands.CreateArtist;

public sealed class CreateArtistCommandValidator : AbstractValidator<CreateArtistCommand>
{
    public CreateArtistCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Artist name is required.")
            .MaximumLength(200).WithMessage("Artist name must not exceed 200 characters.");

        RuleFor(x => x.Genre)
            .MaximumLength(100).WithMessage("Genre must not exceed 100 characters.");

        RuleFor(x => x.Country)
            .MaximumLength(100).WithMessage("Country must not exceed 100 characters.");
    }
}
