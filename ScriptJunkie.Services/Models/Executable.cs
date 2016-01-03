using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ScriptJunkie.Services
{
    public class Executable
    {
        [XmlAttribute]
        public string Path { get; set; }
    }
}
