using GuestBookApp.Data;
using GuestBookApp.Models;
using GuestBookApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GuestBookApp.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync()
        {
            return await _context.Messages.OrderByDescending(m => m.PostedAt).ToListAsync();
        }

        public async Task AddMessageAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }
    }
}