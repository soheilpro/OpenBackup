using System;

namespace OpenBackup.Extension.Backup
{
    public interface IBackupContext
    {
        BackupJobInstance JobInstance
        {
            get;
        }

        IExecutionContext ExecutionContext
        {
            get;
        }
    }
}
