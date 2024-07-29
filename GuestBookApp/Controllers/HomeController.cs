using GuestBookApp.Models;
using GuestBookApp.Models.ViewModels;
using GuestBookApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GuestBookApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMessageRepository _messageRepository;

        public HomeController(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var messages = await _messageRepository.GetMessagesAsync();
            return View(messages);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages()
        {
            var messages = await _messageRepository.GetMessagesAsync();
            return PartialView("_GuestBookEntries", messages);
        }

        [HttpPost]
        public async Task<IActionResult> AddMessage(MessageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var username = HttpContext.Session.GetString("Username");
                if (!string.IsNullOrEmpty(username))
                {
                    var message = new Message
                    {
                        Username = username,
                        Text = model.Text,
                        PostedAt = DateTime.Now
                    };

                    await _messageRepository.AddMessageAsync(message);
                    var messages = await _messageRepository.GetMessagesAsync();
                    return PartialView("_GuestBookEntries", messages);
                }
            }

            return BadRequest(ModelState);
        }
    }
}
