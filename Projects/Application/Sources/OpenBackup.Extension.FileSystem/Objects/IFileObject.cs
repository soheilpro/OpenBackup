using System;
using System.IO;

namespace OpenBackup.Extension.FileSystem
{
    public interface IFileObject : IFileSystemObject
    {
        FileAttributes Attributes
        {
            get;
        }

        long Length
        {
            get;
        }

        IDriveObject Drive
        {
            get;
        }

        DateTime CreationTimeUtc
        {
            get;
        }

        DateTime LastWriteTimeUtc
        {
            get;
        }

        DateTime LastAccessTimeUtc
        {
            get;
        }
    }
}
