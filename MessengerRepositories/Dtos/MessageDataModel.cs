using System;

namespace MessengerRepositories.Dtos
{
    public class MessageDataModel
    {
        public int UserId { get; set; }
        public int ContactId { get; set; }
        public DateTime SendTime { get; set; }
        public DateTime DeliveryTime { get; set; } = DateTime.UtcNow;
        public string Content { get; set; }
    }
}