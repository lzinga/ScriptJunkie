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
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            if (base.Items != null)
            {
                foreach (Argument arg in base.Items)
                {
                    builder.Append(" ");
                    builder.Append(arg.ToString());
                }
            }

            return builder.ToString();
        }
    }
}
