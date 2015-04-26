using System;

namespace OpenBackup
{
    public interface ILoadingContext
    {
        IServiceContainer ServiceContainer
        {
            get;
        }
    }
}
