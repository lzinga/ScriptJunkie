using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptJunkie.Services
{
    public class ScriptResult
    {
        /// <summary>
        /// The exit code of the program.
        /// </summary>
        public int ExitCode { get; set; } = -1;

        /// <summary>
        /// If the program timed out or not.
        /// </summary>
        public bool TimedOut { get; set; }

        /// <summary>
        /// If the exit code counts as a success.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// The output from the program.
        /// </summary>
        public string Output { get; set; }
    }
}
