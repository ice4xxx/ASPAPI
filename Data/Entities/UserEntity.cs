using System;
using System.Collections.Generic;
using System.Text;
using DI;

namespace Data.Entities
{
    class UserEntity : IUser
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
