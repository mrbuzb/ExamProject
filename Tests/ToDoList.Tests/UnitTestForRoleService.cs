using Moq;
using ToDoList.Application.Interfaces;
using ToDoList.Application.Services;
using ToDoList.Domain.Entities;

namespace ToDoList.Tests
{
    public class UnitTestForRoleService
    {
        private readonly Mock<IRoleRepository> _roleRepoMock;
        private readonly RoleService _roleService;

        public UnitTestForRoleService()
        {
            _roleRepoMock = new Mock<IRoleRepository>();
            _roleService = new RoleService(_roleRepoMock.Object);
        }

        [Fact]
        public async Task GetAllRolesAsync_ReturnsMappedRoleGetDtos()
        {
            // Arrange
            var roles = new List<UserRole>
            {
                new UserRole { Id = 1, Name = "Admin", Description = "Administrator" },
                new UserRole { Id = 2, Name = "User", Description = "Regular User" }
            };
            _roleRepoMock.Setup(r => r.GetAllRolesAsync()).ReturnsAsync(roles);

            // Act
            var result = await _roleService.GetAllRolesAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, r => r.Name == "Admin" && r.Description == "Administrator");
            Assert.Contains(result, r => r.Name == "User" && r.Description == "Regular User");
        }

        [Fact]
        public async Task GetRoleIdAsync_ReturnsRoleId()
        {
            // Arrange
            string roleName = "Admin";
            long expectedId = 42;
            _roleRepoMock.Setup(r => r.GetRoleIdAsync(roleName)).ReturnsAsync(expectedId);

            // Act
            var result = await _roleService.GetRoleIdAsync(roleName);

            // Assert
            Assert.Equal(expectedId, result);
        }

        [Fact]
        public async Task GetAllUsersByRoleAsync_ReturnsMappedUserGetDtos()
        {
            // Arrange
            var userRole = new UserRole { Id = 1, Name = "Admin", Description = "Administrator" };
            var users = new List<User>
            {
                new User
                {
                    UserId = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    UserName = "johndoe",
                    Email = "john@example.com",
                    PhoneNumber = "1234567890",
                    Role = userRole
                },
                new User
                {
                    UserId = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    UserName = "janesmith",
                    Email = "jane@example.com",
                    PhoneNumber = "0987654321",
                    Role = userRole
                }
            };
            _roleRepoMock.Setup(r => r.GetAllUsersByRoleAsync("Admin")).ReturnsAsync(users);

            // Act
            var result = await _roleService.GetAllUsersByRoleAsync("Admin");

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, u => u.UserName == "johndoe" && u.Role == "Admin");
            Assert.Contains(result, u => u.UserName == "janesmith" && u.Role == "Admin");
        }
    }
}