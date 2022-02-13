using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessengerRepositories.Data;
using MessengerRepositories.Models;
using Microsoft.EntityFrameworkCore;

namespace MessengerRepositories.Helpers
{
    public class ContactHelper
    {
        private Context _context;
        
        public async Task<DateTime> GetChatLastConversationDate(int userId, int contactId)
        {
            await using (_context = new Context())
            {
                var chat = await _context.Chats.FirstOrDefaultAsync(c =>
                    c.Members.Exists(m => m.Id == userId) && c.Members.Exists(m => m.Id == contactId));
                return chat.LastUpdateTime;
            }
        }

        public async Task UpdateLastUpdateTime(int userId, int contactId, DateTime update)
        {
            await using (_context = new Context())
            {
                var chat = await _context.Chats.FirstOrDefaultAsync(c =>
                    c.Members.Exists(m => m.Id == userId) && c.Members.Exists(m => m.Id == contactId));
                chat.LastUpdateTime = update;
            }
        }

        public void CreateContact(User user1, User user2, DateTime lastUpdateTime)
        {
            user1.Contacts.Add(user2);
            user1.Chats.Add(new Chat { Members = new List<User> {user1, user2}, LastUpdateTime = lastUpdateTime});
        }
    }
}