using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptJunkie.Test
{
    public static class Utilities
    {
        public static string TestPath(string file = "")
        {
            return Path.Combine(Path.GetTempPath(), "ScriptJunkie", file);
        }

        public static string TestPath()
        {
            return Path.Combine(Path.GetTempPath(), "ScriptJunkie");
        }

        public static string GetIncludedFile(string item)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "include", item);
        }
    }
}
