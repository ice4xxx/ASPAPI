using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DI;
using Newtonsoft.Json;
using WPFASP.Entities;

namespace WPFASP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        /// <summary>
        /// Data for users.
        /// </summary>
        private readonly IJsonUserData _userData = ApplicationViewModel.Container.GetInstance<IJsonUserData>();

        /// <summary>
        /// Data for messages.
        /// </summary>
        private readonly IJsonMessageData _messageData = ApplicationViewModel.Container.GetInstance<IJsonMessageData>();

        /// <summary>
        /// Returns Message by sender and receiver id.
        /// </summary>
        /// <param name="senderId">Sender id of the message.</param>
        /// <param name="receiverId">Receiver id of the message.</param>
        /// <response code="200">Returns messages.</response>
        /// <response code="500">If the server cannot access to bd.</response>   
        /// <response code="400">If the senderId or the receiverId are null or bad.</response>   
        /// <response code="404">If users with such senderId or receiverId are not found.</response>   
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("getBySRId")]
        public ActionResult<IEnumerable<IMessage>> Get(int? senderId,int? receiverId)
        {
            if (senderId == null || receiverId == null)
            {
                return BadRequest();
            }

            try
            {
                _userData.GetUserById(senderId.Value);
                _userData.GetUserById(receiverId.Value);

            }
            catch (IOException)
            {
                return StatusCode(500);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }


            return _messageData.GetMessagesBySRId(senderId.Value, receiverId.Value);
        }

        /// <summary>
        /// Returns messages by sender id.
        /// </summary>
        /// <param name="senderId">Sender id of the message.</param>
        /// <response code="200">Returns messages.</response>
        /// <response code="500">If the server cannot access to bd.</response>   
        /// <response code="400">If the senderId are null or bad.</response>   
        /// <response code="404">If user with such senderId is not found.</response>   
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("getBySenderId")]
        public ActionResult<IEnumerable<IMessage>> Get(int? senderId)
        {
            if (senderId == null)
            {
                return BadRequest();
            }

            try
            {
                _userData.GetUserById(senderId.Value);

            }
            catch (IOException)
            {
                return StatusCode(500);
            }
            catch (InvalidOperationException)
            {

                return NotFound();
            }

            return _messageData.GetMessagesBySenderId(senderId.Value);
        }



        /// <summary>
        /// Returns messages by receiver id.
        /// </summary>
        /// <param name="receiverId">Receiver id of the message.</param>
        /// <response code="200">Returns messages.</response>
        /// <response code="500">If the server cannot access to bd.</response>   
        /// <response code="400">If the receiverId are null or bad.</response>   
        /// <response code="404">If user with such receiverId is not found.</response>   
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("getByReceiverId")]
        public ActionResult<IEnumerable<IMessage>> GetByReceiver(int? receiverId)
        {
            if (receiverId == null)
            {
                return BadRequest();
            }

            try
            {
                _userData.GetUserById(receiverId.Value);

            }
            catch (IOException)
            {
                return StatusCode(500);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }

            return _messageData.GetMessagesByReceiverId(receiverId.Value);
        }


        /// <summary>
        /// Sends message.
        /// </summary>
        /// <param name="message">Message to send.</param>
        /// <response code="200">Sends message.</response>
        /// <response code="500">If the server cannot access to bd.</response>   
        /// <response code="400">If the text of message is null or white spaces..</response>   
        /// <response code="404">If users with such senderId or receiverId are not found.</response>   
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("send")]
        [HttpPost]
        public ActionResult<IMessage> Post(MessageEntity message)
        {
            if (string.IsNullOrWhiteSpace(message.MessageText))
            {
                return BadRequest();
            }

            try
            {
                _userData.GetUserById(message.ReceiverId);
                _userData.GetUserById(message.SenderId);
                _messageData.AddMessage(message);
            }
            catch (IOException)
            {
                return StatusCode(500);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }

            return Ok(message);
        }
    }
}
