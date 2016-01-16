using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptJunkie.Services;

namespace ScriptJunkie.Test
{
    [TestClass]
    public class DownloadTests : BaseTest
    {


        /// <summary>
        /// Checks if a file can download.
        /// </summary>
        [TestMethod]
        public void DownloadTest()
        {
            Download download = new Download();

            // If this download url stops working jut find another.
            download.DownloadUrl = "http://mirror.internode.on.net/pub/test/10meg.test3";
            download.DestinationPath = Utilities.TestPath("TestFile.test");

            Exception ex;
            bool passed;
            if (download.TryDownloadFile(out ex, 60, 10))
            {
                passed = true;
            }
            else
            {
                passed = false;
            }

            Assert.IsTrue(passed, ex != null ? ex.Message : "");
        }


        /// <summary>
        /// Tries to unpack zip and then checks if file exists.
        /// </summary>
        [TestMethod]
        public void UnpackZipTest()
        {
            Download download = new Download();

            // If this download url stops working jut find another.
            download.DownloadUrl = "http://files.1337upload.net/PowershellTestZip-94f532.zip";
            download.DestinationPath = Utilities.TestPath("PowershellTestZip.zip");
            download.ExtractionPath = Utilities.TestPath("UnzippedArchives");

            Exception ex;
            ScriptResult result = null;
            if (download.TryDownloadFile(out ex, 60, 10))
            {
                Script script = new Script();
                script.Name = "Zipped Powershell";
                script.Description = "Tries to download a zip file extracts it then runs it.";
                script.Executable = new Executable() { Path = Utilities.TestPath("UnzippedArchives/PowershellTestZip.ps1") };

                script.ExitCodes = new ExitCodeCollection();
                script.ExitCodes.Add(new ExitCode() { Value = 2, IsSuccess = true, Message = "Hit right exit code." });
                script.ExitCodes.Add(new ExitCode() { Value = 0, IsSuccess = false, Message = "Wrong Code!" });

                result = script.Execute();
            }
            else
            {
                Assert.Fail("Failed to download file.");
            }

            if (result != null)
            {
                if (result.IsSuccess)
                {

                }
            }
            else
            {
                Assert.Fail("A result is required");
            }
        }



    }
}
