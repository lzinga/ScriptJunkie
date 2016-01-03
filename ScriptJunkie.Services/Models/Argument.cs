using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ScriptJunkie.Services
{
    public class Argument : IEquatable<Argument>
    {
        [XmlAttribute]
        public string Key { get; set; }

        [XmlAttribute]
        public string Value { get; set; }

        public string BuildArgument()
        {
            return string.Format("{0} '{1}'", this.Key, this.Value);
        }

        public bool Equals(Argument other)
        {
            if(other == null)
            {
                return false;
            }

            return (this.Key.Equals(other.Key));
        }
    }
}
