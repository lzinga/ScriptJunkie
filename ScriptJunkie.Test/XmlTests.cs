using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ScriptJunkie.Test
{
    [TestClass]
    public class XmlTests : BaseTest
    {
        /// <summary>
        /// Expects to get 2 scripts.
        /// </summary>
        [TestMethod]
        public void XmlScriptCountTest()
        {
            Setup setup = new Setup();
            setup.Initalize(Utilities.GetIncludedFile("SetupXmls/DownloadAndScriptCount.xml"));
            Assert.IsTrue(setup.Scripts.Count == 2);
        }

        /// <summary>
        /// Expects to get 2 downloads.
        /// </summary>
        [TestMethod]
        public void XmlDownloadCountTest()
        {
            Setup setup = new Setup();
            setup.Initalize(Utilities.GetIncludedFile("SetupXmls/DownloadAndScriptCount.xml"));
            Assert.IsTrue(setup.Downloads.Count == 2);
        }
    }
}
