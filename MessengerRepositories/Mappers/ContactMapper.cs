using System;
using MessengerRepositories.Dtos;

namespace MessengerRepositories.Mappers
{
    public static class ContactMapper
    {
        public static ContactDataModel MapEntityToDto(int userId, int contactId, DateTime lastUpdateTime)
        {
            return new ContactDataModel
            {
                ContactId = contactId,
                UserId = userId,
                LastUpdateTime = lastUpdateTime
            };
        }
    }
}