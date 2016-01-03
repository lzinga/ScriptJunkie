using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScriptJunkie.Common;

namespace ScriptJunkie.Services
{
    public class ArgumentService : BaseService
    {
        public const string XmlPath = "XmlPath";
        public const string XmlTemplatePath = "XmlTemplatePath";
        public const string DebugMode = "Debug";
        public const string Help = "?";

        private string[] _args;

        public ArgumentCollection Arguments = new ArgumentCollection();

        public ArgumentService()
        {
            _args = Environment.GetCommandLineArgs();
            this.ParseArguments();
        }

        public bool IsDebug
        {
            get
            {
                Argument arg;
                if(this.TryGetArgument("Debug", out arg))
                {
                    return true;
                }

                return false;
            }
        }

        public void AddArgument(Argument argument)
        {
            Argument exists = this.Arguments.SingleOrDefault(i => string.Equals(i.Key, argument.Key, StringComparison.OrdinalIgnoreCase));
            if (exists != null)
            {
                // An argument with that key already exists.
                return;
            }

            this.Arguments.Add(argument);
        }

        public void AddArgument(string key, string value)
        {
            this.Arguments.Add(new Argument() { Key = key, Value = value });
        }

        public void AddArgument(string key)
        {
            this.Arguments.Add(new Argument() { Key = key, Value = "" });
        }

        private void ParseArguments()
        {
            if(_args.Length > 1)
            {
                for(int i = 1; i < _args.Length; i++)
                {
                    string str = _args[i];

                    Argument arg = new Argument();
                    if (str.Contains("="))
                    {
                        arg.Key = str.Split('=')[0].Replace("/", "").Replace("\\", "").Trim();
                        arg.Value = str.Split('=')[1].Trim();
                        Arguments.Add(arg);
                    }
                    else
                    {
                        arg.Key = str.Replace("/", "").Replace("\\", "").Trim();
                        arg.Value = string.Empty;
                        Arguments.Add(arg);
                    }

                }
            }
        }

        public bool TryGetArgument(string key, out Argument arg)
        {
            arg = null;
            if (this.Arguments.Any(i => string.Equals(i.Key, key, StringComparison.OrdinalIgnoreCase)))
            {
                arg = this.Arguments.SingleOrDefault(i => string.Equals(i.Key, key, StringComparison.OrdinalIgnoreCase));
                return true;
            }

            return false;
        }

    }
}
