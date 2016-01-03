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
    public class ArgumentCollection : Collection<Argument>
    {

    }
}
