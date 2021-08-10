using System;
using System.Collections.Generic;
using System.Text;
using DI;

namespace WPFASP.Entities
{
    /// <summary>
    /// Message entity for post methods.
    /// </summary>
    public class MessageEntity : IMessage
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
