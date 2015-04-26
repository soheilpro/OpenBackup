using System;
using System.Collections.Generic;
using OpenBackup.Framework;

namespace OpenBackup.Extension.Backup
{
    public abstract class SourceInstanceBase : ISourceInstance
    {
        public virtual IEnumerable<IOperation> Initialize(IBackupContext context)
        {
            yield break;
        }

        public abstract IEnumerable<IObject> GetObjects(IBackupContext context);

        public virtual IEnumerable<IOperation> Shutdown(IBackupContext context)
        {
            yield break;
        }
    }
}
