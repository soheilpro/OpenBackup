using System;
using System.Collections.Generic;
using System.IO;

namespace OpenBackup.Extension.FileSystem
{
    public interface IDirectoryObject : IFileSystemObject
    {
        FileAttributes Attributes
        {
            get;
        }

        IDriveObject Drive
        {
            get;
        }

        IEnumerable<IFileObject> GetFiles(IExecutionContext context);

        IEnumerable<IDirectoryObject> GetDirectories(IExecutionContext context);

        IEnumerable<IFileSystemObject> GetChildren(bool recursive, IExecutionContext context);

        IDirectoryObject CombineDirectory(string path);

        IFileObject CombineFile(string path);
    }
}
