using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace OpenBackup.ConsoleMode
{
    internal static class CommandLine
    {
        private static NameValueCollection _switches = new NameValueCollection();

        private static List<string> _arguemnts = new List<string>();

        static CommandLine()
        {
            var regex = new Regex(@"^/(?<name>\w+)(=(?<value>[^/]*))?$", RegexOptions.IgnoreCase);

            foreach (var arg in Environment.GetCommandLineArgs())
            {
                var match = regex.Match(arg);

                if (match.Success)
                    _switches.Add(match.Groups["name"].Value, match.Groups["value"].Value);
                else
                    _arguemnts.Add(arg);
            }
        }

        public static string GetSwitchValue(string name)
        {
            return _switches[name];
        }

        public static bool IsSwitchSpecified(string name)
        {
            return _switches[name] != null;
        }

        public static string[] GetArguments()
        {
            return _arguemnts.ToArray();
        }

        public static string GetArgument(int index)
        {
            return _arguemnts[index];
        }
    }
}
