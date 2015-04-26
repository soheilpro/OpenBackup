using System;

namespace OpenBackup.Framework
{
    public class ExecutionException : Exception
    {
        public ExecutionException(Exception innerException) : base(innerException.Message, innerException)
        {
        }
    }
}
