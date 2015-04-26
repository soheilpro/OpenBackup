using System;

namespace OpenBackup.Extension.Sync
{
    public class SyncContext : ISyncContext
    {
        public SyncJobInstance JobInstance
        {
            get;
            private set;
        }

        public IExecutionContext ExecutionContext
        {
            get;
            private set;
        }

        public SyncContext(SyncJobInstance jobInstance, IExecutionContext executionContext)
        {
            JobInstance = jobInstance;
            ExecutionContext = executionContext;
        }
    }
}
