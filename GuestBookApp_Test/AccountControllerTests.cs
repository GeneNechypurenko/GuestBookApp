using GuestBookApp.Controllers;
using GuestBookApp.Models;
using GuestBookApp.Models.ViewModels;
using GuestBookApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Text;

namespace GuestBookApp.Tests
{
    public class AccountControllerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly AccountController _controller;
        private readonly Mock<ISession> _sessionMock;

        public AccountControllerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _sessionMock = new Mock<ISession>();

            var httpContext = new DefaultHttpContext();
            httpContext.Session = _sessionMock.Object;

            _controller = new AccountController(_userRepositoryMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };
        }

        [Fact]
        public void Login_ValidModel_ReturnsOkResult()
        {
            // Arrange
            var model = new LoginViewModel { Username = "TestUser" };
            _sessionMock.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
                        .Callback<string, byte[]>((key, value) =>
                        {
                            if (key == "Username")
                            {
                                var storedValue = Encoding.UTF8.GetString(value);
                                Assert.Equal("TestUser", storedValue);
                            }
                        });

            // Act
            var result = _controller.Login(model);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void Login_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Username", "Required");

            // Act
            var result = _controller.Login(new LoginViewModel());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async Task Register_ValidModelAndNewUser_ReturnsOkResult()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync("NewUser"))
                               .ReturnsAsync((User)null);

            var model = new RegisterViewModel { Username = "NewUser", Password = "Password123" };

            // Act
            var result = await _controller.Register(model);

            // Assert
            Assert.IsType<OkResult>(result);
            _userRepositoryMock.Verify(repo => repo.AddUserAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task Register_ExistingUser_ReturnsBadRequest()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync("ExistingUser"))
                               .ReturnsAsync(new User { Username = "ExistingUser" });

            var model = new RegisterViewModel { Username = "ExistingUser", Password = "Password123" };

            // Act
            var result = await _controller.Register(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public void Logout_ClearsSession_ReturnsOkResult()
        {
            // Arrange
            _sessionMock.Setup(s => s.Remove("Username")).Verifiable();

            // Act
            var result = _controller.Logout();

            // Assert
            Assert.IsType<OkResult>(result);
            _sessionMock.Verify(s => s.Remove("Username"), Times.Once);
        }
    }
}
