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

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.GetUserByUsernameAsync(model.Username);
                if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                {
                    HttpContext.Session.SetString("Username", user.Username);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid username or password.");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register() => View();

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
                    return RedirectToAction("Login");
                }

                ModelState.AddModelError("", "Username already exists.");
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
