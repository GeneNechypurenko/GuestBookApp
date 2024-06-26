using GuestBookApp.Models;

namespace GuestBookApp.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task AddUserAsync(User user);
    }
}