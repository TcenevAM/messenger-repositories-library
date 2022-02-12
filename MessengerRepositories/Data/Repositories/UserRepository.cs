using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessengerRepositories.Dtos;
using MessengerRepositories.Mappers;
using MessengerRepositories.Models;

namespace MessengerRepositories.Data.Repositories
{
    public class UserRepository
    {
        private Context _context;

        public async Task<UserDataModel?> GetUserById(int userId)
        {
            await using (_context = new Context())
            {
                var user = await _context.Users.FindAsync(userId);
                var dto = UserMapper.MapUserToDto(user);
                return dto;
            }
        }

        public UserDataModel? GetUserByName(string username)
        {
            using (_context = new Context())
            {
                var user = _context.Users.FirstOrDefault(u => u.Name.ToLower() == username.ToLower());
                var dto = UserMapper.MapUserToDto(user);
                return dto;
            }
        }

        public IEnumerable<UserDataModel?> SearchUserByName(string query)
        {
            using (_context = new Context())
            {
                var users = _context.Users.Where(u => u.Name.ToLower().Contains(query.ToLower()));
                var dto = UserMapper.MapUsersToDtos(users);
                return dto;
            }
        }

        public async Task<int> AddUser(UserDataModel dto)
        {
            await using (_context = new Context())
            {
                var user = UserMapper.MapDtoToUser(dto, new User());
                await _context.AddAsync(user);
                await _context.SaveChangesAsync();
                return user.Id;
            }
        }

        public async Task UpdateUser(UserDataModel dto)
        {
            await using (_context = new Context())
            {
                var user = await _context.Users.FindAsync(dto.Id);
                UserMapper.MapDtoToUser(dto, user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateUserState(int userId, UserStates state)
        {
            await using (_context = new Context())
            {
                var user = await _context.Users.FindAsync(userId);
                user.State = state;
                await _context.SaveChangesAsync();
            }
        }
    }
}