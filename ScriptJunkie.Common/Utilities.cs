using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptJunkie.Common
{
    public static class Utilities
    {
        /// <summary>
        /// Helps throw an exception.
        /// </summary>
        /// <typeparam name="T">The type of exception to throw.</typeparam>
        /// <param name="str"></param>
        /// <param name="args"></param>
        /// <returns>Exception</returns>
        public static Exception Throw<T>(string str, params object[] args) where T: Exception
        {
            return (T)Activator.CreateInstance(typeof(T), string.Format(str, args));
        }

    }
}
