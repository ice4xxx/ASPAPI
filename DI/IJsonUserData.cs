using System;
using System.Collections.Generic;
using System.Text;

namespace DI
{
    public interface IJsonUserData
    {
        /// <summary>
        /// Saves changes.
        /// </summary>
        /// <param name="users">Users to save.</param>
        void SaveChanges(List<IUser> users);

        /// <summary>
        /// Returns user by his id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IUser GetUserById(int id);

        /// <summary>
        /// Adds new user.
        /// Throws argument exception if user with such email already registered.
        /// </summary>
        /// <param name="user"></param>
        void AddUser(IUser user);

        /// <summary>
        /// Returns all users in the system.
        /// </summary>
        /// <returns></returns>
        List<IUser> GetAllUsers();
    }
}
