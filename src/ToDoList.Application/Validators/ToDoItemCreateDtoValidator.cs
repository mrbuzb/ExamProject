using FluentValidation;
using ToDoList.Application.Dtos;

namespace ToDoList.Application.Validators;

public class ToDoItemCreateDtoValidator : AbstractValidator<ToDoItemCreateDto>
{
    public ToDoItemCreateDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .MaximumLength(100)
            .WithMessage("Title must not exceed 100 characters.");
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(250)
            .WithMessage("Description must not exceed 500 characters.");
        RuleFor(x => x.DueDate)
            .NotEmpty()
            .WithMessage("Due date is required.")
            .GreaterThan(DateTime.Now)
            .WithMessage("Due date must be in the future.");
    }
}

