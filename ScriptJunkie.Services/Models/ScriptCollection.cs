using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using ScriptJunkie.Services;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using System.Xml.XPath;

namespace ScriptJunkie.Services
{
    [Serializable]
    public class ScriptCollection : IEnumerable<Script>
    {
        private int _timeout = 60;
        private int _refreshRate = 10;

        [XmlAttribute]
        public int TimeOut
        {
            get
            {
                return _timeout;
            }
            set
            {
                _timeout = value;
            }
        }

        [XmlAttribute]
        public int RefreshRate
        {
            get
            {
                return _refreshRate;
            }
            set
            {
                _refreshRate = value;
            }
        }

        [XmlIgnore]
        private List<Script> _scripts = new List<Script>();

        [XmlIgnore]
        public int Count
        {
            get
            {
                return _scripts.Count;
            }
        }

        public Script this[int index]
        {
            get
            {
                return _scripts[index];
            }
        }

        /// <summary>
        /// Executes all scripts in the collection.
        /// </summary>
        public void Execute()
        {
            foreach(Script script in this._scripts)
            {
                // Run the script and get the results.
                ScriptResult result = script.Execute();

                // Only check exit code if the exit code has a value.
                if (result.ExitCode.HasValue)
                {
                    ExitCode code;
                    if (script.ExitCodes.TryGetExitCode(result.ExitCode.Value, out code))
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

        public void Add(Script script)
        {
            _scripts.Add(script);
        }

        public IEnumerator<Script> GetEnumerator()
        {
            return _scripts.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _scripts.GetEnumerator();
        }
    }
}
