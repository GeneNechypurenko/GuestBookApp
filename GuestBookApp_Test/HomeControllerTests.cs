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
    public class HomeControllerTests
    {
        private readonly Mock<IMessageRepository> _messageRepositoryMock;
        private readonly HomeController _controller;
        private readonly Mock<ISession> _sessionMock;

        public HomeControllerTests()
        {
            _messageRepositoryMock = new Mock<IMessageRepository>();
            _sessionMock = new Mock<ISession>();

            var httpContext = new DefaultHttpContext
            {
                Session = _sessionMock.Object
            };

            _controller = new HomeController(_messageRepositoryMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfMessages()
        {
            // Arrange
            var messages = new List<Message> { new Message { Text = "Hello", Username = "User" } };
            _messageRepositoryMock.Setup(repo => repo.GetMessagesAsync())
                                  .ReturnsAsync(messages);

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Message>>(viewResult.Model);
            Assert.Single(model);
        }

        [Fact]
        public async Task AddMessage_ValidModel_ReturnsOkResult()
        {
            // Arrange
            var messageModel = new MessageViewModel { Text = "New Message" };
            SetupSessionValue("Username", "Test");

            // Act
            var result = await _controller.AddMessage(messageModel);

            // Assert
            Assert.IsType<OkResult>(result);
            _messageRepositoryMock.Verify(repo => repo.AddMessageAsync(It.IsAny<Message>()), Times.Once);
        }

        [Fact]
        public async Task AddMessage_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Text", "Required");

            // Act
            var result = await _controller.AddMessage(new MessageViewModel());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async Task PartialGuestBookEntries_ReturnsPartialView_WithMessages()
        {
            // Arrange
            var messages = new List<Message> { new Message { Text = "Hello", Username = "User" } };
            _messageRepositoryMock.Setup(repo => repo.GetMessagesAsync())
                                  .ReturnsAsync(messages);

            // Act
            var result = await _controller.PartialGuestBookEntries();

            // Assert
            var partialViewResult = Assert.IsType<PartialViewResult>(result);
            Assert.Equal("_GuestBookEntries", partialViewResult.ViewName);
            var model = Assert.IsAssignableFrom<IEnumerable<Message>>(partialViewResult.Model);
            Assert.Single(model);
        }

        private void SetupSessionValue(string key, string value)
        {
            _sessionMock.Setup(s => s.TryGetValue(key, out It.Ref<byte[]>.IsAny))
                        .Callback(new TryGetValueCallback((string k, out byte[] val) =>
                        {
                            val = Encoding.UTF8.GetBytes(value);
                        }));

            _sessionMock.Setup(s => s.Set(key, It.IsAny<byte[]>()));
        }

        private delegate void TryGetValueCallback(string key, out byte[] value);
    }
}
