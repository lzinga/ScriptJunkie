﻿using ScriptJunkie.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ScriptJunkie.Services
{
    [Serializable]
    public class Script
    {
        #region Private Constant Fields
        private const string CommandPrompt = "cmd.exe ";
        #endregion

        #region Public Properties
        /// <summary>
        /// The name of the script.
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// The description of the script.
        /// </summary>
        [XmlAttribute]
        public string Description { get; set; }

        /// <summary>
        /// The file that will be executed.
        /// TODO: This could probably be turned into a simple object instead of
        /// individual class.
        /// </summary>
        public Executable Executable;

        /// <summary>
        /// The results of the script.
        /// </summary>
        [XmlIgnore]
        public ScriptResult Results;

        /// <summary>
        /// Arguments to run the executable with.
        /// </summary>
        public ArgumentCollection Arguments;

        /// <summary>
        /// Exit codes the program can exit.
        /// </summary>
        public ExitCodeCollection ExitCodes;
        #endregion

        #region Public Methods
        /// <summary>
        /// Executes the script.
        /// </summary>
        /// <returns>ScriptResult</returns>
        public ScriptResult Execute(int timeout = 60, int refreshRate = 10)
        {
            ScriptResult result = new ScriptResult();
            Results = result;
            // If the file to be executed doesn't exist skip.
            if (!File.Exists(this.Executable.Path))
            {
                ServiceManager.Services.LogService.WriteSubHeader("Skipping \"{0}\" cannot find \"{1}\".", this.Name, this.Executable.Path);
                return new ScriptResult() { IsSuccess = false, TimedOut = false, Output = "Skipped" };
            }


            string fullCommand = "";
            Process proc = new Process();
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.UseShellExecute = false;
            FileInfo info = new FileInfo(this.Executable.Path);

            string arguments = this.Arguments == null ? string.Empty : this.Arguments.ToString();
            switch (info.Extension)
            {
                case ".ps1":
                    proc.StartInfo.FileName = "powershell.exe";
                    proc.StartInfo.WorkingDirectory = info.Directory.FullName;
                    proc.StartInfo.Arguments = string.Format("-executionpolicy unrestricted .\\{0} {1}; exit $LASTEXITCODE", info.Name, arguments);
                    break;
                default:
                    proc.StartInfo.FileName = this.Executable.Path;
                    proc.StartInfo.Arguments = string.Format("{0}", this.Arguments.ToString());
                    break;
            }

            fullCommand = string.Format("{0} {1}", proc.StartInfo.FileName, proc.StartInfo.Arguments);
            ServiceManager.Services.LogService.WriteSubHeader("Running \"{0}\".", this.Name);
            ServiceManager.Services.LogService.WriteLine("Command: {0} {1}".Trim(), this.Executable.Path, arguments);

            proc.Start();
            string output = proc.StandardOutput.ReadToEnd();

            TimeSpan waitTime = new TimeSpan(0, 0, 0, timeout);
            Stopwatch watch = new Stopwatch();
            watch.Start();

            // Wait for process exit.
            while (!proc.WaitForExit(timeout * 1000))
            {
                // The max wait time has occurred, break out and stop process.
                if (watch.Elapsed > waitTime)
                {
                    result.TimedOut = true;
                    proc.Close();
                    break;
                }

                if (watch.Elapsed.TotalSeconds % refreshRate == 0)
                {
                    ServiceManager.Services.LogService.WriteLine("\"{0}\" is still running...",
                        this.Name);
                }
            }

            // place exit code and output into the script result.
            result.ExitCode = proc.ExitCode;
            result.Output = output;

            // If the exit code from the program is inside the exit code list
            // and that exit code counts as a pass.
            result.IsSuccess = this.ExitCodes.Any(i => i.Value == result.ExitCode && i.IsSuccess);

            

            return result;
        }
        #endregion
    }
}
