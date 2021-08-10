using System;
using BLL;
using Data;
using DI;
using SimpleInjector;

namespace IoC
{
    public class IoC
    {
        
        /// <summary>
        /// IoC container.
        /// </summary>
        public Container Container { get; set; }


        /// <summary>
        /// ctor.
        /// </summary>
        public IoC()
        {
            Container = new Container();

            Setup();
        }

        /// <summary>
        /// Setups dependencies.
        /// </summary>
        private void Setup()
        {
            Container.Register<IUser, User>(Lifestyle.Transient);
            Container.Register<IMessage, Message>(Lifestyle.Transient);
            Container.Register<IJsonUserData, JsonUserData>(Lifestyle.Singleton);
            Container.Register<IJsonMessageData, JsonMessageData>(Lifestyle.Singleton);
        }
    }
}
