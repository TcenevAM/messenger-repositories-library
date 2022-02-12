using MessengerRepositories.Models;
using Microsoft.EntityFrameworkCore;

namespace MessengerRepositories.Data
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Chat> Chats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=KaptoiiikaPC;Database=Messanger;Trusted_Connection=True");
        }
    }
}