using System;

namespace OpenBackup.Extension.Sync
{
    public class PairInstance : IPairInstance
    {
        public IContainerInstance Left
        {
            get;
            private set;
        }

        public IContainerInstance Right
        {
            get;
            private set;
        }

        public PairInstance(IPair pair, IExecutionContext context)
        {
            Left = InstantiateContainer(pair.Left, context);
            Right = InstantiateContainer(pair.Right, context);
        }

        private IContainerInstance InstantiateContainer(IContainer container, IExecutionContext context)
        {
            return container.CreateInstance(context);
        }
    }
}
