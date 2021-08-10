using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization.Internal;
using SimpleInjector;

namespace WPFASP.Pages
{
    public class IndexModel : PageModel
    {

        /// <summary>
        /// List of users in the system.
        /// </summary>
        public List<IUser> Users { get; set; } = new List<IUser>();


        /// <summary>
        /// Random generator.
        /// </summary>
        private readonly Random _random = new Random();

        /// <summary>
        /// Sentences for messages.
        /// </summary>
        private string[] _sentences;

        /// <summary>
        /// Gets users from bd.
        /// </summary>
        public void OnGet()
        {
            Users = ApplicationViewModel.Container.GetInstance<IJsonUserData>().GetAllUsers();
        }


        /// <summary>
        /// Generates users.
        /// </summary>
        public void OnPost()
        {

            List<IMessage> messages = new List<IMessage>();

            for (int i = 0; i < 10; i++)
            {
                IUser user = ApplicationViewModel.Container.GetInstance<IUser>();
                user.UserName = GetUserName();
                user.Email = $"{user.UserName}{_random.Next(50000)}@gmail.com";
                user.Id = i;

                Users.Add(user);
            }

            foreach (var user in Users)
            {
                messages.AddRange(GenerateMessagesToUser(user,Users));
            }

            ApplicationViewModel.Container.GetInstance<IJsonUserData>().SaveChanges(Users);
            ApplicationViewModel.Container.GetInstance<IJsonMessageData>().SaveChanges(messages);

            Response.StatusCode = 200;
        }


        /// <summary>
        /// Returns real user name.
        /// </summary>
        /// <returns></returns>
        private string GetUserName()
        {
            string[] names;
            try
            {
                 names = System.IO.File.ReadAllLines("Data/Names.txt");
            }
            catch
            {
                Response.StatusCode = 523;
                throw;
            }

            return names[_random.Next(names.Length)];
        }

        /// <summary>
        /// Returns lit with messages for certain user.
        /// </summary>
        /// <param name="user">User-sender.</param>
        /// <param name="users">All users in the system.</param>
        /// <returns></returns>
        private List<IMessage> GenerateMessagesToUser(IUser user, List<IUser> users)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (_sentences == null)
            {
                _sentences = System.IO.File.ReadAllLines("Data/Sentences.txt", Encoding.GetEncoding(1251)).Select(i => i.Replace("\r\n", ""))
                    .Where(i => i != "" && i.Length > 3).ToArray();
            }

            List<IMessage> messages = new List<IMessage>();

            foreach (var receiver in users)
            {
                int count = _random.Next(1, 5);
                for (int i = 0; i < count;i++)
                {
                    IMessage message = ApplicationViewModel.Container.GetInstance<IMessage>();

                    message.SenderId = user.Id;
                    message.ReceiverId = receiver.Id;
                    message.MessageText = _sentences[_random.Next(_sentences.Length)];

                    messages.Add(message);
                }
            }

            return messages;
        }
    }
}
