using System;
using OpenBackup.Framework;

namespace OpenBackup.Extension.FileSystem
{
    public interface IFileSystemObject : IObject, IMetadataProvider
    {
        FileSystemPath Path
        {
            get;
        }

        string Name
        {
            get;
        }

        bool Exists
        {
            get;
        }

        IFileSystemObject Parent
        {
            get;
        }

        // TODO
        string ActualName
        {
            get;
        }
    }
}
