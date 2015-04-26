using System;
using System.Collections.Generic;

namespace OpenBackup.Tests
{
    public class ExecutionContext : IExecutionContext
    {
        public IServiceContainer ServiceContainer
        {
            get;
            private set;
        }

        public IEnumerable<Exception> Errors
        {
            get;
            private set;
        }

        private ExecutionContext()
        {
            Errors = new List<Exception>();
        }

        public ExecutionContext(IServiceContainer serviceContainer) : this()
        {
            ServiceContainer = serviceContainer;
        }

        public void RegisterError(Exception error)
        {
            ((List<Exception>)Errors).Add(error);
        }
    }
}
