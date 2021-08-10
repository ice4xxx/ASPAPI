using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Data.Entities;
using DI;
using Newtonsoft.Json;

namespace Data
{
    public class JsonMessageData : IJsonMessageData
    {
        /// <summary>
        /// Saves changes.
        /// </summary>
        /// <param name="messages">Messages to save.</param>
        public void SaveChanges(List<IMessage> messages)
        {
            Directory.CreateDirectory("Data");

            using StreamWriter sw = new StreamWriter("Data/Messages.json");
            sw.WriteLine(JsonConvert.SerializeObject(messages));
        }


        /// <summary>
        /// Adds new message.
        /// </summary>
        /// <param name="message">Message to add.</param>
        public void AddMessage(IMessage message)
        {
            List<IMessage> messages = new List<IMessage>();
            using (StreamReader sr = new StreamReader("Data/Messages.json"))
            {
                messages = JsonConvert.DeserializeObject<List<MessageEntity>>(sr.ReadLine())
                    .Select(i => i as IMessage).ToList();
                messages.Add(message);
            }

            SaveChanges(messages);
        }


        /// <summary>
        /// Returns messages by sender and receiver ids.
        /// </summary>
        /// <param name="senderId">Sender's id.</param>
        /// <param name="receiverId">Receiver's id.</param>
        /// <returns></returns>
        public List<IMessage> GetMessagesBySRId(int senderId, int receiverId)
        {
            using StreamReader sr = new StreamReader("Data/Messages.json");
            List<IMessage> response = JsonConvert.DeserializeObject<IEnumerable<MessageEntity>>(sr.ReadLine())
                .Where(i => i.ReceiverId == receiverId && i.SenderId == senderId).Select(i => i as IMessage).ToList();

            return response;
        }


        /// <summary>
        /// Returns messages by sender id.
        /// </summary>
        /// <param name="senderId">Sender's id.</param>
        /// <returns></returns>
        public List<IMessage> GetMessagesBySenderId(int senderId)
        {
            using StreamReader sr = new StreamReader("Data/Messages.json");
            List<IMessage> response = JsonConvert.DeserializeObject<IEnumerable<MessageEntity>>(sr.ReadLine())
                .Where(i => i.SenderId == senderId).Select(i => i as IMessage).ToList();

            return response;
        }


        /// <summary>
        /// Returns messages by receiver id.
        /// </summary>
        /// <param name="receiverId">REceiver's id.</param>
        /// <returns></returns>
        public List<IMessage> GetMessagesByReceiverId(int receiverId)
        {
            using StreamReader sr = new StreamReader("Data/Messages.json");
            List<IMessage> response = JsonConvert.DeserializeObject<IEnumerable<MessageEntity>>(sr.ReadLine())
                .Where(i => i.ReceiverId == receiverId).Select(i => i as IMessage).ToList();

            return response;
        }
    }
}
