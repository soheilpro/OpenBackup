using System;
using System.Diagnostics;
using System.Threading;

namespace OpenBackup.ConsoleMode
{
    internal class ColoredConsoleTraceListener : TraceListener
    {
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, format, args, null, null))
                return;

            using (new ColoredConsole(GetEventColor(eventType)))
            {
                if ((TraceOutputOptions & TraceOptions.Timestamp) != 0)
                    Console.Write("+{0} ", Stopwatch.GetTimestamp());

                if ((TraceOutputOptions & TraceOptions.DateTime) != 0)
                    Console.Write("{0:HH:mm:ss} ", DateTime.Now);

                if ((TraceOutputOptions & TraceOptions.ProcessId) != 0)
                    Console.Write("PID: {0} ", Process.GetCurrentProcess().Id);

                if ((TraceOutputOptions & TraceOptions.ThreadId) != 0)
                    Console.Write("TID: {0} ", Thread.CurrentThread.ManagedThreadId);

                Console.Write("{0}: ", source);
                Console.WriteLine(format, args);
            }
        }

        private static ConsoleColor GetEventColor(TraceEventType eventType)
        {
            switch (eventType)
            {
                case TraceEventType.Critical:
                    return ConsoleColor.Red;

                case TraceEventType.Error:
                    return ConsoleColor.DarkRed;

                case TraceEventType.Warning:
                    return ConsoleColor.Yellow;

                case TraceEventType.Information:
                    return ConsoleColor.Cyan;

                case TraceEventType.Verbose:
                    return ConsoleColor.Gray;
            }

            return ConsoleColor.Gray;
        }

        public override void Write(string message)
        {
            Console.Write(message);
        }

        public override void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}
