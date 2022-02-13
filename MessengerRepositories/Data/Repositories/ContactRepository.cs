using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessengerRepositories.Dtos;
using MessengerRepositories.Helpers;
using MessengerRepositories.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MessengerRepositories.Data.Repositories
{
    public class ContactRepository
    {
        private Context _context;
        private ContactHelper _contactHelper = new ContactHelper();

        public async Task<ContactDataModel?> GetUserContact(int userId, int contactId)
        {
            await using (_context = new Context())
            {
                var lastUpdateTime = await _contactHelper.GetChatLastConversationDate(userId, contactId);
                return ContactMapper.MapEntityToDto(userId, contactId, lastUpdateTime);
            }
        }

        public async Task<IEnumerable<ContactDataModel?>> GetUserContacts(int userId)
        {
            await using (_context = new Context())
            {
                var result = new List<ContactDataModel?>();
                var user = await _context.Users.Include(u => u.Chats).ThenInclude(c => c.Members)
                    .FirstOrDefaultAsync(u => u.Id == userId);
                
                foreach (var contact in user.Contacts.Where(c => c.Id != userId))
                {
                    var lastUpdateTime = await _contactHelper.GetChatLastConversationDate(userId, contact.Id);
                    result.Add(ContactMapper.MapEntityToDto(userId, contact.Id, lastUpdateTime));
                }
                return result;
            }
        }

        public async Task<ContactDataModel?> SearchUserContacts(int userId, string query)
        {
            await using (_context = new Context())
            {
                var user = await _context.Users.FindAsync(userId);
                var target = user.Contacts.FirstOrDefault(c => c.Name.ToLower().Contains(query.ToLower()));
                if (target == null || user == null) return null;
                
                var lastUpdateTime = await _contactHelper.GetChatLastConversationDate(userId, target.Id);
                return ContactMapper.MapEntityToDto(userId, target.Id, lastUpdateTime);
            }
        }

        public async Task AddContact(ContactDataModel dto)
        {
            await using (_context = new Context())
            {
                var user = await _context.Users.FindAsync(dto.UserId);
                var contact = await _context.Users.FindAsync(dto.ContactId);
                _contactHelper.CreateContact(user, contact, dto.LastUpdateTime);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateContact(ContactDataModel dto)
        {
            await using (_context = new Context())
            {
                await _contactHelper.UpdateLastUpdateTime(dto.UserId, dto.ContactId, dto.LastUpdateTime);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteContact(int userId, int contactId)
        {
            await using (_context = new Context())
            {
                var user = await _context.Users.Include(u => u.Contacts).FirstOrDefaultAsync(u => u.Id == userId);
                var contact = await _context.Users.Include(u => u.Contacts).FirstOrDefaultAsync(u => u.Id == contactId);
                user.Contacts.Remove(contact);
                contact.Contacts.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}