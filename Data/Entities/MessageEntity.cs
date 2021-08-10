using System;
using System.Collections.Generic;
using System.Text;
using DI;

namespace Data.Entities
{
    /// <summary>
    /// Message entity for deserialize.
    /// </summary>
    class MessageEntity : IMessage
    {
        public string Subject { get; set; }

        /// <summary>
        /// Text of the message.
        /// </summary>
        public string MessageText { get; set; }

        /// <summary>
        /// Id of the receiver.
        /// </summary>
        public int ReceiverId { get; set; }

        /// <summary>
        /// Id of the sender.
        /// </summary>
        public int SenderId { get; set; }
    }
}
