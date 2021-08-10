using System;
using DI;

namespace BLL
{
    public class User : IUser
    {
        /// <summary>
        /// Id of the user.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Email of the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Name of the user.
        /// </summary>
        public string UserName { get; set; }

        public override string ToString()
        {
            return Email;
        }
    }
}
