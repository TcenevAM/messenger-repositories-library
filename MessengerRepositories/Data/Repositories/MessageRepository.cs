using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessengerRepositories.Dtos;
using MessengerRepositories.Helpers;
using Microsoft.EntityFrameworkCore;

namespace MessengerRepositories.Data.Repositories
{
    public class MessageRepository
    {
        private Context _context;
        private readonly MessageHelper _messageHelper = new MessageHelper();
        private readonly ContactHelper _contactHelper = new ContactHelper();

        public async Task<IEnumerable<MessageDataModel>?> GetUserMessages(int userId)
        {
            await using (_context = new Context())
            {
                var user = await _context.Users
                    .Include(u => u.Chats)
                    .ThenInclude(c => c.Messages)
                    .Include(m => m.Chats)
                    .ThenInclude(c => c.Members)
                    .FirstOrDefaultAsync(u => u.Id == userId);
                
                return _messageHelper.GetUserMessages(user);
            }
        }

        public async Task<MessageDataModel?> SearchUserMessages(int userId, int contactId, string query)
        {
            await using (_context = new Context())
            {
                var chat = await _contactHelper.GetChatByUsersIds(userId, contactId, _context);
                var message = await _context.Entry(chat).Collection(c => c.Messages).Query()
                    .FirstOrDefaultAsync(m => m.Content.ToLower().Contains(query.ToLower()));

                return _messageHelper.CreateDto(contactId, message);
            }
        }

        public async Task AddMessage(MessageDataModel dto)
        {
            await using (_context = new Context())
            {
                var user = await _context.Users.FindAsync(dto.UserId);
                var chat = await _contactHelper.GetChatByUsersIds(dto.UserId, dto.ContactId, _context);

                //chat.Members = null;
                chat.Messages.Add(_messageHelper.MapDtoToMessage(dto, user));
                _context.Update(chat);
                await _context.SaveChangesAsync();
            }
        }
    }
}