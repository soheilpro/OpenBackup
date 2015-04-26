using System;
using System.Collections.Generic;
using OpenBackup.Framework;

namespace OpenBackup.Extension.Sync
{
    public interface IContainerInstance
    {
        IEnumerable<IOperation> Initialize(ISyncContext context);

        IEnumerable<IObject> GetObjects(ISyncContext context);

        IObject FindObject(IObject obj, ISyncContext context);

        IEnumerable<IOperation> AddObject(IObject obj, ISyncContext context);

        IEnumerable<IChange> GetChanges(IObject obj, IObject baseObject, ISyncContext context);

        IEnumerable<IOperation> UpdateObject(IObject obj, IObject baseObject, IChange change, ISyncContext context);

        IEnumerable<IOperation> RemoveObject(IObject obj, ISyncContext context);

        IEnumerable<IOperation> Shutdown(ISyncContext context);
    }
}
