using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleInjector;

namespace WPFASP
{
    public class ApplicationViewModel
    {
        /// <summary>
        /// IoC container.
        /// </summary>
        public static Container Container = new IoC.IoC().Container;
    }
}
