using System;

namespace OpenBackup.Extension.Sync
{
    public interface IPairInstance
    {
        IContainerInstance Left
        {
            get;
        }

        IContainerInstance Right
        {
            get;
        }
    }
}
