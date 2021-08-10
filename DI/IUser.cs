using System;

namespace DI
{
    public interface IUser
    {
        /// <summary>
        /// Id of the user.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Email of the user.
        /// </summary>
        string Email { get; set; }

        /// <summary>
        /// Name of the user.
        /// </summary>
        string UserName { get; set; }

        string ToString()
        {
            return Email;
        }
    }
}
