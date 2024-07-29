using GuestBookApp.Models.ViewModels;
using GuestBookApp.Models;
using GuestBookApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GuestBookApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            HttpContext.Session.SetString("Username", model.Username);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userRepository.GetUserByUsernameAsync(model.Username);
                if (existingUser == null)
                {
                    var user = new User
                    {
                        Username = model.Username,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)
                    };

                    await _userRepository.AddUserAsync(user);
                    return Ok();
                }

                ModelState.AddModelError("", "Username already exists.");
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Ok();
        }
    }
}
