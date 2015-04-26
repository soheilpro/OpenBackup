using System;
using System.Collections.Generic;
using OpenBackup.Framework;

namespace OpenBackup.Extension.Backup
{
    public abstract class StorageInstanceBase : IStorageInstance
    {
        public virtual IEnumerable<IOperation> Initialize(IBackupContext context)
        {
            yield break;
        }

        public abstract IEnumerable<IOperation> Store(IObject obj, IBackupContext context);

        public virtual IEnumerable<IOperation> Shutdown(IBackupContext context)
        {
            yield break;
        }
    }
}
