using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptJunkie.Services;

namespace ScriptJunkie.Test
{
    [TestClass]
    public class GenerateXml
    {
        [TestMethod]
        public void GenerateExampleXml()
        {
            Script script = new Script();
            script.Name = "Blah Script";
            script.Description = "This script is a test generation.";

            Executable executable = new Executable();
            executable.Path = "C:/Temp/update.exe";

            ArgumentCollection argCollection = new ArgumentCollection();
            argCollection.Add(new Argument() { Key = "-i", Value = "C:/Temp/something.bin" });
            argCollection.Add(new Argument() { Key = "-x", Value = "" });

            ExitCodeCollection exitCollection = new ExitCodeCollection();
            exitCollection.Add(new ExitCode() { Value = 1, Message = "Message One" });
            exitCollection.Add(new ExitCode() { Value = 2, Message = "Message Two" });
            exitCollection.Add(new ExitCode() { Value = 3, Message = "Message Three" });

            // Add all elements into the single script file.
            script.Arguments = argCollection;
            script.ExitCodes = exitCollection;
            script.Executable = executable;

            // Add the single script above into a collection of scripts.
            ScriptCollection scriptCollection = new ScriptCollection();
            scriptCollection.Add(script);
            
            DownloadCollection downloadCollection = new DownloadCollection();
            downloadCollection.Add(new Download() { Name = "Steam", Description = "Steam Games", DownloadUrl = "www.steampowered.com", DestinationPath = "C:/Temp/Downloads" });

            // Add the 2 main elements, the scripts to run and the downloads to download.
            Setup setup = new Setup();
            setup.Scripts = scriptCollection;
            setup.Downloads = downloadCollection;

            // Serialize it into the xml.
            setup.Serialize(@"C:\Temp\Object.xml");
        }
    }
}
