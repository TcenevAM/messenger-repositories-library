using MessengerRepositories.Models;
using Microsoft.EntityFrameworkCore;

namespace MessengerRepositories.Data
{
    public class Context : DbContext
    {
        public DbSet<User> UserDataModels { get; set; }
        public DbSet<Message> MessageDataModels { get; set; }
        public DbSet<Chat> ContactDataModels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=KaptoiiikaPC;Database=Messanger;Trusted_Connection=True");
        }
    }
}