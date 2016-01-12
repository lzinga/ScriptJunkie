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
    public class ScriptCollection : Collection<Script>, IXmlSerializable
    {
        private int _timeout = 60;
        private int _refreshRate = 10;

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

        /// <summary>
        /// Executes all scripts in the collection.
        /// </summary>
        public void Execute()
        {
            foreach(Script script in base.Items)
            {
                // Run the script and get the results.
                ScriptResult result = script.Execute();
                
                ExitCode code;
                if (script.ExitCodes.TryGetExitCode(result.ExitCode, out code))
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

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            base.Clear();

            int timeout = 0;
            if (int.TryParse(reader["TimeOut"], out timeout))
            {
                _timeout = timeout;
            }

            int refreshRate = 0;
            if (int.TryParse(reader["RefreshRate"], out timeout))
            {
                _refreshRate = refreshRate;
            }

            reader.MoveToContent();
            XPathNavigator n = new XPathDocument(reader.ReadSubtree()).CreateNavigator();
            XPathNodeIterator nodes = n.Select("//Script");
            XmlSerializer x = new XmlSerializer(typeof(Script));

            while (nodes.MoveNext())
            {
                using (TextReader stream = new StringReader(nodes.Current.OuterXml))
                {
                    object o = x.Deserialize(stream);
                    if (o is Script) base.Add((Script)o);
                }
            }
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            // All script TimeOut and RefreshRate
            writer.WriteAttributeString("TimeOut", _timeout.ToString());
            writer.WriteAttributeString("RefreshRate", _refreshRate.ToString());

            foreach (Script script in base.Items)
            {
                writer.WriteStartElement("Script");
                {
                    // Script Attributes
                    writer.WriteAttributeString("Name", script.Name);
                    writer.WriteAttributeString("Description", script.Description);

                    // Script Executable
                    writer.WriteStartElement("Executable");
                    {
                        writer.WriteAttributeString("Path", script.Executable.Path);
                    }
                    writer.WriteEndElement();

                    // Script Arguments
                    writer.WriteStartElement("Arguments");
                    {
                        foreach (Argument arg in script.Arguments)
                        {
                            writer.WriteStartElement("Argument");
                            {
                                writer.WriteAttributeString("Key", arg.Key);
                                writer.WriteAttributeString("Value", arg.Value);
                            }
                            writer.WriteEndElement();
                        }
                    }
                    writer.WriteEndElement();

                    // Script ExitCodes
                    writer.WriteStartElement("ExitCodes");
                    {
                        foreach (ExitCode exit in script.ExitCodes)
                        {
                            writer.WriteStartElement("ExitCode");
                            {
                                writer.WriteAttributeString("Value", exit.Value.ToString());
                                writer.WriteAttributeString("Message", exit.Message);
                                writer.WriteAttributeString("IsSuccess", exit.IsSuccess.ToString().ToLower());
                            }
                            writer.WriteEndElement();
                        }
                    }
                    writer.WriteEndElement();


                }
                writer.WriteEndElement();
            }
        }
    }
}
