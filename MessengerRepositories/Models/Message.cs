using System;

namespace MessengerRepositories.Models
{
    public class Message
    {
        public int Id { get; set; }
        public DateTime SendDateTime { get; set; }
        public DateTime DeliveryDateTime { get; set; }
        public string Content { get; set; }
    }
}