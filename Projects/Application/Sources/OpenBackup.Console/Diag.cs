using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace OpenBackup.ConsoleMode
{
    internal static class Diag
    {
        private static bool _isStarted;

        private static TraceListener _consoleTraceListener = new ColoredConsoleTraceListener
        {
            TraceOutputOptions = TraceOptions.DateTime | TraceOptions.ProcessId | TraceOptions.ThreadId,
            Filter = new EventTypeFilter(SourceLevels.All)
        };

        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int pid);

        public static void Start()
        {
            if (_isStarted)
                return;

            DefaultTraceSource.Instance.Listeners.Add(_consoleTraceListener);

            if (!AttachConsole(-1))
                AllocConsole();

            _isStarted = true;
        }

        public static void Stop()
        {
            if (!_isStarted)
                return;

            Console.WriteLine();
            FreeConsole();

            DefaultTraceSource.Instance.Listeners.Remove(_consoleTraceListener);

            _isStarted = false;
        }
    }
}
