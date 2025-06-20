using FluentValidation.TestHelper;
using ToDoList.Application.Dtos;
using ToDoList.Application.Validators;

namespace ToDoList.Tests;

public class TestForToDoItemUpdateDtoValidator
{
    private readonly ToDoItemUpdateDtoValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_ToDoItemId_Is_Zero_Or_Negative()
    {
        var model = new ToDoItemUpdateDto
        {
            ToDoItemId = 0,
            Title = "Valid Title",
            Description = "Valid Description",
            DueDate = DateTime.Now.AddDays(1)
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.ToDoItemId);

        model.ToDoItemId = -5;
        result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.ToDoItemId);
    }

    [Fact]
    public void Should_Not_Have_Error_When_ToDoItemId_Is_Positive()
    {
        var model = new ToDoItemUpdateDto
        {
            ToDoItemId = 1,
            Title = "Valid Title",
            Description = "Valid Description",
            DueDate = DateTime.Now.AddDays(1)
        };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.ToDoItemId);
    }

    [Fact]
    public void Should_Have_Error_When_Title_Is_Empty_Or_Too_Long()
    {
        var model = new ToDoItemUpdateDto
        {
            ToDoItemId = 1,
            Title = "",
            Description = "Valid Description",
            DueDate = DateTime.Now.AddDays(1)
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Title);

        model.Title = new string('a', 101);
        result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Title_Is_Valid()
    {
        var model = new ToDoItemUpdateDto
        {
            ToDoItemId = 1,
            Title = "Valid Title",
            Description = "Valid Description",
            DueDate = DateTime.Now.AddDays(1)
        };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Empty_Or_Too_Long()
    {
        var model = new ToDoItemUpdateDto
        {
            ToDoItemId = 1,
            Title = "Valid Title",
            Description = "",
            DueDate = DateTime.Now.AddDays(1)
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Description);

        model.Description = new string('a', 252);
        result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Description_Is_Valid()
    {
        var model = new ToDoItemUpdateDto
        {
            ToDoItemId = 1,
            Title = "Valid Title",
            Description = "Valid Description",
            DueDate = DateTime.Now.AddDays(1)
        };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Have_Error_When_DueDate_Is_Default_Or_Past()
    {
        var model = new ToDoItemUpdateDto
        {
            ToDoItemId = 1,
            Title = "Valid Title",
            Description = "Valid Description",
            DueDate = default
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.DueDate);

        model.DueDate = DateTime.Now.AddMinutes(-1);
        result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.DueDate);
    }

    [Fact]
    public void Should_Not_Have_Error_When_DueDate_Is_In_The_Future()
    {
        var model = new ToDoItemUpdateDto
        {
            ToDoItemId = 1,
            Title = "Valid Title",
            Description = "Valid Description",
            DueDate = DateTime.Now.AddDays(1)
        };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.DueDate);
    }
}