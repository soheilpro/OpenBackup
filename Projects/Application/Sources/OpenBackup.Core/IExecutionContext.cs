using System;
using System.Collections.Generic;

namespace OpenBackup
{
    public interface IExecutionContext
    {
        IServiceContainer ServiceContainer
        {
            get;
        }

        IEnumerable<Exception> Errors
        {
            get;
        }

        void RegisterError(Exception error);
    }
}
