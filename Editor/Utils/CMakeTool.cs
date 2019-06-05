using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace cmake
{
    public class CMakeTool
    {
        private string m_executable;

        private CMakeGenerator m_generator;

        public CMakeTool(string executable)
        {
            m_executable = executable;
        }

        private static string argsToString(string[] args)
        {
            if(args == null)
            {
                return "";
            }

            StringBuilder argBuilder = new StringBuilder();
            foreach(string arg in args)
            {
                argBuilder.Append("'").Append(arg.Replace("'", @"\'")).Append("'");
            }

            return string.Join(" ", args);
        }

        private static string[] defsToArgs(Dictionary<string, string> defs)
        {
            string[] args = new string[defs.Count];
            int i = 0;

            foreach(KeyValuePair<string, string> entry in defs)
            {
                string arg = new StringBuilder("-D").Append(entry.Key).Append('=').Append(entry.Value).ToString();
                args[i] = arg;
                i++;
            }

            return args;
        }

        private Process CreateProcess(string[] args = null)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(m_executable);
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;

            if(args != null)
            {
                startInfo.Arguments = argsToString(args);
            }

            var p = new Process();
            p.StartInfo = startInfo;
            return p;
        }

        public Process Configure(string sourceFolder = null, string buildFolder = null, string[] args = null, Dictionary<string, string> defs = null)
        {
            Directory.CreateDirectory(buildFolder);

            List<string> fullArgs = new List<string>();
            if(args != null)
            {
                fullArgs.AddRange(args);
            }
            if(defs != null)
            {
                fullArgs.AddRange(defsToArgs(defs));
            }
            if(sourceFolder != null)
            {
                fullArgs.Add(sourceFolder);
            }

            var p = CreateProcess(fullArgs.ToArray());
            if(buildFolder != null)
            {
                p.StartInfo.WorkingDirectory = buildFolder;
            }
            return p;
        }

        public Process Build(string target, string buildFolder = null, string[] args = null)
        {
            return Build(new string[]{ target }, buildFolder, args);
        }

        public Process Build(string[] targets = null , string buildFolder = null, string[] args = null)
        {
            List<string> fullArgs = new List<string>();
            fullArgs.Add("--build");
            if(args != null)
            {
                fullArgs.AddRange(args);
            }

            var p = CreateProcess(fullArgs.ToArray());
            if(buildFolder != null)
            {
                p.StartInfo.WorkingDirectory = buildFolder;
            }
            return p;
        }

        public Process Install(string buildFolder = null, string[] args = null)
        {
            return Build("install", buildFolder, args);
        }
    }
}
