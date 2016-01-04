using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptJunkie.Services;
using ScriptJunkie.Common;

namespace ScriptJunkie.Test
{
    [TestClass]
    public class SetupTests
    {
        [TestMethod]
        public void TestDownload()
        {
            Download download = new Download();
            
            // If this download url stops working jut find another.
            download.DownloadUrl = "http://mirror.internode.on.net/pub/test/10meg.test3";
            download.DestinationPath = System.IO.Path.GetTempPath() + "/TestFile.test";

            Exception ex;
            bool passed;
            if(download.TryDownloadFile(out ex, 60, 10))
            {
                passed = true;
            }
            else
            {
                passed = false;
            }

            download.DestinationPath = System.IO.Path.GetTempPath() + "/TestFile.test";
            download.DeleteDownload();
            Assert.IsTrue(passed, ex != null ? ex.Message : "");
        }


    }
}
