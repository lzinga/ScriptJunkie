using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptJunkie.Services;
using ScriptJunkie.Common;

namespace ScriptJunkie.Test
{
    [TestClass]
    public class SetupTests
    {
        private const string PowershellTest_ScriptFail = "PowershellTest_ScriptFail.ps1";

        private string GetScriptObject(string script)
        {
            return string.Format("{0}\\Scripts\\{1}", AppDomain.CurrentDomain.BaseDirectory, script);
        }

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            // Add logging service.
            ServiceManager.Services.Add(new LogService());
        }

        [TestMethod]
        public void DownloadTest()
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

        /// <summary>
        /// Tests if the script result passes.
        /// </summary>
        [TestMethod]
        public void PowershellScript_ResultPass()
        {

            Script script = new Script();
            script.Name = "Powershell test script.";

            // The powershell script.
            script.Executable = new Executable() { Path = GetScriptObject("PowershellTest_ScriptPass.ps1") };

            // Exit Codes
            ExitCodeCollection exitCollection = new ExitCodeCollection();
            exitCollection.Add(new ExitCode() { Value = 0, Message = "Success", IsSuccess = true });
            exitCollection.Add(new ExitCode() { Value = 1, Message = "Failure" });

            // Add all elements into the single script file.
            script.ExitCodes = exitCollection;

            // Add the single script above into a collection of scripts.
            ScriptCollection scriptCollection = new ScriptCollection();
            scriptCollection.Add(script);

            // No Downloads.
            DownloadCollection downloadCollection = new DownloadCollection();

            // Add the 2 main elements, the scripts to run and the downloads to download.
            Setup setup = new Setup();
            setup.Scripts = scriptCollection;
            setup.Downloads = downloadCollection;

            int exitCode = -1;
            if (setup.Initalize())
            {
                exitCode = setup.Execute();
            }

            // The exit code should be 0, since the script matched IsSuccess ExitCode.
            Assert.IsTrue(exitCode == 0);
        }

        /// <summary>
        /// Tests if the script result passes.
        /// </summary>
        [TestMethod]
        public void PowershellScript_ResultFail()
        {
            Script script = new Script();
            script.Name = "Powershell test script.";

            // The powershell script.
            script.Executable = new Executable() { Path = GetScriptObject("PowershellTest_ScriptFail.ps1") };

            // Exit Codes
            ExitCodeCollection exitCollection = new ExitCodeCollection();
            exitCollection.Add(new ExitCode() { Value = 0, Message = "Success", IsSuccess = true });
            exitCollection.Add(new ExitCode() { Value = 1, Message = "Failure" });

            // Add all elements into the single script file.
            script.ExitCodes = exitCollection;

            // Add the single script above into a collection of scripts.
            ScriptCollection scriptCollection = new ScriptCollection();
            scriptCollection.Add(script);

            // No Downloads.
            DownloadCollection downloadCollection = new DownloadCollection();

            // Add the 2 main elements, the scripts to run and the downloads to download.
            Setup setup = new Setup();
            setup.Scripts = scriptCollection;
            setup.Downloads = downloadCollection;


            int exitCode = -1;
            if (setup.Initalize())
            {
                exitCode = setup.Execute();
            }

            // The exit code should be 1, since the script did not match IsSuccess ExitCode.
            Assert.IsTrue(exitCode == 1);
        }


        /// <summary>
        /// One script passes and one fails.
        /// Setup exit code should be 1 ( error ).
        /// </summary>
        [TestMethod]
        public void PowershellScript_Both()
        {
            #region Script 1
            Script script1 = new Script();
            script1.Name = "Powershell test script 1.";
            script1.Executable = new Executable() { Path = GetScriptObject("PowershellTest_ScriptPass.ps1") };

            // Exit Codes
            ExitCodeCollection exitCollection1 = new ExitCodeCollection();
            exitCollection1.Add(new ExitCode() { Value = 0, Message = "Success", IsSuccess = true });
            exitCollection1.Add(new ExitCode() { Value = 1, Message = "Failure" });

            // Add all elements into the script1
            script1.ExitCodes = exitCollection1;
            #endregion

            #region Script 2
            Script script2 = new Script();
            script2.Name = "Powershell test script 2.";
            script2.Executable = new Executable() { Path = GetScriptObject("PowershellTest_ScriptFail.ps1") };

            // Exit Codes
            ExitCodeCollection exitCollection2 = new ExitCodeCollection();
            exitCollection2.Add(new ExitCode() { Value = 0, Message = "Success", IsSuccess = true });
            exitCollection2.Add(new ExitCode() { Value = 1, Message = "Failure" });

            // Add all elements into the script1
            script2.ExitCodes = exitCollection2;
            #endregion

            // Add the single script above into a collection of scripts.
            ScriptCollection scriptCollection = new ScriptCollection();
            scriptCollection.Add(script1);
            scriptCollection.Add(script2);

            // No Downloads.
            DownloadCollection downloadCollection = new DownloadCollection();

            // Add the 2 main elements, the scripts to run and the downloads to download.
            Setup setup = new Setup();
            setup.Scripts = scriptCollection;
            setup.Downloads = downloadCollection;


            int exitCode = -1;
            if (setup.Initalize())
            {
                exitCode = setup.Execute();
            }

            // Exit code should be 1 as 1 of the 2 scripts didn't have the expected result.
            Assert.IsTrue(exitCode == 1);
        }

    }
}
