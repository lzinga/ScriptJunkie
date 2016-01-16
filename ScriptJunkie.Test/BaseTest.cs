using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptJunkie.Services;
using System.IO;

namespace ScriptJunkie.Test
{
    [TestClass]
    public class BaseTest
    {
        public BaseTest()
        {
            if(!ServiceManager.Services.ServiceExists<LogService>())
            {
                ServiceManager.Services.Add(new LogService());
            }
        }

        ~BaseTest()
        {
            // Clean up all files for test.
            if (Directory.Exists(Utilities.TestPath()))
            {
                Directory.Delete(Utilities.TestPath(), true);
            }
        }
    }
}
