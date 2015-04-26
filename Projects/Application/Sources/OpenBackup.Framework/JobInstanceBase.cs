using System;
using System.Collections.Generic;

namespace OpenBackup.Framework
{
    public abstract class JobInstanceBase : IJobInstance
    {
        public abstract IJob Job
        {
            get;
        }

        public virtual IEnumerable<IOperation> Run(IExecutionContext context)
        {
            foreach (var operation in Initialize(context))
                yield return operation;

            foreach (var operation in RunJob(context))
                yield return operation;

            foreach (var operation in Shutdown(context))
                yield return operation;
        }

        public virtual IEnumerable<IOperation> Initialize(IExecutionContext context)
        {
            yield break;
        }

        public abstract IEnumerable<IOperation> RunJob(IExecutionContext context);

        public virtual IEnumerable<IOperation> Shutdown(IExecutionContext context)
        {
            yield break;
        }
    }
}
