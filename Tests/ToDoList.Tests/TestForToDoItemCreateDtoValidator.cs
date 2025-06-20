using FluentValidation.TestHelper;
using ToDoList.Application.Dtos;
using ToDoList.Application.Validators;

namespace ToDoList.Tests;

public class TestForToDoItemCreateDtoValidator
{
    private readonly ToDoItemCreateDtoValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Title_Is_Empty()
    {
        var model = new ToDoItemCreateDto { Title = "", Description = "desc", DueDate = DateTime.Now.AddDays(1) };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("Title is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Title_Exceeds_MaxLength()
    {
        var model = new ToDoItemCreateDto
        {
            Title = new string('a', 101),
            Description = "desc",
            DueDate = DateTime.Now.AddDays(1)
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("Title must not exceed 100 characters.");
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Empty()
    {
        var model = new ToDoItemCreateDto { Title = "title", Description = "", DueDate = DateTime.Now.AddDays(1) };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("Description is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Description_Exceeds_MaxLength()
    {
        var model = new ToDoItemCreateDto
        {
            Title = "title",
            Description = new string('b', 251),
            DueDate = DateTime.Now.AddDays(1)
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("Description must not exceed 500 characters.");
    }

    [Fact]
    public void Should_Have_Error_When_DueDate_Is_Default()
    {
        var model = new ToDoItemCreateDto { Title = "title", Description = "desc", DueDate = default };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.DueDate)
            .WithErrorMessage("Due date is required.");
    }

    [Fact]
    public void Should_Have_Error_When_DueDate_Is_Not_In_The_Future()
    {
        var model = new ToDoItemCreateDto
        {
            Title = "title",
            Description = "desc",
            DueDate = DateTime.Now.AddMinutes(-1)
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.DueDate)
            .WithErrorMessage("Due date must be in the future.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Model_Is_Valid()
    {
        var model = new ToDoItemCreateDto
        {
            Title = "Valid Title",
            Description = "Valid Description",
            DueDate = DateTime.Now.AddDays(1)
        };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}