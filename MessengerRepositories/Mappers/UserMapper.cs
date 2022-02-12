using System.Collections.Generic;
using System.Linq;
using MessengerRepositories.Dtos;
using MessengerRepositories.Models;

namespace MessengerRepositories.Mappers
{
    public static class UserMapper
    {
        public static IEnumerable<UserDataModel?> MapUsersToDtos(IEnumerable<User> users)
        {
            return users.Select(MapUserToDto);
        }
        
        public static UserDataModel? MapUserToDto(User? user)
        {
            if (user == null)
                return null;
            
            return new UserDataModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Password = user.Password,
                    State = user.State
                };
        }

        public static User MapDtoToUser(UserDataModel dto, User user)
        {
            user.Name = dto.Name;
            user.Password = dto.Password;
            user.State = dto.State;
            return user;
        }
    }
}