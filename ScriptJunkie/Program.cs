using ScriptJunkie.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ScriptJunkie.Services;

namespace ScriptJunkie
{
    class Program
    {

        static void Main(string[] args)
        {
            // Set Title.
            Console.Title = "Script Junkie";

            // Add Services.
            ServiceManager.Services.Add(new LogService());
            ServiceManager.Services.Add(new ArgumentService());

            // Debug Arguments
            // Arguments passed through args variable over power below arguments.
#if (DEBUG)
            // Example of programatically added arguments.
            ServiceManager.Services.ArgumentService.AddArgument(ArgumentService.XmlPath, @"C:\Users\Lucas\Desktop\TestScripts\Setup.xml");
            //ServiceManager.Services.ArgumentService.AddArgument(ArgumentService.XmlTemplatePath, @"C:\Users\Lucas\Desktop\TestScripts\Template.xml");
#endif

            // Start Script Junkie.
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            ServiceManager.Services.LogService.WriteHeader("Starting Script Junkie v({0})", version.ToString());
            ServiceManager.Services.LogService.WriteLine("If executable of script needs to be ran as administrator, run ScriptJunkie as admin.", ConsoleColor.Yellow);
            // Check if debug mode.
            if (ServiceManager.Services.ArgumentService.IsDebug)
            {
                ServiceManager.Services.LogService.WriteSubHeader("Paused for debug mode. Attach to debugger then press any key to continue.");
                Console.ReadKey(true);
            }

            // Initialization.
            Setup setup = new Setup();
            int exitCode = -1;

            // Display Arguments
            ServiceManager.Services.LogService.WriteSubHeader("Arguments");
            foreach(Argument arg in ServiceManager.Services.ArgumentService.Arguments)
            {
                if (string.IsNullOrEmpty(arg.Value))
                {
                    ServiceManager.Services.LogService.WriteLine("\"{0}\" = \"True\"", arg.Key, arg.Value);
                }
                else
                {
                    ServiceManager.Services.LogService.WriteLine("\"{0}\" = \"{1}\"", arg.Key, arg.Value);
                }
            }

            // Checks if it should generate an example xml template.
            Argument xmlTemplatePath;
            if (ServiceManager.Services.ArgumentService.TryGetArgument(ArgumentService.XmlTemplatePath, out xmlTemplatePath))
            {
                setup.GenerateXmlTemplate(xmlTemplatePath.Value);
            }

            // Checks if it should run setup execution.
            Argument xmlPath;
            if (ServiceManager.Services.ArgumentService.TryGetArgument(ArgumentService.XmlPath, out xmlPath))
            {
                if (setup.Initalize(xmlPath.Value))
                {
                    exitCode = setup.Execute();
                }
            }
            else
            {
                ServiceManager.Services.LogService.WriteSubHeader("Script Junkie needs an xml file to execute.", ConsoleColor.Red);
            }

            // End Script Junkie.
            ServiceManager.Services.LogService.WriteHeader("Script Junkie has completed.");
#if (RELEASE)
            Environment.Exit(exitCode);
#elif (DEBUG)
            Console.ReadLine();
#endif
        }
    }
}
