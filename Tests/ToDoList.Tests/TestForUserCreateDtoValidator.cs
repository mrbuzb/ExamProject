using FluentValidation.TestHelper;
using ToDoList.Application.Dtos;
using ToDoList.Application.Validators;

namespace ToDoList.Tests;

public class TestForUserCreateDtoValidator
{
    private readonly UserCreateDtoValidator _validator = new();

    private UserCreateDto GetValidDto() => new UserCreateDto
    {
        FirstName = "John",
        LastName = "Doe",
        UserName = "john_doe123",
        Email = "john.doe@example.com",
        PhoneNumber = "+12345678901",
        Password = "Password1"
    };

    [Fact]
    public void Should_Pass_Validation_For_Valid_Dto()
    {
        var dto = GetValidDto();
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Should_Have_Error_When_FirstName_Is_Empty(string? firstName)
    {
        var dto = GetValidDto();
        dto.FirstName = firstName;
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Fact]
    public void Should_Have_Error_When_FirstName_Exceeds_MaxLength()
    {
        var dto = GetValidDto();
        dto.FirstName = new string('A', 51);
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Fact]
    public void Should_Have_Error_When_LastName_Exceeds_MaxLength()
    {
        var dto = GetValidDto();
        dto.LastName = new string('B', 51);
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Should_Have_Error_When_UserName_Is_Empty(string? userName)
    {
        var dto = GetValidDto();
        dto.UserName = userName;
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.UserName);
    }

    [Fact]
    public void Should_Have_Error_When_UserName_Exceeds_MaxLength()
    {
        var dto = GetValidDto();
        dto.UserName = new string('C', 51);
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.UserName);
    }

    [Theory]
    [InlineData("invalid username")]
    [InlineData("user!@#")]
    public void Should_Have_Error_When_UserName_Has_Invalid_Characters(string userName)
    {
        var dto = GetValidDto();
        dto.UserName = userName;
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.UserName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Should_Have_Error_When_Email_Is_Empty(string? email)
    {
        var dto = GetValidDto();
        dto.Email = email;
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Exceeds_MaxLength()
    {
        var dto = GetValidDto();
        dto.Email = new string('a', 321) + "@example.com";
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Should_Have_Error_When_PhoneNumber_Is_Empty(string? phone)
    {
        var dto = GetValidDto();
        dto.PhoneNumber = phone;
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [Fact]
    public void Should_Have_Error_When_PhoneNumber_Exceeds_MaxLength()
    {
        var dto = GetValidDto();
        dto.PhoneNumber = "+1234567890123456";
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("phone123")]
    [InlineData("+12abc345678")]
    public void Should_Have_Error_When_PhoneNumber_Is_Invalid(string phone)
    {
        var dto = GetValidDto();
        dto.PhoneNumber = phone;
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Should_Have_Error_When_Password_Is_Empty(string? password)
    {
        var dto = GetValidDto();
        dto.Password = password;
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Too_Short()
    {
        var dto = GetValidDto();
        dto.Password = "Abc123";
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Too_Long()
    {
        var dto = GetValidDto();
        dto.Password = new string('A', 129) + "1a";
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Missing_Uppercase()
    {
        var dto = GetValidDto();
        dto.Password = "password1";
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must contain at least one uppercase letter.");
    }

    [Fact]
    public void Should_Have_Error_When_Password_Missing_Lowercase()
    {
        var dto = GetValidDto();
        dto.Password = "PASSWORD1";
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must contain at least one lowercase letter.");
    }

    [Fact]
    public void Should_Have_Error_When_Password_Missing_Number()
    {
        var dto = GetValidDto();
        dto.Password = "Password";
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must contain at least one number.");
    }
}