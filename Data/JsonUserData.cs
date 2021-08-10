using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Data.Entities;
using DI;
using Newtonsoft.Json;

namespace Data
{
    public class JsonUserData : IJsonUserData
    {

        /// <summary>
        /// Saves changes.
        /// </summary>
        /// <param name="users">List of user to save.</param>
        public void SaveChanges(List<IUser> users)
        {
            Directory.CreateDirectory("Data");

            using StreamWriter sw = new StreamWriter("Data/Users.json");
            sw.WriteLine(JsonConvert.SerializeObject(users.OrderBy(i => i.Email)));
        }


        /// <summary>
        /// Return user by his id.
        /// </summary>
        /// <param name="id">Id of the user.</param>
        /// <returns></returns>
        public IUser GetUserById(int id)
        {

            using StreamReader sr = new StreamReader("Data/Users.json");
            IUser response = (JsonConvert.DeserializeObject<IEnumerable<UserEntity>>(sr.ReadLine() ?? string.Empty) ?? Array.Empty<UserEntity>())
                .Single(entity => entity.Id == id);

            return response;
        }

        /// <summary>
        /// Adds new user.
        /// Throws argument exception if user with such email already registred.
        /// </summary>
        /// <param name="user">User to add.</param>
        public void AddUser(IUser user)
        {
            var users = GetAllUsers();

            if (users.Count(i => i.Email == user.Email) != 0) throw new ArgumentException();

            user.Id = users.Count;
            users.Add(user);
            SaveChanges(users);
        }

        /// <summary>
        /// Returns all users in the system.
        /// </summary>
        /// <returns></returns>
        public List<IUser> GetAllUsers()
        {
            using StreamReader sr = new StreamReader("Data/Users.json");
            List<IUser> response = JsonConvert.DeserializeObject<List<UserEntity>>(sr.ReadLine()).Select(i => i as IUser).ToList();

            return response;
        }
    }
}
