using System;
using System.Collections.Generic;
using OpenBackup.Framework;

namespace OpenBackup.Extension.Backup
{
    public interface ISourceInstance
    {
        IEnumerable<IOperation> Initialize(IBackupContext context);

        IEnumerable<IObject> GetObjects(IBackupContext context);

        IEnumerable<IOperation> Shutdown(IBackupContext context);
    }
}
