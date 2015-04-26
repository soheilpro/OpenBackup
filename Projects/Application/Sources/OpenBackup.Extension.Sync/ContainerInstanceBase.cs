using System;
using System.Collections.Generic;
using OpenBackup.Framework;

namespace OpenBackup.Extension.Sync
{
    public abstract class ContainerInstanceBase : IContainerInstance
    {
        public virtual IEnumerable<IOperation> Initialize(ISyncContext context)
        {
            yield break;
        }

        public abstract IEnumerable<IObject> GetObjects(ISyncContext context);

        public abstract IObject FindObject(IObject obj, ISyncContext context);

        public abstract IEnumerable<IOperation> AddObject(IObject obj, ISyncContext context);

        public abstract IEnumerable<IChange> GetChanges(IObject obj, IObject baseObject, ISyncContext context);

        public abstract IEnumerable<IOperation> UpdateObject(IObject obj, IObject baseObject, IChange change, ISyncContext context);

        public abstract IEnumerable<IOperation> RemoveObject(IObject obj, ISyncContext context);

        public virtual IEnumerable<IOperation> Shutdown(ISyncContext context)
        {
            yield break;
        }
    }
}
