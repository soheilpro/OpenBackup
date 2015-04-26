using System;
using System.Collections.Generic;
using System.Linq;
using OpenBackup.Framework;

namespace OpenBackup.Extension.Sync.Tests
{
    internal class TestContainerInstance : ContainerInstanceBase
    {
        private TestContainer _container;

        public TestContainerInstance(TestContainer container, IExecutionContext context)
        {
            _container = container;
        }

        public override IEnumerable<IObject> GetObjects(ISyncContext context)
        {
            return _container.Objects;
        }

        public override IObject FindObject(IObject obj, ISyncContext context)
        {
            return _container.Objects.FirstOrDefault(o => Equals(o.Id, obj.Id));
        }

        public override IEnumerable<IOperation> AddObject(IObject obj, ISyncContext context)
        {
            yield return new AddObjectOperation(obj, context.ExecutionContext);
        }

        public override IEnumerable<IChange> GetChanges(IObject obj, IObject baseObject, ISyncContext context)
        {
            if (Equals(obj, baseObject))
                yield break;
            else
                yield return new ValueChange();
        }

        public override IEnumerable<IOperation> UpdateObject(IObject obj, IObject baseObject, IChange change, ISyncContext context)
        {
            yield return new UpdateObjectOperation(obj, baseObject, change, context.ExecutionContext);
        }

        public override IEnumerable<IOperation> RemoveObject(IObject obj, ISyncContext context)
        {
            yield return new RemoveObjectOperation(obj, context.ExecutionContext);
        }
    }
}
