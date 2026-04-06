using FluentValidation;

namespace MusicShop.Application.UseCases.Catalog.MasterReleases.Commands.CreateMasterRelease;

public sealed class CreateMasterReleaseCommandValidator : AbstractValidator<CreateMasterReleaseCommand>
{
    public CreateMasterReleaseCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Album title is required.")
            .MaximumLength(300).WithMessage("Title must not exceed 300 characters.");

        RuleFor(x => x.Year)
            .InclusiveBetween(1800, DateTime.Now.Year + 5).WithMessage("Invalid release year.");

        RuleFor(x => x.ArtistId)
            .NotEmpty().WithMessage("An artist must be assigned to this album.");

        RuleFor(x => x.Genre)
            .MaximumLength(150).WithMessage("Genre name is too long.");

        RuleFor(x => x.CoverUrl)
            .MaximumLength(500).WithMessage("Cover URL must not exceed 500 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.");
    }
}
