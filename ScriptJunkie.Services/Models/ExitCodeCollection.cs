using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace ScriptJunkie.Services
{
    [Serializable]
    [XmlRoot(ElementName = "ExitCodes")]
    public class ExitCodeCollection : Collection<ExitCode>
    {
        /// <summary>
        /// Tries to get an ExitCode that has the exit code value.
        /// </summary>
        /// <param name="exitCode"></param>
        /// <param name="exit"></param>
        /// <returns></returns>
        public bool TryGetExitCode(int exitCode, out ExitCode exit)
        {
            exit = null;

            if(base.Items.Any(i => i.Value == exitCode))
            {
                exit = base.Items.SingleOrDefault(i => i.Value == exitCode);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
