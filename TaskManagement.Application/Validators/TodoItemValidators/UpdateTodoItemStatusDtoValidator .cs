using FluentValidation;
using TaskManagement.Application.DTOs.TodoItemDTOs;

namespace TaskManagement.Application.Validators.TodoItemValidators;

public class UpdateTodoItemStatusDtoValidator : AbstractValidator<UpdateTodoItemStatusDto>
{
    public UpdateTodoItemStatusDtoValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Invalid status value.");
    }
}
