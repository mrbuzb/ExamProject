using FluentValidation.TestHelper;
using ToDoList.Application.Dtos;
using ToDoList.Application.Validators;

namespace ToDoList.Tests;
public class UserLoginDtoValidatorTests
{
    private readonly UserLoginDtoValidator _validator = new();

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Should_Have_Error_When_UserName_Is_Null_Or_Empty(string userName)
    {
        var model = new UserLoginDto { UserName = userName, Password = "ValidPass1" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.UserName)
            .WithErrorMessage("Username is required.");
    }

    [Fact]
    public void Should_Have_Error_When_UserName_Exceeds_Max_Length()
    {
        var model = new UserLoginDto { UserName = new string('a', 51), Password = "ValidPass1" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.UserName)
            .WithErrorMessage("Username must not exceed 50 characters.");
    }

    [Theory]
    [InlineData("invalid name")]
    [InlineData("user!")]
    [InlineData("user@name")]
    public void Should_Have_Error_When_UserName_Has_Invalid_Characters(string userName)
    {
        var model = new UserLoginDto { UserName = userName, Password = "ValidPass1" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.UserName)
            .WithErrorMessage("Username can only contain letters, numbers, and underscores.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_UserName_Is_Valid()
    {
        var model = new UserLoginDto { UserName = "Valid_User123", Password = "ValidPass1" };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.UserName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Should_Have_Error_When_Password_Is_Null_Or_Empty(string password)
    {
        var model = new UserLoginDto { UserName = "ValidUser", Password = password };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Password_Too_Short()
    {
        var model = new UserLoginDto { UserName = "ValidUser", Password = "Abc123" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must be at least 8 characters.");
    }

    [Fact]
    public void Should_Have_Error_When_Password_Too_Long()
    {
        var model = new UserLoginDto { UserName = "ValidUser", Password = new string('A', 129) };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must not exceed 128 characters.");
    }

    [Theory]
    [InlineData("password1")] // no uppercase
    [InlineData("PASSWORD1")] // no lowercase
    [InlineData("Password")]  // no number
    public void Should_Have_Error_When_Password_Missing_Required_Character_Types(string password)
    {
        var model = new UserLoginDto { UserName = "ValidUser", Password = password };
        var result = _validator.TestValidate(model);

        if (!password.Any(char.IsUpper))
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Password must contain at least one uppercase letter.");
        if (!password.Any(char.IsLower))
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Password must contain at least one lowercase letter.");
        if (!password.Any(char.IsDigit))
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Password must contain at least one number.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Password_Is_Valid()
    {
        var model = new UserLoginDto { UserName = "ValidUser", Password = "ValidPass1" };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }
}