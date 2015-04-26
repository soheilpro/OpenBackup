using System;
using System.Collections.Generic;

namespace OpenBackup
{
    public interface IJobInstance
    {
        IJob Job
        {
            get;
        }

        IEnumerable<IOperation> Run(IExecutionContext context);
    }
}
