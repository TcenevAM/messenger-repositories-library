using System;
using System.Collections.Generic;

namespace MessengerRepositories.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public User Contact { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public List<Message> Messages { get; set; } = new List<Message>();
    }
}