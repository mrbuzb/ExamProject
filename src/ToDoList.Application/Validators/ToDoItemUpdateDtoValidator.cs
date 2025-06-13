using FluentValidation;
using ToDoList.Application.Dtos;

namespace ToDoList.Application.Validators;

public class ToDoItemUpdateDtoValidator : AbstractValidator<ToDoItemUpdateDto>
{
    public ToDoItemUpdateDtoValidator()
    {
        RuleFor(x => x.ToDoItemId)
            .GreaterThan(0)
            .WithMessage("ToDoItemId is required.");
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .MaximumLength(100)
            .WithMessage("Title must not exceed 100 characters.");
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(251)
            .WithMessage("Description must not exceed 500 characters.");
        RuleFor(x => x.DueDate)
            .NotEmpty()
            .WithMessage("Due date is required.")
            .GreaterThan(DateTime.Now)
            .WithMessage("Due date must be in the future.");
    }
}

