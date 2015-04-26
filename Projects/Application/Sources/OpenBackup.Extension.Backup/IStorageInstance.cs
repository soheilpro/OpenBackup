using System;
using System.Collections.Generic;
using OpenBackup.Framework;

namespace OpenBackup.Extension.Backup
{
    public interface IStorageInstance
    {
        IEnumerable<IOperation> Initialize(IBackupContext context);

        IEnumerable<IOperation> Store(IObject obj, IBackupContext context);

        IEnumerable<IOperation> Shutdown(IBackupContext context);
    }
}
