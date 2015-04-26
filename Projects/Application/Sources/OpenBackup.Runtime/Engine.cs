using System;
using System.Collections.Generic;

namespace OpenBackup.Runtime
{
    public class Engine
    {
        private IExecutionContext _executionContext;

        public IEnumerable<Exception> Errors
        {
            get
            {
                return _executionContext.Errors;
            }
        }

        public Engine()
        {
            var serviceContainer = new AppDomainServiceContainer();
            _executionContext = new ExecutionContext(serviceContainer);
        }

        public IEnumerable<IOperation> RunJob(IJob job)
        {
            var jobInstance = job.CreateInstance(_executionContext);

            foreach (var operation in jobInstance.Run(_executionContext))
                yield return operation;
        }
    }
}
