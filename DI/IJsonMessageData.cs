using System;
using System.Collections.Generic;
using System.Text;

namespace DI
{
    public interface IJsonMessageData
    {
        /// <summary>
        /// Saves changes.
        /// </summary>
        /// <param name="messages">Messages to save.</param>
        void SaveChanges(List<IMessage> messages);


        /// <summary>
        /// Adds new message.
        /// </summary>
        /// <param name="message">Message to add.</param>
        void AddMessage(IMessage message);

        /// <summary>
        /// Returns messages by sender and receiver ids.
        /// </summary>
        /// <param name="senderId">Sender's id.</param>
        /// <param name="receiverId">Receiver's id.</param>
        /// <returns></returns>
        List<IMessage> GetMessagesBySRId(int senderId, int receiverId);


        /// <summary>
        /// Returns messages by sender id.
        /// </summary>
        /// <param name="senderId">Sender's id.</param>
        /// <returns></returns>
        List<IMessage> GetMessagesBySenderId(int senderId);

        /// <summary>
        /// Returns messages by receiver id.
        /// </summary>
        /// <param name="receiverId">REceiver's id.</param>
        /// <returns></returns>
        List<IMessage> GetMessagesByReceiverId(int receiverId);
    }
}
