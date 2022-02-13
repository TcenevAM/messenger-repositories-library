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
        public async Task<Chat?> GetChatByUsersIds(int userId, int contactId, Context context)
        {
            var chats = context.Chats.Include(c => c.Members).AsQueryable();
            foreach (var chat in chats)
            {
                if (chat.Members.Any(m => m.Id == userId) && chat.Members.Any(m => m.Id == contactId))
                    return chat;
            }

            return null;
        }

        public async Task UpdateLastUpdateTime(int userId, int contactId, DateTime update, Context context)
        {
            await using (context = new Context())
            {
                var chat = await GetChatByUsersIds(userId, contactId, context);
                chat.LastUpdateTime = update;
            }
        }

        public void CreateContact(User user1, User user2, DateTime lastUpdateTime)
        {
            user1.Contacts.Add(user2);
            user2.Contacts.Add(user1);
            user1.Chats.Add(new Chat { Members = new List<User> {user1, user2}, LastUpdateTime = lastUpdateTime});
        }
    }
}