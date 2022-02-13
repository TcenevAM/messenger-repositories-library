using System.Collections.Generic;
using System.Linq;
using MessengerRepositories.Dtos;
using MessengerRepositories.Models;

namespace MessengerRepositories.Helpers
{
    public class MessageHelper
    {
        public IEnumerable<MessageDataModel>? GetUserMessages(User? user)
        {
            if (user == null) return null;
            var messages = new List<MessageDataModel>();
            foreach (var chat in user.Chats)
            {
                foreach (var message in chat.Messages)
                {
                    var contact = chat.Members.First(u => u.Id != user.Id);
                    messages.Add(new MessageDataModel()
                    {
                        Content = message.Content, ContactId = contact.Id, UserId = user.Id,
                        DeliveryTime = message.DeliveryDateTime, SendTime = message.SendDateTime
                    });
                }
            }

            return messages;
        }

        public MessageDataModel? CreateDto(int contactId, Message message)
        {
            return new MessageDataModel
            {
                Content = message.Content,
                ContactId = contactId,
                DeliveryTime = message.DeliveryDateTime,
                SendTime = message.SendDateTime,
                UserId = message.Sender.Id
            };
        }

        public Message MapDtoToMessage(MessageDataModel dto, User sender)
        {
            return new Message
            {
                Content = dto.Content,
                DeliveryDateTime = dto.DeliveryTime,
                SendDateTime = dto.SendTime.ToUniversalTime(),
                Sender = sender
            };
        }
    }
}