using System;

namespace OpenBackup.Framework
{
    public class OperationException : ExecutionException
    {
        public IOperation Operation
        {
            get;
            private set;
        }

        public OperationException(IOperation operation, Exception innerException) : base(innerException)
        {
            Operation = operation;
        }
    }
}
