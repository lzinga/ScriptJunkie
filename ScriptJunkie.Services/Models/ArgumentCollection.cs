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
    [XmlRoot(ElementName = "Arguments")]
    public class ArgumentCollection : IEnumerable<Argument>
    {
        private List<Argument> _arguments = new List<Argument>();

        public int Count
        {
            get
            {
                return this._arguments.Count;
            }
        }

        public IEnumerator<Argument> GetEnumerator()
        {
            return this._arguments.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._arguments.GetEnumerator();
        }

        public void Add(Argument argument)
        {
            this._arguments.Add(argument);
        }

        public void Remove(Argument argument)
        {
            this._arguments.Remove(argument);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            if (this._arguments != null)
            {
                foreach (Argument arg in this._arguments)
                {
                    builder.Append(" ");
                    builder.Append(arg.ToString());
                }
            }

            return builder.ToString();
        }
    }
}
