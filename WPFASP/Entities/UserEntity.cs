using System;
using System.Collections.Generic;
using System.Text;
using DI;

namespace WPFASP.Entities
{
    /// <summary>
    /// User entity for post methods.
    /// </summary>
    public class UserEntity : IUser
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
