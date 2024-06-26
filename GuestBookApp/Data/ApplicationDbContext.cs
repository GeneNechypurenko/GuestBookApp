using GuestBookApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GuestBookApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    }
}