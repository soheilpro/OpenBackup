using System;
using System.Collections.Generic;
using OpenBackup.Framework;

namespace OpenBackup.Extension.FileSystem
{
    public interface IFileSystemStorableObject : IObject
    {
        IEnumerable<IOperation> Store(IDirectoryObject directory, IFileSystem fileSystem, IExecutionContext context);
    }
}
