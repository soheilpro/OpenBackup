using System;
using System.Collections.Generic;
using OpenBackup.Framework;

namespace OpenBackup.Extension.Sync
{
    public class SyncJobInstance : JobInstanceBase
    {
        private SyncJob _job;
        private List<IPairInstance> _pairs = new List<IPairInstance>();

        public override IJob Job
        {
            get
            {
                return _job;
            }
        }

        public SyncJobInstance(SyncJob job, IExecutionContext context)
        {
            _job = job;

            _pairs.AddRange(InstantiatePairs(context));
        }

        private IEnumerable<IPairInstance> InstantiatePairs(IExecutionContext context)
        {
            foreach (var pair in _job.Pairs)
                yield return pair.CreateInstance(context);
        }

        public override IEnumerable<IOperation> RunJob(IExecutionContext context)
        {
            foreach (var pair in _pairs)
                foreach (var operation in RunPair(pair, context))
                    yield return operation;
        }

        private IEnumerable<IOperation> RunPair(IPairInstance pair, IExecutionContext context)
        {
            var syncContext = new SyncContext(this, context);

            foreach (var operation in pair.Left.Initialize(syncContext))
                yield return operation;

            foreach (var operation in pair.Right.Initialize(syncContext))
                yield return operation;

            // TODO
            foreach (var operation in Mirror(pair.Left, pair.Right, syncContext))
                yield return operation;

            foreach (var operation in pair.Left.Shutdown(syncContext))
                yield return operation;

            foreach (var operation in pair.Right.Shutdown(syncContext))
                yield return operation;
        }

        private static IEnumerable<IOperation> Mirror(IContainerInstance left, IContainerInstance right, ISyncContext context)
        {
            // Add new and modified Left objects to Right
            foreach (var leftObject in left.GetObjects(context))
            {
                var rightObject = right.FindObject(leftObject, context);

                if (rightObject == null)
                {
                    foreach (var operation in right.AddObject(leftObject, context))
                        yield return operation;
                }
                else
                {
                    foreach (var change in right.GetChanges(leftObject, rightObject, context))
                        foreach (var operation in right.UpdateObject(rightObject, leftObject, change, context))
                            yield return operation;
                }
            }

            // Remove objects that exist in Right but do not exist in Left
            foreach (var rightObject in right.GetObjects(context))
            {
                var leftObject = left.FindObject(rightObject, context);

                if (leftObject == null)
                    foreach (var operation in right.RemoveObject(rightObject, context))
                        yield return operation;
            }
        }
    }
}
