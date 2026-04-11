using FluentValidation;

using TaskManagement.Application.DTOs.CategoryDTOs;

namespace TaskManagement.Application.Validators.CategoryValidators;

public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
{
    public UpdateCategoryDtoValidator()
    {

        RuleFor(x => x.Name)
             .NotEmpty().WithMessage("Name is required.")
             .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(500);

    }
}
