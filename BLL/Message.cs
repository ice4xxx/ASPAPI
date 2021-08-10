using System;
using System.Collections.Generic;
using System.Text;
using DI;

namespace BLL
{
    public class Message : IMessage
    {
        /// <summary>
        /// Subject of the message.
        /// </summary>
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
