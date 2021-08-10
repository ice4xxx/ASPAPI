using System;
using System.Collections.Generic;
using System.Text;

namespace DI
{
    public interface IMessage
    {
        string Subject { get; set; }

        /// <summary>
        /// Text of the message.
        /// </summary>
        string MessageText { get; set; }

        /// <summary>
        /// REceiver id of the message.
        /// </summary>
        int ReceiverId { get; set; }

        /// <summary>
        /// Sender id of the message.
        /// </summary>
        int SenderId { get; set; }
    }
}
