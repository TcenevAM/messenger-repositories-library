using System.Collections.Generic;

namespace MessengerRepositories.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public List<Chat> Chats { get; set; } = new List<Chat>();
        public List<User> Contacts { get; set; } = new List<User>();
        public UserStates State { get; set; }
    }
}