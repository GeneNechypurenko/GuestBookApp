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

		[HttpPost]
		public async Task<IActionResult> AddMessage(MessageViewModel model)
		{
			if (ModelState.IsValid)
			{
				var message = new Message
				{
					Text = model.Text,
					Username = HttpContext.Session.GetString("Username"),
					PostedAt = DateTime.Now
				};

				await _messageRepository.AddMessageAsync(message);
				return Ok();
			}

			return BadRequest(ModelState);
		}

		[HttpGet]
		public async Task<IActionResult> PartialGuestBookEntries()
		{
			var messages = await _messageRepository.GetMessagesAsync();
			return PartialView("_GuestBookEntries", messages);
		}
	}
}
