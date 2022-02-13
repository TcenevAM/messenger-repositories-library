using System;
using System.Collections.Generic;

namespace MessengerRepositories.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public List<User> Members { get; set; } = new List<User>();
        public DateTime LastUpdateTime { get; set; }
        public List<Message> Messages { get; set; } = new List<Message>();
    }
}