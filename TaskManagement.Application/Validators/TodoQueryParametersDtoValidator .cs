using FluentValidation;
using TaskManagement.Application.DTOs;

namespace TaskManagement.Application.Validators;

public class TodoQueryParametersDtoValidator : AbstractValidator<TodoQueryParametersDto>
{
    public TodoQueryParametersDtoValidator()
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
                x == "title" ||
                x == "createdAt" ||
                x == "dueDate")
            .When(x => !string.IsNullOrWhiteSpace(x.SortBy))
            .WithMessage("Invalid SortBy value.");
    }
}