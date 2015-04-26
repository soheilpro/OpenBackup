using System;

namespace OpenBackup.Extension.Backup
{
    public class BackupContext : IBackupContext
    {
        public BackupJobInstance JobInstance
        {
            get;
            private set;
        }

        public IExecutionContext ExecutionContext
        {
            get;
            private set;
        }

        public BackupContext(BackupJobInstance jobInstance, IExecutionContext execeutionContext)
        {
            JobInstance = jobInstance;
            ExecutionContext = execeutionContext;
        }
    }
}
