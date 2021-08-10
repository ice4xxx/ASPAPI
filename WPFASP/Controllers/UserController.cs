using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using DI;
using Newtonsoft.Json;
using WPFASP.Entities;

namespace WPFASP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
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
        /// Random generator.
        /// </summary>
        private readonly Random _random = new Random();

        /// <summary>
        /// Sentences to generate random messages.
        /// </summary>
        private string[] _sentences;


        /// <summary>
        /// Returns Users.
        /// </summary>
        /// <param name="offset">The index number of user,starting from which need to get information.</param>
        /// <param name="limit">Count of users to return.</param>
        /// <response code="200">Returns users</response>
        /// <response code="500">If the server cannot access to bd.</response>   
        /// <response code="400">If the offset or the limit is null or bad.</response>   
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("getAll")]
        public ActionResult<IEnumerable<IUser>> Get(int? offset, int? limit)
        {
            var users = new List<IUser>();

            try
            {
                users = _userData.GetAllUsers();

                if (offset != null && limit != null)
                {
                    if (offset.Value < 0 || limit.Value <= 0)
                    {
                        return BadRequest();
                    }

                    return Ok(users.Skip(offset.Value).Take(limit.Value));
                }

                if (offset != null)
                {
                    if (offset.Value < 0)
                    {
                        return BadRequest();
                    }

                    return Ok(users.Skip(offset.Value));
                }

                if (limit != null)
                {
                    if (limit <= 0)
                    {
                        return BadRequest();
                    }

                    return Ok(users.Take(limit.Value));
                }

                return Ok(_userData.GetAllUsers());
            }
            catch (IOException)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Returns user by id.
        /// </summary>
        /// <param name="id">The id of the user which need to return.</param>
        /// <response code="200">Return user</response>
        /// <response code="500">If the server cannot access to bd.</response>   
        /// <response code="400">If the id is null.</response>   
        /// <response code="400">User with such id have not found.</response>   
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("getById")]
        public ActionResult<IUser> Get(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new ArgumentException();
                }

                return Ok(_userData.GetUserById(id.Value));
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (IOException)
            {
                return StatusCode(500);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }


        /// <summary>
        /// Generates 10 users.
        /// </summary>
        /// <response code="200">Generates user.</response>   
        /// <response code="500">If the server cannot access to bd.</response>   
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("generate")]
        [HttpPost]
        public ActionResult Post()
        {
            List<IMessage> messages = new List<IMessage>();
            List<IUser> users = new List<IUser>();

            try
            {
                for (int i = 0; i < 10; i++)
                {
                    IUser user = ApplicationViewModel.Container.GetInstance<IUser>();
                    user.UserName = GetUserName();
                    user.Email = $"{user.UserName}{_random.Next(50000)}@gmail.com";
                    user.Id = i;

                    users.Add(user);
                }

                foreach (var user in users)
                {
                    messages.AddRange(GenerateMessagesToUser(user, users));
                }

                ApplicationViewModel.Container.GetInstance<IJsonUserData>().SaveChanges(users);
                ApplicationViewModel.Container.GetInstance<IJsonMessageData>().SaveChanges(messages);
            }
            catch 
            {
                return StatusCode(500);
            }

            return Ok();
        }

        /// <summary>
        /// Register a user in the system.
        /// (user id sets automatically.it's unique).
        /// </summary>
        /// <param name="user">User to register (set any id. it doesn't matter).</param>
        /// <response code="200">Generates user.</response>   
        /// <response code="500">If the server cannot access to bd.</response>   
        /// <response code="400">If user is null or user.email already registered.</response>   
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("register")]
        [HttpPost]
        public ActionResult<IUser> Post(UserEntity user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            try
            {
                _userData.AddUser(user);
            }
            catch (IOException)
            {
                return StatusCode(500);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }

            return Ok(user);
        }

        /// <summary>
        /// Returns real user name.
        /// </summary>
        /// <returns></returns>
        private string GetUserName()
        {
            string[] names;
            names = System.IO.File.ReadAllLines("Data/Names.txt");

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
                for (int i = 0; i < count; i++)
                {
                    IMessage message = ApplicationViewModel.Container.GetInstance<IMessage>();

                    message.Subject = "Война и мир";
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
