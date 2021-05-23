using FluentValidation;
using Lab1_.NET.ViewModels;

namespace Lab1_.NET.Validators
{
    public class MovieValidator : AbstractValidator<MovieViewModel>
    {
        public MovieValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required."); ;
            RuleFor(x => x.Genre).IsInEnum();
            RuleFor(x => x.Rating).InclusiveBetween(1, 10);
        }
    }
}
