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

namespace ScriptJunkie.Services
{
    public class DownloadCollection : Collection<Download>, IXmlSerializable
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

        public void DownloadAllFiles()
        {
            ServiceManager.Services.LogService.WriteLine("Downloads will time out in \"{0}\" seconds.", _timeout);
            ServiceManager.Services.LogService.WriteLine("Downloads will update every \"{0}\" seconds.", _refreshRate);

            foreach (Download download in base.Items)
            {
                ServiceManager.Services.LogService.WriteSubHeader("Name: \"{0}\"", download.Name);
                Exception ex;

                // Download file, based on the Timeout and refresh rate in download attributes
                if (!download.TryDownloadFile(out ex, _timeout, _refreshRate))
                {
                    ServiceManager.Services.LogService.WriteLine("\"{0}\" failed to download ({1}).", ConsoleColor.Red, download.Name, ex.Message);
                }
                else
                {
                    if (download.TimedOut)
                    {
                        ServiceManager.Services.LogService.WriteLine("Skipping \"{0}\", took to long.", ConsoleColor.Red, download.Name);
                    }
                    else
                    {
                        ServiceManager.Services.LogService.WriteLine("\"{0}\" has finished downloading.", download.Name);
                    }
                }
            }
        }


        /// <summary>
        /// Gets all downloads that have timed out.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Download> DownloadsTimedOut()
        {
            return base.Items.Where(i => i.TimedOut == true);
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            base.Clear();
            XmlReader subR = reader.ReadSubtree();

            int timeout = 60;
            if(int.TryParse(reader["TimeOut"], out timeout))
            {
                _timeout = timeout;
            }

            int refreshRate = 10;
            if (int.TryParse(reader["RefreshRate"], out timeout))
            {
                _refreshRate = refreshRate;
            }

            
            reader.MoveToContent();

            while (subR.ReadToFollowing("Download"))
            {
                XmlSerializer x = new XmlSerializer(typeof(Download));
                object o = x.Deserialize(subR);
                if (o is Download) base.Add((Download)o);
            }

        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("TimeOut", _timeout.ToString());
            writer.WriteAttributeString("RefreshRate", _refreshRate.ToString());

            foreach(Download download in base.Items)
            {
                writer.WriteStartElement("Download");
                {
                    writer.WriteAttributeString("Name", download.Name);
                    writer.WriteAttributeString("Description", download.Description);

                    writer.WriteElementString("DownloadUrl", download.DownloadUrl);
                    writer.WriteElementString("DestinationPath", download.DestinationPath);
                }
                writer.WriteEndElement();
            }


        }
    }
}
