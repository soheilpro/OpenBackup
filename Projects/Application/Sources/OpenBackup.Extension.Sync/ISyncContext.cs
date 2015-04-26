using System;

namespace OpenBackup.Extension.Sync
{
    public interface ISyncContext
    {
        SyncJobInstance JobInstance
        {
            get;
        }

        IExecutionContext ExecutionContext
        {
            get;
        }
    }
}
