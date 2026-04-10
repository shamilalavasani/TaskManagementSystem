using FluentValidation;
using TaskManagement.Application.DTOs;

namespace TaskManagement.Application.Validators;

public class UpdateTodoItemStatusDtoValidator : AbstractValidator<UpdateTodoItemStatusDto>
{
    public UpdateTodoItemStatusDtoValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Invalid status value.");
    }
}
