using System;
using System.Diagnostics;

namespace OpenBackup
{
    public static class DefaultTraceSource
    {
        private static TraceSource _instance;
        private static int _defaultEventId;

        public static TraceSource Instance
        {
            get
            {
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        [Conditional("TRACE")]
        public static void TraceEvent(TraceEventType eventType, int id, string message, params object[] args)
        {
            if (_instance == null)
                return;

            _instance.TraceEvent(eventType, id, message, args);
        }

        [Conditional("TRACE")]
        public static void TraceEvent(TraceEventType eventType, string message, params object[] args)
        {
            TraceEvent(eventType, _defaultEventId, message, args);
        }

        [Conditional("TRACE")]
        public static void TraceVerbose(int id, string message, params object[] args)
        {
            TraceEvent(TraceEventType.Verbose, id, message, args);
        }

        [Conditional("TRACE")]
        public static void TraceVerbose(string message, params object[] args)
        {
            TraceEvent(TraceEventType.Verbose, _defaultEventId, message, args);
        }

        [Conditional("TRACE")]
        public static void TraceInformation(int id, string message, params object[] args)
        {
            TraceEvent(TraceEventType.Information, id, message, args);
        }

        [Conditional("TRACE")]
        public static void TraceInformation(string message, params object[] args)
        {
            TraceEvent(TraceEventType.Information, _defaultEventId, message, args);
        }

        [Conditional("TRACE")]
        public static void TraceWarning(int id, string message, params object[] args)
        {
            TraceEvent(TraceEventType.Warning, id, message, args);
        }

        [Conditional("TRACE")]
        public static void TraceWarning(string message, params object[] args)
        {
            TraceEvent(TraceEventType.Warning, _defaultEventId, message, args);
        }

        [Conditional("TRACE")]
        public static void TraceWarning(int id, Exception exception)
        {
            TraceEvent(TraceEventType.Warning, id, FormatException(exception));
        }

        [Conditional("TRACE")]
        public static void TraceWarning(Exception exception)
        {
            TraceEvent(TraceEventType.Warning, _defaultEventId, FormatException(exception));
        }

        [Conditional("TRACE")]
        public static void TraceError(int id, string message, params object[] args)
        {
            TraceEvent(TraceEventType.Error, id, message, args);
        }

        [Conditional("TRACE")]
        public static void TraceError(string message, params object[] args)
        {
            TraceEvent(TraceEventType.Error, _defaultEventId, message, args);
        }

        [Conditional("TRACE")]
        public static void TraceError(int id, Exception exception)
        {
            TraceEvent(TraceEventType.Error, id, FormatException(exception));
        }

        [Conditional("TRACE")]
        public static void TraceError(Exception exception)
        {
            TraceEvent(TraceEventType.Error, _defaultEventId, FormatException(exception));
        }

        [Conditional("TRACE")]
        public static void TraceCritical(int id, string message, params object[] args)
        {
            TraceEvent(TraceEventType.Critical, id, message, args);
        }

        [Conditional("TRACE")]
        public static void TraceCritical(string message, params object[] args)
        {
            TraceEvent(TraceEventType.Critical, _defaultEventId, message, args);
        }

        [Conditional("TRACE")]
        public static void TraceCritical(int id, Exception exception)
        {
            TraceEvent(TraceEventType.Critical, id, FormatException(exception));
        }

        [Conditional("TRACE")]
        public static void TraceCritical(Exception exception)
        {
            TraceEvent(TraceEventType.Critical, _defaultEventId, FormatException(exception));
        }

        private static string FormatException(Exception exception)
        {
            var message = exception.ToString();
            message = message.Replace("{", "{{");
            message = message.Replace("}", "}}");

            return message;
        }
    }
}
