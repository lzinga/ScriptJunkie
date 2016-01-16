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
    public class ExitCodeCollection : IEnumerable<ExitCode>
    {
        private List<ExitCode> _exitCodes = new List<ExitCode>();

        /// <summary>
        /// Tries to get an ExitCode that has the exit code value.
        /// </summary>
        /// <param name="exitCode"></param>
        /// <param name="exit"></param>
        /// <returns></returns>
        public bool TryGetExitCode(int exitCode, out ExitCode exit)
        {
            exit = null;

            if (this._exitCodes.Any(i => i.Value == exitCode))
            {
                exit = this._exitCodes.SingleOrDefault(i => i.Value == exitCode);
                return true;
            }
            else
            {
                return false;
            }
        }


        public void Add(ExitCode exitCode)
        {
            _exitCodes.Add(exitCode);
        }

        public IEnumerator<ExitCode> GetEnumerator()
        {
            return this._exitCodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._exitCodes.GetEnumerator();
        }
    }
}
