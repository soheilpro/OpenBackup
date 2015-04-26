using System;
using System.IO;

namespace OpenBackup.Extension.FileSystem
{
    public interface IDriveObject : IFileSystemObject
    {
        DriveType DriveType
        {
            get;
        }

        bool SupportsHardLinks
        {
            get;
        }

        IDirectoryObject Root
        {
            get;
        }
    }
}
