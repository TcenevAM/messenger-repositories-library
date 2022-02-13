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
                var chat = await _contactHelper.GetChatByUsersIds(userId, contactId, _context);
                return ContactMapper.MapEntityToDto(userId, contactId, chat.LastUpdateTime);
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
                    var chat = await _contactHelper.GetChatByUsersIds(userId, contact.Id, _context);
                    result.Add(ContactMapper.MapEntityToDto(userId, contact.Id, chat.LastUpdateTime));
                }
                return result;
            }
        }

        public async Task<ContactDataModel?> SearchUserContacts(int userId, string query)
        {
            await using (_context = new Context())
            {
                var user = await _context.Users.Include(u => u.Contacts).FirstOrDefaultAsync(u => u.Id == userId);
                var target = user.Contacts.FirstOrDefault(c => c.Name.ToLower().Contains(query.ToLower()));
                if (target == null || user == null) return null;
                
                var chat = await _contactHelper.GetChatByUsersIds(userId, target.Id, _context);
                return ContactMapper.MapEntityToDto(userId, target.Id, chat.LastUpdateTime);
            }
        }

        public async Task AddContact(ContactDataModel dto)
        {
            await using (_context = new Context())
            {
                var user = await _context.Users.FindAsync(dto.UserId);
                var contact = await _context.Users.FindAsync(dto.ContactId);
                _contactHelper.CreateContact(user, contact, dto.LastUpdateTime);
                await _context.Chats.AddAsync(user.Chats.Last());
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateContact(ContactDataModel dto)
        {
            await using (_context = new Context())
            {
                await _contactHelper.UpdateLastUpdateTime(dto.UserId, dto.ContactId, dto.LastUpdateTime, _context);
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