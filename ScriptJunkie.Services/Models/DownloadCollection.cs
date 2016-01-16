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
using System.ComponentModel;

namespace ScriptJunkie.Services
{
    [Serializable]
    [XmlRoot("Downloads")]
    [XmlInclude(typeof(Download))]
    public class DownloadCollection : IEnumerable<Download>
    {
        private List<Download> _downloads = new List<Download>();

        private int _timeout = 60;
        private int _refreshRate = 10;

        [XmlAttribute(AttributeName = "TimeOut")]
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

        [XmlAttribute(AttributeName = "RefreshRate")]
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

        public int Count
        {
            get
            {
                return this._downloads.Count;
            }
        }

        public void DownloadAllFiles()
        {
            ServiceManager.Services.LogService.WriteLine("Downloads will time out in \"{0}\" seconds.", _timeout);
            ServiceManager.Services.LogService.WriteLine("Downloads will update every \"{0}\" seconds.", _refreshRate);

            foreach (Download download in this._downloads)
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

        public void Add(Download download)
        {
            this._downloads.Add(download);
        }

        /// <summary>
        /// Gets all downloads that have timed out.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Download> DownloadsTimedOut()
        {
            return this._downloads.Where(i => i.TimedOut == true);
        }

        public IEnumerator<Download> GetEnumerator()
        {
            return this._downloads.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._downloads.GetEnumerator();
        }
    }
}
