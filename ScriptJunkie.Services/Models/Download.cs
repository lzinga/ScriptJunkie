using ScriptJunkie.Common;
using ScriptJunkie.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ScriptJunkie.Services
{
    public class Download
    {
        #region Private Fields
        private bool _isDownloading;
        private bool _timedOut;
        private bool _timeOutCalled;
        private Exception _exception;
        private WebClient _client;

        private int _progressPercent;
        private long _bytesReceived;
        private long _totalBytesToReceive;
        #endregion

        #region Public Properties
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Description { get; set; }

        /// <summary>
        /// The url to the download file.
        /// </summary>
        public string DownloadUrl { get; set; }

        /// <summary>
        /// The path the file will be downloaded to.
        /// </summary>
        public string DestinationPath { get; set; }

        /// <summary>
        /// If the file is currently in the state of downloading.
        /// </summary>
        [XmlIgnore]
        public bool IsDownloading
        {
            get { return _isDownloading; }
        }

        /// <summary>
        /// If the file has timed out in downloading.
        /// </summary>
        [XmlIgnore]
        public bool TimedOut
        {
            get { return _timedOut; }
        }
        #endregion

        #region Public Events
        public delegate void DownloadComplete(System.ComponentModel.AsyncCompletedEventArgs e);
        public event DownloadComplete DownloadCompleted;

        public delegate void DownloadProgressChange(DownloadProgressChangedEventArgs e);
        public event DownloadProgressChange DownloadProgressChanged;
        #endregion

        #region Public Methods
        /// <summary>
        /// Downloads the file from the DownloadUrl.
        /// </summary>
        /// <param name="timeOutEnabled">If WaitWithTimeout should be called with downloading the file.</param>
        public bool TryDownloadFile(out Exception ex, int timeout, int recheckRate)
        {
            _isDownloading = true;
            using (_client = new WebClient())
            {
                // Create the folder path if it doesn't exist.
                FileInfo file = new FileInfo(this.DestinationPath);
                if (!Directory.Exists(file.DirectoryName))
                {
                    Directory.CreateDirectory(file.DirectoryName);
                }

                _client.DownloadProgressChanged += Client_DownloadProgressChanged;
                _client.DownloadFileCompleted += Client_DownloadFileCompleted;
                _client.DownloadFileAsync(new Uri(this.DownloadUrl), this.DestinationPath);
            }

            this.Wait(timeout, recheckRate);
            ex = _exception;

            if(ex == null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Tells the calling thread to wait for the file to finish downloading.
        /// </summary>
        /// <param name="timeout">How long it takes to time out the file download in seconds.</param>
        /// <param name="reCheckInterval">Every x seconds it will show notice it is still downloading.</param>
        public void Wait(int timeout = 60, int reCheckInterval = 10)
        {
            // Make sure this is only called once.
            if (_timeOutCalled)
            {
                return;
            }

            _timeOutCalled = true;

            TimeSpan waitTime = new TimeSpan(0, 0, 0, timeout);
            Stopwatch watch = new Stopwatch();
            watch.Start();

            while (_isDownloading)
            {
                // The max wait time has occurred, break out.
                if (watch.Elapsed > waitTime)
                {
                    _timedOut = true;
                    _client.CancelAsync();
                    break;
                }

                if(watch.Elapsed.TotalSeconds % reCheckInterval == 0)
                {
                    ServiceManager.Services.LogService.WriteLine("\"{0}\" is still downloading... ({1}/{2}) - {3}%",
                        this.Name, _bytesReceived.ToFileSize(), _totalBytesToReceive.ToFileSize(), _progressPercent);
                }
            }

            // Waiting is finished, we can call this again if needed.
            _timeOutCalled = false;
        }
        #endregion

        #region Private Events
        /// <summary>
        /// When the file is done downloading.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                // If it was cancelled because of time out or anything delete the file.
                if (e.Cancelled)
                {
                    if (File.Exists(this.DestinationPath))
                    {
                        File.Delete(this.DestinationPath);
                    }
                }

                if (this.DownloadCompleted != null)
                {
                    this.DownloadCompleted(e);
                }
            }
            catch(Exception ex)
            {
                _exception = ex;
            }
            finally
            {
                _isDownloading = false;
            }
        }

        /// <summary>
        /// Triggers event every time the progress is changed on the file being downloaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (this.DownloadProgressChanged != null)
            {
                this.DownloadProgressChanged(e);
            }
            _progressPercent = e.ProgressPercentage;
            _bytesReceived = e.BytesReceived;
            _totalBytesToReceive = e.TotalBytesToReceive;
        }
        #endregion
    }
}
