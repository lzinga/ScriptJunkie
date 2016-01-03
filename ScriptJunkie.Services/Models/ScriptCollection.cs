using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Collections.ObjectModel;
using ScriptJunkie.Services;

namespace ScriptJunkie.Services
{
    [Serializable]
    [XmlRoot(ElementName = "Scripts")]
    public class ScriptCollection : Collection<Script>
    {
        /// <summary>
        /// Executes all scripts in the collection.
        /// </summary>
        public void Execute()
        {
            foreach(Script script in base.Items)
            {
                // Run the script and get the results.
                ScriptResult result = script.Execute();
                
                ExitCode code;
                if (script.ExitCodes.TryGetExitCode(result.ExitCode, out code))
                {
                    ServiceManager.Services.LogService.WriteLine("\"{0}\" says \"{1}\".", script.Name, code.Message, result.ExitCode);
                    ServiceManager.Services.LogService.WriteLine("Exit Code: {0} ( {1} )", result.ExitCode, result.IsSuccess ? "Passed" : "Failed");
                }
                else
                {
                    ServiceManager.Services.LogService.WriteLine("\"{0}\" had no exit code in xml.", script.Name);
                }
            }
        }
    }
}
