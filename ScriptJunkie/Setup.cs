using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ScriptJunkie.Common;
using ScriptJunkie.Services;

namespace ScriptJunkie
{
    [XmlRoot(ElementName = "Setup")]
    public class Setup
    {

        #region Public Properties
        /// <summary>
        /// Collection of all downloads to be downloaded.
        /// </summary>
        public DownloadCollection Downloads;

        /// <summary>
        /// Collection of all scripts to be ran.
        /// </summary>
        public ScriptCollection Scripts;
        #endregion

        #region Constructors
        /// <summary>
        /// c'tor
        /// </summary>
        public Setup() { }
        #endregion

        #region Public Methods
        /// <summary>
        /// Initialize setup with the xml file.
        /// </summary>
        /// <param name="xmlFile">The xml file to load</param>
        /// <returns>Pass or Fail</returns>
        public bool Initalize(string xmlFile)
        {
            if (!File.Exists(xmlFile))
            {
                ServiceManager.Services.LogService.WriteLine("\"{0}\" does not exist.", ConsoleColor.Red, xmlFile);
                return false;
            }

            FileInfo info = new FileInfo(xmlFile);
            if(!string.Equals(info.Extension, ".xml", StringComparison.OrdinalIgnoreCase))
            {
                ServiceManager.Services.LogService.WriteLine("\"{0}\" doesn't seem to be an xml file.", ConsoleColor.Red, xmlFile);
                return false;
            }

            // Initialize Data.
            Setup result = this.Deserialize(xmlFile);
            this.Downloads = result.Downloads;
            this.Scripts = result.Scripts;

            // Write Details.
            ServiceManager.Services.LogService.WriteSubHeader("Details");
            ServiceManager.Services.LogService.WriteLine("\"{0}\" File(s) to download.", Downloads == null ? 0 : Downloads.Count);
            ServiceManager.Services.LogService.WriteLine("\"{0}\" Script(s) to execute.", Scripts.Count);

            return true;
        }

        /// <summary>
        /// Initializes setup from a base setup object.
        /// </summary>
        /// <returns></returns>
        public bool Initalize()
        {
            // Write Details.
            ServiceManager.Services.LogService.WriteSubHeader("Details");
            ServiceManager.Services.LogService.WriteLine("\"{0}\" File(s) to download.", Downloads.Count);
            ServiceManager.Services.LogService.WriteLine("\"{0}\" Script(s) to execute.", Scripts.Count);

            return true;
        }

        /// <summary>
        /// Run Setup.
        /// </summary>
        /// <returns>Exit Code</returns>
        public int Execute()
        {
            // Download all files.
            ServiceManager.Services.LogService.WriteSubHeader("Downloading Files");
            if(Downloads != null)
            {
                Downloads.DownloadAllFiles();
                if (Downloads.Count > 0)
                {
                    ServiceManager.Services.LogService.WriteSubHeader("All Downloads Complete");
                }
                else
                {
                    ServiceManager.Services.LogService.WriteLine("No downloads required.");
                }
            }
            else
            {
                ServiceManager.Services.LogService.WriteLine("No downloads required.");
            }

            // Execute all scripts.
            ServiceManager.Services.LogService.WriteHeader("Executing Scripts");
            if(Scripts != null)
            {
                if (Scripts.Count > 0)
                {
                    Scripts.Execute();
                    ServiceManager.Services.LogService.WriteSubHeader("All scripts complete");
                }
                else
                {
                    ServiceManager.Services.LogService.WriteLine("No scripts found.");
                }
            }
            else
            {
                ServiceManager.Services.LogService.WriteLine("No scripts found.");
            }

            // Check script junkie exit code.
            ServiceManager.Services.LogService.WriteHeader("Determining Script Junkie Exit Code");
            if(this.Scripts.Any(i => !i.Results.IsSuccess))
            {
                ServiceManager.Services.LogService.WriteLine("Exit 1");
                return 1;
            }

            ServiceManager.Services.LogService.WriteLine("Exit 0");
            return 0;
        }

        /// <summary>
        /// Deserializes XML into setup object.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Setup Deserialize(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Setup));
            Setup collection;

            using (FileStream reader = new FileStream(path, FileMode.Open))
            {
                collection = serializer.Deserialize(reader) as Setup;
            }

            return collection;
        }

        /// <summary>
        /// Serializes setup into XML.
        /// </summary>
        /// <param name="path"></param>
        public void Serialize(string path)
        {
            FileInfo pathInfo = new FileInfo(path);
            if (!Directory.Exists(pathInfo.DirectoryName))
            {
                Directory.CreateDirectory(pathInfo.DirectoryName);
            }
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(path))
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(this.GetType());
                serializer.Serialize(writer, this);
            }
        }


        /// <summary>
        /// Generates a template xml to base custom scripts off of.
        /// </summary>
        /// <param name="savePath">The location the template will be saved.</param>
        public void GenerateXmlTemplate(string savePath)
        {
            ServiceManager.Services.LogService.WriteHeader("Generating Xml Template...");

            #region Script 1
            Script script = new Script();
            script.Name = "Script 1";
            script.Description = "Does nothing";

            Executable executable = new Executable();
            executable.Path = "C:/Temp/nothing.ps1";

            ArgumentCollection argCollection = new ArgumentCollection();
            argCollection.Add(new Argument() { Key = "-i", Value = "C:/Temp/something.bin" });
            argCollection.Add(new Argument() { Key = "-x", Value = "" });

            ExitCodeCollection exitCollection = new ExitCodeCollection();
            exitCollection.Add(new ExitCode() { Value = 0, Message = "Files deleted", IsSuccess = true });
            exitCollection.Add(new ExitCode() { Value = 1, Message = "Files failed to delete" });
            exitCollection.Add(new ExitCode() { Value = 2, Message = "Couldn't find any files" });

            // Add all elements into the single script file.
            script.Arguments = argCollection;
            script.ExitCodes = exitCollection;
            script.Executable = executable;
            #endregion

            #region Script 2
            Script script2 = new Script();
            script2.Name = "Script 2";
            script2.Description = "Does nothing";

            Executable executable2 = new Executable();
            executable2.Path = "C:/Temp/Downloads/Extracted/SomethingThatWasInAZip.exe";

            ArgumentCollection argCollection2 = new ArgumentCollection();
            argCollection2.Add(new Argument() { Key = "-install"});
            argCollection2.Add(new Argument() { Key = "-silent" });

            ExitCodeCollection exitCollection2 = new ExitCodeCollection();
            exitCollection2.Add(new ExitCode() { Value = 0, Message = "Script 2 has installed", IsSuccess = true });
            exitCollection2.Add(new ExitCode() { Value = 1, Message = "Failed to install." });
            exitCollection2.Add(new ExitCode() { Value = 2, Message = "Installed but limited." });

            // Add all elements into the single script file.
            script2.Arguments = argCollection;
            script2.ExitCodes = exitCollection;
            script2.Executable = executable;

            // Add the single script above into a collection of scripts.
            ScriptCollection scriptCollection = new ScriptCollection();
            scriptCollection.Add(script);
            scriptCollection.Add(script2);
            #endregion

            ServiceManager.Services.LogService.WriteLine("Generating Download Collection...");
            DownloadCollection downloadCollection = new DownloadCollection();
            downloadCollection.TimeOut = 60;
            downloadCollection.RefreshRate = 10;
            downloadCollection.Add(new Download() { Name = "Nothing Powershell Script", Description = "This script does nothing", DownloadUrl = "www.blank.com/nothing.ps1", DestinationPath = "C:/Temp/Downloads/nothing.ps1" });
            downloadCollection.Add(new Download() { Name = "Test Zip File", Description = "This zip has nothing", DownloadUrl = "www.blank.com/nothing.zip", DestinationPath = "C:/Temp/Downloads", ExtractionPath = "C:/Temp/Downloads/Extracted" });

            // Add the 2 main elements, the scripts to run and the downloads to download.
            Setup setup = new Setup();
            setup.Scripts = scriptCollection;
            setup.Downloads = downloadCollection;

            ServiceManager.Services.LogService.WriteLine("Saving file to: \"{0}\"", savePath);
            setup.Serialize(savePath);
        }
        #endregion
    }
}
