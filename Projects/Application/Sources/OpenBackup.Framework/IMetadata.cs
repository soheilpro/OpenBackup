using System;

namespace OpenBackup.Framework
{
    public interface IMetadata
    {
        string Name
        {
            get;
        }

        object Value
        {
            get;
        }
    }
}
