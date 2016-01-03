using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using ScriptJunkie.Services;

namespace ScriptJunkie.Services
{
    public class DownloadCollection : Collection<Download>
    {
        public void DownloadAllFiles()
        {
            foreach (Download download in base.Items)
            {
                ServiceManager.Services.LogService.WriteSubHeader("Name: \"{0}\"", download.Name);
                Exception ex;

                // TODO: Put Timeout time and the Recheck time into an xml option for downloads list as attribute.
                // I have tried multiple things and a property Timeout will not be serialized into the XML from inside this class.
                if (!download.TryDownloadFile(out ex, 20, 10))
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

    }
}
