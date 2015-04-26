using System;

namespace OpenBackup.Runtime
{
    public class LoadingContext : ILoadingContext
    {
        public IServiceContainer ServiceContainer
        {
            get;
            private set;
        }

        public LoadingContext(IServiceContainer serviceContainer)
        {
            ServiceContainer = serviceContainer;
        }
    }
}
