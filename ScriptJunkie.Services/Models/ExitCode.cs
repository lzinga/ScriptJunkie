using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ScriptJunkie.Services
{
    public class ExitCode
    {
        [XmlAttribute]
        public int Value { get; set; }

        [XmlAttribute]
        public string Message { get; set; }

        [XmlAttribute]
        public bool IsSuccess { get; set; }
    }
}
