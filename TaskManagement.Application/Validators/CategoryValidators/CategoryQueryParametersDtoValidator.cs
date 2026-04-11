using FluentValidation;
using TaskManagement.Application.DTOs.QueryParametersDTOs;

namespace TaskManagement.Application.Validators.CategoryValidators;

public class CategoryQueryParametersDtoValidator : AbstractValidator<CategoryQueryParametersDto>
{
    public CategoryQueryParametersDtoValidator()
    {


        RuleFor(x => x.PageNumber)
            .GreaterThan(0);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 50);

        RuleFor(x => x.SortDirection)
            .Must(x => x == "asc" || x == "desc")
            .When(x => !string.IsNullOrWhiteSpace(x.SortDirection))
            .WithMessage("SortDirection must be 'asc' or 'desc'.");

        RuleFor(x => x.SortBy)
            .Must(x =>
                x == "Name" ||
                x == "createdAt")
            .When(x => !string.IsNullOrWhiteSpace(x.SortBy))
            .WithMessage("Invalid SortBy value.");
    }
}
