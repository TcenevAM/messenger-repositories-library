using System;

namespace MessengerRepositories.Dtos
{
    public class ContactDataModel
    {
        public int UserId { get; set; }
        public int ContactId { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}