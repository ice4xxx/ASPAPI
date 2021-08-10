using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WPFASP.Pages
{
    public class MessageModel : PageModel
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
        /// List of users in the system.
        /// </summary>
        public SelectList Users { get; set; }

        /// <summary>
        /// Sender id of the message.
        /// </summary>
        [BindProperty]
        public int? SenderId { get; set; }

        /// <summary>
        /// Receiver id of the message.
        /// </summary>
        [BindProperty]
        public int? ReceiverId { get; set; }

        /// <summary>
        /// Text of the message.
        /// </summary>
        [BindProperty] public string Message { get; set; } = "";

        /// <summary>
        /// Loads users in the system.
        /// </summary>
        public void OnGet()
        {
            Users = new SelectList(_userData.GetAllUsers(), nameof(IUser.Id),nameof(IUser.Email));
        }

        
        /// <summary>
        /// Sends message.
        /// </summary>
        public void OnPost()
        {
            try
            {
                if (!SenderId.HasValue || !ReceiverId.HasValue || string.IsNullOrWhiteSpace(Message))
                {
                    throw new ArgumentException();
                }

                _userData.GetUserById(ReceiverId.Value);
                _userData.GetUserById(SenderId.Value);
            }
            catch (InvalidOperationException)
            {
                Response.ContentType = "text/html";
                Response.StatusCode = 400;

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                Response.WriteAsync(
                    "<div align=\"center\"> <p>Пользователь с таким Id не найден</p><a href=\"/Message\">Назад<a/><div/>",
                    Encoding.GetEncoding(1251));

                return;
            }
            catch (ArgumentException)
            {
                Response.ContentType = "text/html";
                Response.StatusCode = 400;

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                Response.WriteAsync(
                    "<div align=\"center\"> <p>Некорректный запрос</p><a href=\"/Message\">Назад<a/><div/>",
                    Encoding.GetEncoding(1251));

                return;
            }

            IMessage message = ApplicationViewModel.Container.GetInstance<IMessage>();

            message.SenderId = SenderId.Value;
            message.ReceiverId = ReceiverId.Value;
            message.MessageText = Message;

            _messageData.AddMessage(message);
            Response.StatusCode = 200;

            Response.Redirect("/Message");
        }
    }
}
