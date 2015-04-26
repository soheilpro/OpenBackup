using System;

namespace OpenBackup.Framework
{
    public abstract class OperationBase : IOperation
    {
        protected IExecutionContext Context
        {
            get;
            private set;
        }

        protected OperationBase()
        {
        }

        protected OperationBase(IExecutionContext context)
        {
            Context = context;
        }

        public void Execute()
        {
            try
            {
                ExecuteOperation();
            }
            catch (Exception exception)
            {
                Context.RegisterError(new OperationException(this, exception));
            }
        }

        public abstract void ExecuteOperation();
    }
}
