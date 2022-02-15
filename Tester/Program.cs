using System;
using System.Threading.Tasks;
using MessengerRepositories;
using MessengerRepositories.Data.Repositories;
using MessengerRepositories.Dtos;

namespace Tester
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //do not forget to change connection string in Data -> Context
            await UserRepositoryTest();
            await ContactRepositoryTest();
            await MessageRepositoryTest();
        }

        private static async Task UserRepositoryTest()
        {
            var userRepository = new UserRepository();
            var user1 = new UserDataModel { Name = "user1", Password = "qwerty" };
            var user2 = new UserDataModel { Name = "user2", Password = "qwerty" };
            var user1Update = new UserDataModel { Id = 1, Name = "user1Update", Password = "qwerty" };
            var addUserResult = await userRepository.AddUser(user1);
            Console.WriteLine($"AddUser method result: {addUserResult}");
            addUserResult = await userRepository.AddUser(user2);
            Console.WriteLine($"AddUser method result: {addUserResult} \r\n");

            var user = await userRepository.GetUserById(1);
            Console.WriteLine($"User1 before update: id: {user.Id}, Name: {user.Name}, Password: {user.Password}, State: {user.State}");
            await userRepository.UpdateUser(user1Update);
            Console.WriteLine($"User1 after update: id: {user.Id}, Name: {user.Name}, Password: {user.Password}, State: {user.State}\r\n");

            await userRepository.UpdateUserState(1, UserStates.Online);
            user = await userRepository.GetUserById(1);
            Console.WriteLine($"User1 after state update to online: id: {user.Id}, Name: {user.Name}, Password: {user.Password}, State: {user.State}");
            await userRepository.UpdateUserState(1, UserStates.Offline);
            user = await userRepository.GetUserById(1);
            Console.WriteLine($"User1 after state update to offline: id: {user.Id}, Name: {user.Name}, Password: {user.Password}, State: {user.State}\r\n");

            user = userRepository.GetUserByName("user2");
            Console.WriteLine($"Invoke GetUserByName with \"user2\": id: {user.Id}, Name: {user.Name}, Password: {user.Password}, State: {user.State}");
            try
            {
                user = userRepository.GetUserByName("user3");
                Console.Write($"Invoke GetUserByName with \"user3\": ");
                Console.WriteLine($"id: {user.Id}, Name: {user.Name}, Password: {user.Password}, State: {user.State}\r\n");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}\r\n");
            }

            var users = userRepository.SearchUserByName("user");
            Console.WriteLine($"Invoke SearchUserByName by name \"user\"");
            foreach (var client in users)
            {
                Console.WriteLine($"id: {client.Id}, Name: {client.Name}, Password: {client.Password}, State: {client.State}");
            }
        }

        private static async Task ContactRepositoryTest()
        {
            var contactRepository = new ContactRepository();
            var contact = new ContactDataModel { ContactId = 2, UserId = 1 };
            var contactUpdate = new ContactDataModel
                { ContactId = 2, UserId = 1, LastUpdateTime = DateTime.Now.AddHours(5) };
            Console.WriteLine("User1 contacts before AddContact\r\n");
            foreach (var contactData in await contactRepository.GetUserContacts(1))
            {
                Console.WriteLine($"UserId: {contactData.UserId} ContactId: {contactData.ContactId}, LastUpdateTime: {contactData.LastUpdateTime}");
            }
            await contactRepository.AddContact(contact);
            contact = await contactRepository.GetUserContact(1, 2);
            Console.WriteLine($"User1 contacts after AddContact UserId: {contact.UserId} ContactId: {contact.ContactId}, LastUpdateTime: {contact.LastUpdateTime}\r\n");
            
            await contactRepository.UpdateContact(contactUpdate);
            Console.Write("User1 contacts after UpdateContact ");
            foreach (var contactData in await contactRepository.GetUserContacts(1))
            {
                Console.Write($"UserId: {contactData.UserId} ContactId: {contactData.ContactId}, LastUpdateTime: {contactData.LastUpdateTime} \r\n");
            }
            
            contact = await contactRepository.SearchUserContacts(1, "user");
            Console.WriteLine($"SearchUserContacts for userId: 1 and query: \"user\": UserId: {contact.UserId} ContactId: {contact.ContactId}, LastUpdateTime: {contact.LastUpdateTime}");
            
            await contactRepository.DeleteContact(1, 2);
            Console.WriteLine("User1 contacts after DeleteContact with contactId 2\r\n");
            foreach (var contactData in await contactRepository.GetUserContacts(1))
            {
                Console.WriteLine($"UserId: {contactData.UserId} ContactId: {contactData.ContactId}, LastUpdateTime: {contactData.LastUpdateTime}");
            }
        }

        private static async Task MessageRepositoryTest()
        {
            var messageRepository = new MessageRepository();
            var messageDto1 = new MessageDataModel
                { Content = "message1", ContactId = 2, SendTime = DateTime.UtcNow.AddMinutes(-1), UserId = 1 };
            var messageDto2 = new MessageDataModel
                { Content = "same message1", ContactId = 1, SendTime = DateTime.UtcNow.AddMinutes(-1), UserId = 2 };
            var messageDto3 = new MessageDataModel
                { Content = "new letter", ContactId = 2, SendTime = DateTime.UtcNow.AddMinutes(-1), UserId = 1 };
            var messageDto4 = new MessageDataModel
                { Content = "brand new message1", ContactId = 2, SendTime = DateTime.UtcNow.AddMinutes(-1), UserId = 1 };
            var messages = await messageRepository.GetUserMessages(1);
            Console.WriteLine("GetUserMessages with userId 1 before message added");
            foreach (var message in messages)
            {
                Console.WriteLine($"Content: {message.Content} contactId: {message.ContactId} deliveryTime: {message.DeliveryTime} sendTime: {message.SendTime} userId: {message.UserId}");
            }
            Console.WriteLine("\r\n\r\n");
            
            await messageRepository.AddMessage(messageDto1);
            await messageRepository.AddMessage(messageDto2);
            await messageRepository.AddMessage(messageDto3);
            await messageRepository.AddMessage(messageDto4);
            
            messages = await messageRepository.GetUserMessages(1);
            Console.WriteLine("GetUserMessages with userId 1 after messages added");
            foreach (var message in messages)
            {
                Console.WriteLine($"Content: {message.Content} contactId: {message.ContactId} deliveryTime: {message.DeliveryTime} sendTime: {message.SendTime} userId: {message.UserId}");
            }
            Console.WriteLine("\r\n\r\n");
            
            messages = await messageRepository.GetUserMessages(2);
            Console.WriteLine("GetUserMessages with userId 2");
            foreach (var message in messages)
            {
                Console.WriteLine($"Content: {message.Content} contactId: {message.ContactId} deliveryTime: {message.DeliveryTime} sendTime: {message.SendTime} userId: {message.UserId}");
            }
            Console.WriteLine("\r\n\r\n");

            var userMessage = await messageRepository.SearchUserMessages(1, 2, "message1");
            Console.WriteLine($"SearchUserMessages with query \"message1\" Content: {userMessage.Content} contactId: {userMessage.ContactId} deliveryTime: {userMessage.DeliveryTime} sendTime: {userMessage.SendTime} userId: {userMessage.UserId}");
        }
    }
}