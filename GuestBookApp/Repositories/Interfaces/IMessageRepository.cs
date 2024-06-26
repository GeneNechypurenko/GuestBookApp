using GuestBookApp.Models;

namespace GuestBookApp.Repositories.Interfaces
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> GetMessagesAsync();
        Task AddMessageAsync(Message message);
    }
}